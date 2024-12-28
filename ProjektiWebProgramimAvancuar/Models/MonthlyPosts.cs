namespace ProjektiWebProgramimAvancuar.Models
{
    public class MonthlyPosts
    {
        public string Month { get; set; } = "December";
        public List<PostSummary> Posts { get; set; } = new List<PostSummary>();

    }
    public class PostSummary
    {
        public Guid PostId { get; set; }
        public string Title { get; set; }

        public int ViewCount { get; set; }  
    }
}
