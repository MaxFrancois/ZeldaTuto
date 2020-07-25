using System.Collections;
using UnityEngine;

public class ArcaneBeam : MonoBehaviour
{

    [SerializeField] float startDelay = default;
    [SerializeField] float laserDuration = default;
    [SerializeField] float pauseDuration = default;

    Animator animator;
    bool started;
    bool isFiring;
    float tracker;

    void Start()
    {
        animator = GetComponent<Animator>();
        tracker = pauseDuration;
        started = false;
        isFiring = false;
        StartCoroutine(InitialDelay());
    }

    IEnumerator InitialDelay()
    {
        yield return new WaitForSeconds(startDelay);
        started = true;
    }

    void Update()
    {
        if (started)
        {
            tracker -= Time.deltaTime;
            if (tracker <= 0)
            {
                isFiring = !isFiring;
                animator.SetBool("isFiring", isFiring);
                tracker = isFiring ? laserDuration : pauseDuration;
            }
        }
    }
}
