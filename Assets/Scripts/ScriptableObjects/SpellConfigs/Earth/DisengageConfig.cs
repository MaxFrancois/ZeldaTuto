using UnityEngine;

[CreateAssetMenu(fileName = "DisengageConfig", menuName = "SpellConfigs/Earth/DisengageConfig")]
public class DisengageConfig : SpellConfig
{
    [Header("Dash")]
    public float DashSpeed;
    public float DashDuration;
    [Header("Illusion")]
    public Disengage Illusion;
    public GameObject ExplosionParticles;
    public StunConfig StunStatusConfig;
    [Header("Gas DoT")]
    public DisengageGas Gas;
    public float GasDuration;
    public PoisonConfig PoisonStatusConfig;

    public override void Cast(Transform source, Vector3 direction)
    {
        //var illusion = Instantiate(Illusion, source.position, Quaternion.identity);
        //illusion.Initialize(this);
        var backwardsDirection = new Vector3(-direction.x, -direction.y, 0);
        source.GetComponent<PlayerMovement>().Dash(DashDuration, DashSpeed, backwardsDirection);
    }
}
