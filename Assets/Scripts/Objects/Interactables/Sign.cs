using UnityEngine;
using UnityEngine.UI;

public class Sign : Interactable
{
    public GameObject DialogBox;
    public Text DialogText;
    public string DialogString;

    private CharacterState playerState;

    void Awake()
    {
        playerState = GameObject.FindWithTag("Player").GetComponent<CharacterState>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact") && IsActive)
        {
            if (DialogBox.activeInHierarchy)
            {
                DialogBox.SetActive(false);
                playerState.MovementState = CharacterMovementState.Idle;
            }
            else
            {
                DialogBox.SetActive(true);
                DialogText.text = DialogString;
                playerState.MovementState = CharacterMovementState.Interacting;
            }
        }
    }

    //private void OnTriggerEnter2D(Collider2D collidedObject)
    //{
    //    if (collidedObject.CompareTag("Player") && !collidedObject.isTrigger)
    //    {
    //        Context.Raise(true);
    //        IsActive = true;
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collidedObject)
    //{
    //    if (collidedObject.CompareTag("Player") && !collidedObject.isTrigger)
    //    {
    //        IsActive = false;
    //        DialogBox.SetActive(false);
    //        Context.Raise(false);
    //    }
    //}
}
