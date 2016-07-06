using log4net;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using vtortola.WebSockets;
using vtortola.WebSockets.Rfc6455;

namespace WSDemo.Server
{
    public delegate void WebSocketEventListenerOnConnect(WebSocket webSocket);
    public delegate void WebSocketEventListenerOnDisconnect(WebSocket webSocket);
    public delegate void WebSocketEventListenerOnMessage(WebSocket webSocket, byte[] data);
    public delegate void WebSocketEventListenerOnError(WebSocket webSocket, Exception error);

    public class WebSocketEventListener : IDisposable
    {
        public event WebSocketEventListenerOnConnect OnConnect;
        public event WebSocketEventListenerOnDisconnect OnDisconnect;
        public event WebSocketEventListenerOnMessage OnMessage;
        public event WebSocketEventListenerOnError OnError;

        readonly WebSocketListener _listener;
        readonly ILog _log;
        readonly IPEndPoint _endpoint;
        readonly CancellationTokenSource _cancellation;

        public WebSocketEventListener(ILog log, IPEndPoint endpoint, string certThumbprint = null)
        {
            _endpoint = endpoint;
            _log = log;
            _cancellation = new CancellationTokenSource();

            var options = new WebSocketListenerOptions();
            options.NegotiationQueueCapacity = 256;
            options.ParallelNegotiations = 32;
            _listener = new WebSocketListener(_endpoint, options);
            RegisterWss(certThumbprint, _listener);
            _listener.Standards.RegisterStandard(new WebSocketFactoryRfc6455(_listener));
        }

        public void Start()
        {
            _listener.Start();
            _log.InfoFormat("Websocket Listener Started At {0}", _endpoint);
            Task.Run((Func<Task>)ListenAsync);
        }

        public void Stop()
        {
            _cancellation.Cancel();
            _listener.Stop();
        }

        private async Task ListenAsync()
        {
            while (_listener.IsStarted)
            {
                try
                {
                    var webSocket = await _listener.AcceptWebSocketAsync(_cancellation.Token)
                                                           .ConfigureAwait(false);
                    if (webSocket != null)
                        Task.Run(() => HandleWebSocketAsync(webSocket));
                }
                catch (Exception ex)
                {
                    _log.Error("Websocket Accept Async", ex);
                }
            }
        }

        private async Task HandleWebSocketAsync(WebSocket webSocket)
        {
            try
            {
                if (OnConnect != null)
                    OnConnect.Invoke(webSocket);

                while (webSocket.IsConnected)
                {
                    var readStream = await webSocket.ReadMessageAsync(_cancellation.Token);
                    if (readStream == null)
                        break;
                    using (var ms = new MemoryStream())
                    {
                        await readStream.CopyToAsync(ms);
                        var bytes = ms.ToArray();
                        if (0 < bytes.Length && OnMessage != null)
                            OnMessage.Invoke(webSocket, bytes);
                    }
                }

                if (OnDisconnect != null)
                    OnDisconnect.Invoke(webSocket);
            }
            catch (Exception ex)
            {
                if (OnError != null)
                    OnError.Invoke(webSocket, ex);
            }
            finally
            {
                webSocket.Dispose();
            }
        }

        private void RegisterWss(string certThumbprint, WebSocketListener listener)
        {
            if (string.IsNullOrEmpty(certThumbprint))
            {
                _log.Info("Insecure Websockets Used");
                return;
            }

            try
            {
                var store = new X509Store(StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadOnly);
                var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, certThumbprint, false);
                if (certificates.Count != 0)
                {
                    var cert = certificates[0];
                    listener.ConnectionExtensions.RegisterExtension(new WebSocketSecureConnectionExtension(cert));
                    _log.Info("Websocket certificate found and binded " + certificates.Count);
                }
                else
                    _log.Warn("Websocket certificate not found");
                store.Close();
            }
            catch (Exception ex)
            {
                _log.Warn("Websocket certificate binding failed", ex);
            }
        }

        public void Dispose()
        {
            _listener.Dispose();
        }
    }
}
