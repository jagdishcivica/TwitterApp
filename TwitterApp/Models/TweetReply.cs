namespace TwitterApp.Models
{
    public class TweetReply
    {   
        public int Id { get; set; }
        public Tweet Tweet { get; set; }
        public string Reply { get; set; }
        public int UserId { get; set; }
        public int LikeCount { get; set; }
    }
}
