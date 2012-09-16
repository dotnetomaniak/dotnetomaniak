namespace Kigg.Infrastructure
{
    using System;
    using System.Collections.Generic;

    public class DefaultSpamProtection : BaseSpamProtection
    {
        private const string Source = "Default";

        private readonly IConfigurationSettings _settings;
        private readonly IHttpForm _httpForm;

        private readonly int _storyThreshold;
        private readonly ISpamWeightCalculator[] _storyLocalCalculators;
        private readonly ISpamWeightCalculator[] _storyRemoteCalculators;

        private readonly int _commentThreshold;
        private readonly ISpamWeightCalculator[] _commentCalculators;

        public DefaultSpamProtection(IConfigurationSettings settings, IHttpForm httpForm, int storyThreshold, ISpamWeightCalculator[] storyLocalCalculators, ISpamWeightCalculator[] storyRemoteCalculators, int commentThreshold, ISpamWeightCalculator[] commentCalculators)
        {
            Check.Argument.IsNotNull(settings, "settings");
            Check.Argument.IsNotNull(httpForm, "httpForm");

            _settings = settings;
            _httpForm = httpForm;

            _storyThreshold = storyThreshold;
            _storyLocalCalculators = storyLocalCalculators;
            _storyRemoteCalculators = storyRemoteCalculators;

            _commentThreshold = commentThreshold;
            _commentCalculators = commentCalculators;
        }

        public override bool IsSpam(SpamCheckContent spamCheckContent)
        {
            Check.Argument.IsNotNull(spamCheckContent, "spamCheckContent");

            bool isSpam = IsComment(spamCheckContent) ? IsSpamComment(spamCheckContent) : IsSpamStory(spamCheckContent);

            if ((!isSpam) && (NextHandler != null))
            {
                isSpam = NextHandler.IsSpam(spamCheckContent);
            }

            return isSpam;
        }

        public override void IsSpam(SpamCheckContent spamCheckContent, Action<string, bool> callback)
        {
            Check.Argument.IsNotNull(spamCheckContent, "spamCheckContent");
            Check.Argument.IsNotNull(callback, "callback");

            if (IsComment(spamCheckContent))
            {
                if (IsSpamComment(spamCheckContent))
                {
                    callback(Source, true);
                }
                else if (NextHandler != null)
                {
                    NextHandler.IsSpam(spamCheckContent, callback);
                }
                else
                {
                    callback(Source, false);
                }
            }
            else
            {
                //First run it over the story description
                int total = CalculateTotal(spamCheckContent.Content, _storyLocalCalculators);

                if (total > _storyThreshold)
                {
                    callback(Source, true);
                }
                else
                {
                    _httpForm.GetAsync(
                                            new HttpFormGetRequest { Url = spamCheckContent.Url},
                                            httpResponse =>
                                            {
                                                if (!string.IsNullOrEmpty(httpResponse.Response))
                                                {
                                                    total += CalculateTotal(httpResponse.Response, _storyRemoteCalculators);
                                                }

                                                if (total > _storyThreshold)
                                                {
                                                    callback(Source, true);
                                                }
                                                else if (NextHandler != null)
                                                {
                                                    NextHandler.IsSpam(spamCheckContent, callback);
                                                }
                                                else
                                                {
                                                    callback(Source, false);
                                                }
                                            },
                                            e =>
                                            {
                                                // When exception occurs try next handler
                                                if (NextHandler != null)
                                                {
                                                    NextHandler.IsSpam(spamCheckContent, callback);
                                                }
                                                else
                                                {
                                                    callback(Source, false);
                                                }
                                            }
                        );
                }
            }
        }

        private static int CalculateTotal(string content, IEnumerable<ISpamWeightCalculator> calculators)
        {
            int total = 0;

            foreach (ISpamWeightCalculator calculator in calculators)
            {
                total += calculator.Calculate(content);
            }

            return total;
        }

        private bool IsSpamComment(SpamCheckContent spamCheckContent)
        {
            int total = CalculateTotal(spamCheckContent.Content, _commentCalculators);

            return (total > _commentThreshold);
        }

        private bool IsSpamStory(SpamCheckContent spamCheckContent)
        {
            //First run it over the story description
            int total = CalculateTotal(spamCheckContent.Content, _storyLocalCalculators);

            //If it is small then run it over story url
            if (total <= _storyThreshold)
            {
                string response = _httpForm.Get(new HttpFormGetRequest { Url = spamCheckContent.Url }).Response;

                if (!string.IsNullOrEmpty(response))
                {
                    total += CalculateTotal(response, _storyRemoteCalculators);
                }
            }

            return (total > _storyThreshold);
        }

        private bool IsComment(SpamCheckContent conent)
        {
            return conent.Url.StartsWith(_settings.RootUrl, StringComparison.OrdinalIgnoreCase);
        }
    }
}