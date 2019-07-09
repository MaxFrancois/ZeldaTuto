using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[CustomEditor(typeof(MainMenuButton))]
public class MenuButtonEditor : Editor
{
    //public override void OnInspectorGUI()
    //{
    //    //MainMenuButton targetMenuButton = (MainMenuButton)target;

    //    //targetMenuButton.Cursor = (GameObject)EditorGUILayout.ObjectField(targetMenuButton.Cursor, typeof(GameObject), true);

    //    // Show default inspector property editor
    //    //base.OnInspectorGUI();
    //}
}

public class MainMenuButton : Button
{
    [SerializeField] public GameObject Cursor;
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        Cursor.transform.position = new Vector3(transform.position.x, transform.position.y + 10, 0);
    }
}
