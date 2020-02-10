using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StationControllerUi.ViewModels
{
    public class MainMenuViewModel : INotifyPropertyChanged
    {
        #region privare variables
        private Commands.RelayCommand _openCommand;
        private Commands.RelayCommand _saveCommand;
        private Commands.RelayCommand _saveAsCommand;
        private Commands.RelayCommand _exitCommand;
        private ObservableCollection<string> _rescentFiles;
        #endregion

        #region public properties
      
        public ObservableCollection<string> RescentFiles
        {
            get
            {
                return _rescentFiles;
            }
            set
            {
                _rescentFiles = value;
                OnPropertyChanged();
            }
        }
        public Commands.RelayCommand OpenCommand
        {
            get
            {
                return _openCommand;
            }
            set
            {
                _openCommand = value;
                OnPropertyChanged();
            }
        }

        public Commands.RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand;
            }
            set
            {
                _saveCommand = value;
                OnPropertyChanged();
            }
        }

        public Commands.RelayCommand SaveAsCommand
        {
            get
            {
                return _saveAsCommand;
            }
            set
            {
                _saveAsCommand = value;
                OnPropertyChanged();
            }
        }

        public Commands.RelayCommand ExitCommand
        {
            get
            {
                return _exitCommand;
            }
            set
            {
                _exitCommand = value;
                OnPropertyChanged();
            }
        }
        #endregion



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
