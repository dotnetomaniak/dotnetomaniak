namespace Kigg.DomainObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Infrastructure;
    using Repository;

    public partial class User : IUser
    {
        public decimal CurrentScore
        {
            get
            {
                return GetScoreBetween(Constants.ProductionDate, SystemTime.Now());
            }
        }

        public ICollection<ITag> Tags
        {
            get
            {
                return UserTags.Select(ut => ut.Tag).OrderBy(t => t.Name).Cast<ITag>().ToList().AsReadOnly();
            }
        }

        public int TagCount
        {
            get
            {
                return UserTags.Count();
            }
        }

        public virtual void ChangeEmail(string email)
        {
            Check.Argument.IsNotInvalidEmail(email, "email");
            Check.Argument.IsNotOutOfLength(email, 256, "email");

            IUser sameEmailUser = IoC.Resolve<IUserRepository>().FindByEmail(email.Trim());

            if ((sameEmailUser != null) && (Id != sameEmailUser.Id))
            {
                throw new InvalidOperationException("U¿ytkownik z tym samym adresem e-mail ju¿ istnieje.");
            }

            Email = email.ToLowerInvariant();
            LastActivityAt = SystemTime.Now();
        }

        public virtual void ChangePassword(string oldPassword, string newPassword)
        {
            if (this.IsOpenIDAccount())
            {
                throw new InvalidOperationException("Konto Open ID nie umo¿liwia zmiany has³a. U¿yj strony swojego dostawcy Open ID.");
            }

            Check.Argument.IsNotEmpty(oldPassword, "oldPassword");
            Check.Argument.IsNotEmpty(newPassword, "newPassword");
            Check.Argument.IsNotOutOfLength(newPassword, 64, "password");

            if (string.Compare(Password, oldPassword.Trim().Hash(), StringComparison.OrdinalIgnoreCase) != 0)
            {
                throw new InvalidOperationException("Stare has³o nie pasuje do obecnie u¿ywanego.");
            }

            Password = newPassword.Trim().Hash();
            LastActivityAt = SystemTime.Now();
        }

        public virtual string ResetPassword()
        {
            if (this.IsOpenIDAccount())
            {
                throw new InvalidOperationException("Konto Open ID nie pozwala na resetowanie has³a. U¿yj strony swojego dostawcy Open ID aby odzyskaæ has³o.");
            }

            string password = CreateRandomString(6, 8);

            Password = password.Hash();

            return password;
        }

        public virtual void Lock()
        {
            IsLockedOut = true;
        }

        public virtual void Unlock()
        {
            IsLockedOut = false;
        }

        public virtual decimal GetScoreBetween(DateTime startTimestamp, DateTime endTimestamp)
        {
            Check.Argument.IsNotInFuture(startTimestamp, "startTimestamp");
            Check.Argument.IsNotInFuture(endTimestamp, "endTimestamp");

            return IoC.Resolve<IUserRepository>().FindScoreById(Id, startTimestamp, endTimestamp);
        }

        public virtual void IncreaseScoreBy(decimal score, UserAction reason)
        {
            Check.Argument.IsNotNegativeOrZero(score, "score");

            AddScore(score, reason);
        }

        public virtual void DecreaseScoreBy(decimal score, UserAction reason)
        {
            Check.Argument.IsNotNegativeOrZero(score, "score");

            AddScore(-score, reason);
        }

        public virtual void AddTag(ITag tag)
        {
            Check.Argument.IsNotNull(tag, "tag");
            Check.Argument.IsNotEmpty(tag.Id, "tag.Id");
            Check.Argument.IsNotEmpty(tag.Name, "tag.Name");

            if (!ContainsTag(tag))
            {
                UserTags.Add(new UserTag { Tag = (Tag) tag });
            }
        }

        public virtual void RemoveTag(ITag tag)
        {
            Check.Argument.IsNotNull(tag, "tag");
            Check.Argument.IsNotEmpty(tag.Name, "tag.Name");

            UserTags.Remove(UserTags.SingleOrDefault(st => st.Tag.Name == tag.Name));
        }

        public virtual void RemoveAllTags()
        {
            UserTags.Clear();
        }

        public virtual bool ContainsTag(ITag tag)
        {
            Check.Argument.IsNotNull(tag, "tag");
            Check.Argument.IsNotEmpty(tag.Name, "tag.Name");

            return UserTags.Any(ut => ut.Tag.Name == tag.Name);
        }

        private static string CreateRandomString(int minLegth, int maxLength)
        {
            const string Characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!@#$&";
            Random rnd = new Random();

            int length = rnd.Next(minLegth, maxLength);
            char[] result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = Characters[rnd.Next(0, Characters.Length)];
            }

            return new string(result);
        }

        private void AddScore(decimal score, UserAction reason)
        {
            UserScore userScore = new UserScore
                                      {
                                          Timestamp = SystemTime.Now(),
                                          Score = score,
                                          ActionType = reason,
                                      };

            UserScores.Add(userScore);
        }
    }
}