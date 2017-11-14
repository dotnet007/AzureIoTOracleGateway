using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Configuration;
using System.Text;

namespace AzureIoT.OracleGateway
{
    public class IoTHubClient
    {
        private static Hashtable Devices;

        public static async void SendToAzure(string deviceId, Hashtable values)
        {
            deviceId = IoTHubClient.ReplaceBadChracters(deviceId);
            if (IoTHubClient.Devices == null)
                IoTHubClient.Devices = new Hashtable();
            if (!IoTHubClient.Devices.Contains((object)deviceId))
                IoTHubClient.Devices.Add((object)deviceId, (object)DeviceClient.CreateFromConnectionString(ConfigurationManager.AppSettings[deviceId + "_IoTHubCS"]));
            DeviceClient client = (DeviceClient)IoTHubClient.Devices[(object)deviceId];
            Message msg = new Message(Encoding.UTF8.GetBytes(IoTHubClient.BuildMessage(values)));
            await client.SendEventAsync(msg);
            Log.LogLine("Sent message to Azure.");
        }

        private static string BuildMessage(Hashtable values)
        {
            JObject jobject = new JObject();
            jobject.Add(ConfigurationManager.AppSettings["MessageCreationDateName"], (JToken)DateTime.Now);
            foreach (object key in (IEnumerable)values.Keys)
                jobject.Add((string)key, JToken.FromObject(values[key]));
            return jobject.ToString();
        }

        private static string ReplaceBadChracters(string deviceId)
        {
            return deviceId.ToUpper().Replace(" ", "").Replace("Á", "A").Replace("É", "E").Replace("Í", "I").Replace("Ó", "O").Replace("Ú", "U").Replace("Ñ", "N");
        }
    }
}
