using System.Collections;
using UnityEngine;

public class FloorTrap : MonoBehaviour
{
    [SerializeField] float StartDelay = default;

    void Awake()
    {
        StartCoroutine(StartAnimation());
    }

    IEnumerator StartAnimation()
    {
        yield return new WaitForSeconds(StartDelay);
        GetComponent<Animator>().enabled = true;
    }
}
