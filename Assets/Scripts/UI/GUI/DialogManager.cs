using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DialogManager : MonoBehaviour
{
    Image dialogBubble;
    Animator animator;
    bool awaitingInput;
    bool writingText;
    int currentDialogIndex;
    Conversation currentConversation;
    Color[] colors = { new Color(0, 0, 1, 1), new Color(1, 1, 0, 1), new Color(1, 0, 0, 1) };
    Coroutine currentDialogCoRoutine;
    string currentTimelineId;

    [SerializeField] GameObject continueButton = default;
    [SerializeField] StringSignal conversationOver = default;
    [Header("Left Alignment")]
    [SerializeField] GameObject leftContainer = default;
    [SerializeField] Image leftIconImage = default;
    [SerializeField] Image leftCharacterNameBackground = default;
    [SerializeField] TextMeshProUGUI leftText = default;
    [SerializeField] TextMeshProUGUI leftCharacterName = default;

    [Header("Right Alignment")]
    [SerializeField] GameObject rightContainer = default;
    [SerializeField] Image rightIconImage = default;
    [SerializeField] Image rightCharacterNameBackground = default;
    [SerializeField] TextMeshProUGUI rightText = default;
    [SerializeField] TextMeshProUGUI rightCharacterName = default;

    void Awake()
    {
        dialogBubble = GetComponent<Image>();
        animator = GetComponent<Animator>();
        awaitingInput = false;
        currentDialogIndex = 0;
        currentTimelineId = string.Empty;
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (awaitingInput)
            {
                awaitingInput = false;
                currentDialogIndex++;
                continueButton.SetActive(false);
                if (currentDialogIndex >= currentConversation.Dialogs.Count)
                    StartCoroutine(RemoveBubble());

                else
                    currentDialogCoRoutine = StartCoroutine(NextDialogCo());
            }
            else if (writingText)
            {
                if (currentDialogCoRoutine != null)
                    StopCoroutine(currentDialogCoRoutine);
                writingText = false;
                awaitingInput = true;
                var dialog = currentConversation.Dialogs[currentDialogIndex];
                if (dialog.Alignment == DialogAlignment.Left)
                    leftText.text = dialog.Text;
                else if (dialog.Alignment == DialogAlignment.Right)
                    rightText.text = dialog.Text;
                continueButton.SetActive(true);
            }
        }
    }

    IEnumerator RemoveBubble()
    {
        if (currentConversation.CloseBubbleOnFinish)
        {
            leftContainer.SetActive(false);
            rightContainer.SetActive(false);
            animator.SetBool("Visible", false);
            yield return new WaitForSeconds(0.6f);
            dialogBubble.enabled = false;
        }
        if (!string.IsNullOrEmpty(currentTimelineId))
            conversationOver.Raise(currentTimelineId);
        else
            PermanentObjects.Instance.Player.Unfreeze();
    }

    public void StartConversation(Conversation conversation)
    {
        currentConversation = conversation;
        currentDialogIndex = 0;
        PermanentObjects.Instance.Player.Freeze();

        currentDialogCoRoutine = StartCoroutine(NextDialogCo());
    }

    public void StartTimelineConversation(Conversation conversation, string timelineId)
    {
        currentTimelineId = timelineId;
        currentConversation = conversation;
        currentDialogIndex = 0;

        currentDialogCoRoutine = StartCoroutine(NextDialogCo());
    }

    IEnumerator NextDialogCo()
    {
        continueButton.SetActive(false);
        if (currentConversation.OpenBubbleOnStart && currentDialogIndex == 0)
        {
            dialogBubble.enabled = true;
            animator.SetBool("Visible", true);
            yield return new WaitForSeconds(0.5f);
        }
        var nextDialog = currentConversation.Dialogs[currentDialogIndex];
        leftContainer.SetActive(false);
        rightContainer.SetActive(false);
        if (nextDialog.Alignment == DialogAlignment.Left)
        {
            leftContainer.SetActive(true);
            leftIconImage.sprite = nextDialog.CharacterIcon;
            leftCharacterNameBackground.color = colors[(int)nextDialog.SpeakerRelation];
            leftCharacterName.text = nextDialog.CharacterName;
            leftText.text = "";
            writingText = true;
            foreach (char c in nextDialog.Text)
            {
                leftText.text += c;
                yield return new WaitForSeconds(0.03f);
            }
        }
        else if (nextDialog.Alignment == DialogAlignment.Right)
        {

            rightContainer.SetActive(true);
            rightIconImage.sprite = nextDialog.CharacterIcon;
            rightCharacterNameBackground.color = colors[(int)nextDialog.SpeakerRelation];
            rightCharacterName.text = nextDialog.CharacterName;
            rightText.text = "";
            writingText = true;
            foreach (char c in nextDialog.Text)
            {
                rightText.text += c;
                yield return new WaitForSeconds(0.03f);
            }
        }
        yield return new WaitForSeconds(0.5f);
        writingText = false;
        awaitingInput = true;
        continueButton.SetActive(true);
    }
}
