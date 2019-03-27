namespace Arcadia.Ask.Storage.Questions
{
    using Arcadia.Ask.Models.DTO;
    using Arcadia.Ask.Models.Entities;
    using Arcadia.Ask.Storage;
    using Arcadia.Ask.Storage.Exceptions;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

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
            var foundQuestion = await this.FindQuestionEntityByIdAsync(questionId);

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
            var entity = await this.FindQuestionEntityByIdAsync(questionId);
            if (entity == null)
                ThrowQuestionNotFound(questionId);

            this.dbCtx.Questions.Remove(entity);
            await this.dbCtx.SaveChangesAsync();
        }

        public async Task<QuestionEntity> ApproveQuestion(Guid questionId)
        {
            var entity = await this.FindQuestionEntityByIdAsync(questionId);
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

            if (await this.dbCtx.Votes.Where(v => v.QuestionId == questionId && v.UserId == userId).FirstOrDefaultAsync() != null)
                ThrowQuestionUpvoted(questionId);

            await this.dbCtx.Votes.AddAsync(new VoteEntity() { QuestionId = questionId, UserId = userId });
            await this.dbCtx.SaveChangesAsync();

            return await FindQuestionEntityByIdAsync(questionId);
        }

        public async Task<QuestionEntity> DownvoteQuestion(Guid questionId, Guid userId)
        {
            var entity = await FindQuestionEntityByIdAsync(questionId);
            if (entity == null)
                ThrowQuestionNotFound(questionId);

            var voteEntity = await this.dbCtx.Votes
                .Where(v => v.QuestionId == questionId && v.UserId == userId)
                .FirstOrDefaultAsync();
            if (voteEntity == null)
                return entity;

            this.dbCtx.Remove(voteEntity);
            await this.dbCtx.SaveChangesAsync();

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