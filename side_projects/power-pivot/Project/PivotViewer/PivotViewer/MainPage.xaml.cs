using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Browser;

namespace PivotViewer
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            pivot.LinkClicked += new EventHandler<System.Windows.Pivot.LinkEventArgs>(pivot_LinkClicked);            
            pivot.LoadCollection(@"http://pivot.dotnetomaniak.pl/dotnetomaniak.pl.cxml", string.Empty);
        }

        void pivot_LinkClicked(object sender, System.Windows.Pivot.LinkEventArgs e)
        {
            OpenLink(e.Link.ToString());
        }

        private void OpenLink(string linkUri)
        {         
                // Link points to a normal web page. Open it in a new tab.
            HtmlPage.Window.Navigate(new Uri(linkUri, UriKind.RelativeOrAbsolute), "dotnetomaniak");
        }
    }
}
