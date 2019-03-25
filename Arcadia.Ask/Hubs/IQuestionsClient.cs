namespace Arcadia.Ask.Hubs
{
    using System;
    using System.Threading.Tasks;
    using Arcadia.Ask.Models.DTO;
    using Storage.Questions;

    public interface IQuestionsClient
    {
        Task QuestionIsChanged(QuestionDto question);

        Task QuestionIsRemoved(Guid questionId);
    }
}