namespace Kigg.DomainObjects
{
    using System;
    using System.Diagnostics;

    using Infrastructure;

    public static class UserExtension
    {
        [DebuggerStepThrough]
        public static bool HasDefaultOpenIDEmail(this IUser user)
        {
            return user.IsOpenIDAccount() && (string.Compare(IoC.Resolve<IConfigurationSettings>().DefaultEmailOfOpenIdUser, user.Email, StringComparison.OrdinalIgnoreCase) == 0);
        }

        [DebuggerStepThrough]
        public static bool IsOpenIDAccount(this IUser user)
        {
            return string.IsNullOrEmpty(user.Password);
        }

        [DebuggerStepThrough]
        public static bool IsAdministrator(this IUser user)
        {
            return HasRole(user, Roles.Administrator);
        }

        [DebuggerStepThrough]
        public static bool IsModerator(this IUser user)
        {
            return HasRole(user, Roles.Moderator);
        }

        [DebuggerStepThrough]
        public static bool IsBot(this IUser user)
        {
            return HasRole(user, Roles.Bot);
        }

        [DebuggerStepThrough]
        public static bool IsPublicUser(this IUser user)
        {
            return !IsBot(user) && !IsModerator(user) && !IsAdministrator(user);
        }

        [DebuggerStepThrough]
        public static bool CanModerate(this IUser user)
        {
            return IsAdministrator(user) || IsModerator(user);
        }

        [DebuggerStepThrough]
        public static bool ShouldHideCaptcha(this IUser user)
        {
            return (user != null) && (!IsPublicUser(user) || (user.CurrentScore > IoC.Resolve<IConfigurationSettings>().MaximumUserScoreToShowCaptcha));
        }

        [DebuggerStepThrough]
        private static bool HasRole(IUser user, Roles role)
        {
            return (user.Role & role) == role;
        }

        //[DebuggerStepThrough]
        public static bool HasRightsToEditStory(this IUser user, IStory story)
        {
            if (story == null || user == null) 
                return false;
            return user.CanModerate() || user.Id == story.PostedBy.Id
                    && SystemTime.Now() < story.CreatedAt.AddMinutes(20)
                    && story.IsPublished() == false;
        }
    }
}