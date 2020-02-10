using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace StationControllerUi.Controls
{
    public class HighlightLineBackgroundRenderer : IBackgroundRenderer
    {
        private TextEditor _editor;
        public HighlightLineBackgroundRenderer(TextEditor editor)
        {
            LineToHighlight = 1;
            _editor = editor;
        }
        public int LineToHighlight { get; set; }
        public KnownLayer Layer
        {
            get
            {
                return KnownLayer.Selection;
            }
        }

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            //currently not used
            return;

            if(_editor.Document == null)
            {
                return;
            }

            textView.EnsureVisualLines();

            var line = _editor.Document.GetLineByNumber(LineToHighlight);
            foreach (var rect in BackgroundGeometryBuilder.GetRectsForSegment(textView, line))
            {
                drawingContext.DrawRectangle(Brushes.LightYellow, null, new Rect(rect.Location, new Size(rect.Width, rect.Height)));
            }
        }
    }
}
