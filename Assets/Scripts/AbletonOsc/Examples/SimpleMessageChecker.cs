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
            LiveOscManager.Instance.OnDataReceived.AddListener(OnDataReceived);
        }

        private void OnDataReceived(Message message)
        {
            if (message.address != responseAddress) return;

            if (message.values.Length <= 0) return;

            var value = message.values[0];

            if (!(value is float)) return;

            var floatNum = (float) value;
            var scale = new Vector3(floatNum, floatNum, floatNum);
            transform.localScale = scale;
        }
    }
}