using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityScript.Macros;

[System.Serializable]
public class EnemyWave
{
    public List<EnemyBase> Enemies;
}

public class BattleRoom : MonoBehaviour
{
    [SerializeField] TriggerData Data = default;
    [SerializeField] CinemachineVirtualCamera BattleRoomCamera = default;
    [SerializeField] CinemachineVirtualCamera ParentRoomCamera = default;
    [SerializeField] PlayableDirector RoomStartDirector = default;
    [SerializeField] PlayableDirector RoomEndDirector = default;
    [SerializeField] GameObject ClosedDoorsParent = default;
    [SerializeField] GameObject OpenDoorsParent = default;
    PlayerMovement Player;

    public List<EnemyWave> EnemyWaves;
    int currentWave;
    bool isInProgress;

    void Awake()
    {
        currentWave = 0;
        isInProgress = false;
    }

    public void EnableBattleRoom()
    {
        StartCoroutine(StartRoomCo());
    }

    public void OnTriggerEnter2D(Collider2D collidedObject)
    {
        if (collidedObject.CompareTag("Player") && !collidedObject.isTrigger)
        {
            if (Data.IsActive && !isInProgress)
            {
                Player = PermanentObjects.Instance.Player;
                EnableBattleRoom();
            }
        }
    }

    IEnumerator StartRoomCo()
    {
        if (RoomStartDirector)
        {
            Player.Freeze();
            RoomStartDirector.Play();
            yield return new WaitForSeconds((float)RoomStartDirector.duration);
            Player.Unfreeze();
        }
        //CameraConfiner.m_BoundingShape2D = BattleRoomCollider;
        if (BattleRoomCamera)
        {
            BattleRoomCamera.gameObject.SetActive(true);
            ParentRoomCamera.gameObject.SetActive(false);
        }
        if (ClosedDoorsParent && OpenDoorsParent)
        {
            ClosedDoorsParent.SetActive(true);
            OpenDoorsParent.SetActive(false);
        }
        //CameraConfiner.InvalidatePathCache();
        StartNextWave();
        isInProgress = true;
    }

    void DisableBattleRoom()
    {
        StartCoroutine(EndRoomCo());
    }

    IEnumerator EndRoomCo()
    {

        if (RoomEndDirector)
        {
            Player.Freeze();
            RoomEndDirector.Play();
            yield return new WaitForSeconds((float)RoomEndDirector.duration);
            Player.Unfreeze();
        }
        if (BattleRoomCamera)
        {
            BattleRoomCamera.gameObject.SetActive(false);
            ParentRoomCamera.gameObject.SetActive(true);
        }
        if (ClosedDoorsParent && OpenDoorsParent)
        {
            ClosedDoorsParent.SetActive(false);
            OpenDoorsParent.SetActive(true);
        }
        Data.IsActive = false;
        isInProgress = false;
        //gameObject.SetActive(false);
    }

    void Update()
    {
        if (isInProgress)
        {
            if (IsWaveOver())
            {
                if (currentWave < EnemyWaves.Count - 1)
                {
                    currentWave++;
                    StartNextWave();
                }
                else
                    DisableBattleRoom();
            }
        }
    }

    void StartNextWave()
    {
        foreach (var enemy in EnemyWaves[currentWave].Enemies)
            enemy.gameObject.SetActive(true);
    }

    bool IsWaveOver()
    {
        bool isOver = true;
        foreach (var enemy in EnemyWaves[currentWave].Enemies)
            if (!enemy.IsDead)
                isOver = false;
        return isOver;
    }
}
