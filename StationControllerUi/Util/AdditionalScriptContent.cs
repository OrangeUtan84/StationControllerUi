using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StationControllerUi.Util
{
    internal static class AdditionalScriptContent
    {
        public const string DEBUG_CONNECTION_LABEL =
            "label debug_connection\r\n" +
            "\tlet command = get_tmp_var(ARG0)\r\n" +
            "\tprint \"DEBUG command: \":$command\r\n" +
            "\tif($command eq \"label\")\r\n" +
            "\t\tlet parameter = get_tmp_var(ARG1)\r\n" +
            "\t\tgoto $parameter\r\n" +
            "\tend_if";

        public const string DEBUG_CONNECTION_STRING = "open socket_client name=debug_connection proto=standard host=127.0.0.1 service=5001 options=\"wait_for_reply=no rcv_eol=\\\"<EOF>\\\" xmt_eol=\\\"<EOF>\\\"\" no_error";
    }
}
