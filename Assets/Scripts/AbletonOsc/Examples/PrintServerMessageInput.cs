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
            var msg = message.Address + " : ";

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