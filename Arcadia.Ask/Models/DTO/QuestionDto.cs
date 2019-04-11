namespace Arcadia.Ask.Models.DTO
{
    using System;

    using Arcadia.Ask.Models.Entities;

    public class QuestionDto
    {
        public Guid QuestionId { get; set; }

        public string Text { get; set; }

        public string Author { get; set; }

        public DateTimeOffset PostedAt { get; set; }

        public bool IsApproved { get; set; }

        public int Votes { get; set; }

        public QuestionDto(QuestionEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            this.QuestionId = entity.QuestionId;
            this.Author = entity.Author;
            this.Text = entity.Text;
            this.PostedAt = entity.PostedAt;
            this.IsApproved = entity.IsApproved;
            this.Votes = entity.Votes?.Count ?? 0;
        }
    }
}