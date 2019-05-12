using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RoomMove : MonoBehaviour
{
    public Vector2 minCameraChange;
    public Vector2 maxCameraChange;
    public Vector3 playerChange;
    private CameraMovement cam;
    public bool showPlaceName;
    public string placeName;
    public GameObject text;
    public Text placeText;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.GetComponent<CameraMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collidedObject)
    {
        if (collidedObject.CompareTag("Player") && !collidedObject.isTrigger)
        {
            cam.minPosition = minCameraChange;
            cam.maxPosition = maxCameraChange;
            GameObject.FindWithTag("Player").transform.position += playerChange;
            if (showPlaceName)
            {
                StartCoroutine(ShowPlaceName());
            }
        }
    }

    private IEnumerator ShowPlaceName()
    {
        text.SetActive(true);
        placeText.text = placeName;
        //StartCoroutine(FadeTextToFullAlpha(placeText));
        //yield return new WaitForSeconds(3f);
        //StartCoroutine(FadeTextToZeroAlpha(placeText));
        yield return new WaitForSeconds(3f);
        text.SetActive(false);
    }

    //public IEnumerator FadeTextToFullAlpha(Text i)
    //{
    //    i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
    //    while (i.color.a < 1.0f)
    //    {
    //        i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / 1f));
    //        yield return null;
    //    }
    //}

    //public IEnumerator FadeTextToZeroAlpha(Text i)
    //{
    //    i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
    //    while (i.color.a > 0.0f)
    //    {
    //        i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / 1f));
    //        yield return null;
    //    }
    //}
}
