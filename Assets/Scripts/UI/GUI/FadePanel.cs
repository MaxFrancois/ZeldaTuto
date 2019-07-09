using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum FadeType
{
    To = 0,
    From = 1
}

public class FadePanel : MonoBehaviour
{
    Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Fade(FadeType type, Color c, int duration)
    {
        image.color = c;
        if (type == FadeType.To)
            StartCoroutine(FadeIn(duration));
        else
            StartCoroutine(FadeOut(duration));
    }

    IEnumerator FadeIn(int duration)
    {
        for (float f = .05f; f <= 1.1; f += .05f)
        {
            Color c = image.color;
            c.a = f;
            image.color = c;
            yield return new WaitForSeconds(.05f * duration);
        }
    }

    IEnumerator FadeOut(int duration)
    {
        for (float f = 1f; f >= -.05f; f -= .05f)
        {
            Color c = image.color;
            c.a = f;
            image.color = c;
            yield return new WaitForSeconds(.05f * duration);
        }
    }

    //    Panel.SetActive(true);
    //    image.color = c;
    //    image.CrossFadeAlpha((int)type, duration, true);     
    //}

    //private void Update()
    //{
    //    if (image.color.a <= 0)
    //        Panel.SetActive(false);
    //}
}
