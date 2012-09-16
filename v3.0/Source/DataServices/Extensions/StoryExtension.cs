using System.Linq;
namespace Kigg.DataServices
{
    using DomainObjects;
    using Data;
    using System.Text;
    
    internal static class StoryExtension
    {
        internal static ShoutedStory Convert(this IStory story)
        {
            var shouted = new ShoutedStory
                              {
                                  Url = story.Url,
                                  Title = story.Title,
                                  Category = story.BelongsTo.Name,
                                  Description = story.TextDescription,
                                  ShoutCount = story.VoteCount,
                                  CommentCount = story.CommentCount,
                                  CreatedOn = story.CreatedAt,
                                  PublishedOn = story.PublishedAt,
                                  User = new ShoutUser {UserName = story.PostedBy.UserName},
                              };
            if (story.Tags != null)
            {
                var tags = story.Tags.Select(t => t.Name);
                shouted.Tags = tags.ToArray().Split(',');
            }
            return shouted;
        }

        private static string Split(this string[] array, char separator)
        {
            if(array != null)
            {
                var splittedTags = new StringBuilder(256);

                for (var i = 0; i < array.Length; i++)
                {
                    if (i != array.Length - 1)
                    {
                        splittedTags.AppendFormat("{0}{1}", array[i], separator);
                    }
                    else
                    {
                        splittedTags.Append(array[i]);
                    }
                }
                return splittedTags.ToString();
            }
            return string.Empty;
        }
    }
}
