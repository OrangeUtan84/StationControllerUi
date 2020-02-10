using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StationControllerUi.Util
{
    public class ClientEventArgs : EventArgs
    {
        public string ClientName { get; private set; }
        public ClientEventArgs(string clientName)
        {
            ClientName = clientName;
        }
    }
}
