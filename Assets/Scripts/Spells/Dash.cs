using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : Spell
{
    public float Distance;

    private bool CanMove(Transform source, Vector3 direction)
    {
        //can't find the right collider for map ?
        return Physics2D.Raycast(source.position, direction, Distance).collider == null;
    }

    public override void Cast(Transform source, Vector3 direction)
    {
        if (CanMove(source, direction))
        {
            StartCoroutine(DashCo(source, direction, Distance));
        }
    }

    private IEnumerator DashCo(Transform source, Vector3 direction, float distance)
    {
        var currentPosition = source.position;
        AnimationInstance = Instantiate(Animation, currentPosition, Quaternion.identity);
        AnimationInstance.transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(direction));
        float dashEffectWidth = 3f;
        AnimationInstance.transform.localScale = new Vector3(distance / dashEffectWidth, 1f, 1f);
        source.position += direction * distance;
        Destroy(AnimationInstance, 0.1f);
        Destroy(this.gameObject, 0.1f);
        yield return null;
    }
}
