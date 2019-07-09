using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SecretRoom : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Tilemap map;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        map = GetComponent<Tilemap>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.isTrigger)
        {
            StopAllCoroutines();
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeIn()
    {
        for (float f = spriteRenderer.color.a; f <= 1.1; f += .05f)
        {
            Color c = spriteRenderer.color;
            c.a = f;
            spriteRenderer.color = c;
            yield return new WaitForSeconds(.05f);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.isTrigger)
        {
            StopAllCoroutines();
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeOut()
    {
        for (float f = spriteRenderer.color.a; f >= -.05f; f -= .05f)
        {
            Color c = spriteRenderer.color;
            c.a = f;
            spriteRenderer.color = c;
            yield return new WaitForSeconds(.05f);
        }
    }
}
