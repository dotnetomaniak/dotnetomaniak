using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Kigg.DomainObjects;
using Kigg.Infrastructure.DomainRepositoryExtensions;

namespace Kigg.Infrastructure.EF.DomainObjects
{
    public class User: Entity, IUser
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }

        public bool IsLockedOut { get; set; }

        public Kigg.DomainObjects.Roles Role { get; set; }

        public System.DateTime LastActivityAt { get; set; }

        public string FbId { get; set; }

        public ICollection<UserTag> UserTags { get; set; }

        public ICollection<CommentSubscribtion> CommentSubscribtions { get; set; }

        public ICollection<Story> Stories { get; set; }

        public ICollection<StoryComment> StoryComments { get; set; }

        public ICollection<StoryMarkAsSpam> StoryMarkAsSpams { get; set; }

        public ICollection<StoryVote> StoryVotes { get; set; }

        public ICollection<UserScore> UserScores { get; set; }

        public ICollection<UserAchievement> UserAchievements { get; set; }

        [NotMapped]
        public ICollection<ITag> Tags => UserTags.Select(ut => ut.Tag).OrderBy(t => t.Name).Cast<ITag>().ToList().AsReadOnly();

        [NotMapped]
        public int TagCount => UserTags.Count;

        [NotMapped]
        public decimal CurrentScore => GetScoreBetween(CreatedAt, SystemTime.Now());

        [NotMapped]
        public PagedResult<IUserAchievement> Achievements => new PagedResult<IUserAchievement>(UserAchievements.Cast<IUserAchievement>().ToList(), UserAchievements.Count);

        [NotMapped]
        public PagedResult<IUserAchievement> NewAchievements {
            get
            {
                var list = UserAchievements.Where(a => a.Displayed == false).Cast<IUserAchievement>().ToList();
                return new PagedResult<IUserAchievement>(list, list.Count);
            }
        }

        public void ChangeEmail(string email)
        {
            Check.Argument.IsNotInvalidEmail(email, "email");
            Check.Argument.IsNotOutOfLength(email, 256, "email");

            if (!this.IsUniqueEmail(email))
            {
                throw new InvalidOperationException("User with the same email already exists.");
            }

            Email = email.ToLowerInvariant();
            LastActivityAt = SystemTime.Now();
        }

        public void ChangePassword(string oldPassword, string newPassword)
        {
            if (this.IsOpenIDAccount())
            {
                throw new InvalidOperationException("Open ID account does not support change password. Please use your Open ID provider.");
            }

            Check.Argument.IsNotEmpty(oldPassword, "oldPassword");
            Check.Argument.IsNotEmpty(newPassword, "newPassword");
            Check.Argument.IsNotOutOfLength(newPassword, 64, "password");

            if (string.Compare(Password, oldPassword.Trim().Hash(), StringComparison.OrdinalIgnoreCase) != 0)
            {
                throw new InvalidOperationException("Old password does not match with the current password.");
            }

            Password = newPassword.Trim().Hash();
            LastActivityAt = SystemTime.Now();
        }

        public string ResetPassword()
        {
            if (this.IsOpenIDAccount())
            {
                throw new InvalidOperationException("Open ID account does not support reset password. Please use your Open ID provider to recover your lost password.");
            }

            string password = CreateRandomString(6, 8);

            Password = password.Hash();

            return password;
        }

        public void Lock()
        {
            IsLockedOut = true;
        }

        public void Unlock()
        {
            IsLockedOut = false;
        }

        public decimal GetScoreBetween(DateTime startTimestamp, DateTime endTimestamp)
        {
            Check.Argument.IsNotInFuture(startTimestamp, "startTimestamp");
            Check.Argument.IsNotInFuture(endTimestamp, "endTimestamp");

            return this.GetScore(startTimestamp, endTimestamp);
        }

        public void IncreaseScoreBy(decimal score, UserAction reason)
        {
            Check.Argument.IsNotNegativeOrZero(score, "score");

            AddScore(score, reason);
        }

        public void DecreaseScoreBy(decimal score, UserAction reason)
        {
            Check.Argument.IsNotNegativeOrZero(score, "score");

            AddScore(-score, reason);
        }

        public void MarkAchievementsAsDisplayed()
        {
            foreach (var userAchievement in UserAchievements)
            {
                userAchievement.Displayed = true;
            }
        }

        public void AddTag(ITag tag)
        {
            Check.Argument.IsNotNull(tag, "tag");
            Check.Argument.IsNotEmpty(tag.Id, "tag.Id");
            Check.Argument.IsNotEmpty(tag.Name, "tag.Name");

            if (!ContainsTag(tag))
            {
                UserTags.Add(new UserTag { Tag = (Tag)tag });
            }
        }

        public void RemoveTag(ITag tag)
        {
            Check.Argument.IsNotNull(tag, "tag");
            Check.Argument.IsNotEmpty(tag.Name, "tag.Name");

            UserTags.Remove(UserTags.SingleOrDefault(st => st.Tag.Name == tag.Name));
        }

        public void RemoveAllTags()
        {
            UserTags.Clear();
        }

        public bool ContainsTag(ITag tag)
        {
            Check.Argument.IsNotNull(tag, "tag");
            Check.Argument.IsNotEmpty(tag.Name, "tag.Name");

            return UserTags.Any(ut => ut.Tag.Name == tag.Name);
        }

        private static string CreateRandomString(int minLegth, int maxLength)
        {
            const string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!@#$&";
            var rnd = new Random();

            int length = rnd.Next(minLegth, maxLength);
            var result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = characters[rnd.Next(0, characters.Length)];
            }

            return new string(result);
        }

        private void AddScore(decimal score, UserAction reason)
        {
            var userScore = new UserScore
            {
                Timestamp = SystemTime.Now(),
                Score = score,
                ActionType = reason,
            };

            UserScores.Add(userScore);
        }

        public void SetFbId(string fbId)
        {
            FbId = fbId;
        }
    }
}