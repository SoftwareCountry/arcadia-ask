namespace Arcadia.Ask.Hubs
{
    using System;
    using System.Threading.Tasks;

    using Arcadia.Ask.Questions;

    public interface IQuestionsClient
    {
        Task QuestionIsChanged(Question question);

        Task QuestionIsRemoved(Guid questionId);
    }
}