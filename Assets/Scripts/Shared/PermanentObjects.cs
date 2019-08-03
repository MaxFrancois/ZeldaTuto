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
        UI.GetComponent<MenuManager>().HideMenus();
        Player.gameObject.SetActive(false);
        MainCamera.gameObject.SetActive(false);
        UI.gameObject.SetActive(false);
    }

    public void EnableVisible()
    {
        Player.gameObject.SetActive(true);
        MainCamera.gameObject.SetActive(true);
        UI.gameObject.SetActive(true);
    }

    public PlayerMovement Player;
    public SaveManager SaveManager;
    public Camera MainCamera;
    public SpellBar SpellBar;
    public UIManager UI;
}
