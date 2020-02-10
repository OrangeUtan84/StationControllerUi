using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StationControllerUi.Util
{
    /// <summary>
    /// Handles the monitoring, the starting, stopping and debugging of the process
    /// </summary>
    public class StationController : IDisposable
    {
        /// <summary>
        /// fired when the state of the process has changed
        /// </summary>
        public event Action<bool> RunningStateChanged;

        private Process _scProcess;
        private string _scPath;
        private bool _isRunning;
        private SocketConnector _connector;

        public event EventHandler<ClientEventArgs> ClientConnected;
        public event EventHandler ClientDisconnected;
        public event EventHandler<DataReceivedEventArgs> DataReceived;
        public event EventHandler<DataReceivedEventArgs> PrintReceived;

        public List<Scenario> Scenarios { get; set; } = new List<Scenario>();

        /// <summary>
        /// Value that indicates if the process is currently running
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
            private set
            {
                _isRunning = value;
                RunningStateChanged?.Invoke(_isRunning);
            }
        }
        /// <summary>
        /// creates a new StationController instance with the given path to the sc interpreter
        /// </summary>
        /// <param name="scPath">path to the sc interpreter</param>
        public StationController(string scPath, int debugPort)
        {
            IsRunning = false;
            _scProcess = new Process();
            _scPath = scPath;
            _scProcess.Exited += _scProcess_Exited;
            _connector = new SocketConnector(debugPort);
            _connector.ClientConnected += StationControllerConnected;
            _connector.ClientDisconnected += StationControllerDisconneccted;
            _connector.DataReceived += StationControllerDataReceived;
            

            _connector.Start();
        }

        protected void StationControllerDataReceived(object sender, DataReceivedEventArgs e)
        {
            DataReceived?.Invoke(this, e);
            switch(e.Data.Split().First())
            {
                case "PRINT":
                    PrintReceived?.Invoke(this, new DataReceivedEventArgs(e.Data.Replace("PRINT", "").Trim()));
                    break;
            }
        }

        protected void StationControllerDisconneccted(object sender, EventArgs e)
        {
            ClientDisconnected?.Invoke(this, e);
        }

        protected void StationControllerConnected(object sender, ClientEventArgs e)
        {
            ClientConnected?.Invoke(this, e);
        }

        /// <summary>
        /// fired when the SC process has exited for any reason
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _scProcess_Exited(object sender, EventArgs e)
        {
            IsRunning = false;
        }


        /// <summary>
        /// starts the process in run mode for the given script
        /// </summary>
        /// <param name="script">the interpreter script to be executed</param>
        public void Start(string script)
        {
            _scProcess.StartInfo = new ProcessStartInfo
            {
                FileName = _scPath,
                Arguments = $"-i -d 1 -c \"read -P '{script}'\""
            };
           _scProcess.Start();
            IsRunning = true;
            Thread monitorThread = new Thread(new ParameterizedThreadStart(MonitorProcess));
            monitorThread.Start(_scProcess.Id);
        }


        public void StartDebugger(string script)
        {
            //first create a local copy of the script

            var tempDir = Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "tmp"));
            var tempScript = Path.Combine(tempDir.FullName, Path.GetFileName(script));
            File.Copy(script, tempScript, true);

            var originalContent = File.ReadAllLines(tempScript);
            var debugStartContent = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "debug_script_start.sc"));
            var debugEndContent = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "debug_script_end.sc"));
            var parsed = ParseScript(originalContent.ToList());
            var merged = debugStartContent.Concat(parsed.Concat(debugEndContent));
            
            File.WriteAllLines(tempScript, merged);
            Start(tempScript);
        }

        public void SendCommand(string command)
        {
            _connector.Send(command);
        }

        private List<string> ParseScript(List<string> merged)
        {
            var replacedScript = merged.Select(s => s.Replace("print", "gosub __print")).ToList();

            var scenarioButtonLines = replacedScript.Where(w => w.StartsWith("#/Button"));

           Scenarios = (from s in scenarioButtonLines
                                    let splittedString = s.Split(':')
                                    let labelName = splittedString[1]
                                    let description = splittedString[2]
                                    let parameters = splittedString.Length > 3 ? splittedString[3].Split('/') : new string[0]
                                    select new Scenario
                                    {
                                        Name = labelName,
                                        Description = description,
                                        Parameters = parameters.ToList()
                                    }).ToList();

            return replacedScript;
        }

        /// <summary>
        /// method is called periodically and checks if the process is still running
        /// </summary>
        /// <param name="id"></param>
        protected void MonitorProcess(object id)
        {
            while (true)
            {
                Process currentProcess;
                try
                {
                    currentProcess = Process.GetProcessById((int)id);
                }
                catch
                {
                    //process does not exist
                    break;
                }
                if (currentProcess != null)
                {
                    Thread.Sleep(100);
                }
                else
                {
                    break;
                }
            }
            IsRunning = false;
        }

        public void Stop()
        {
            _scProcess.Kill();
            IsRunning = false;
        }


        /// <summary>
        /// dispose method ensures that the process will be stopped if running
        /// </summary>
        public void Dispose()
        {
            if (_scProcess != null && !_scProcess.HasExited)
            {
                _scProcess.Kill();
                _scProcess.Dispose();
                _scProcess = null;
                IsRunning = false;
            }
            if(_connector != null)
            {
                _connector.Stop();
                _connector = null;
            }
        }
    }
}
