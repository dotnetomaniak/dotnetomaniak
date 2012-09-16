namespace Kigg.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;

    public class BadWordMatchSpamWeightCalculator : ISpamWeightCalculator
    {
        private readonly Dictionary<Regex, int> _badWordExpressionsWithValue = new Dictionary<Regex, int>();

        public BadWordMatchSpamWeightCalculator(IFile file, string fileName)
        {
            Check.Argument.IsNotNull(file, "file");
            Check.Argument.IsNotEmpty(fileName, "fileName");

            BuildBadWordExpressions(file, fileName);
        }

        public int Calculate(string content)
        {
            int total = 0;

            content = content.StripHtml();

            foreach(KeyValuePair<Regex, int> pair in _badWordExpressionsWithValue)
            {
                total += pair.Key.Matches(content).Count * pair.Value;
            }

            return total;
        }

        private void BuildBadWordExpressions(IFile fileReader, string fileName)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            string content = fileReader.ReadAllText(path);

            XElement root = XDocument.Parse(content).Root;

            if (root != null)
            {
                foreach(XElement item in root.Descendants("item"))
                {
                    string expression = (string) item.Element("exp");
                    int value = (int) item.Element("value");

                    if (!string.IsNullOrEmpty(expression))
                    {
                        _badWordExpressionsWithValue.Add(new Regex(expression, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled), value);
                    }
                }
            }
        }
    }
}