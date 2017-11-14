using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Configuration;
using System.Data.OracleClient;
using System.IO;
using System.Threading;

namespace AzureIoT.OracleGateway
{
    class Program
    {
        private static string GetQuery()
        {
            return File.ReadAllText(ConfigurationManager.AppSettings["QueryFile"]);
        }

        private static void Main(string[] args)
        {
            Log.LogLine("Azure IoT Oracle Gateway Started.");
            Log.LogLine("Press Ctrl + C to cancell at any time.");
            while (true)
            {
                try
                {
                    ReadData();
                }
                catch (Exception ex)
                {
                    Log.LogLine("Error Message: " + ex.Message);
                    Log.LogLine("Stacktrace: " + ex.StackTrace);
                }
                Log.LogLine("Waiting 1 minute for next call...");
                Thread.Sleep(int.Parse(ConfigurationManager.AppSettings["MillisecondsToWait"]));
            }
        }

        public static DateTime GetLastDate()
        {
            JObject jobject = JObject.Parse(File.ReadAllText(ConfigurationManager.AppSettings["LastDateFile"]));
            return new DateTime((int)jobject["Year"], (int)jobject["Month"], (int)jobject["Day"], (int)jobject["Hour"], (int)jobject["Minute"], (int)jobject["Second"]);
        }

        public static void SetLastDate(DateTime lastDate)
        {
            File.WriteAllText(ConfigurationManager.AppSettings["LastDateFile"], new JObject()
      {
        {
          "Year",
          (JToken) lastDate.Year
        },
        {
          "Month",
          (JToken) lastDate.Month
        },
        {
          "Day",
          (JToken) lastDate.Day
        },
        {
          "Hour",
          (JToken) lastDate.Hour
        },
        {
          "Minute",
          (JToken) lastDate.Minute
        },
        {
          "Second",
          (JToken) lastDate.Second
        }
      }.ToString());
            Log.LogLine("Set LastDate to: " + (object)lastDate);
        }

        public static void ReadData()
        {
            string appSetting = ConfigurationManager.AppSettings["OracleConnectionString"];
            Log.LogLine("Conecction String to use: " + appSetting);
            using (OracleConnection oracleConnection = new OracleConnection())
            {
                oracleConnection.ConnectionString = appSetting;
                oracleConnection.Open();
                Log.LogLine("Connection Status: {0}", (object)oracleConnection.State);
                OracleCommand command = oracleConnection.CreateCommand();
                command.CommandText = Program.GetQuery();
                DateTime lastDate = Program.GetLastDate();
                OracleParameter oracleParameter = new OracleParameter("LastDate", (object)lastDate.ToString("MM/dd/yyyy H:mm:ss"));
                command.Parameters.Add(oracleParameter);
                OracleDataReader oracleDataReader = command.ExecuteReader();
                for (int ordinal = 0; ordinal < oracleDataReader.FieldCount; ++ordinal)
                    Log.LogText(oracleDataReader.GetName(ordinal) + " | ");
                Log.LogLine("");
                string columnDeviceName = ConfigurationManager.AppSettings["DeviceColumn"];
                string columnLastDate = ConfigurationManager.AppSettings["LastDateColumn"];
                while (oracleDataReader.Read())
                {
                    Hashtable values = new Hashtable();
                    for (int ordinal = 0; ordinal < oracleDataReader.FieldCount; ++ordinal)
                    {
                        values.Add((object)oracleDataReader.GetName(ordinal), oracleDataReader[ordinal]);
                        Log.LogText(oracleDataReader[oracleDataReader.GetName(ordinal)].ToString() + " | ");
                    }
                    IoTHubClient.SendToAzure((string)oracleDataReader[columnDeviceName], values);
                    lastDate = (DateTime)oracleDataReader[columnLastDate];
                }
                Log.LogLine("");
                Program.SetLastDate(lastDate);
            }
        }
    }
}
