using System.Collections;
using UnityEngine;

public class BlinkOnHit : MonoBehaviour
{
    [SerializeField]
    protected Color flashColor = new Color(255, 130, 130, 180);
    [SerializeField]
    protected Color regularColor = new Color(255, 255, 255, 255);
    [SerializeField]
    protected float numberOfFlashes = 3;
    [SerializeField]
    protected float flashDuration = 0.07f;

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
