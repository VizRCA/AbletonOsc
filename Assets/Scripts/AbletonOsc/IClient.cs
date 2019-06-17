using uOSC;

namespace AbletonOsc
{
    public interface IClient
    {
        void Send(string address, params object[] values);


        void Send(Message message);


        void Send(Bundle bundle);

    }
}