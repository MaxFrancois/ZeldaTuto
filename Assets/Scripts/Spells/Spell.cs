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
    public float Damage;
    public GameObject Animation;
    public float LifeTime;
    protected float lifeTimeTracker;
    public GameObject AnimationInstance;
    protected bool isDestroyed = false;

    public virtual void Cast(Transform source, Vector3 direction)
    {
        Debug.Log("Casting spell!" + Name);
    }
}
