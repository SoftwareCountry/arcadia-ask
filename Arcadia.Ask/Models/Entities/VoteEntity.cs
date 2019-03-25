namespace Arcadia.Ask.Models.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class VoteEntity
    {
        [ForeignKey("Question")]
        public Guid QuestionId { get; set; }

        public Guid UserId { get; set; }

        public virtual QuestionEntity Question { get; set; }
    }
}