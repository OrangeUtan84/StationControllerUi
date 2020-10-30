using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StationControllerUi.Util
{
    //Thanks to 
    public class SocketConnector
    {
        private Socket _listener;
        private Socket _client;
        Thread _dataListener;
        System.Timers.Timer _disconnectDetectTimer;
        public event EventHandler<DataReceivedEventArgs> DataReceived;
        public event EventHandler<ClientEventArgs> ClientConnected;
        public event EventHandler ClientDisconnected;

        protected void OnDataReceived(string data)
        {
            DataReceived?.Invoke(this, new DataReceivedEventArgs(data));
        }

        protected void OnClientConnected(string clientName)
        {
            ClientConnected?.Invoke(this, new ClientEventArgs(clientName));
        }

        protected void OnClientDisconnected()
        {
            ClientDisconnected?.Invoke(this, new EventArgs());
        }

        public SocketConnector(int port)
        {
            _listener = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _listener.Bind(new IPEndPoint(IPAddress.Any, port));
            _disconnectDetectTimer = new System.Timers.Timer();
            _disconnectDetectTimer.AutoReset = true;
            _disconnectDetectTimer.Elapsed += _disconnectDetectTimer_Elapsed;
        }

        public void Start()
        {
            _listener.Listen(1);
            _listener.BeginAccept(new AsyncCallback(AcceptCallback), _listener);
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                _client = _listener.EndAccept(ar);
            }
            catch
            {
                //Handle disposed object exception
                return;
            }

            var clientName = $"{(_client.RemoteEndPoint as IPEndPoint).Address}:{(_client.RemoteEndPoint as IPEndPoint).Port}";
            OnClientConnected(clientName);
            _dataListener = new Thread(new ThreadStart(Listen));
            _dataListener.Start();
            _disconnectDetectTimer.Start();
        }

        private void Listen()
        {
            string data = "";
            while (_client.Connected)
            {
                var buffer = new byte[1024];
                int bytesRec;
                try
                {
                    bytesRec = _client.Receive(buffer);
                }catch
                {
                    //error receiving data, client my be disconnected
                    continue;
                }
                data += Encoding.ASCII.GetString(buffer, 0, bytesRec);
                int idx;
                if ((idx = data.IndexOf("<EOF>")) > -1)
                {
                    data = data.Remove(idx);
                    OnDataReceived(data);
                    break;
                }
            }
            if (_client.Connected)
            {
                Listen();
            }
        }

        public void Send(string content)
        {
            content += "<EOF>";
            byte[] data = Encoding.ASCII.GetBytes(content);
            if (_client != null)
            {
                _client.Send(data, data.Length, SocketFlags.None);
            }
        }

        public void Stop()
        {
            if(_dataListener != null && _dataListener.IsAlive)
            {
                _dataListener.Abort();
            }

            if(_disconnectDetectTimer != null && _disconnectDetectTimer.Enabled)
            {
                _disconnectDetectTimer.Stop();
                _disconnectDetectTimer.Dispose();
            }

            if(_listener != null)
            {
                if(_client != null && _client.Connected)
                {
                    _client.Close();
                    _client.Dispose();
                }
                _listener.Close();
                _listener.Dispose();
            }
        }

        private void _disconnectDetectTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            bool connected = !(_client.Poll(1, SelectMode.SelectRead) && _client.Available == 0);
            if (!connected)
            {
                _disconnectDetectTimer.Stop();
                OnClientDisconnected();
                //begin accept new connections
                _listener.BeginAccept(new AsyncCallback(AcceptCallback), _listener);
            }
        }
    }
}
