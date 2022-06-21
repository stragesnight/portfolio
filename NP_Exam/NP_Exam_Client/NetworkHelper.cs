using System;
using System.Threading.Tasks;
using NP_Exam_Library;

namespace NP_Exam_Client
{
    public static class NetworkHelper
    {
        private static readonly string _serverIP = "127.0.0.1";
        private static readonly int _serverPort = 1024;

        public static async Task<T> GetResponseAsync<T>(object request)
        {
            try
            {
                ServerResponse response = await new AsyncTCPClient().PerformConversationAsync(
                    _serverIP, _serverPort, request  
                ) as ServerResponse;

                return (T)response?.ResponseObject;
            }
            catch (Exception)
            { return default; }
        }
    }
}
