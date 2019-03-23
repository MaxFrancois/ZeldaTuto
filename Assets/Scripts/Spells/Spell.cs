using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    //public SpellConfig Config;
    //public string Id;
    //public Sprite Icon;
    //public Sprite SelectedIcon;
    //public string Name;
    //public float Cooldown;
    //protected float cooldownTracker;
    //public float ManaCost;
    //public float PushTime;
    //public float PushForce;
    //public float Damage;
    //public GameObject Animation;
    //public float LifeTime;
    //protected float lifeTimeTracker;
    //public GameObject AnimationInstance;
    //protected bool isDestroyed = false;

    public abstract void Cast(Transform source, Vector3 direction);// { }

    public abstract SpellConfig GetConfig();// { return null; }
   
}
