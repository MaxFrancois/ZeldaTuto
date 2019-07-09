using UnityEngine;

[CreateAssetMenu(fileName = "FadePanel", menuName = "Signals/FadePanelSignal")]
public class FadePanelSignal : SignalBase
{
    public FadeType Type;
    public Color Color;
    public int Duration;

    public void Raise()
    {
        for (int i = Listeners.Count - 1; i >= 0; i--)
        {
            (Listeners[i] as FadePanelSignalListener).OnSignalRaised(Type, Color, Duration);
        }
    }
}