namespace Arcadia.Ask.Questions
{
    using System;

    public class Question
    {
        public Guid QuestionId { get; }

        public string Text { get; }

        public string Author { get; }

        public DateTimeOffset PostedAt { get; }

        public bool IsApproved { get; private set; }

        public int Votes { get; private set; }

        public Question(Guid questionId, string text, string author, DateTimeOffset postedAt)
            : this(questionId, text, author, postedAt, false, 0)
        {
        }

        public Question(Guid questionId, string text, string author, DateTimeOffset postedAt, bool isApproved, int votes)
        {
            this.QuestionId = questionId;
            this.Text = text;
            this.Author = author;
            this.PostedAt = postedAt;
            this.IsApproved = isApproved;
            this.Votes = votes;
        }

        public Question Clone()
        {
            return new Question(
                this.QuestionId,
                this.Text,
                this.Author,
                this.PostedAt,
                this.IsApproved,
                this.Votes);
        }

        public Question Approve()
        {
            var newValue = this.Clone();
            newValue.IsApproved = true;
            return newValue;
        }

        public Question ChangeVotes(int diff)
        {
            var newValue = this.Clone();
            newValue.Votes += diff;
            return newValue;
        }
    }
}