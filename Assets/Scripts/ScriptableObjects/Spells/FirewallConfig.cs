using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FirewallConfig", menuName = "SpellConfigs/FirewallConfig")]
public class FirewallConfig : SpellConfig
{
    public GameObject StaticFire;
    public int NumberOfFires;
    public float SpawnDistance;
    public float DelayBetweenFires;

    private List<GameObject> staticFireInstances = new List<GameObject>();

    public override void Cast(Transform source, Vector3 direction)
    {
        for (int i = 0; i < NumberOfFires; i++)
        {
            var idx = i - Mathf.Ceil(NumberOfFires / 2);
            StartFire(source, direction, idx);
        }
    }

    private void StartFire(Transform source, Vector3 direction, float staticFireIndex)
    {
        var staticFirePosition = new Vector3(source.position.x, source.position.y, 0);
        if (direction.x == 0 || direction.y == 0)
        {
            //straight line
            if (direction.y < 0)
            {
                staticFirePosition.y -= SpawnDistance * 80/100;
                staticFirePosition.x -= staticFireIndex * 80 / 100;
            }
            if (direction.y > 0)
            {
                staticFirePosition.y += SpawnDistance * 80 / 100;
                staticFirePosition.x += staticFireIndex * 80 / 100;
            }
            if (direction.x > 0)
            {
                staticFirePosition.y += staticFireIndex * 80 / 100;
                staticFirePosition.x += SpawnDistance * 80 / 100;
            }
            if (direction.x < 0)
            {
                staticFirePosition.y -= staticFireIndex * 80 / 100;
                staticFirePosition.x -= SpawnDistance * 80 / 100;
            }
        }
        else
        {
            //diagonal
            if (direction.y > 0 && direction.x > 0)
            {
                //top right
                staticFirePosition.x += SpawnDistance + staticFireIndex / 2;
                staticFirePosition.y += SpawnDistance - staticFireIndex / 2;
            }
            if (direction.y < 0 && direction.x < 0)
            {
                //bottom left
                staticFirePosition.x -= SpawnDistance + staticFireIndex / 2;
                staticFirePosition.y -= SpawnDistance - staticFireIndex / 2;
            }
            if (direction.y > 0 && direction.x < 0)
            {
                //top left
                staticFirePosition.x -= SpawnDistance - staticFireIndex / 2;
                staticFirePosition.y += SpawnDistance + staticFireIndex / 2;
            }
            if (direction.y < 0 && direction.x > 0)
            {
                //bottom right
                staticFirePosition.x += SpawnDistance - staticFireIndex / 2;
                staticFirePosition.y -= SpawnDistance + staticFireIndex / 2;
            }
        }
        var staticFireInstance = Instantiate(StaticFire, staticFirePosition, Quaternion.identity);
        var firescript = staticFireInstance.GetComponent<StaticFire>();
        firescript.Initialize(Damage, PushTime, PushForce, LifeTime, DelayBetweenFires);
        staticFireInstances.Add(staticFireInstance);
    }
}
