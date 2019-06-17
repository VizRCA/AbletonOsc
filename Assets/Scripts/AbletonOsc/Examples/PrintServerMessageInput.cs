using UnityEngine;
using uOSC;

namespace AbletonOsc.Examples
{
    public class PrintServerMessageInput : MonoBehaviour
    {
        private void Start()
        {
            LiveOscManager.Instance.OnDataReceived.AddListener(OnDataReceived);
        }

        private static void OnDataReceived(Message message)
        {
            // address
            var msg = message.address + " : ";

            // timestamp
            msg += "(" + message.timestamp.ToLocalTime() + ") ";

            // values
            foreach (var value in message.values)
            {
                msg += value.GetString() + " ";
            }

            Debug.Log(msg);
        }
    }
}