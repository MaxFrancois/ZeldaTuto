using UnityEngine;

[CreateAssetMenu(fileName = "LightningBallsConfig", menuName = "SpellConfigs/LightningBallsConfig")]
public class LightningBallsConfig : SpellConfig
{
    public GameObject LightningBallsSpell;
    public GameObject LightningBallInstance;
    public GameObject LightningBallExplosion;
    public float Radius;
    public float AmountOfBalls;
    public float RotationSpeed;

    public override void Cast(Transform source, Vector3 direction)
    {
        var spell = Instantiate(LightningBallsSpell, source.position, Quaternion.identity);
        spell.GetComponent<LightningBalls>().Initialize(this);
    }
}