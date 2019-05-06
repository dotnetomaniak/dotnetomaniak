using System;
using Kigg.Infrastructure.EF.POCO;
using Microsoft.EntityFrameworkCore;

namespace Kigg.Infrastructure.EF
{
    public class DotnetomaniakContext: DbContext
    {
        public DotnetomaniakContext(DbContextOptions<DotnetomaniakContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CommentSubscribtion> CommentSubscribtions { get; set; }
        public DbSet<CommingEvent> CommingEvents { get; set; }
        public DbSet<KnownSource> KnownSources { get; set; }
        public DbSet<Recommendation> Recommendations { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<StoryComment> StoryComments { get; set; }
        public DbSet<StoryMarkAsSpam> StoryMarkAsSpams { get; set; }
        public DbSet<StoryTag> StoryTags { get; set; }
        public DbSet<StoryView> StoryViews { get; set; }
        public DbSet<StoryVote> StoryVotes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAchievement> UserAchievements { get; set; }
        public DbSet<UserScore> UserScores { get; set; }
        public DbSet<UserTag> UserTags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
