using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwitterApp.Data;
using TwitterApp.Dtos;
using TwitterApp.Models;
using TwitterApp.Repository;

namespace TwitterApp.Controllers
{
    [Route("api/v1.0/tweets/")]
    [ApiController]
    public class TweetsController : ControllerBase
    {
        private readonly ITweetRepository _tweetRepository;

        public TweetsController(ITweetRepository tweetRepository)
        {
            _tweetRepository = tweetRepository;
        }

        // GET: api/Tweets
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Tweet>>> GetTweets()
        {
            return Ok(await _tweetRepository.GetAllTweets());
        }

        // GET: api/Tweets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tweet>> GetTweet(int id)
        {

            var response = await _tweetRepository.GetTweetById(id);
            if (response.Data == null)
            {
                response.Success = false;
                response.Message = "Record Not Found!";
                return NotFound(response);
            }

            return Ok(response);
        }

        // PUT: api/Tweets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTweet(int id, UpdateTweetDto updateTweetDto)
        {
            var response = await _tweetRepository.UpdateTweet(id,updateTweetDto);
            if (response.Data == null)
            {
                response.Success = false;
                response.Message = "Record Not Found!";
                return NotFound(response);
            }
            return Ok(response);
        }

        // POST: api/Tweets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tweet>> PostTweet(AddTweetDto tweet)
        {
            var response = await _tweetRepository.AddTweet(tweet);
            if (response.Data == null)
            {
                response.Success = false;
                response.Message = "Record Not Found!";
                return NotFound(response);
            }
            return Ok(response);
        }

        // DELETE: api/Tweets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTweet(int id)
        {
            var response = await _tweetRepository.DeleteTweet(id);
            if (response.Data == null)
            {
                response.Success = false;
                response.Message = "Record Not Found!";
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPut("like/{id}")]
        public async Task<IActionResult> LikeTweet(int id)
        {
            var response = await _tweetRepository.LikeTweet(id);
            if (response.Data == null)
            {
                response.Success = false;
                response.Message = "Record Not Found!";
                return NotFound(response);
            }
            return Ok(response);
        }

        // POST: api/Tweets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("reply/{id}")]
        public async Task<ActionResult<Tweet>> PostTweet(int id ,AddReplyTweetDto addReplyTweetDto)
        {
            var response = await _tweetRepository.AddReplyTweet(id,addReplyTweetDto);
            if (response.Data == null)
            {
                response.Success = false;
                response.Message = "Record Not Found!";
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
