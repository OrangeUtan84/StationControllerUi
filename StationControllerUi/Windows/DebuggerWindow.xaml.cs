using StationControllerUi.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace StationControllerUi.Windows
{
    /// <summary>
    /// Interaktionslogik für DebuggerWindow.xaml
    /// </summary>
    public partial class DebuggerWindow : Window
    {
        private StationController _stationController;
        private ViewModels.LogViewModel _logViewModel;
        private ViewModels.SendCommandViewModel _sendCommandViewModel;
        private ViewModels.ScenarioSelectViewModel _scenarioSelectViewModel;
        public DebuggerWindow(StationController stationController)
        {
            InitializeComponent();

            Dispatcher.Invoke(() => IsEnabled = false); //disable until connected
            _stationController = stationController;
            _logViewModel = new ViewModels.LogViewModel();
            logView.DataContext = _logViewModel;
            
            _sendCommandViewModel = new ViewModels.SendCommandViewModel();
            sendCommandView.DataContext = _sendCommandViewModel;

            _scenarioSelectViewModel = new ViewModels.ScenarioSelectViewModel();
            _scenarioSelectViewModel.ScenarioCalled += ScenarioViewModel_ScenarioCalled;
            scenarioSelectView.DataContext = _scenarioSelectViewModel;

            _sendCommandViewModel.ExecuteExpressionCommand = new Commands.RelayCommand(command =>
            {
                _stationController.SendCommand((string)command);
                _sendCommandViewModel.CommandExpression = string.Empty;
            },
            _ => !string.IsNullOrEmpty(_sendCommandViewModel.CommandExpression));

            _stationController.ClientConnected += StationController_ClientConnected;
            _stationController.ClientDisconnected += StationController_ClientDisconnected;
            _stationController.DataReceived += StationController_DataReceived;
            _stationController.RunningStateChanged += StationController_RunningStateChanged;

            _scenarioSelectViewModel.Scenarios = new System.Collections.ObjectModel.ObservableCollection<Scenario>(_stationController.Scenarios);
        }

        private void ScenarioViewModel_ScenarioCalled(Scenario scenario)
        {
            _stationController.SendCommand($"label {scenario.Name}");
        }

        private void StationController_RunningStateChanged(bool running)
        {
            //Station Controller's gone...we can also go
            if (running == false)
            {
                //bye
                Dispatcher.BeginInvoke(new Action(() => Close()));

            }
        }


        private void StationController_DataReceived(object sender, DataReceivedEventArgs e)
        {
            AppendLog($"Data received: {e.Data}");
        }

        private void StationController_ClientDisconnected(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() => IsEnabled = false);
            AppendLog($"Client disconnected");
        }

        private void StationController_ClientConnected(object sender, ClientEventArgs e)
        {
            Dispatcher.Invoke(() => IsEnabled = true);
            AppendLog($"Client connected: {e.ClientName}");
        }

        public void AppendLog(string log)
        {
            Dispatcher.BeginInvoke(new Action(() => _logViewModel.Append(log)));
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            //e.Cancel = true;
            base.OnClosing(e);
        }

    }
}
