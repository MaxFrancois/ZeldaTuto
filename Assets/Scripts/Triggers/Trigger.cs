using System;
using UnityEngine;

public abstract class Trigger : MonoBehaviour
{
    [SerializeField] protected TriggerData Data;

    void Awake()
    {
        if (Data == null)
        {
            Debug.LogError("Trigger " + name + " doesn't have its TriggerData scriptableObject");
            UnityEditor.EditorApplication.isPlaying = false;
            return;
        }
        if (string.IsNullOrWhiteSpace(Data.TriggerId))
        {
            Data.TriggerId = Guid.NewGuid().ToString();
            gameObject.SetActive(Data.IsActiveDefaultValue);
        }
        else
            gameObject.SetActive(Data.IsActive);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.isTrigger)
        {
            OnPlayerEnter();
        }
    }

    protected virtual void OnPlayerEnter()
    {
        Debug.LogError("Trigger " + name + "OnPlayerEnter hasn't been implemented");
    }

    public string ID { get { return Data.TriggerId; } }
}
