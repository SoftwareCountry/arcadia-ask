namespace Arcadia.Ask.Hubs
{
    using System;
    using System.Threading.Tasks;
    using Arcadia.Ask.Models.DTO;
    using Arcadia.Ask.Questions;

    public interface IQuestionsClient
    {
        Task QuestionIsChanged(QuestionDTO question);

        Task QuestionIsRemoved(Guid questionId);
    }
}