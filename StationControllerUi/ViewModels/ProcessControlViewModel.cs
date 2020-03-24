using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StationControllerUi.ViewModels
{
    public class ProcessControlViewModel : INotifyPropertyChanged
    {
        private Commands.RelayCommand _startCommand;
        private Commands.RelayCommand _stopCommand;
        private Commands.RelayCommand _debugCommand;
        private Commands.RelayCommand _displayNetworkCommand;

        public Commands.RelayCommand DisplayNetworkCommand
        {
            get
            {
                return _displayNetworkCommand;
            }
            set
            {
                _displayNetworkCommand = value;
                OnPropertyChanged();
            }
        }


        public Commands.RelayCommand DebugCommand
        {
            get
            {
                return _debugCommand;
            }
            set
            {
                _debugCommand = value;
                OnPropertyChanged();
            }
        }
        public Commands.RelayCommand StartCommand
        {
            get
            {
                return _startCommand;
            }
            set
            {
                _startCommand = value;
                OnPropertyChanged();
            }
        }

        public Commands.RelayCommand StopCommand
        {
            get
            {
                return _stopCommand;
            }
            set
            {
                _stopCommand = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
