using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace StationControllerUi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModels.EditorViewModel _editorViewModel;
        private ViewModels.ProcessControlViewModel _processControlViewModel;
        private ViewModels.MainMenuViewModel _mainMenuViewModel;
        private Util.StationController _stationController;
        private Util.RescentFilesHandler _rescentFilesHandler;
        private Config _config;
        public MainWindow()
        {
            InitializeComponent();
            _config = Config.Load("config.xml");

            _processControlViewModel = new ViewModels.ProcessControlViewModel();
            processControlView.DataContext = _processControlViewModel;
            _processControlViewModel.StartCommand = new Commands.RelayCommand(
                _ =>
                {
                    if (!_editorViewModel.Saved && !SaveFile())
                    {
                        return;
                    }
                    _stationController.Start(_editorViewModel.FilePath);
                },
                _ => !_stationController.IsRunning);

            _processControlViewModel.DebugCommand = new Commands.RelayCommand(
                _ =>
                {
                    if (!_editorViewModel.Saved && !SaveFile())
                    {
                        return;
                    }
                    _stationController.StartDebugger(_editorViewModel.FilePath);
                    var debugWindow = new Windows.DebuggerWindow(_stationController);
                    debugWindow.Show();
                },
                _ => !_stationController.IsRunning);

            _processControlViewModel.StopCommand = new Commands.RelayCommand(
                _ => _stationController.Stop(),
                _ => _stationController.IsRunning);

            editorView.Initialize(_config.SyntaxDescriptionFile);

            _editorViewModel = new ViewModels.EditorViewModel();
            editorView.DataContext = _editorViewModel;
            _editorViewModel.PropertyChanged += EditorViewModelPropertyChanged;

            #region Main Menu
            _mainMenuViewModel = new ViewModels.MainMenuViewModel();
            mainMenuView.DataContext = _mainMenuViewModel;
            _mainMenuViewModel.OpenCommand = new Commands.RelayCommand(fileName => OpenFile((string)fileName));
            _mainMenuViewModel.SaveCommand = new Commands.RelayCommand(_ => SaveFile());
            _mainMenuViewModel.SaveAsCommand = new Commands.RelayCommand(_ => SaveFileAs());
            _mainMenuViewModel.ExitCommand = new Commands.RelayCommand(_ => this.Close());
           

            #endregion
            _rescentFilesHandler = new Util.RescentFilesHandler("rescent.txt");
            _mainMenuViewModel.RescentFiles = _rescentFilesHandler.RescentFiles;
            _stationController = new Util.StationController(_config.ScPath, _config.DebugPort);
           
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            //ensure that rescent files handler is disposed correctly, because the list will be stored during disposing
            _stationController.Dispose();
            _rescentFilesHandler.Dispose();
            base.OnClosing(e);
        }


        private void EditorViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case (nameof(ViewModels.EditorViewModel.Saved)):
                    {
                        Title = (string.IsNullOrWhiteSpace(_editorViewModel.FilePath)
                            ? "Station Controller"
                            : "Station Controller - " + System.IO.Path.GetFileName(_editorViewModel.FilePath));
                        if (!_editorViewModel.Saved)
                        {
                            Title += "*";
                        }
                    }
                    break;

            }
        }

        private void OpenFile(string fileName = null)
        {
            if (fileName == null)
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "StationController files (*.sc)|*.sc|All files (*.*)|*.*";
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    fileName = dlg.FileName;
                }
                else
                {
                    return;
                }
            }
            else if (!File.Exists(fileName))
            {
                System.Windows.MessageBox.Show("File does not exist.");
                return;
            }

            var path = fileName;
            _editorViewModel.FilePath = path;
            _editorViewModel.Load();
            _rescentFilesHandler.Put(path);
        }


        private bool SaveFile()
        {
            if (string.IsNullOrWhiteSpace(_editorViewModel.FilePath))
            {
                return SaveFileAs();
            }
            else
            {
                _editorViewModel.Save();
            }
            return true;
        }

        private bool SaveFileAs()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "StationController files (*.sc)|*.sc|All files (*.*)|*.*";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var path = dlg.FileName;
                _editorViewModel.FilePath = path;
                _editorViewModel.Save();
                return true;
            }
            return false;
        }



    }

}
