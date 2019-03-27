﻿namespace Arcadia.Ask.Models.DTO
{
    using System;

    public class QuestionDto
    {
        public Guid QuestionId { get; set; }

        public string Text { get; set; }

        public string Author { get; set; }

        public DateTimeOffset PostedAt { get; set; }

        public bool IsApproved { get; set; }

        public int Votes { get; set; }

        public bool DidVote { get; set; }
    }
}