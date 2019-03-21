using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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
