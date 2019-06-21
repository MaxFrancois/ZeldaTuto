using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class DeactivateObjectClip : PlayableAsset, ITimelineClipAsset
{
    public ExposedReference<GameObject> Target;
    // Create the runtime version of the clip, by creating a copy of the template
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        ScriptPlayable<DeactivateObjectBehaviour> playable = ScriptPlayable<DeactivateObjectBehaviour>.Create(graph);

        DeactivateObjectBehaviour playableBehaviour = playable.GetBehaviour();

        playableBehaviour.Target = Target.Resolve(graph.GetResolver());

        return playable;
    }

    // Make sure we disable all blending since we aren't handling that in the mixer
    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }
}