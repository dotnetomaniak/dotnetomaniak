using System.Diagnostics;

using WindowsLive.Writer.Api;

namespace Kigg.Extra.LiveWriter
{
    public class Settings
    {
        private const string CONTENT = "content";

        private const string BORDER_COLOR = "borderColor";
        private const string SHOUT_IT_BACK_COLOR = "textBackColor";
        private const string SHOUT_IT_FORE_COLOR = "textForeColor";
        private const string COUNT_BACK_COLOR = "countBackColor";
        private const string COUNT_FORE_COLOR = "countForeColor";

        private readonly IProperties _properties;

        public Settings(IProperties properties)
        {
            _properties = properties;
        }

        public string Content
        {
            [DebuggerStepThrough]
            get
            {
                return _properties.GetString(CONTENT, string.Empty);
            }
            [DebuggerStepThrough]
            set
            {
                _properties.SetString(CONTENT, value);
            }
        }

        public string BorderColor
        {
            [DebuggerStepThrough]
            get
            {
                return _properties.GetString(BORDER_COLOR, DotNetShoutoutCounterGenerator.DefaultBorderColor);
            }
            [DebuggerStepThrough]
            set
            {
                _properties.SetString(BORDER_COLOR, value);
            }
        }

        public string ShoutItBackColor
        {
            [DebuggerStepThrough]
            get
            {
                return _properties.GetString(SHOUT_IT_BACK_COLOR, DotNetShoutoutCounterGenerator.DefaultTextBackColor);
            }
            [DebuggerStepThrough]
            set
            {
                _properties.SetString(SHOUT_IT_BACK_COLOR, value);
            }
        }

        public string ShoutItForeColor
        {
            [DebuggerStepThrough]
            get
            {
                return _properties.GetString(SHOUT_IT_FORE_COLOR, DotNetShoutoutCounterGenerator.DefaultTextForeColor);
            }
            [DebuggerStepThrough]
            set
            {
                _properties.SetString(SHOUT_IT_FORE_COLOR, value);
            }
        }

        public string CountBackColor
        {
            [DebuggerStepThrough]
            get
            {
                return _properties.GetString(COUNT_BACK_COLOR, DotNetShoutoutCounterGenerator.DefaultCountBackColor);
            }
            [DebuggerStepThrough]
            set
            {
                _properties.SetString(COUNT_BACK_COLOR, value);
            }
        }

        public string CountForeColor
        {
            [DebuggerStepThrough]
            get
            {
                return _properties.GetString(COUNT_FORE_COLOR, DotNetShoutoutCounterGenerator.DefaultCountForeColor);
            }
            [DebuggerStepThrough]
            set
            {
                _properties.SetString(COUNT_FORE_COLOR, value);
            }
        }

        public void Reset()
        {
            BorderColor = DotNetShoutoutCounterGenerator.DefaultBorderColor;
            ShoutItBackColor = DotNetShoutoutCounterGenerator.DefaultTextBackColor;
            ShoutItForeColor = DotNetShoutoutCounterGenerator.DefaultTextForeColor;
            CountBackColor = DotNetShoutoutCounterGenerator.DefaultCountBackColor;
            CountForeColor = DotNetShoutoutCounterGenerator.DefaultCountForeColor;
        }
    }
}