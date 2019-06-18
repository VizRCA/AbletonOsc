using System;
using System.Collections.Generic;
using UnityEngine;
using uOSC;

namespace AbletonOsc
{
    /// <summary>
    /// This was an initial idea, been replaced by simpler hashtable method in the server, means messages only need to be relayed once at point of being dequed
    /// </summary>
    public class CommandsLookup : MonoBehaviour
    {
        private IServer _server;

        // How to link address patterns to methods
        private Dictionary<string, Delegate> _commands = new Dictionary<string, Delegate>();


        // Use this for initialization
        private void Start()
        {
            _server = GetComponent<IServer>();
            _server.OnDataReceived.AddListener(OnDataReceived);
            _commands.Add(EnumUtils<LiveCommands>.GetCommand(LiveCommands.Start),
                new System.Action<float>(StartCommand));
        }

        private void OnDataReceived(Message message)
        {
            ParseCommand(message);
        }

        private void ParseCommand(Message packet)
        {
            if (_commands.ContainsKey(packet.Address))
            {
                _commands[packet.Address].DynamicInvoke(packet.Values[0]);
            }
        }

        private static void StartCommand(float input)
        {
            Debug.Log("Start commands received : " + input);
        }
    }
}