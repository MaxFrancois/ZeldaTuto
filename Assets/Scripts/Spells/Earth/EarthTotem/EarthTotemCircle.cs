using System.Collections.Generic;
using UnityEngine;

public class EarthTotemCircle : MonoBehaviour
{
    List<Collider2D> ObjectsInsideCircle;
    EarthTotemConfig config;

    public void Initialize(EarthTotemConfig cfg)
    {
        config = cfg;
        ObjectsInsideCircle = new List<Collider2D>();
    }

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.collider.isTrigger && collision.collider.CompareTag("Player"))
    //    {
    //        var script = collision.gameObject.GetComponent<CharacterHealth>();
    //        script.GainHealth(config.Healing * Time.deltaTime * (1 - SlowTimeCoefficient), false);
    //    }
    //}

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.isTrigger && collision.CompareTag("Player"))
    //    {
    //        var script = collision.GetComponent<CharacterHealth>();
    //        var value = config.Healing * Time.deltaTime * (1 - SlowTimeCoefficient);
    //        script.GainHealth(value, false);
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger && (collision.CompareTag("Player") || collision.CompareTag("Enemy")))
        {
            ObjectsInsideCircle.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.isTrigger && (collision.CompareTag("Player") || collision.CompareTag("Enemy")))
        {
            if (ObjectsInsideCircle.Contains(collision))
                ObjectsInsideCircle.Remove(collision);
        }
    }

    public void TriggerHealing()
    {
        foreach (var obj in ObjectsInsideCircle)
        {
            var script = obj.GetComponent<CharacterHealth>();
            if (script)
                script.GainHealth(config.Healing, true);
        }
    }

    private void OnDestroy()
    {
        ObjectsInsideCircle.Clear();
    }
}
