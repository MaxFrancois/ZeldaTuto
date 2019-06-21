using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(1.0f, 0.0f, 0.0f)]
[TrackClipType(typeof(DeactivateObjectClip))]
// No track binding since we're executing general logic during our scene
public class DeactivateObjectTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        // This Mixer won't do anything, but I want to use our own track type instead of a PlayableTrack to keep things simple
        return ScriptPlayable<DeactivateObjectMixerBehaviour>.Create(graph, inputCount);
    }
}