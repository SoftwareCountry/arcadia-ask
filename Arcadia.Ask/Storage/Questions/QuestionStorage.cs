namespace Arcadia.Ask.Storage.Questions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Exceptions;
    using Microsoft.EntityFrameworkCore;
    using Models.Entities;

    public class QuestionStorage : IQuestionStorage
    {
        private readonly DatabaseContext dbCtx;

        public QuestionStorage(DatabaseContext dbCtx)
        {
            this.dbCtx = dbCtx;
        }

        public async Task<IEnumerable<QuestionEntity>> GetQuestions()
        {
            return await dbCtx.Questions.Include(q => q.Votes).ToListAsync();
        }

        public async Task<QuestionEntity> GetQuestion(Guid questionId)
        {
            var foundQuestion = await FindQuestionEntityByIdAsync(questionId);

            if (foundQuestion == null)
                ThrowQuestionNotFound(questionId);

            return foundQuestion;
        }

        public async Task<QuestionEntity> UpsertQuestion(QuestionEntity question)
        {
            await dbCtx.AddAsync(question);
            await dbCtx.SaveChangesAsync();

            return question;
        }

        public async Task DeleteQuestion(Guid questionId)
        {
            var entity = await FindQuestionEntityByIdAsync(questionId);
            if (entity == null)
                ThrowQuestionNotFound(questionId);

            dbCtx.Questions.Remove(entity);
            await dbCtx.SaveChangesAsync();
        }

        public async Task<QuestionEntity> ApproveQuestion(Guid questionId)
        {
            var entity = await FindQuestionEntityByIdAsync(questionId);
            if (entity == null)
                ThrowQuestionNotFound(questionId);

            entity.IsApproved = true;
            await dbCtx.SaveChangesAsync();
            return entity;
        }

        public async Task<QuestionEntity> UpvoteQuestion(Guid questionId, Guid userId)
        {
            var entity = await FindQuestionEntityByIdAsync(questionId);
            if (entity == null)
                ThrowQuestionNotFound(questionId);

            if (await dbCtx.Votes.Where(v => v.QuestionId == questionId && v.UserId == userId).FirstOrDefaultAsync() != null)
                ThrowQuestionUpvoted(questionId);

            await dbCtx.Votes.AddAsync(new VoteEntity { QuestionId = questionId, UserId = userId });
            await dbCtx.SaveChangesAsync();

            return await FindQuestionEntityByIdAsync(questionId);
        }

        public async Task<QuestionEntity> DownvoteQuestion(Guid questionId, Guid userId)
        {
            var entity = await FindQuestionEntityByIdAsync(questionId);
            if (entity == null)
                ThrowQuestionNotFound(questionId);

            var voteEntity = await dbCtx.Votes
                .Where(v => v.QuestionId == questionId && v.UserId == userId)
                .FirstOrDefaultAsync();
            if (voteEntity == null)
                return entity;

            dbCtx.Remove(voteEntity);
            await dbCtx.SaveChangesAsync();

            return await FindQuestionEntityByIdAsync(questionId);
        }

        private async Task<QuestionEntity> FindQuestionEntityByIdAsync(Guid questionId)
        {
            return await dbCtx.Questions
                .Where(q => q.QuestionId == questionId)
                .Include(q => q.Votes)
                .FirstOrDefaultAsync();
        }

        private static void ThrowQuestionNotFound(Guid questionId)
        {
            throw new QuestionNotFoundException(questionId);
        }

        private static void ThrowQuestionUpvoted(Guid questionId)
        {
            throw new QuestionUpvotedException(questionId);
        }
    }
}