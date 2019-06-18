using System.Collections;
using AbletonOsc;
using UnityEngine;
using UnityEngine.Events;

namespace uOSC
{
    public class uOscServer : MonoBehaviour, IServer
    {
        [SerializeField] int port = 3333;

#if NETFX_CORE
    Udp udp_ = new Uwp.Udp();
    Thread thread_ = new Uwp.Thread();
#else
        Udp udp_ = new DotNet.Udp();
        Thread thread_ = new DotNet.Thread();
#endif
        Parser parser_ = new Parser();
        Hashtable _addressTable;

        public DataReceiveEvent OnDataReceived { get; set; }

        public void SetAddressHandler(string address, MessageHandler messageHandler)
        {
            var al = (ArrayList) Hashtable.Synchronized(_addressTable)[address];

            if (al == null)
            {
                al = new ArrayList();
                al.Add(messageHandler);
                Hashtable.Synchronized(_addressTable).Add(address, al);
            }
            else
            {
                al.Add(messageHandler);
            }
        }

        void Awake()
        {
            _addressTable = new Hashtable();
            OnDataReceived = new DataReceiveEvent();
        }

        void OnEnable()
        {
            udp_.StartServer(port);
            thread_.Start(UpdateMessage);
        }

        void OnDisable()
        {
            thread_.Stop();
            udp_.Stop();
        }

        void Update()
        {
            while (parser_.MessageCount > 0)
            {
                var message = parser_.Dequeue();
                OnDataReceived.Invoke(message);
                var al = (ArrayList) Hashtable.Synchronized(_addressTable)[message.Address];

                if (al == null) continue;

                foreach (MessageHandler h in al)
                {
                    h(message);
                }
            }
        }

        void UpdateMessage()
        {
            while (udp_.MessageCount > 0)
            {
                var buf = udp_.Receive();
                int pos = 0;
                parser_.Parse(buf, ref pos, buf.Length);
            }
        }
    }
}