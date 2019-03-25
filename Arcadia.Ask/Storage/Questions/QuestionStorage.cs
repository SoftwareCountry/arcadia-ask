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

        private QuestionDTO EntityToDTO(QuestionEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return new QuestionDTO
            {
                Author = entity.Author,
                IsApproved = entity.IsApproved,
                PostedAt = entity.PostedAt,
                QuestionId = entity.QuestionId,
                Text = entity.Text,
                Votes = entity.Votes?.Count ?? 0,

            };
        }

        private QuestionDTO EntityToDTO(QuestionEntity entity, Guid userId)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return new QuestionDTO
            {
                Author = entity.Author,
                IsApproved = entity.IsApproved,
                PostedAt = entity.PostedAt,
                QuestionId = entity.QuestionId,
                Text = entity.Text,
                Votes = entity.Votes?.Count(v => v.QuestionId == entity.QuestionId) ?? 0,
                DidVote = entity.Votes?.Count(voteEntity => voteEntity.UserId == userId && voteEntity.QuestionId == entity.QuestionId) > 0
            };
        }

        private async Task<QuestionEntity> FindQuestionEntityByIdAsync(Guid questionId)
        {
            return await this.dbCtx.Questions
                .Where(q => q.QuestionId == questionId)
                .Include(q => q.Votes)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<QuestionDTO>> GetQuestionsForSpecificUser(Guid userId)
        {
            var questions = await this.dbCtx.Questions.Include(q => q.Votes).ToListAsync();
            return questions.Select(q => this.EntityToDTO(q, userId));
        }

        public async Task<QuestionDTO> GetQuestionForSpecificUser(Guid questionId, Guid userId)
        {
            var foundQuestion = await this.FindQuestionEntityByIdAsync(questionId);

            if (foundQuestion == null)
                ThrowQuestionNotFound(questionId);

            return this.EntityToDTO(foundQuestion, userId);
        }

        public async Task<QuestionDTO> UpsertQuestion(QuestionDTO question)
        {
            var entity = new QuestionEntity()
            {
                QuestionId = Guid.NewGuid(),
                Author = question.Author,
                IsApproved = question.IsApproved,
                PostedAt = question.PostedAt,
                Text = question.Text
            };

            await this.dbCtx.Questions.AddAsync(entity);
            await this.dbCtx.SaveChangesAsync();

            return this.EntityToDTO(entity);
        }

        public async Task DeleteQuestion(Guid questionId)
        {
            var entity = await this.FindQuestionEntityByIdAsync(questionId);
            if (entity == null)
                ThrowQuestionNotFound(questionId);

            this.dbCtx.Questions.Remove(entity);
            await this.dbCtx.SaveChangesAsync();
        }

        private void ThrowQuestionNotFound(Guid questionId) => throw new QuestionNotFoundException(questionId);

        private void ThrowQuestionUpvoted(Guid questionId) => throw new QuestionUpvotedException(questionId);

        public async Task<QuestionDTO> ApproveQuestion(Guid questionId)
        {
            var entity = await this.FindQuestionEntityByIdAsync(questionId);
            if (entity == null)
                this.ThrowQuestionNotFound(questionId);

            entity.IsApproved = true;
            await this.dbCtx.SaveChangesAsync();
            return EntityToDTO(entity);
        }

        public async Task<QuestionDTO> UpvoteQuestion(Guid questionId, Guid userId)
        {
            var entity = await FindQuestionEntityByIdAsync(questionId);
            if (entity == null)
                this.ThrowQuestionNotFound(questionId);

            if (await this.dbCtx.Votes.Where(v => v.QuestionId == questionId && v.UserId == userId).FirstOrDefaultAsync() != null)
                this.ThrowQuestionUpvoted(questionId);

            await this.dbCtx.Votes.AddAsync(new VoteEntity() { QuestionId = questionId, UserId = userId });
            await this.dbCtx.SaveChangesAsync();

            var questionDto = EntityToDTO(entity);
            questionDto.Votes = await this.dbCtx.Votes
                .Where(v => v.QuestionId == questionId)
                .Select(v => v.UserId)
                .CountAsync();
            questionDto.DidVote = true;

            return questionDto;
        }

        public async Task<QuestionDTO> DownvoteQuestion(Guid questionId, Guid userId)
        {
            var entity = await FindQuestionEntityByIdAsync(questionId);
            if (entity == null)
                this.ThrowQuestionNotFound(questionId);

            var voteEntity = await this.dbCtx.Votes
                .Where(v => v.QuestionId == questionId && v.UserId == userId)
                .FirstOrDefaultAsync();
            if (voteEntity == null)
                return EntityToDTO(entity);

            this.dbCtx.Remove(voteEntity);
            await this.dbCtx.SaveChangesAsync();

            var questionDto = EntityToDTO(entity);
            questionDto.Votes = await this.dbCtx.Votes
                .Where(v => v.QuestionId == questionId)
                .Select(v => v.UserId)
                .CountAsync();
            questionDto.DidVote = false;

            return questionDto;
        }
    }
}