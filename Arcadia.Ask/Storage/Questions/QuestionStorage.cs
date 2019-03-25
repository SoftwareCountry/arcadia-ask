namespace Arcadia.Ask.Storage.Questions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Exceptions;
    using Microsoft.EntityFrameworkCore;
    using Models.DTO;
    using Models.Entities;

    public class QuestionStorage : IQuestionStorage
    {
        private readonly DatabaseContext dbCtx;

        public QuestionStorage(DatabaseContext dbCtx)
        {
            this.dbCtx = dbCtx;
        }

        public async Task<IEnumerable<QuestionDto>> GetQuestionsForSpecificUser(Guid userId)
        {
            var questions = await dbCtx.Questions.Include(q => q.Votes).ToListAsync();
            return questions.Select(q => EntityToDto(q, userId));
        }

        public async Task<QuestionDto> GetQuestionForSpecificUser(Guid questionId, Guid userId)
        {
            var foundQuestion = await FindQuestionEntityByIdAsync(questionId);

            if (foundQuestion == null)
                ThrowQuestionNotFound(questionId);

            return EntityToDto(foundQuestion, userId);
        }

        public async Task<QuestionDto> UpsertQuestion(QuestionDto question)
        {
            var entity = new QuestionEntity
            {
                QuestionId = Guid.NewGuid(),
                Author = question.Author,
                IsApproved = question.IsApproved,
                PostedAt = question.PostedAt,
                Text = question.Text
            };

            await dbCtx.Questions.AddAsync(entity);
            await dbCtx.SaveChangesAsync();

            return EntityToDto(entity);
        }

        public async Task DeleteQuestion(Guid questionId)
        {
            var entity = await FindQuestionEntityByIdAsync(questionId);
            if (entity == null)
                ThrowQuestionNotFound(questionId);

            dbCtx.Questions.Remove(entity);
            await dbCtx.SaveChangesAsync();
        }

        public async Task<QuestionDto> ApproveQuestion(Guid questionId)
        {
            var entity = await FindQuestionEntityByIdAsync(questionId);
            if (entity == null)
                ThrowQuestionNotFound(questionId);

            entity.IsApproved = true;
            await dbCtx.SaveChangesAsync();
            return EntityToDto(entity);
        }

        public async Task<QuestionDto> UpvoteQuestion(Guid questionId, Guid userId)
        {
            var entity = await FindQuestionEntityByIdAsync(questionId);
            if (entity == null)
                ThrowQuestionNotFound(questionId);

            if (await dbCtx.Votes.Where(v => v.QuestionId == questionId && v.UserId == userId).FirstOrDefaultAsync() != null)
                ThrowQuestionUpvoted(questionId);

            await dbCtx.Votes.AddAsync(new VoteEntity { QuestionId = questionId, UserId = userId });
            await dbCtx.SaveChangesAsync();

            var questionDto = EntityToDto(entity);
            questionDto.Votes = await dbCtx.Votes
                .Where(v => v.QuestionId == questionId)
                .Select(v => v.UserId)
                .CountAsync();
            questionDto.DidVote = true;

            return questionDto;
        }

        public async Task<QuestionDto> DownvoteQuestion(Guid questionId, Guid userId)
        {
            var entity = await FindQuestionEntityByIdAsync(questionId);
            if (entity == null)
                ThrowQuestionNotFound(questionId);

            var voteEntity = await dbCtx.Votes
                .Where(v => v.QuestionId == questionId && v.UserId == userId)
                .FirstOrDefaultAsync();
            if (voteEntity == null)
                return EntityToDto(entity);

            dbCtx.Remove(voteEntity);
            await dbCtx.SaveChangesAsync();

            var questionDto = EntityToDto(entity);
            questionDto.Votes = await dbCtx.Votes
                .Where(v => v.QuestionId == questionId)
                .Select(v => v.UserId)
                .CountAsync();
            questionDto.DidVote = false;

            return questionDto;
        }

        private QuestionDto EntityToDto(QuestionEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return new QuestionDto
            {
                Author = entity.Author,
                IsApproved = entity.IsApproved,
                PostedAt = entity.PostedAt,
                QuestionId = entity.QuestionId,
                Text = entity.Text,
                Votes = entity.Votes?.Count ?? 0

            };
        }

        private QuestionDto EntityToDto(QuestionEntity entity, Guid userId)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return new QuestionDto
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
            return await dbCtx.Questions
                .Where(q => q.QuestionId == questionId)
                .Include(q => q.Votes)
                .FirstOrDefaultAsync();
        }

        private void ThrowQuestionNotFound(Guid questionId)
        {
            throw new QuestionNotFoundException(questionId);
        }

        private void ThrowQuestionUpvoted(Guid questionId)
        {
            throw new QuestionUpvotedException(questionId);
        }
    }
}