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

    [SerializeField] GameObject continueButton;
    [Header("Left Alignment")]
    [SerializeField] GameObject leftContainer;
    [SerializeField] Image leftIconImage;
    [SerializeField] Image leftCharacterNameBackground;
    [SerializeField] TextMeshProUGUI leftText;
    [SerializeField] TextMeshProUGUI leftCharacterName;

    [Header("Right Alignment")]
    [SerializeField] GameObject rightContainer;
    [SerializeField] Image rightIconImage;
    [SerializeField] Image rightCharacterNameBackground;
    [SerializeField] TextMeshProUGUI rightText;
    [SerializeField] TextMeshProUGUI rightCharacterName;

    void Awake()
    {
        dialogBubble = GetComponent<Image>();
        animator = GetComponent<Animator>();
        awaitingInput = false;
        currentDialogIndex = 0;
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
            yield return new WaitForSeconds(1f);
            dialogBubble.enabled = false;
        }
        PermanentObjects.Instance.Player.Unfreeze();
    }

    public void StartConversation(Conversation conversation)
    {
        currentConversation = conversation;
        currentDialogIndex = 0;
        PermanentObjects.Instance.Player.Freeze();

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
                yield return new WaitForSeconds(0.05f);
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
                yield return new WaitForSeconds(0.05f);
            }
        }
        yield return new WaitForSeconds(1);
        awaitingInput = true;
        continueButton.SetActive(true);
    }
}
