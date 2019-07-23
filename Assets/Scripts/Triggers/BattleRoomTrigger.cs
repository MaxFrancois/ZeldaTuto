using UnityEngine;

public class BattleRoomTrigger : Trigger
{
    public VoidSignal StartBattleSignal;

    protected override void OnPlayerEnter()
    {
        StartBattleSignal.Raise();
        gameObject.SetActive(false);
    }
}
