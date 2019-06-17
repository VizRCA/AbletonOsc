
namespace AbletonOsc
{
	
	public enum OscCommands
	{
        [OscCommand("/do/this/now")]
        DoThis,
        [OscCommand("/do/that/now")]
        DoSomethingElse
	}

    public enum LiveCommands
    {
        [OscCommand("/song/start")]
        Start,
        [OscCommand("/clip/fire")]
        Fire,
        [OscCommand("/clip/stop")]
        Stop,
        [OscCommand("/song/quantization")]
        SetQuantization
    }

	public enum Quantization
	{
		None,
		EightBars, 
		FourBars,
		TwoBars, 
		OneBar,
		HalfBar,
		HalfT,
		Quarter,
		QuarterT,
		Eighth,
		EighthT,
		Sixteenth,
		SixteenthT,
		ThirtySecond
	}

}