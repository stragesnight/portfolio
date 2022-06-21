using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;

namespace NP_Exam_Server
{
    public class AsyncTCPServer
    {
        public delegate object ResponseDelegate(object request);

        private ResponseDelegate _responseDelegate = null;
        private TcpListener _listener = null;
        private bool _keepRunning = false;

        public AsyncTCPServer(string ipString, int port, ResponseDelegate response)
        {
            _responseDelegate = response;
            _listener = new TcpListener(IPAddress.Parse(ipString), port);
        }

        public async Task StartAsync()
        {
            _listener.Start();
            _keepRunning = true;

            while (_keepRunning)
            {
                TcpClient client = await _listener.AcceptTcpClientAsync();
                await HandleConversationAsync(client);
            }

            Console.WriteLine("Сервер завершил свою работу");
            _listener.Stop();
        }

        public void Stop()
        {
            _keepRunning = false;
        }

        private async Task HandleConversationAsync(TcpClient client)
        {
            NetworkStream ns = client.GetStream();
            object request = await ReadObjectAsync(ns);
            if (request == null)
                return;
            object response = _responseDelegate.Invoke(request);
            await WriteObjectAsync(response, ns);
        }

        private async Task<object> ReadObjectAsync(NetworkStream ns)
        {
            byte[] buffer = new byte[1024];

            try
            {
                do
                {
                    await ns.ReadAsync(buffer, buffer.Length - 1024, 1024);
                    Array.Resize(ref buffer, buffer.Length + 1024);
                } while (ns.DataAvailable);

                Console.WriteLine($"\nReceived {buffer.Length} bytes");

                using (MemoryStream ms = new MemoryStream(buffer))
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
