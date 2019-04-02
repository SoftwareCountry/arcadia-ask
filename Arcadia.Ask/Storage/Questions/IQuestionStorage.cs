namespace Arcadia.Ask.Storage.Questions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Arcadia.Ask.Models.Entities;

    public interface IQuestionStorage
    {
        Task<IEnumerable<QuestionEntity>> GetQuestions();

        Task<QuestionEntity> GetQuestion(Guid questionId);

        Task<QuestionEntity> UpsertQuestion(QuestionEntity question);

        Task DeleteQuestion(Guid questionId);

        Task<QuestionEntity> ApproveQuestion(Guid questionId);

        Task<QuestionEntity> UpvoteQuestion(Guid questionId, Guid userId);

        Task<QuestionEntity> DownvoteQuestion(Guid questionId, Guid userId);
    }
}