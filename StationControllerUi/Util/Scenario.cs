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

        public string Type { get; set; }

        public string Color
        {
            get
            {
                var allColors = (from param in Parameters
                                 let splittedParam = param.Split('=')
                                 where splittedParam[0] == "Color" && splittedParam.Length > 1
                                 select splittedParam[1]);
                return allColors.FirstOrDefault() ?? string.Empty;
            }
        }
    }
}
