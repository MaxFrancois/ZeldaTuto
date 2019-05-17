using UnityEngine;

[CreateAssetMenu(fileName = "WhirlwindConfig", menuName = "SpellConfigs/Air/WhirlwindConfig")]
public class WhirlwindConfig : SpellConfig
{
    public float SpawnDistance;
    public GameObject Whirlwind;
    public float PullRadius;
    public float PullForce;

    public override void Cast(Transform source, Vector3 direction)
    {
        var whirlwindPosition = new Vector3(source.position.x, source.position.y, 0);
        if (direction.x == 0 || direction.y == 0)
        {
            //straight line
            if (direction.y < 0)
            {
                whirlwindPosition.y -= SpawnDistance;
            }
            if (direction.y > 0)
            {
                whirlwindPosition.y += SpawnDistance;
            }
            if (direction.x > 0)
            {
                whirlwindPosition.x += SpawnDistance;
            }
            if (direction.x < 0)
            {
                whirlwindPosition.x -= SpawnDistance;
            }
        }
        else
        {
            //diagonal
            if (direction.y > 0 && direction.x > 0)
            {
                //top right
                whirlwindPosition.x += SpawnDistance / 2;
                whirlwindPosition.y += SpawnDistance / 2;
            }
            if (direction.y < 0 && direction.x < 0)
            {
                //bottom left
                whirlwindPosition.x -= SpawnDistance / 2;
                whirlwindPosition.y -= SpawnDistance / 2;
            }
            if (direction.y > 0 && direction.x < 0)
            {
                //top left
                whirlwindPosition.x -= SpawnDistance / 2;
                whirlwindPosition.y += SpawnDistance / 2;
            }
            if (direction.y < 0 && direction.x > 0)
            {
                //bottom right
                whirlwindPosition.x += SpawnDistance / 2;
                whirlwindPosition.y -= SpawnDistance / 2;
            }
        }
        var staticFireInstance = Instantiate(Whirlwind, whirlwindPosition, Quaternion.Euler(-45, 0, 0));
        var whirlwindscript = staticFireInstance.GetComponent<Whirlwind>();
        whirlwindscript.Initialize(Damage, LifeTime, PullRadius, PullForce);
    }
}
