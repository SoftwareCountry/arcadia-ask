namespace Arcadia.Ask.Storage.Questions
{
    using Arcadia.Ask.Models.DTO;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IQuestionStorage
    {
        Task<IEnumerable<QuestionDTO>> GetQuestions();

        Task<QuestionDTO> GetQuestion(Guid questionId);
        
        Task<QuestionDTO> UpsertQuestion(QuestionDTO question);

        Task DeleteQuestion(Guid questionId);

        Task<QuestionDTO> ApproveQuestion(Guid questionId);

        Task<QuestionDTO> UpvoteQuestion(Guid questionId, Guid userId);

        Task<QuestionDTO> DownvoteQuestion(Guid questionId, Guid userId);
    }
}