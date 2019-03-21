using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcadia.Ask.Models.Entities
{
    public class VoteEntity
    {
        public Guid QuestionId { get; set; }

        public Guid UserId { get; set; }

        public virtual QuestionEntity Question { get; set; }
    }
}
