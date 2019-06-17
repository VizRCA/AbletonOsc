using System;
using System.Collections.Generic;
using UnityEngine;
using uOSC;

namespace AbletonOsc
{
    public class CommandsLookup : MonoBehaviour {
        private IServer _server;

        // How to link address patterns to methods
        private Dictionary<string, Delegate> _commands = new Dictionary<string, Delegate>();


        // Use this for initialization
        private void Start () {
            _server = GetComponent<IServer>();
            _server.OnDataReceived.AddListener(OnDataReceived);
            _commands.Add(EnumUtils<LiveCommands>.GetCommand(LiveCommands.Start), new System.Action<float>(StartCommand));

        }

        private void OnDataReceived(Message message)
        {
            // address
            var msg = message.address + ": ";

            // timestamp
            msg += "(" + message.timestamp.ToLocalTime() + ") ";

            // values
            foreach (var value in message.values)
            {
                msg += value.GetString() + " ";
            }

            Debug.Log(msg);

            ParseCommand(message);
        }

        private void ParseCommand(Message packet)
        {
            if (_commands.ContainsKey(packet.address))
            {
                _commands[packet.address].DynamicInvoke(packet.values[0]);
            }

        }

        private static void StartCommand(float input)
        {
            Debug.Log("Start commands received : " + input);
        }
    }
}
