using UnityEngine;
using UnityEngine.Playables;

public class DeactivateObjectMixerBehaviour : PlayableBehaviour
{
    // Override the ProcessFrame because we want to have our own color coded tracks
    // to keep things in the Editor visually clean
    public override void ProcessFrame(Playable playable, FrameData info, object playerData) { }
}