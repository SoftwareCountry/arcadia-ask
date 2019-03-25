﻿namespace Arcadia.Ask.Storage.Questions
{
    using Arcadia.Ask.Models.DTO;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IQuestionStorage
    {
        Task<IEnumerable<QuestionDto>> GetQuestions();

        Task<QuestionDto> GetQuestion(Guid questionId);
        
        Task<QuestionDto> UpsertQuestion(QuestionDto question);

        Task DeleteQuestion(Guid questionId);

        Task<QuestionDto> ApproveQuestion(Guid questionId);

        Task<QuestionDto> UpvoteQuestion(Guid questionId, Guid userId);

        Task<QuestionDto> DownvoteQuestion(Guid questionId, Guid userId);
    }
}