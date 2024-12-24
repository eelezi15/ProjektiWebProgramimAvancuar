namespace ProjektiWebProgramimAvancuar.Models
{
    public class Comment
    {
        public Guid CommentId { get; set; }
        public string Title { get; set; }

        public Guid PostId { get; set; }

        public string Name { get; set; }

        public string? Email { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
