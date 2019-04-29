using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Kigg.Infrastructure.EF.POCO
{
    public class Story
    {
        public Guid Id { get; set; }

        public string UniqueName { get; set; }

        public string Title { get; set; }

        public System.Data.Linq.Link<string> HtmlDescription { get; set; }

        public string TextDescription { get; set; }

        public string Url { get; set; }

        public string UrlHash { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public string IPAddress { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastActivityAt { get; set; }

        public DateTime? ApprovedAt { get; set; }

        public DateTime? PublishedAt { get; set; }

        public int? Rank { get; set; }

        public DateTime? LastProcessedAt { get; set; }

        public ICollection<CommentSubscribtion> CommentSubscribtions { get; set; }

        public ICollection<StoryComment> StoryComments { get; set; }

        public ICollection<StoryMarkAsSpam> StoryMarkAsSpams { get; set; }

        public ICollection<StoryTag> StoryTags { get; set; }

        public ICollection<StoryView> StoryViews { get; set; }

        public ICollection<StoryVote> StoryVotes { get; set; }
    }
}