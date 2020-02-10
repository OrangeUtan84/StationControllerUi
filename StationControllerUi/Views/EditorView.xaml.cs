using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit.Search;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace StationControllerUi.Views
{
    /// <summary>
    /// Interaction logic for EditorView.xaml
    /// </summary>
    public partial class EditorView : UserControl
    {
        public EditorView()
        {
            InitializeComponent();
            
        }

        public void Initialize(string syntaxFile)
        {
            using (Stream s = new FileStream(syntaxFile, FileMode.Open))
            {
                using (XmlTextReader reader = new XmlTextReader(s))
                {
                    syntaxEdit.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                    
                    SearchPanel.Install(syntaxEdit);
                }
            }
        }

        private void cbMethodList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            syntaxEdit.InvalidateProperty(Controls.BindableAvalonEditor.SelectedLineProperty);
        }
    }
}
