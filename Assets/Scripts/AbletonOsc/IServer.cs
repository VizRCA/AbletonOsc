using UnityEngine.Events;
using uOSC;

namespace AbletonOsc
{
    public interface IServer
    {
        DataReceiveEvent OnDataReceived { get; set; }
        void SetAddressHandler(string address, MessageHandler messageHandler);
    }

    public class DataReceiveEvent : UnityEvent<Message> { };

    public delegate void MessageHandler(Message msg);

}