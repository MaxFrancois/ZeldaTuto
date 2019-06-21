using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class DeactivateObjectBehaviour : PlayableBehaviour
{
    public GameObject Target;
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        // Only execute in Play mode
        if (Application.isPlaying)
        {
            //// Execute your starting logic here, calling into a singleton for example
            Debug.Log("Clip started!");
            Target.SetActive(false);
        }
    }

    // source: https://forum.unity.com/threads/code-example-how-to-detect-the-end-of-the-playable-clip.659617/
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        // Only execute in Play mode
        if (Application.isPlaying)
        {
            var duration = playable.GetDuration();
            var time = playable.GetTime();
            var count = time + info.deltaTime;

            if ((info.effectivePlayState == PlayState.Paused && count > duration) || Mathf.Approximately((float)time, (float)duration))
            {
                // Execute your finishing logic here:
                Debug.Log("Clip done!");
            }
            return;
        }
    }
}