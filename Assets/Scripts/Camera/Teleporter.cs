using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Teleporter : MonoBehaviour
{
    [Header("Teleporter")]
    [SerializeField] GameObject Destination;
    [SerializeField] bool RequiresInput;
    [SerializeField] GameObject Animation;
    [SerializeField] float AnimationDuration;

    [Header("Signals")]
    [SerializeField] BoolSignal InRangeSignal;
    [SerializeField] FadePanelSignal FadeToSignal;
    [SerializeField] FadePanelSignal FadeFromSignal;

    [Header("Location")]
    [SerializeField] bool ShowLocationName;
    [SerializeField] string LocationName;
    [SerializeField] GameObject LocationTextObject;
    [SerializeField] Text LocationTextField;


    public Image Image;
    bool playerInRange;
    GameObject player;

    void Start()
    {
        playerInRange = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (RequiresInput && playerInRange)
        {
            if (Input.GetButtonDown("Interact"))
            {
                StartCoroutine(StartTeleport());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collidedObject)
    {
        if (collidedObject.CompareTag("Player") && !collidedObject.isTrigger)
        {
            if (player == null)
                player = collidedObject.gameObject;
            if (!RequiresInput)
            {
                StartCoroutine(StartTeleport());
            }
            else
            {
                playerInRange = true;
                if (InRangeSignal) InRangeSignal.Raise(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            playerInRange = false;
            if (InRangeSignal) InRangeSignal.Raise(false);
        }
    }

    IEnumerator StartTeleport()
    {
        if (InRangeSignal) InRangeSignal.Raise(false);
        if (Animation)
        {
            var anim = Instantiate(Animation, transform.position, Quaternion.identity);
            Destroy(anim, AnimationDuration);
        }
        if (FadeToSignal)
        {
            player.GetComponent<PlayerMovement>().Freeze();
            FadeToSignal.Raise();
            yield return new WaitForSeconds(FadeToSignal.Duration);
        }
        player.transform.position = Destination.transform.position;
        if (FadeFromSignal)
        {
            yield return new WaitForSeconds(1f);
            FadeFromSignal.Raise();
            yield return new WaitForSeconds(FadeFromSignal.Duration);
            player.GetComponent<PlayerMovement>().Unfreeze();
        }
        if (ShowLocationName) StartCoroutine(ShowPlaceName());
        yield return null;
    }

    //TODO make a location message script with a signal
    //use fade time/duration for the text
    private IEnumerator ShowPlaceName()
    {
        LocationTextObject.SetActive(true);
        LocationTextField.text = LocationName;
        StartCoroutine(FadeTextToFullAlpha(LocationTextField));
        yield return new WaitForSeconds(3f);
        StartCoroutine(FadeTextToZeroAlpha(LocationTextField));
        yield return new WaitForSeconds(3f);
        LocationTextObject.SetActive(false);
    }

    public IEnumerator FadeTextToFullAlpha(Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / 1f));
            yield return null;
        }
    }

    public IEnumerator FadeTextToZeroAlpha(Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / 1f));
            yield return null;
        }
    }
}
