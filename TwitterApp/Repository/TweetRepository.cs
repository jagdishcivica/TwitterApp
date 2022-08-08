using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TwitterApp.Data;
using TwitterApp.Dtos;
using TwitterApp.Models;

namespace TwitterApp.Repository
{
    public class TweetRepository : ITweetRepository
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public TweetRepository(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public DataContext DataContext { get; }

        public async Task<ServiceResponse<List<GetTweetDto>>> AddReplyTweet(int id, AddReplyTweetDto addReplyTweetDto)
        {
            var response = new ServiceResponse<List<GetTweetDto>>();
            var tweet = await _context.Tweets.FirstOrDefaultAsync(x => x.Id == id);
            TweetReply tweetReply = _mapper.Map<TweetReply>(addReplyTweetDto);
            tweetReply.Tweet = tweet;
            _context.TweetsReply.Add(tweetReply);
            await _context.SaveChangesAsync();
            response.Data = await _context.Tweets.Select(c => _mapper.Map<GetTweetDto>(c)).ToListAsync();
            response.Message = "New reply to tweet added successfully";
            return response;
        }

        public async Task<ServiceResponse<List<GetTweetDto>>> AddTweet(AddTweetDto addTweetDto)
        {
            var response = new ServiceResponse<List<GetTweetDto>>();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == addTweetDto.UserId);
            Tweet tweet = _mapper.Map<Tweet>(addTweetDto);
            tweet.User = user;
            _context.Tweets.Add(tweet);
            await _context.SaveChangesAsync();
            response.Data = await _context.Tweets.Select(c => _mapper.Map<GetTweetDto>(c)).ToListAsync();
            response.Message = "New tweet added successfully";
            return response;
        }

        public async Task<ServiceResponse<List<GetTweetDto>>> DeleteTweet(int id)
        {
            ServiceResponse<List<GetTweetDto>> response = new ServiceResponse<List<GetTweetDto>>();
            try
            {
                Tweet tweet = await _context.Tweets.FirstOrDefaultAsync(c => c.Id == id);
                _context.Tweets.Remove(tweet);
                await _context.SaveChangesAsync();
                response.Data = await _context.Tweets.Select(c => _mapper.Map<GetTweetDto>(c)).ToListAsync();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<List<GetTweetDto>>> GetAllTweets()
        {
            ServiceResponse<List<GetTweetDto>> serviceResponse = new ServiceResponse<List<GetTweetDto>>();
            serviceResponse.Data = await _context.Tweets.Select(x => _mapper.Map<GetTweetDto>(x)).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetTweetDto>> GetTweetById(int id)
        {
            var response = new ServiceResponse<GetTweetDto>();
            var dbTweets = await _context.Tweets.FirstOrDefaultAsync(c => c.Id == id);
            response.Data = _mapper.Map<GetTweetDto>(dbTweets);
            return response;
        }

        public async Task<ServiceResponse<List<GetTweetDto>>> LikeTweet(int id)
        {
            ServiceResponse<List<GetTweetDto>> response = new ServiceResponse<List<GetTweetDto>>();
            try
            {
                Tweet tweet = await _context.Tweets.FirstOrDefaultAsync(c => c.Id == id);
                Tweet tweetToUpdate = await _context.Tweets.FirstOrDefaultAsync(c => c.Id == id);
                ++tweetToUpdate.LikeCount;
                _mapper.Map(tweetToUpdate, tweet);
                await _context.SaveChangesAsync();

                response.Data = await _context.Tweets.Select(c => _mapper.Map<GetTweetDto>(c)).ToListAsync();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<GetTweetDto>> UpdateTweet(int id, UpdateTweetDto updateTweetDto)
        {
            ServiceResponse<GetTweetDto> serviceResponse = new ServiceResponse<GetTweetDto>();
            try
            {
                Tweet tweet = await _context.Tweets.FirstOrDefaultAsync(c => c.Id == id);
                _mapper.Map(updateTweetDto, tweet);
                await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetTweetDto>(tweet);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
    }
}
