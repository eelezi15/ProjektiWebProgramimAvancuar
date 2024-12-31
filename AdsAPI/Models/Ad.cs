namespace AdsAPI.Models
{
    public class Ad
    {
        public Guid AdId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public string Link { get; set; }
    }
}
