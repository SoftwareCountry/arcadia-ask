namespace Arcadia.Ask.Questions
{
    using System;

    public class Question
    {
        public string QuestionId { get; set; }

        public string Text { get; set; }

        public string Author { get; set; }

        public DateTimeOffset PostedAt { get; set; }

        public bool IsApproved { get; set; }

        public int Votes { get; set; }
    }
}