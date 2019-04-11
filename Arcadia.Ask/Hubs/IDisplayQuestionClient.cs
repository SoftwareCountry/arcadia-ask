namespace Arcadia.Ask.Hubs
{
    using System;
    using System.Threading.Tasks;

    using Arcadia.Ask.Models.DTO;

    public interface IDisplayQuestionClient
    {
        Task DisplayedQuestionChanged(QuestionDto question);

        Task DisplayedQuestionHidden();
    }
}