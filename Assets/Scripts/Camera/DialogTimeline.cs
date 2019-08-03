using Cinemachine;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DialogTimeline : MonoBehaviour
{
    [SerializeField] PlayableDirector TimelineDirector;
    bool awaitInput = false;
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.isTrigger)
        {
            try
            {
                var cameraTrack = (TimelineDirector.playableAsset as TimelineAsset).outputs.FirstOrDefault(c => c.streamName == "Camera");
                TimelineDirector.SetGenericBinding(cameraTrack.sourceObject, PermanentObjects.Instance.MainCamera.GetComponent<CinemachineBrain>());
                PermanentObjects.Instance.Player.Freeze();
                TimelineDirector.Play();
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error in DialogTimeline when setting the camera. Make sure the track is named Camera" + ex.Message);
            }
        }
    }

    void Update()
    {
        if (awaitInput)
            if (Input.GetButtonDown("Interact"))
            {
                awaitInput = false;
                PermanentObjects.Instance.UI.ContinueButton.SetActive(false);
                TimelineDirector.Play();
            }
    }

    public void WaitForInput()
    {
        PermanentObjects.Instance.UI.ContinueButton.SetActive(true);
        TimelineDirector.Pause();
        awaitInput = true;
    }

    public void OnTimelineFinish()
    {
        PermanentObjects.Instance.Player.Unfreeze();
    }
}
