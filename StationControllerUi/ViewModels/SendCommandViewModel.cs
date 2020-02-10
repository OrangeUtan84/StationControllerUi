using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StationControllerUi.ViewModels
{
    public class SendCommandViewModel : INotifyPropertyChanged
    {
        private string _commandExpression;
        private Commands.RelayCommand _executeExpressionCommand;

        public string CommandExpression
        {
            get
            {
                return _commandExpression;
            }
            set
            {
                _commandExpression = value;
                OnPropertyChanged();
            }
        }

        public Commands.RelayCommand ExecuteExpressionCommand
        {
            get
            {
                return _executeExpressionCommand;
            }
            set
            {
                _executeExpressionCommand = value;
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
