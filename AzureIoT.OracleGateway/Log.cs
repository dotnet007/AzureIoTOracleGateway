using System;

namespace AzureIoT.OracleGateway
{
    public static class Log
    {
        public static void LogText(string text)
        {
            Console.Write(text);
        }

        public static void LogLine(string line)
        {
            Console.WriteLine(line);
        }

        public static void LogLine(string line, params object[] param)
        {
            Console.WriteLine(line, param);
        }
    }
}
