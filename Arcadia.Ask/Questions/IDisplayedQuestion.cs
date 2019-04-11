namespace Arcadia.Ask.Questions
{
    using System;

    public interface IDisplayedQuestion
    {
        Guid CurrentDisplayedQuestionId { get; set; }
    }
}