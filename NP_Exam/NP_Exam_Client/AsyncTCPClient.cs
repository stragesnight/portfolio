using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;

namespace NP_Exam_Client
{
    public class AsyncTCPClient
    {
        public async Task<object> PerformConversationAsync(string ipString, int port, object request)
        {
            TcpClient client = new TcpClient();
            await client.ConnectAsync(ipString, port);
            NetworkStream ns = client.GetStream();
            
            await WriteObjectAsync(request, ns);
            return await ReadObjectAsync(ns);
        }

        private async Task<object> ReadObjectAsync(NetworkStream ns)
        {
            //while (_client.Available < 1)
            //    Thread.Sleep(10);

            byte[] bytes = new byte[512];

            try
            {
                do
                {
                    await ns.ReadAsync(bytes, bytes.Length - 512, 512);
                    Array.Resize(ref bytes, bytes.Length + 512);
                } while (ns.DataAvailable);

                Console.WriteLine($"Received {bytes.Length} bytes");

                using (MemoryStream ms = new MemoryStream(bytes))
                    return new BinaryFormatter().Deserialize(ms);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private async Task WriteObjectAsync(object obj, NetworkStream ns)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    new BinaryFormatter().Serialize(ms, obj);
                    byte[] bytes = ms.ToArray();
                    await ns.WriteAsync(bytes, 0, bytes.Length);
                    Console.WriteLine($"Sent {bytes.Length} bytes");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
