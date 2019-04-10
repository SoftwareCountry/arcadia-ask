namespace Arcadia.Ask.Hubs
{
    using System;
    using System.Threading.Tasks;
    using Arcadia.Ask.Models.DTO;

    public interface IQuestionsClient
    {
        Task QuestionIsChanged(QuestionDto question);

        Task QuestionIsRemoved(Guid questionId);

        Task QuestionIsVoted(Guid questionId);
    }
}