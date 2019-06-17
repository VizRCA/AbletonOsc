using UnityEngine;
using uOSC;

namespace AbletonOsc.Examples
{
    public class SendMessageExample : MonoBehaviour
    {
        private void Start()
        {
            // Send a message to the live api to change song quantization at startup.
            string address = "/song/quantization";
            int sixteenths = 11; // 11 == 1/16 in live quantization labels.
            Message setQuantization = new Message(address, sixteenths);

            Message sendMultipleDataTypes =
                new Message("/sum/funky/parameter", 1.4f, 1, "words", new byte[] {1, 5, 3, 7});
            
            LiveOscManager.Instance.Send(setQuantization);
            LiveOscManager.Instance.Send(sendMultipleDataTypes);
        }
    }
}