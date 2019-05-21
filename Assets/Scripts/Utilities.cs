using UnityEngine;

public static class Utilities
{
    public static GameObject FindClosestEnemyInRadius(Transform transform, float radius)
    {
        GameObject closestEnemy = null;
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, radius))
        {
            if (collider.isTrigger && collider.gameObject.CompareTag("Enemy"))
            {
                if (closestEnemy == null)
                    closestEnemy = collider.gameObject;
                else if (Vector3.Distance(collider.transform.position, transform.position) < Vector3.Distance(closestEnemy.transform.position, transform.position))
                    closestEnemy = collider.gameObject;
            }
        }
        return closestEnemy;
    }

    public static bool FadeOutSprite(SpriteRenderer sprite, float fadeSpeed, float time)
    {
        var startFadeTime = time;
        float t = (Time.time - startFadeTime) / fadeSpeed;
        sprite.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1f, 0f, t));
        if (sprite.color.a == 0f)
            return true;
        return false;
    }
}
