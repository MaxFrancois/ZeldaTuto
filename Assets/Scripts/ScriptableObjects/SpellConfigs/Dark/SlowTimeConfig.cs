using UnityEngine;

[CreateAssetMenu(fileName = "SlowTimeConfig ", menuName = "SpellConfigs/Dark/SlowTimeConfig ")]
public class SlowTimeConfig : SpellConfig
{
    public GameObject SlowTimeInstance;
    [Range(0, 1)]
    public float SlowSpeed;
    public float ExpandSpeed = 1;
    public float MaxScale = 1;

    public override void Cast(Transform source, Vector3 direction)
    {
        var instance = Instantiate(SlowTimeInstance, source.position, Quaternion.identity);
        instance.GetComponent<SlowTime>().Initialize(this);
    }
}
