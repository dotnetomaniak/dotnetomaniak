namespace Kigg.Extra.LiveWriter
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal partial class ColorControl : UserControl
    {
        public event EventHandler ColorChanged;

        public ColorControl()
        {
            InitializeComponent();
        }

        public string Caption
        {
            get
            {
                return lblCaption.Text;
            }
            set
            {
                lblCaption.Text = value;
            }
        }

        public string Value
        {
            get
            {
                return txtValue.Text;
            }
            set
            {
                txtValue.Text = value;
            }
        }

        private void PickColor()
        {
            string value = Value;

            if (!string.IsNullOrEmpty(value))
            {
                if (!value.StartsWith("#"))
                {
                    value = "#" + value;
                }

                try
                {
                    dlgColor.Color = ColorTranslator.FromHtml(value);
                }
                // ReSharper disable EmptyGeneralCatchClause
                catch(Exception)
                // ReSharper restore EmptyGeneralCatchClause
                {
                }
            }

            if (dlgColor.ShowDialog() != DialogResult.Cancel)
            {
                Color color = dlgColor.Color;

                Value = string.Format("{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);

                OnColorChange();
            }
        }

        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            OnColorChange();
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            PickColor();
        }

        private void OnColorChange()
        {
            if (ColorChanged != null)
            {
                ColorChanged(this, EventArgs.Empty);
            }
        }
    }
}