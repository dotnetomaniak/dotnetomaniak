namespace Kigg.EF.Repository
{
    using System;
    using System.Linq;

    using DomainObjects;
   
    internal class UniqueNameGenerator
    {
        // we have to generate
        // Name     UniqueName
        // ====     ==========
        // C        C
        // C++      C-2
        // C#       C-3
        // And ForStory those which contains - (dash) we have to ignore it if it does not contain number after it
        // IoC/DI   IoC-DI
        // IoC      IoC

        public static string GenerateFrom(IQueryable<Category> dataSource, string target)
        {
            string legalUrlName = target.ToLegalUrl();

            string lastEntityUniqueName = dataSource.Where(e => (e.UniqueName == legalUrlName) || e.UniqueName.StartsWith(legalUrlName + "-"))
                                                    .OrderByDescending(e => e.CreatedAt)
                                                    .Select(e => e.UniqueName)
                                                    .FirstOrDefault();

            return Generate(legalUrlName, lastEntityUniqueName);
        }

        public static string GenerateFrom(IQueryable<Tag> dataSource, string target)
        {
            string legalUrlName = target.ToLegalUrl();

            string lastEntityUniqueName = dataSource.Where(e => (e.UniqueName == legalUrlName) || e.UniqueName.StartsWith(legalUrlName + "-"))
                                                    .OrderByDescending(e => e.CreatedAt)
                                                    .Select(e => e.UniqueName)
                                                    .FirstOrDefault();
            
            return Generate(legalUrlName, lastEntityUniqueName);
        }

        public static string GenerateFrom(IQueryable<Story> dataSource, string target)
        {
            string legalUrlName = target.ToLegalUrl();

            string lastEntityUniqueName = dataSource.Where(e => (e.UniqueName == legalUrlName) || e.UniqueName.StartsWith(legalUrlName + "-"))
                                                    .OrderByDescending(e => e.CreatedAt)
                                                    .Select(e => e.UniqueName)
                                                    .FirstOrDefault();
            
            return Generate(legalUrlName, lastEntityUniqueName);
        }

        private static string Generate(string legalUrlName, string lastEntityUniqueName)
        {
            if (string.IsNullOrEmpty(lastEntityUniqueName))
            {
                return legalUrlName;
            }

            var number = 0;

            if (string.Compare(lastEntityUniqueName, legalUrlName, StringComparison.OrdinalIgnoreCase) != 0)
            {
                int index = lastEntityUniqueName.LastIndexOf("-", StringComparison.Ordinal);

                if ((index > -1) && (index < (lastEntityUniqueName.Length - 1)))
                {
                    string numberInString = lastEntityUniqueName.Substring(index + 1);

                    if (!int.TryParse(numberInString, out number))
                    {
                        return legalUrlName;
                    }
                }
            }

            number = ((number < 1) ? 1 : number) + 1;

            return "{0}-{1}".FormatWith(legalUrlName, number);
        }
    }
}