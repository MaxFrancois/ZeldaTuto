using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firewall : Spell
{
    public GameObject StaticFire;
    public int NumberOfFires;
    public float SpawnDistance;
    private List<GameObject> staticFireInstances = new List<GameObject>();

    private void Start()
    {
        lifeTimeTracker = LifeTime;
    }

    void Update()
    {
        if (LifeTime != 0)
        {
            lifeTimeTracker -= Time.deltaTime;
            if (lifeTimeTracker <= 0)
            {
                DestroyThis();
            }
        }
    }

    private void DestroyThis()
    {
        if (!isDestroyed)
        {
            isDestroyed = true;
            //foreach (var inst in staticFireInstances)
            //    Destroy(inst, 0.1f);
            Destroy(this.gameObject, 0.1f);
        }
    }

    public override void Cast(Transform source, Vector3 direction)
    {
        StartCoroutine(StartWallCo(source, direction));
    }

    IEnumerator StartWallCo(Transform source, Vector3 direction)
    {
        for (int i = 0; i < NumberOfFires; i++)
        {
            var idx = i - Mathf.Ceil(NumberOfFires / 2);
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
                staticFirePosition.y -= SpawnDistance;
                staticFirePosition.x -= staticFireIndex;
            }
            if (direction.y > 0)
            {
                staticFirePosition.y += SpawnDistance;
                staticFirePosition.x += staticFireIndex;
            }
            if (direction.x > 0)
            {
                staticFirePosition.y += staticFireIndex;
                staticFirePosition.x += SpawnDistance;
            }
            if (direction.x < 0)
            {
                staticFirePosition.y -= staticFireIndex;
                staticFirePosition.x -= SpawnDistance;
            }
        }
        else
        {
            //diagonal
            if (direction.y > 0 && direction.x > 0)
            {
                //top right
                staticFirePosition.x += SpawnDistance + staticFireIndex;
                staticFirePosition.y += SpawnDistance - staticFireIndex;
            }
            if (direction.y < 0 && direction.x < 0)
            {
                //bottom left
                staticFirePosition.x -= SpawnDistance + staticFireIndex;
                staticFirePosition.y -= SpawnDistance - staticFireIndex;
            }
            if (direction.y > 0 && direction.x < 0)
            {
                //top left
                staticFirePosition.x -= SpawnDistance - staticFireIndex;
                staticFirePosition.y += SpawnDistance + staticFireIndex;
            }
            if (direction.y < 0 && direction.x > 0)
            {
                //bottom right
                staticFirePosition.x += SpawnDistance - staticFireIndex;
                staticFirePosition.y -= SpawnDistance + staticFireIndex;
            }
        }
        var staticFireInstance = Instantiate(StaticFire, staticFirePosition, Quaternion.identity);
        var firescript = staticFireInstance.GetComponent<StaticFire>();
        firescript.Initialize(Damage, PushTime, PushForce, LifeTime);
        staticFireInstances.Add(staticFireInstance);
        yield return null;
    }
}
