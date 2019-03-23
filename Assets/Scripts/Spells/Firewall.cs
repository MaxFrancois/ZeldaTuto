using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firewall : Spell
{
    public FirewallConfig Config;
    private List<GameObject> staticFireInstances = new List<GameObject>();

    void Update()
    {
        if (staticFireInstances.Count == Config.NumberOfFires)
            Destroy(this.gameObject, 0.1f);
        //DestroyThis();
    }

    //private void DestroyThis()
    //{
    //    if (!isDestroyed)
    //    {
    //        isDestroyed = true;
    //        Destroy(this.gameObject, 0.1f);
    //    }
    //}

    public override void Cast(Transform source, Vector3 direction)
    {
        StartCoroutine(StartWallCo(source, direction));
    }

    IEnumerator StartWallCo(Transform source, Vector3 direction)
    {
        for (int i = 0; i < Config.NumberOfFires; i++)
        {
            var idx = i - Mathf.Ceil(Config.NumberOfFires / 2);
            StartCoroutine(StartFireCo(source, direction, idx));
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    IEnumerator StartFireCo(Transform source, Vector3 direction, float staticFireIndex)
    {
        var staticFirePosition = new Vector3(source.position.x, source.position.y, 0);
        if (direction.x == 0 || direction.y == 0)
        {
            //straight line
            if (direction.y < 0)
            {
                staticFirePosition.y -= Config.SpawnDistance;
                staticFirePosition.x -= staticFireIndex;
            }
            if (direction.y > 0)
            {
                staticFirePosition.y += Config.SpawnDistance;
                staticFirePosition.x += staticFireIndex;
            }
            if (direction.x > 0)
            {
                staticFirePosition.y += staticFireIndex;
                staticFirePosition.x += Config.SpawnDistance;
            }
            if (direction.x < 0)
            {
                staticFirePosition.y -= staticFireIndex;
                staticFirePosition.x -= Config.SpawnDistance;
            }
        }
        else
        {
            //diagonal
            if (direction.y > 0 && direction.x > 0)
            {
                //top right
                staticFirePosition.x += Config.SpawnDistance + staticFireIndex;
                staticFirePosition.y += Config.SpawnDistance - staticFireIndex;
            }
            if (direction.y < 0 && direction.x < 0)
            {
                //bottom left
                staticFirePosition.x -= Config.SpawnDistance + staticFireIndex;
                staticFirePosition.y -= Config.SpawnDistance - staticFireIndex;
            }
            if (direction.y > 0 && direction.x < 0)
            {
                //top left
                staticFirePosition.x -= Config.SpawnDistance - staticFireIndex;
                staticFirePosition.y += Config.SpawnDistance + staticFireIndex;
            }
            if (direction.y < 0 && direction.x > 0)
            {
                //bottom right
                staticFirePosition.x += Config.SpawnDistance - staticFireIndex;
                staticFirePosition.y -= Config.SpawnDistance + staticFireIndex;
            }
        }
        var staticFireInstance = Instantiate(Config.StaticFire, staticFirePosition, Quaternion.identity);
        var firescript = staticFireInstance.GetComponent<StaticFire>();
        firescript.Initialize(Config.Damage, Config.PushTime, Config.PushForce, Config.LifeTime, 0);
        staticFireInstances.Add(staticFireInstance);
        yield return null;
    }

    public override SpellConfig GetConfig()
    {
        return Config;
    }
}
