#if !NETFX_CORE

using UnityEngine;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace uOSC.DotNet
{

public class Udp : uOSC.Udp
{
    private enum State
    {
        Stop,
        Server,
        Client,
    }

    private State _state = State.Stop;

    private Queue<byte[]> _messageQueue = new Queue<byte[]>();
    private object _lockObject = new object();

    private UdpClient _udpClient;
    private IPEndPoint _endPoint;
    private Thread _thread = new Thread();

    public override int MessageCount
    {
        get { return _messageQueue.Count; }
    }

    public override void StartServer(int port)
    {
        Stop();
        _state = State.Server;

        _endPoint = new IPEndPoint(IPAddress.Any, port);
        _udpClient = new UdpClient(_endPoint);
        _thread.Start(() => 
        {
            while (_udpClient.Available > 0) 
            {
                var buffer = _udpClient.Receive(ref _endPoint);
                lock (_lockObject)
                {
                    _messageQueue.Enqueue(buffer);
                }
            }
        });
    }

    public override void StartClient(string address, int port)
    {
        Stop();
        _state = State.Client;

        var ip = IPAddress.Parse(address);
        _endPoint = new IPEndPoint(ip, port);
        _udpClient = new UdpClient();
    }

    public override void Stop()
    {
        if (_state == State.Stop) return;

        _thread.Stop();
        _udpClient.Close();
        _state = State.Stop;
    }

    public override void Send(byte[] data, int size)
    {
        _udpClient.Send(data, size, _endPoint);
    }

    public override byte[] Receive()
    {
        byte[] buffer;
        lock (_lockObject)
        {
            buffer = _messageQueue.Dequeue();
        }
        return buffer;
    }
}

}

#endif