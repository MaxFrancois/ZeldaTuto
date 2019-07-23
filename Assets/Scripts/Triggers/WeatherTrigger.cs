public class WeatherTrigger : Trigger
{
    public WeatherType WeatherType;
    public FloatSignal WeatherSignal;

    protected override void OnPlayerEnter()
    {
        WeatherSignal.Raise((float)WeatherType);
    }
}
