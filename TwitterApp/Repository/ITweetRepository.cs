using TwitterApp.Dtos;
using TwitterApp.Models;

namespace TwitterApp.Repository
{
    public interface ITweetRepository
    {
        public Task<ServiceResponse<List<GetTweetDto>>> AddTweet(AddTweetDto addTweetDto);
        public Task<ServiceResponse<List<GetTweetDto>>> GetAllTweets();
        public Task<ServiceResponse<GetTweetDto>> GetTweetById(int id);
        public Task<ServiceResponse<GetTweetDto>> UpdateTweet(int id,UpdateTweetDto updateTweetDto);
        public Task<ServiceResponse<List<GetTweetDto>>> DeleteTweet(int id);
        public Task<ServiceResponse<List<GetTweetDto>>> LikeTweet(int id);

        public Task<ServiceResponse<List<GetTweetDto>>> AddReplyTweet(int id,AddReplyTweetDto addReplyTweetDto);
    }
}
