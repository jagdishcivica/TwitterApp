namespace TwitterApp.Dtos
{
    public class GetTweetDto
    {
        public string TweetContent { get; set; }
        public string Tags { get; set; }
        public int LikeCount { get; set; }
    }
}
