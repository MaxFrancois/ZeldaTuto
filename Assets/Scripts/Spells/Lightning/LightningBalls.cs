using UnityEngine;

public class LightningBalls : ITime
{
    LightningBallsConfig config;
    GameObject target;
    public void Initialize(LightningBallsConfig cfg, int i)
    {
        config = cfg;
        target = GameObject.FindWithTag("Player");
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + config.Radius, 0);
        var instance = Instantiate(config.LightningBallInstance, spawnPosition, Quaternion.identity, transform);
        instance.GetComponent<LightningBallInstance>().Initialize(config);
        transform.Rotate(Vector3.back, 360 / config.AmountOfBalls * i);
        Destroy(gameObject, config.LifeTime + 0.5f);
    }

    void Update()
    {
        transform.position = target.transform.position;
        transform.Rotate(Vector3.back * (config.RotationSpeed * Time.deltaTime * ( 1 - SlowTimeCoefficient)));
    }
}
