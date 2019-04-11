namespace Arcadia.Ask.Hubs
{
    using System;
    using System.Threading.Tasks;

    public interface IDisplayQuestionClient
    {
        Task DisplayedQuestionChanged(Guid questionId);

        Task DisplayedQuestionHidden();
    }
}