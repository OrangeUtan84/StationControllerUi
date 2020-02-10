using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StationControllerUi.Util
{
    public class RescentFilesHandler : INotifyPropertyChanged, IDisposable
    {
        private string _fileName;
        private ObservableCollection<string> _rescentFiles;
        public RescentFilesHandler(string fileName)
        {
            _fileName = fileName;
            RescentFiles = new ObservableCollection<string>();
            if (File.Exists(_fileName))
            {
                RescentFiles = new ObservableCollection<string>(File.ReadAllLines(_fileName).ToList());
            }
        }
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Put(string fileName)
        {
            if (RescentFiles.Contains(fileName))
            {
                //already in list... remove and put to the beginning
                RescentFiles.Remove(fileName);
            }
            //the rescent file list should not contain more than 5 entries. If count exceeds 5 remove the last one

            if (RescentFiles.Count > 5)
            {
                RescentFiles.RemoveAt(RescentFiles.Count - 1);

            }
            RescentFiles.Insert(0, fileName);
            OnPropertyChanged(nameof(RescentFiles));
        }

        public void Dispose()
        {
            //integrity check, to be sure that every file only exist once:
            var distinctList = RescentFiles.Distinct<string>();
            File.WriteAllLines(_fileName, distinctList);
        }
    }
}
