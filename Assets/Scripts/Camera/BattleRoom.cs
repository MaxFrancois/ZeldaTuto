﻿using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class EnemyWave
{
    public List<EnemyBase> Enemies;
}

public class BattleRoom : MonoBehaviour
{
    [SerializeField] TriggerData Data;
    [SerializeField] CinemachineVirtualCamera BattleRoomCamera;
    [SerializeField] CinemachineVirtualCamera ParentRoomCamera;
    [SerializeField] PlayableDirector RoomStartDirector;
    [SerializeField] PlayableDirector RoomEndDirector;
    PlayerMovement Player;

    public List<EnemyWave> EnemyWaves;
    int currentWave;
    bool isActive;

    void Awake()
    {
        currentWave = 0;
        isActive = false;
        Player = PermanentObjects.Instance.Player;
    }

    public void EnableBattleRoom()
    {
        StartCoroutine(StartRoomCo());
    }

    IEnumerator StartRoomCo()
    {
        Player.Freeze();
        if (RoomStartDirector)
            RoomStartDirector.Play();
        yield return new WaitForSeconds((float)RoomStartDirector.duration);
        Player.Unfreeze();
        //CameraConfiner.m_BoundingShape2D = BattleRoomCollider;
        BattleRoomCamera.gameObject.SetActive(true);
        ParentRoomCamera.gameObject.SetActive(false);
        //CameraConfiner.InvalidatePathCache();
        StartNextWave();
        isActive = true;
    }

    void DisableBattleRoom()
    {
        StartCoroutine(EndRoomCo());
    }

    IEnumerator EndRoomCo()
    {
        Player.Freeze();
        if (RoomEndDirector)
            RoomEndDirector.Play();
        yield return new WaitForSeconds((float)RoomEndDirector.duration);
        BattleRoomCamera.gameObject.SetActive(false);
        ParentRoomCamera.gameObject.SetActive(true);
        Player.Unfreeze();
        Data.IsActive = false;
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (isActive)
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
