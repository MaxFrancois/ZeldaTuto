using UnityEngine;

public abstract class ITime : MonoBehaviour
{
    protected float SlowTimeCoefficient;

    public void SetSlowTimeCoefficient(float value)
    {
        SlowTimeCoefficient = value;
    }
}
