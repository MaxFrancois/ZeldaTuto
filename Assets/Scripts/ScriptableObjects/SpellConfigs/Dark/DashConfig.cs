using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DashConfig", menuName = "SpellConfigs/Dark/DashConfig")]
public class DashConfig : SpellConfig
{
    public GameObject DashInstance;
    public float Duration;
    public float Speed;

    public override bool CanCast(Transform source, Vector3 direction)
    {
        return true;

        //can't find the right collider for map ?
        //return Physics2D.Raycast(source.position, direction, Distance).collider == null ||
        //    !Physics2D.Raycast(source.position, direction, Distance).collider.CompareTag("WorldCollision");
    }

    public override void Cast(Transform source, Vector3 direction)
    {
        //if (CanCast(source, direction))
        //{
        //    var currentPosition = source.position;
        //    var instance = Instantiate(DashInstance, currentPosition, Quaternion.identity);
        //    instance.transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(direction));
        //    instance.transform.localScale = new Vector3(Distance / DashWidth, 1f, 1f);
        //    source.position += direction.normalized * Distance;
        //    Destroy(instance, 0.1f);
        //}

        source.GetComponent<PlayerMovement>().Dash(Duration, Speed, direction);
    }
}
