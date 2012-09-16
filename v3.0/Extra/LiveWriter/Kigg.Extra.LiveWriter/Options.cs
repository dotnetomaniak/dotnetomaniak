namespace Kigg.Extra.LiveWriter
{
    using System;
    using System.Windows.Forms;

    internal partial class Options : Form
    {
        private readonly Settings _settings;

        internal Options(Settings defaultSettings) : this()
        {
            _settings = defaultSettings;
        }

        internal Options()
        {
            InitializeComponent();
        }

        private void LoadSettings()
        {
            borderColor.Value = _settings.BorderColor;
            shoutItBackColor.Value = _settings.ShoutItBackColor;
            shoutItForeColor.Value = _settings.ShoutItForeColor;
            countBackColor.Value = _settings.CountBackColor;
            countForeColor.Value = _settings.CountForeColor;
        }

        private void ResetSettings()
        {
            _settings.Reset();
            LoadSettings();
        }

        private void UpdatePreview()
        {
            picPreview.ImageLocation = DotNetShoutoutCounterGenerator.ImageSource(DotNetShoutoutCounterGenerator.BaseUrl, borderColor.Value, shoutItBackColor.Value, shoutItForeColor.Value, countBackColor.Value, countForeColor.Value);
        }

        private void Options_Load(object sender, EventArgs e)
        {
            LoadSettings();
            UpdatePreview();
        }

        private void borderColor_ColorChanged(object sender, EventArgs e)
        {
            _settings.BorderColor = borderColor.Value;
            UpdatePreview();
        }

        private void shoutItBackColor_ColorChanged(object sender, EventArgs e)
        {
            _settings.ShoutItBackColor = shoutItBackColor.Value;
            UpdatePreview();
        }

        private void shoutItForeColor_ColorChanged(object sender, EventArgs e)
        {
            _settings.ShoutItForeColor = shoutItForeColor.Value;
            UpdatePreview();
        }

        private void countBackColor_ColorChanged(object sender, EventArgs e)
        {
            _settings.CountBackColor = countBackColor.Value;
            UpdatePreview();
        }

        private void countForeColor_ColorChanged(object sender, EventArgs e)
        {
            _settings.CountForeColor = countForeColor.Value;
            UpdatePreview();
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            ResetSettings();
            UpdatePreview();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}