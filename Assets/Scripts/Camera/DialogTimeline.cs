using Cinemachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DialogTimeline : Trigger
{
    [SerializeField] PlayableDirector TimelineDirector = default;
    bool awaitInput = false;

    [Header("Conversations")]
    [SerializeField] List<Conversation> conversations = default;
    [SerializeField] TimelineConversationSignal conversationSignal = default;
    int currentConversationIndex;

    protected override void OnPlayerEnter()
    {
        try
        {
            currentConversationIndex = 0;
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

    public void StartConversation()
    {
        conversationSignal.Raise(conversations[currentConversationIndex], Data.TriggerId);
        TimelineDirector.Pause();
    }

    public void ConversationFinished(string timelineId)
    {
        if (Data.TriggerId == timelineId)
        {
            TimelineDirector.Play();
            currentConversationIndex++;
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
