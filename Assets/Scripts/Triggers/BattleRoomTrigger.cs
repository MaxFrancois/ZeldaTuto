using UnityEngine;

public class BattleRoomTrigger : Trigger
{
    public VoidSignal StartBattleSignal;

    protected override void OnPlayerEnter()
    {
        StartBattleSignal.Raise();
        Data.IsActive = false;
        gameObject.SetActive(false);
    }
}
