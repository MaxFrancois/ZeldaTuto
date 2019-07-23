using UnityEngine;

public class PermanentObjects : MonoBehaviour
{
    public static PermanentObjects Instance;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
        DontDestroyOnLoad(this);
    }

    public void DisableVisible()
    {
        Player.gameObject.SetActive(false);
        MainCamera.gameObject.SetActive(false);
        UI.SetActive(false);
    }

    public void EnableVisible()
    {
        Player.gameObject.SetActive(true);
        MainCamera.gameObject.SetActive(true);
        UI.SetActive(true);
    }

    public PlayerMovement Player;
    public SaveManager SaveManager;
    public Camera MainCamera;
    public SpellBar SpellBar;
    public GameObject UI;
}
