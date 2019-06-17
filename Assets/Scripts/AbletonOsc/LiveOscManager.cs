using System.Collections.Generic;
using System.IO;
using UnityEngine;
using uOSC;

namespace AbletonOsc
{
    public class LiveOscManager : MonoBehaviour, IServer, IClient
    {

        [SerializeField] private int serverPort = 8888;
        private Udp _udpServer = new uOSC.DotNet.Udp();
        private Thread _threadServer = new uOSC.DotNet.Thread();
        private Parser _parser = new Parser();
        public DataReceiveEvent OnDataReceived { get; set; }



        private const int BufferSize = 8192;
        private const int MaxQueueSize = 100;
        [SerializeField] private string address = "127.0.0.1";
        [SerializeField] private int clientPort = 7777;
        private Queue<object> _messages = new Queue<object>();
        private object _lockObject = new object();
        private Udp _udpClient = new uOSC.DotNet.Udp();
        private Thread _threadClient = new uOSC.DotNet.Thread();

        private void Awake()
        {
            OnDataReceived = new DataReceiveEvent();
            Init();
        }

        private void OnEnable()
        {
            _udpServer.StartServer(serverPort);
            _threadServer.Start(UpdateMessage);


            _udpClient.StartClient(address, clientPort);
            _threadClient.Start(UpdateSend);
        }

        private void OnDisable()
        {
            _threadServer.Stop();
            _udpServer.Stop();


            _threadClient.Stop();
            _udpClient.Stop();
        }

        private void Update()
        {
            while (_parser.messageCount > 0)
            {
                var message = _parser.Dequeue();
                OnDataReceived.Invoke(message);
            }
        }

        private void UpdateMessage()
        {
            while (_udpServer.messageCount > 0)
            {
                var buf = _udpServer.Receive();
                int pos = 0;
                _parser.Parse(buf, ref pos, buf.Length);
            }
        }


        private void UpdateSend()
        {
            while (_messages.Count > 0)
            {
                object message;
                lock (_lockObject)
                {
                    message = _messages.Dequeue();
                }

                using (var stream = new MemoryStream(BufferSize))
                {
                    if (message is Message)
                    {
                        ((Message)message).Write(stream);
                    }
                    else if (message is Bundle)
                    {
                        ((Bundle)message).Write(stream);
                    }
                    else
                    {
                        return;
                    }
                    _udpClient.Send(Util.GetBuffer(stream), (int)stream.Position);
                }
            }
        }

        private void Add(object data)
        {
            lock (_lockObject)
            {
                _messages.Enqueue(data);

                while (_messages.Count > MaxQueueSize)
                {
                    _messages.Dequeue();
                }
            }
        }

        public void Send(string address, params object[] values)
        {
            Send(new Message()
            {
                address = address,
                values = values
            });
        }

        public void Send(Message message)
        {
            Add(message);
        }

        public void Send(Bundle bundle)
        {
            Add(bundle);
        }

        #region Singleton

        static LiveOscManager _instance;

        public static LiveOscManager Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                _instance = FindObjectOfType<LiveOscManager>();
                if (_instance != null)
                    return _instance;

                var obj = new GameObject("LiveOscManager");
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localRotation = Quaternion.identity;
                obj.transform.localScale = Vector3.one;
                _instance = obj.AddComponent<LiveOscManager>();
                return _instance;
            }

            set { _instance = value; }
        }

        private void Init()
        {
            Instance = this;
        }

        

        #endregion

    }
}

