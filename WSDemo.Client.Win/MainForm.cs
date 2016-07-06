using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WSDemo.Client.Win
{
    public partial class MainForm : Form
    {
        private readonly Action<string> _appendLog;

        private readonly Action<byte[]> _appendLogBytes;

        private CancellationTokenSource _tokenSource;

        private bool _isConnected;

        private readonly string _userDataFile = ConfigurationManager.AppSettings["UserDataFile"];

        public MainForm()
        {
            InitializeComponent();

            _appendLog = s => tbLog.AppendText(string.Format("{0}\n", s));
            _appendLogBytes = b =>
            {
                tbLog.AppendText(string.Format("{0}", new string(Encoding.UTF8.GetChars(b))));
                tbLog.AppendText("\n");
            };
            _tokenSource = new CancellationTokenSource();

            SetBtnAvailability();
        }

        private void btConnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbUser.Text) || string.IsNullOrWhiteSpace(tbPassword.Text))
            {
                _appendLog("Cancelled: Empty <User name> or <Password>");
                return;
            }
            Task.Run(async () =>
            {
                var socket = new ClientWebSocket();
                _tokenSource = new CancellationTokenSource();

                tbLog.Invoke(_appendLog, "Connecting..");
                await socket.ConnectAsync(new Uri(ConfigurationManager.AppSettings["ServerAddr"]), _tokenSource.Token);
                tbLog.Invoke(_appendLog, "Connected");
                var message = GetMessage();
                await Task.WhenAll(Receive(socket), Send(socket, message));
            });
            _isConnected = !_isConnected;
            SetBtnAvailability();
        }

        private void btDisconnect_Click(object sender, EventArgs e)
        {
            _tokenSource.Cancel();
            _isConnected = !_isConnected;
            SetBtnAvailability();
            _appendLog("Disconnected");
        }

        async Task Receive(WebSocket socket)
        {
            var buffer = new byte[8192];
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), _tokenSource.Token);
                if (result.MessageType == WebSocketMessageType.Close || _tokenSource.IsCancellationRequested)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                }
                else
                {
                    tbLog.Invoke(_appendLogBytes, buffer);
                }
            }
        }

        async Task Send(WebSocket socket, string message)
        {
            await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)), WebSocketMessageType.Text, true,
                _tokenSource.Token);
            tbLog.Invoke(_appendLog, "Data sent");
            while (socket.State == WebSocketState.Open)
            {
                if (_tokenSource.IsCancellationRequested)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "User requested disconnect",
                        CancellationToken.None);
                    break;
                }
                await Task.Delay(10);
            }
        }

        private void SetBtnAvailability()
        {
            btConnect.Enabled = !_isConnected;
            btDisconnect.Enabled = _isConnected;
        }

        private string GetMessage()
        {
            var data = GetUserData();
            var obj = new
            {
                method = "POST",
                uri = "/users/accessToken",
                clientID = data.ClientID,
                headers = new { authorization = tbPassword.Text },
                @params = new
                {
                    userID = tbUser.Text,
                    clientOS = "Windows",
                    clientID = data.DeviceID
                },
                body =
                    new
                    {
                        userKey = data.UserKey
                    }
            };
            return JsonConvert.SerializeObject(obj);
        }

        private UserData GetUserData()
        {
            if (File.Exists(_userDataFile))
            {
                var lines = File.ReadAllLines(_userDataFile);
                return new UserData { ClientID = lines[0], DeviceID = lines[1], UserKey = lines[2] };
            }
            var data = new UserData();
            data.ClientID = new Random().Next(int.MaxValue).ToString();
            data.DeviceID = string.Format("{0}-{1}-{2}-{3}", // seeding different values to RNG
                new Random(DateTime.Now.Year).Next(100000, 999999),
                new Random(DateTime.Now.Month).Next(1000000, 9999999),
                new Random(DateTime.Now.Day).Next(100000, 999999),
                new Random(DateTime.Now.Hour).Next(10000, 99999));
            data.UserKey = GenerateRSA2048String();
            File.WriteAllLines(_userDataFile, new[] { data.ClientID, data.DeviceID, data.UserKey });
            _appendLog("User data file saved");
            return data;
        }

        private string GenerateRSA2048String()
        {
            return BitConverter.ToString(GenerateRSA2048()).Replace("-", "");
        }

        private byte[] GenerateRSA2048()
        {
            byte[] result = null;
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    var keyInfo = rsa.ExportParameters(false);
                    result = keyInfo.Modulus;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
            return result;
        }

        private void btDeleteKeyPair_Click(object sender, EventArgs e)
        {
            if (File.Exists(_userDataFile))
            {
                File.Delete(_userDataFile);
                _appendLog("User data file deleted");
            }
        }
    }
}
