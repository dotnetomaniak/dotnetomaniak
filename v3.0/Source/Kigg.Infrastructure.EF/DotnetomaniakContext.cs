using System;
using Kigg.Infrastructure.EF.DomainObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Kigg.Infrastructure.EF
{
    public class DotnetomaniakContext: DbContext
    {
        private readonly IConfigurationManager _configurationManager;

        public DotnetomaniakContext(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
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
            optionsBuilder.UseSqlServer(_configurationManager.GetConnectionString("dotnetomaniak"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.Relational().TableName = entityType.DisplayName();
            }

            modelBuilder.Entity<StoryTag>()
                .HasKey(st => new { st.StoryId, st.TagId });

            modelBuilder.Entity<StoryVote>()
                .HasKey(sv => new {sv.StoryId, sv.UserId});

            modelBuilder.Entity<UserAchievement>()
                .HasKey(ua => new {ua.UserId, ua.AchievementId});

            modelBuilder.Entity<UserTag>()
                .HasKey(ut => new {ut.UserId, ut.TagId});
        }

        //Stored Procedures
        public void _10kPoints()
        {
            this.Database.ExecuteSqlCommand("_10kPoints");
        }

        public void _1kPoints()
        {
            this.Database.ExecuteSqlCommand("_1kPoints");
        }

        public void Commenter()
        {
            this.Database.ExecuteSqlCommand("Commenter");
        }

        public void Dotnetomaniak()
        {
            this.Database.ExecuteSqlCommand("Dotnetomaniak");
        }

        public void EarlyBird()
        {
            this.Database.ExecuteSqlCommand("EarlyBird");
        }

        public void Facebook()
        {
            this.Database.ExecuteSqlCommand("Facebook");
        }

        public void Globetrotter()
        {
            this.Database.ExecuteSqlCommand("Globetrotter");
        }

        public void GoodStory()
        {
            this.Database.ExecuteSqlCommand("GoodStory");
        }

        public void GreatStory()
        {
            this.Database.ExecuteSqlCommand("GreatStory");
        }

        public void MultiAdder()
        {
            this.Database.ExecuteSqlCommand("MultiAdder");
        }

        public void NightOwl()
        {
            this.Database.ExecuteSqlCommand("NightOwl");
        }

        public void PopularStory()
        {
            this.Database.ExecuteSqlCommand("PopularStory");
        }

        public void StoryAdder()
        {
            this.Database.ExecuteSqlCommand("StoryAdder");
        }

        public void UpVoter()
        {
            this.Database.ExecuteSqlCommand("UpVoter");
        }
    }
}
