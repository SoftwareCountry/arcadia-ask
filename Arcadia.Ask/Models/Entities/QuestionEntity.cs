using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Arcadia.Ask.Models.Entities
{
    public class QuestionEntity
    {
        [Key]
        public Guid QuestionId { get; set; }

        public string Text { get; set; }

        public string Author { get; set; }

        public DateTimeOffset PostedAt { get; set; }

        public bool IsApproved { get; set; }

        public virtual ICollection<VoteEntity> Votes { get; set; }
    }
}
