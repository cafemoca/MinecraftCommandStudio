using Cafemoca.McSlimUtils.Settings;
using System.Windows.Controls;

namespace Cafemoca.McSlimUtils.Views.Layouts.Documents
{
    /// <summary>
    /// Document.xaml の相互作用ロジック
    /// </summary>
    public partial class Document : UserControl
    {
        public Document()
        {
            InitializeComponent();
            this.Editor.Options = Setting.Current.EditorOptions;
        }
    }
}
