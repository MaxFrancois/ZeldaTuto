using System.Collections;
using UnityEngine;

public class BlinkOnHit : MonoBehaviour
{
    [SerializeField]
    Color flashColor;
    [SerializeField]
    Color regularColor;
    [SerializeField]
    float numberOfFlashes;
    [SerializeField]
    float flashDuration;

    public void Blink(SpriteRenderer spriteRenderer)
    {
        StartCoroutine(BlinkCo(spriteRenderer));
    }

    private IEnumerator BlinkCo(SpriteRenderer spriteRenderer)
    {
        var i = 0;
        while (i < numberOfFlashes)
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = regularColor;
            yield return new WaitForSeconds(flashDuration);
            i++;
        }
        yield return null;
    }
}
