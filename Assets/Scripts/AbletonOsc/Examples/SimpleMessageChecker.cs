using UnityEngine;
using uOSC;

namespace AbletonOsc.Examples
{
    public class SimpleMessageChecker : MonoBehaviour
    {
        // You can use the SimpleMessageCheckerTester.maxpat to send data for testing.

        // Imagine sending track volume and mapping it to scale
        public string responseAddress = "/track/1/volume";

        private void Start()
        {
            LiveOscManager.Instance.SetAddressHandler(responseAddress, OnDataReceived);
        }

        private void OnDataReceived(Message message)
        {
            if (message.Values.Length <= 0) return;

            var value = message.GetFloat(0);

            var scale = new Vector3(value, value, value);

            transform.localScale = scale;
        }
    }
}