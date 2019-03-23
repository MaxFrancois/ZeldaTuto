using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectableButton : Button
{
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        this.transform.localScale = new Vector3(1.3f, 1.3f, 1);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        this.transform.localScale = new Vector3(1, 1, 1);
    }

    
}
