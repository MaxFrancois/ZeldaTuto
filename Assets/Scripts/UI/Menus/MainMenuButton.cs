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
    [SerializeField] public float OffsetX;
    [SerializeField] public float OffsetY;
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        if (Cursor == null)
            Cursor = GameObject.FindWithTag("Cursor");
        Cursor.transform.position = new Vector3(transform.position.x + OffsetX, transform.position.y + OffsetY, 0);
    }
}
