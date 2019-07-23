using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : Trigger
{
    public PlayableDirector playableDirector;
    public GameObject GUI;
    public VoidSignal CutSceneStartAnimationSignal;
    public VoidSignal CutsceneFinishedSignal;
    public float TimeBeforeAnimation;
    public TextMeshProUGUI BossTitle;
    public EnemyBase Boss;
    public BossHealthManager BossHealthManager;
    public PlayerMovement Player;

    protected override void OnPlayerEnter()
    {
        PlayCutscene();
    }

    public void PlayCutscene()
    {
        StartCoroutine(PlayCutsceneCo());
    }

    IEnumerator PlayCutsceneCo()
    {
        Player.Freeze();
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
            BossHealthManager.Initialize(Boss.GetEnemyHealth().MaxHealth, Boss.Name);
        Player.Unfreeze();
        this.gameObject.SetActive(false);
        yield return null;
    }
}
