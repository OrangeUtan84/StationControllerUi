using StationControllerUi.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StationControllerUi.ViewModels
{
    public class EditorViewModel : INotifyPropertyChanged
    {

        #region private variables
        private string _content;
        private string _filePath;
        private bool _saved;
        private List<LabelToLine> _labelToLine;
        private int _selectedLine;
        private List<string> _completionWordList;
        #endregion

        #region public properties

        public List<string> CompletionWordList
        {
            get
            {
                return _completionWordList;
            }
            set
            {
                _completionWordList = value;
                OnPropertyChanged();
            }
        }
        public int SelectedLine
        {
            get
            {
                return _selectedLine;
            }
            set
            {
                _selectedLine = value;
                OnPropertyChanged();
            }
        }

        public List<LabelToLine> LabelToLine
        {
            get
            {
                return _labelToLine;
            }
            set
            {
                _labelToLine = value;
                OnPropertyChanged();
            }
        }
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                OnPropertyChanged();
                //document unsaved as soon as text changed
                Saved = false;
                ParseContent();
            }
        }

        public string FilePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                _filePath = value;
                OnPropertyChanged();
            }
        }

        public bool Saved
        {
            get
            {
                return _saved;
            }

            protected set
            {
                _saved = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public void Save()
        {
            File.WriteAllText(_filePath, Content);
            Saved = true;
        }

        public void Load()
        {
            if (!File.Exists(FilePath))
            {
                throw new FileLoadException($"File {Path.GetFileName(FilePath)} does not exist.");
            }

            var content = File.ReadAllText(FilePath);

            Content = content;
            Saved = true;
            ParseContent();
        }

        private void ParseContent()
        {
            var contentLines = Regex.Split(Content, Environment.NewLine).ToList();
            var label2Line = contentLines.Where(w => w.Trim().StartsWith("label ")).Select(s => new LabelToLine { Label = s.Replace("label ",string.Empty), Line = contentLines.IndexOf(s) });
            LabelToLine = label2Line.ToList();

            var completionLines = contentLines.Where(w => w.Trim().StartsWithAny("let", "glet", "define"));
            Regex regex = new Regex(@"(?<=[let,glet,define]\s).*(?=\s=)");
            CompletionWordList = completionLines.Select(s => regex.Match(s).Value).Where(w => !string.IsNullOrWhiteSpace(w)).ToList();
        }



        #region public events
        #endregion

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class LabelToLine
    {
        public string Label { get; set; }
        public int Line { get; set; }
    }
}
