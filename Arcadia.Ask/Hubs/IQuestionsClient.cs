namespace Arcadia.Ask.Hubs
{
    using System.Threading.Tasks;

    using Arcadia.Ask.Questions;

    public interface IQuestionsClient
    {
        Task QuestionIsChanged(Question question);
    }
}