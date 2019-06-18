using System.IO;
using System.Collections.Generic;

namespace uOSC
{

public class Bundle
{
    private Timestamp _timestamp;
    private List<object> _elements = new List<object>();

    public Bundle()
    {
        _timestamp = Timestamp.Immediate;
    }

    public Bundle(Timestamp timestamp)
    {
        _timestamp = timestamp;
    }

    public void Add(Message message)
    {
        _elements.Add(message);
    }

    public void Add(Bundle bundle)
    {
        _elements.Add(bundle);
    }

    public void Write(MemoryStream stream)
    {
        Writer.Write(stream, Identifier.Bundle);
        Writer.Write(stream, _timestamp);

        for (int i = 0; i < _elements.Count; ++i)
        {
            var elem = _elements[i];
            if (elem is Message)
            {
                Write(stream, (Message)elem);
            }
            else if (elem is Bundle)
            {
                Write(stream, (Bundle)elem);
            }
        }
    }

    private void Write(MemoryStream stream, Message message)
    {
        using (var tmpStream = new MemoryStream())
        {
            message.Write(tmpStream);
            Writer.Write(stream, tmpStream);
        }
    }

    private void Write(MemoryStream stream, Bundle bundle)
    {
        using (var tmpStream = new MemoryStream())
        {
            bundle.Write(tmpStream);
            Writer.Write(stream, tmpStream);
        }
    }
}

}