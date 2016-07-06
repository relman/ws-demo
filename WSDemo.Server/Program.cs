using log4net;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net;
using System.Text;
using vtortola.WebSockets;

namespace WSDemo.Server
{
    class Program
    {
        static readonly ILog Log = LogManager.GetLogger("defaultLogger");

        static void Main(string[] args)
        {
            var wsListener = new WebSocketEventListener(Log, new IPEndPoint(IPAddress.Any, GetWebsocketPort()));
            wsListener.OnConnect += wsListener_OnConnect;
            wsListener.OnDisconnect += wsListener_OnDisconnect;
            wsListener.OnError += wsListener_OnError;
            wsListener.OnMessage += wsListener_OnMessage;
            wsListener.Start();
            Console.WriteLine("Server started. Press Enter to Exit  ...");
            Console.ReadLine();
        }

        static void wsListener_OnMessage(WebSocket websocket, byte[] data)
        {
            var text = Encoding.UTF8.GetChars(data);
            dynamic d = JsonConvert.DeserializeObject(new string(text));
            var cliendId = d.clientID;
            var userId = d.@params.userID;
            var password = d.headers.authorization;
            Log.InfoFormat("Message received from: {0}, clientId: {1}, userId: {2}", websocket.RemoteEndpoint, cliendId, userId);
            object obj;
            if (AuthUser(userId.ToString(), password.ToString()))
            {
                obj = new { method = "POST", uri = "/users/accessToken", status = "200", clientID = cliendId };
            }
            else
            {
                obj = new { method = "POST", uri = "/users/accessToken", status = "403", clientID = cliendId };
            }
            if (!websocket.IsConnected)
            {
                Log.Error("Socket closed");
                return;
            }
            var message = JsonConvert.SerializeObject((object)obj);
            websocket.WriteString(message);
        }

        static void wsListener_OnConnect(WebSocket websocket)
        {
            Log.InfoFormat("Remote ws connected: {0}", websocket.RemoteEndpoint);
        }

        static void wsListener_OnDisconnect(WebSocket webSocket)
        {
            Log.InfoFormat("Remote ws disconnected: {0}", webSocket.RemoteEndpoint);
        }

        static void wsListener_OnError(WebSocket websocket, Exception error)
        {
            Log.Error("Unhandled error", error);
        }

        static int GetWebsocketPort()
        {
            var portStr = ConfigurationManager.AppSettings["WebsocketPort"];
            return string.IsNullOrWhiteSpace(portStr) ? 8080 : int.Parse(portStr);
        }

        static bool AuthUser(string username, string password)
        {
            var users = ConfigurationManager.AppSettings["Users"].Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var user in users)
            {
                var keyValue = user.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                if (keyValue[0] == username && keyValue[1] == password)
                    return true;
            }
            return false;
        }
    }
}
