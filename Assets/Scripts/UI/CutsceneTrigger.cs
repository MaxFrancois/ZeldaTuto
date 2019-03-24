﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public GameObject GUI;
    public VoidSignal CutSceneStartAnimationSignal;
    public VoidSignal CutsceneFinishedSignal;
    public float TimeBeforeAnimation;
    public TextMeshProUGUI BossTitle;
    public MiniBoss Boss;
    public BossHealthManager BossHealthManager;
    public PlayerMovement Player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.isTrigger)
            PlayCutscene();
    }

    public void PlayCutscene()
    {
        StartCoroutine(PlayCutsceneCo());
    }

    IEnumerator PlayCutsceneCo()
    {
        Player.SetFrozenForCutscene(true);
        if (BossTitle)
            BossTitle.text = Boss.Name;
        GUI.SetActive(false);
        playableDirector.Play();
        yield return new WaitForSeconds(TimeBeforeAnimation);
        if (CutSceneStartAnimationSignal)
            CutSceneStartAnimationSignal.Raise();
        yield return new WaitForSeconds((float)playableDirector.duration - TimeBeforeAnimation);
        GUI.SetActive(true);
        if (CutsceneFinishedSignal)
            CutsceneFinishedSignal.Raise();
        if (BossHealthManager)
            BossHealthManager.Initialize(Boss.MaxHealth.InitialValue, Boss.Name);
        Player.SetFrozenForCutscene(false);
        this.gameObject.SetActive(false);
        yield return null;
    }
}