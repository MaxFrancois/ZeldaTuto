using UnityEngine;

public class Spell : MonoBehaviour
{
    public Sprite Icon;
    public string Name;
    public float Cooldown;
    protected float cooldownTracker;
    public float ManaCost;
    public float PushTime;
    public float PushForce;
    public GameObject Animation;
    public float LifeTime;
    protected float lifeTimeTracker;
    public GameObject AnimationInstance;

    void Start()
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
                Debug.Log("Spell lifetime over! " + Name);
                Destroy(this.gameObject);
            }
        }
    }

    public virtual void Cast(Transform source, Vector3 direction)
    {
        Debug.Log("Casting spell!" + Name);
    }
}
