#if !NETFX_CORE

using UnityEngine;
using System;

namespace uOSC.DotNet
{

public class Thread : uOSC.Thread
{
    private System.Threading.Thread _thread;
    private bool _isRunning = false;
    private Action _loopFunc = null;

    public override void Start(Action loopFunc)
    {
        if (_isRunning || loopFunc == null) return;

        _isRunning = true;
        _loopFunc = loopFunc;

        _thread = new System.Threading.Thread(ThreadLoop);
        _thread.Start();
    }

    private void ThreadLoop()
    {
        while (_isRunning)
        {
            try
            {
                _loopFunc();
                System.Threading.Thread.Sleep(IntervalMillisec);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                Debug.LogError(e.StackTrace);
            }
        }
    }

    public override void Stop(int timeoutMilliseconds = 3000)
    {
        if (!_isRunning) return;

        _isRunning = false;

        if (_thread.IsAlive)
        {
            _thread.Join(timeoutMilliseconds);
            if (_thread.IsAlive)
            {
                _thread.Abort();
            }
        }
    }
}

}

#endif