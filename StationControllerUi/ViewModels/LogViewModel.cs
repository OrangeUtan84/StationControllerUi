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
    public class LogViewModel : INotifyPropertyChanged
    {
        public LogViewModel()
        {
            LogList = new ObservableCollection<string>();
        }
        private ObservableCollection<string> _logList;
        
        public ObservableCollection<string> LogList
        {
            get
            {
                return _logList;
            }
            set
            {
                _logList = value;
                OnPropertyChanged();
            }
        }


        public void Append(string text)
        {
            LogList.Add(text);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
