using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;

namespace Cafemoca.McSlimUtils.Internals.Controls
{
    public class HyperlinkText : Hyperlink
    {
        public static readonly DependencyProperty UriProperty =
            DependencyProperty.Register("Uri", typeof(Uri), typeof(HyperlinkText), new UIPropertyMetadata(null));
        public Uri Uri
        {
            get { return (Uri)this.GetValue(UriProperty); }
            set { this.SetValue(UriProperty, value); }
        }

        protected override void OnClick()
        {
            base.OnClick();

            if (this.Uri == null) return;
            try { Process.Start(this.Uri.ToString()); }
            catch { }
        }
    }
}
