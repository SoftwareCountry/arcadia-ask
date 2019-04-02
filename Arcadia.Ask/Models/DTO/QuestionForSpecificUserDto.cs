namespace Arcadia.Ask.Models.DTO
{
    public class QuestionForSpecificUserDto
    {
        public QuestionDto Question { get; set; }

        public bool DidVote { get; set; }
    }
}