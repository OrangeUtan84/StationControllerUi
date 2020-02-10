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
    public class ScenarioSelectViewModel : INotifyPropertyChanged
    {

        public ScenarioSelectViewModel()
        {
            CallScenarioCommand = new Commands.RelayCommand(command =>
            {
                ScenarioCalled?.Invoke((Util.Scenario)command);
            });
        }

        private ObservableCollection<Util.Scenario> _scenarios;

        public event Action<Util.Scenario> ScenarioCalled;

        private Commands.RelayCommand _callScenarioCommand;

        public Commands.RelayCommand CallScenarioCommand
        {
            get
            {
                return _callScenarioCommand;
            }
            set
            {
                _callScenarioCommand = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Util.Scenario> Scenarios
        {
            get
            {
                return _scenarios;
            }
            set
            {
                _scenarios = value;
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
