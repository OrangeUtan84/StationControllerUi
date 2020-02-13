using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StationControllerUi.Controls
{
    public class BindableAvalonEditor : ICSharpCode.AvalonEdit.TextEditor, INotifyPropertyChanged
    {
        public HighlightLineBackgroundRenderer BackgroundRenderer { get; set; }
        public BindableAvalonEditor()
        {
            BackgroundRenderer = new HighlightLineBackgroundRenderer(this);
            this.TextArea.TextView.BackgroundRenderers.Add(BackgroundRenderer);
        }
        /// <summary>
        /// A bindable Text property
        /// </summary>
        public new string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
                RaisePropertyChanged("Text");
            }
        }

        public List<string> CompletionWords
        {
            get
            {
                return (List<string>)GetValue(CompletionWordsProptery);
            }
            set
            {
                SetValue(CompletionWordsProptery, value);
                RaisePropertyChanged("CompletionWords");
            }
        }

        public int SelectedLine
        {
            get
            {
                return (int)GetValue(SelectedLineProperty);
            }
            set
            {
                SetValue(SelectedLineProperty, value);
                RaisePropertyChanged("SelectedLine");
            }
        }

        /// <summary>
        /// The bindable text property dependency property
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text",
                typeof(string),
                typeof(BindableAvalonEditor),
                new FrameworkPropertyMetadata
                {
                    DefaultValue = default(string),
                    BindsTwoWayByDefault = true,
                    PropertyChangedCallback = OnDependencyPropertyChanged
                }
            );

        public static readonly DependencyProperty CompletionWordsProptery =
            DependencyProperty.Register(
                "CompletionWords",
                typeof(List<string>),
                typeof(BindableAvalonEditor),
                new FrameworkPropertyMetadata
                {
                    DefaultValue = new List<string>(),
                    BindsTwoWayByDefault = false
                }
            );

        public static readonly DependencyProperty SelectedLineProperty =
            DependencyProperty.Register(
                "SelectedLine",
                typeof(int),
                typeof(BindableAvalonEditor),
                new FrameworkPropertyMetadata
                {
                    DefaultValue = 1,
                    BindsTwoWayByDefault = false,
                    PropertyChangedCallback = OnNewLineSelected
                }
            );


        protected static void OnNewLineSelected(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((int)e.NewValue == 0)
            {
                //Line zero does not exist => document empty, return
                return;
            }
            var target = (BindableAvalonEditor)d;
            if (target != null && target.TextArea != null)
            {
                double vertOffset = (target.TextArea.TextView.DefaultLineHeight) * (int)e.NewValue;
                target.ScrollToVerticalOffset(vertOffset);
                var line = target.Document.GetLineByNumber((int)e.NewValue + 1);

                target.Select(line.Offset, line.TotalLength - 1);


                //target.BackgroundRenderer.LineToHighlight = (int) e.NewValue + 1;

                //target.TextArea.TextView.InvalidateLayer(KnownLayer.Selection);
            }
        }

        protected static void OnDependencyPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var target = (BindableAvalonEditor)obj;

            if (target.Document != null)
            {
                var caretOffset = target.CaretOffset;
                var newValue = args.NewValue;

                if (newValue == null)
                {
                    newValue = "";
                }

                target.Document.Text = (string)newValue;
                target.CaretOffset = Math.Min(caretOffset, newValue.ToString().Length);
            }
        }
        

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Space && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                e.Handled = true; //needed to discard the input for <space>
                //Auto Completion required
                CustomCompletionWindow completionWindow = new CustomCompletionWindow(TextArea, CompletionWords?.Select(s => $"${s}").ToList());
                completionWindow.Closed += (_, __) => completionWindow = null;
                completionWindow.Show();
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (this.Document != null)
            {
                Text = this.Document.Text;
            }

            base.OnTextChanged(e);
        }

        /// <summary>
        /// Raises a property changed event
        /// </summary>
        /// <param name="property">The name of the property that updates</param>
        public void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
