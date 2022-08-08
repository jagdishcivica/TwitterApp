using AutoMapper;
using TwitterApp.Dtos;
using TwitterApp.Models;

namespace TwitterApp
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Tweet, GetTweetDto>();
            CreateMap<AddTweetDto, Tweet>();
            CreateMap<UpdateTweetDto, Tweet>();
            CreateMap<User, UserListDto>();
            CreateMap<AddReplyTweetDto, TweetReply>();
        }
    }
}
