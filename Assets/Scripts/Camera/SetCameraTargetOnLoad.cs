using UnityEngine;
using Cinemachine;

public class SetCameraTargetOnLoad : MonoBehaviour
{
    void Start()
    {
        GetComponent<CinemachineVirtualCamera>().Follow = PermanentObjects.Instance.Player.transform;
    }
}
