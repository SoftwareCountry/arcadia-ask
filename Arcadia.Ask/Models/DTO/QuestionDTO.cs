using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcadia.Ask.Models.DTO
{
    public class QuestionDTO
    {
        public Guid QuestionId { get; set; }

        public string Text { get; set; }

        public string Author { get; set; }

        public DateTimeOffset PostedAt { get; set; }

        public bool IsApproved { get; set; }

        public IEnumerable<Guid> Votes { get; set; }
    }
}
