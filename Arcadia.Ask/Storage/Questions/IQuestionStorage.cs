﻿namespace Arcadia.Ask.Questions
{
    using Arcadia.Ask.Models.DTO;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IQuestionStorage
    {
        Task<IEnumerable<QuestionDTO>> GetQuestionsForSpecificUser(Guid userId);

        Task<QuestionDTO> GetQuestionForSpecificUser(Guid questionId, Guid userId);
        
        Task<QuestionDTO> UpsertQuestion(QuestionDTO question);

        Task DeleteQuestion(Guid questionId);

        Task<QuestionDTO> ApproveQuestion(Guid questionId);

        Task<QuestionDTO> UpvoteQuestion(Guid questionId, Guid userId);

        Task<QuestionDTO> DownvoteQuestion(Guid questionId, Guid userId);
    }
}