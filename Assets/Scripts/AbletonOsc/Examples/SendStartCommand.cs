using UnityEngine;

namespace AbletonOsc.Examples
{
    public class SendStartCommand : MonoBehaviour
    {
        public string startCommand = "/song/start";

        private void Start()
        {
            LiveOscManager.Instance.Send(startCommand, 1);
        }
    }
}