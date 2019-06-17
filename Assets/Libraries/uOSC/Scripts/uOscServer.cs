using AbletonOsc;
using UnityEngine;
using UnityEngine.Events;

namespace uOSC
{

    public class uOscServer : MonoBehaviour, IServer
{
    [SerializeField]
    int port = 3333;

#if NETFX_CORE
    Udp udp_ = new Uwp.Udp();
    Thread thread_ = new Uwp.Thread();
#else
    Udp udp_ = new DotNet.Udp();
    Thread thread_ = new DotNet.Thread();
#endif
    Parser parser_ = new Parser();

    public DataReceiveEvent OnDataReceived { get;  set; }

    void Awake()
    {
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
        while (parser_.messageCount > 0)
        {
            var message = parser_.Dequeue();
            OnDataReceived.Invoke(message);
        }
    }

    void UpdateMessage()
    {
        while (udp_.messageCount > 0) 
        {
            var buf = udp_.Receive();
            int pos = 0;
            parser_.Parse(buf, ref pos, buf.Length);
        }
    }
}

}