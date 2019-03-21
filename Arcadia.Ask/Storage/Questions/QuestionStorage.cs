﻿namespace Arcadia.Ask.Questions
{
    using Arcadia.Ask.Models.DTO;
    using Arcadia.Ask.Models.Entities;
    using Arcadia.Ask.Storage;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class QuestionStorage : IQuestionStorage
    {
        private readonly DatabaseContext _dbCtx;

        public QuestionStorage(DatabaseContext dbCtx)
        {
            this._dbCtx = dbCtx;
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
            };
        }

        private async Task<QuestionEntity> FindQuestionEntityByIdAsync(Guid questionId)
        {
            return await this._dbCtx.Questions
                .Where(q => q.QuestionId == questionId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<QuestionDTO>> GetQuestions()
        {
            var questions = await this._dbCtx.Questions.ToListAsync();
            return questions.Select(q => this.EntityToDTO(q));
        }

        public async Task<QuestionDTO> GetQuestion(Guid questionId)
        {
            var foundQuestion = await this.FindQuestionEntityByIdAsync(questionId);

            if (foundQuestion == null)
                //TODO: add custom exception
                ThrowQuestionNotFound(questionId);

            return this.EntityToDTO(foundQuestion);
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

            await this._dbCtx.Questions.AddAsync(entity);
            await this._dbCtx.SaveChangesAsync();

            return this.EntityToDTO(entity);
        }

        public async Task DeleteQuestion(Guid questionId)
        {
            var entity = await this.FindQuestionEntityByIdAsync(questionId);
            if (entity == null)
                ThrowQuestionNotFound(questionId);

            this._dbCtx.Questions.Remove(entity);
            await this._dbCtx.SaveChangesAsync();
        }

        private void ThrowQuestionNotFound(Guid _) => throw new InvalidOperationException("No question found");

        public async Task<QuestionDTO> ApproveQuestion(Guid questionId)
        {
            var entity = await this.FindQuestionEntityByIdAsync(questionId);
            if (entity == null)
                this.ThrowQuestionNotFound(questionId);

            entity.IsApproved = true;
            await this._dbCtx.SaveChangesAsync();
            return EntityToDTO(entity);
        }

        public async Task<QuestionDTO> ChangeVotes(Guid questionId, int diff)
        {
            var entity = await FindQuestionEntityByIdAsync(questionId);
            if (entity == null)
                this.ThrowQuestionNotFound(questionId);

            await this._dbCtx.Votes.AddAsync(new VoteEntity() { QuestionId = questionId, UserId = Guid.NewGuid() });
            await this._dbCtx.SaveChangesAsync();

            var questionDto = EntityToDTO(entity);
            questionDto.Votes = await this._dbCtx.Votes
                .Where(v => v.QuestionId == questionId)
                .Select(v => v.UserId)
                .ToListAsync();

            return questionDto;
        }
    }
}