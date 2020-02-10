using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StationControllerUi.Util
{
    public class Scenario
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Parameters { get; set; } = new List<string>();
    }
}
