# AbletonOsc
Project for sending spatial data to ableton to control sound parameters

## How to

For sending and receiving data to Ableton Live from Unity there is the `LiveOscManager` singleton class in the `AbletonOsc` namespace. This wraps functionality for the OSC library, [uOSC](1).

### Sending data

Using the `LiveOscManager` from any script you just define the address and data you wish to send and pass to the class instance, like in the `SendStartCommand` example:

```csharp
public class SendStartCommand : MonoBehaviour
{
    // If you pass this command to a LiveAPI object you can trigger start/stop
    public string startCommand = "/song/start";

    private void Start()
    {
        LiveOscManager.Instance.Send(startCommand, 1);
    }
}
```

If you wish to send messages yourself, like in `SendMessageExample`:

```csharp
public class SendMessageExample : MonoBehaviour
{
    private void Start()
    {
        // Send a message to the live api to change song quantization at startup.
        string address = "/song/quantization";
        int sixteenths = 11; // 11 == 1/16 in live quantization labels.
        Message setQuantization = new Message(address, sixteenths);

        Message sendMultipleDataTypes =
            new Message("/sum/funky/parameterAddress", 1.4f, 1, "words", new byte[] {1, 5, 3, 7});

        LiveOscManager.Instance.Send(setQuantization);
        LiveOscManager.Instance.Send(sendMultipleDataTypes);
    }
}
```

### Receiving data

To see data from ableton, just subscribe to the `OnDataReceived` event (type of `DataReceiveEvent : UnityEvent<Message>`), like the `PrintServerMessageInput`:

```csharp
public class PrintServerMessageInput : MonoBehaviour
{
    private void Start()
    {
        LiveOscManager.Instance.OnDataReceived.AddListener(OnDataReceived);
    }

    private static void OnDataReceived(Message message)
    {
        // address
        var msg = message.address + " : ";

        // timestamp
        msg += "(" + message.timestamp.ToLocalTime() + ") ";

        // values
        foreach (var value in message.values)
        {
            msg += value.GetString() + " ";
        }

        Debug.Log(msg);
    }
}
```
A simple receiver adds its message address to the manager, using `SetAddressHandler`, then does stuff when a received message it matches the address, like `SimpleMessageChecker` script (use the `SimpleMessageCheckerTester` max patch to send data):
```csharp
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
        if (message.values.Length <= 0) return;

        var value = message.GetFloat(0);

        var scale = new Vector3(value, value, value);

        transform.localScale = scale;
    }
}
```

### Data types

+ Int :: `int`
+ Float :: `float`
+ String :: `string`
+ Blobs :: `byte[]`

Do not add things like `float[]` to message parameters, these will be ignored by the client sending process, add each one separately. E.g
```csharp
// THIS WILL NOT WORK
float[] postionXyz = new float[]{1f,2f,3f};
LiveOscManager.Instance.Send("/object/x/position", positionXyz);

// DO THIS
LiveOscManager.Instance.Send("/object/x/position", positionXyz[0], positionXyz[1], positionXyz[1]);
```

If you are sending lots of arrays, you can write a wrapper to convert arrays to lists of object suitable for the clent sending process.

To get data from a message, you can get the type using its index in the message parameters array
```csharp
private void OnDataReceived(Message message)
{
    // Example message is "/fromlive/parameter 0.5f 4 clipped"
    if (message.values.Length <= 3) return;

    var floatValue = message.GetFloat(0);

    var intValue = message.GetInt(1);
    
    var stringValue = message.GetString(2);
}
```

----
### Transform Sender Script

This script collects most of the spatial data needed for passing to ableton. You can enable different send type (position, euler rotation, scale), and it has a set of mapping functions available in the editor.

You can test this function using the `OSC Bundle` M4L patch. The naming is specific, you send `/object/x`, where `x` is is a number 0...n. The data parameter is automatically added to messages in the bundle i.e. sending all data types per update you would receive the following OSC messages (from the bundle) in Ableton Live:
```
/object/1/position 0.5 0.3 0.2
/object/1/rotation 130 145.5 99.3
/object/1/scale 1 1 1
```
You can then use the mapping buttons in the M4L patch to connect this to functionality in Live.


## TODO

+ [ ] Add LiveAPI access through javascript in maxpatch
+ [x] Finish ~~~CommandsLookup system~~~ hashtable look up system to reply to incoming OSC message with function calls and events.

[1]:https://github.com/hecomi/uOSC