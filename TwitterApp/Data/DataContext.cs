using Microsoft.EntityFrameworkCore;
using TwitterApp.Models;

namespace TwitterApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {


        }
        public DbSet<User> Users { get; set; }
        public DbSet<Tweet> Tweets { get; set; }
        public DbSet<TweetReply> TweetsReply { get; set; }
    }
}
