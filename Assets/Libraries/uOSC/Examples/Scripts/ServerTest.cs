using AbletonOsc;
using UnityEngine;

namespace uOSC
{

[RequireComponent(typeof(IServer))]
public class ServerTest : MonoBehaviour
{
    void Start()
    {
        var server = GetComponent<IServer>();
        server.OnDataReceived.AddListener(OnDataReceived);
    }

    void OnDataReceived(Message message)
    {
        // address
        var msg = message.Address + ": ";

        // timestamp
        msg += "(" + message.Timestamp.ToLocalTime() + ") ";

        // values
        foreach (var value in message.Values)
        {
            msg += value.GetString() + " ";
        }

        Debug.Log(msg);
    }
}

}