namespace Arcadia.Ask.Questions
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

        Task<QuestionDTO> ChangeVotes(Guid questionId, int diff);
    }
}