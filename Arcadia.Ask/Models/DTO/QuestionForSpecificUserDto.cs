namespace Arcadia.Ask.Models.DTO
{
    public class QuestionForSpecificUserDto
    {
        public QuestionDto Metadata { get; set; }

        public bool DidVote { get; set; }
    }
}