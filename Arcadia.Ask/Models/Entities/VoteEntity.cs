using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arcadia.Ask.Models.Entities
{
    public class VoteEntity
    {
        [ForeignKey("Question")]
        public Guid QuestionId { get; set; }

        public Guid UserId { get; set; }

        public virtual QuestionEntity Question { get; set; }
    }
}
