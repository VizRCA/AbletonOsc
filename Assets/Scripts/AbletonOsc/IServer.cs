using UnityEngine.Events;
using uOSC;

namespace AbletonOsc
{
    public interface IServer
    {
        DataReceiveEvent OnDataReceived { get; set; }
    }

    public class DataReceiveEvent : UnityEvent<Message> { };

}