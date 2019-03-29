namespace Arcadia.Ask.Storage.Questions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Exceptions;

    using Microsoft.EntityFrameworkCore;

    using Arcadia.Ask.Models.Entities;

    public class QuestionStorage : IQuestionStorage
    {
        private readonly DatabaseContext dbCtx;

        public QuestionStorage(DatabaseContext dbCtx)
        {
            this.dbCtx = dbCtx;
        }

        public async Task<IEnumerable<QuestionEntity>> GetQuestions()
        {
            return await this.dbCtx.Questions
                .Include(q => q.Votes)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<QuestionEntity> GetQuestion(Guid questionId)
        {
            var foundQuestion = await this.FindQuestionEntityByIdAsync(questionId);

            if (foundQuestion == null)
            {
                throw new QuestionNotFoundException(questionId);
            }

            this.DetachEntity(foundQuestion);
            return foundQuestion;
        }

        public async Task<QuestionEntity> UpsertQuestion(QuestionEntity question)
        {
            await this.dbCtx.AddAsync(question);
            await this.dbCtx.SaveChangesAsync();

            this.DetachEntity(question);
            return question;
        }

        public async Task DeleteQuestion(Guid questionId)
        {
            var entity = await this.FindQuestionEntityByIdAsync(questionId);

            if (entity == null)
            {
                throw new QuestionNotFoundException(questionId);
            }

            this.dbCtx.Questions.Remove(entity);
            await this.dbCtx.SaveChangesAsync();
        }

        public async Task<QuestionEntity> ApproveQuestion(Guid questionId)
        {
            var entity = await this.FindQuestionEntityByIdAsync(questionId);

            if (entity == null)
            {
                throw new QuestionNotFoundException(questionId);
            }

            entity.IsApproved = true;
            await this.dbCtx.SaveChangesAsync();

            this.DetachEntity(entity);
            return entity;
        }

        public async Task<QuestionEntity> UpvoteQuestion(Guid questionId, Guid userId)
        {
            return await this.VoteForQuestion(questionId, userId, VoteStatus.Upvoted);
        }

        public async Task<QuestionEntity> DownvoteQuestion(Guid questionId, Guid userId)
        {
            return await this.VoteForQuestion(questionId, userId, VoteStatus.Downvoted);
        }

        private async Task<QuestionEntity> FindQuestionEntityByIdAsync(Guid questionId)
        {
            return await this.dbCtx.Questions
                .Where(q => q.QuestionId == questionId)
                .Include(q => q.Votes)
                .FirstOrDefaultAsync();
        }

        private void DetachEntity(QuestionEntity entity)
        {
            this.dbCtx.Entry(entity).State = EntityState.Detached;
        }

        private async Task<QuestionEntity> VoteForQuestion(Guid questionId, Guid userId, VoteStatus voteStatus)
        {
            var entity = await this.FindQuestionEntityByIdAsync(questionId);

            if (entity == null)
            {
                throw new QuestionNotFoundException(questionId);
            }

            if (
                await this.dbCtx.Votes
                    .Where(v => v.QuestionId == questionId && v.UserId == userId)
                    .FirstOrDefaultAsync() != null
                )
            {
                throw new QuestionVotedException(questionId);
            }

            await this.dbCtx.Votes.AddAsync(new VoteEntity
            {
                QuestionId = questionId,
                UserId = userId,
                IsUpvoted = voteStatus == VoteStatus.Upvoted
            });
            await this.dbCtx.SaveChangesAsync();

            this.DetachEntity(entity);
            return entity;
        }
        
        private enum VoteStatus
        {
            Upvoted,
            Downvoted
        }
    }
}