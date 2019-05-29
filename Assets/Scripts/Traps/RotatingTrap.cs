using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingTrap : MonoBehaviour
{
    public GameObject TrapInstance;
    public Vector3 TrapRotation;
    public float AmountOfTraps;
    public float RotationSpeed;
    public float InitialRotation;

    void Awake()
    {
        for (int i = 0; i < AmountOfTraps; i++)
        {
            var trap = Instantiate(TrapInstance,  transform.position, Quaternion.identity, transform);
            trap.transform.Rotate(TrapRotation);
            transform.Rotate(Vector3.back, 360 / AmountOfTraps);
        }
        transform.Rotate(Vector3.back, InitialRotation);
    }

    void Update()
    {
        transform.Rotate(Vector3.back * (RotationSpeed * Time.deltaTime));
    }
}
