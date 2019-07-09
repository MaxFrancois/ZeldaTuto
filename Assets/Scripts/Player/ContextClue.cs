using UnityEngine;

public class ContextClue : MonoBehaviour
{
    public GameObject Clue;
    public bool IsActive = false;

    public void ChangeActive(bool isActive)
    {
        if (IsActive != isActive)
        {
            IsActive = isActive;
            Clue.SetActive(IsActive);
        }
    }
}
