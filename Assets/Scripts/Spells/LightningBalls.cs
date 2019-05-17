using UnityEngine;

public class LightningBalls : MonoBehaviour
{
    LightningBallsConfig config;
    GameObject target;
    public void Initialize(LightningBallsConfig cfg)
    {
        config = cfg;
        target = GameObject.FindWithTag("Player");
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + config.Radius, 0);
        for (int i = 0; i < config.AmountOfBalls; i++)
        {
            var instance = Instantiate(config.LightningBallInstance, spawnPosition, Quaternion.identity, transform);
            instance.GetComponent<LightningBall>().Initialize(config);
            transform.Rotate(Vector3.back, 360 / config.AmountOfBalls);
        }
    }

    void Update()
    {
        transform.position = target.transform.position;
        transform.Rotate(Vector3.back * (config.RotationSpeed * Time.deltaTime));
    }
}
