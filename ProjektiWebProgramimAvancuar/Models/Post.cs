namespace ProjektiWebProgramimAvancuar.Models
{
    public class Post
    {
        public Guid PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public Guid UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int ViewCount { get; set; }

        public virtual User? User { get; set; }  
        public virtual ICollection<Comment>? Comments { get; set; }
    }
}
