namespace TwitterApp.Models
{
    public class Tweet
    {
        public int Id { get; set; }
        public string TweetContent { get; set; }
        public string Tags { get; set; }
        public int LikeCount { get; set; }
        public User? User { get; set; }
    }
}
