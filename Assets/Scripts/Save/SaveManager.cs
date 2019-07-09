using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    [SerializeField] GameData GameData;
    public static SaveManager instance;
    string gamePath;
    void Start()
    {
        gamePath = Application.persistentDataPath;
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        DontDestroyOnLoad(this);
    }

    public void SaveGame()
    {
        if (!Directory.Exists(gamePath))
        {
            Directory.CreateDirectory(gamePath);
        }
        GameData.SceneData.First(c => c.name == SceneManager.GetActiveScene().name).IsCurrentScene = true;
        
        using (Stream s = File.Open(Path.Combine(gamePath, "gamedata.dat"), FileMode.OpenOrCreate))
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(s, GameData);
        }
    }

    public void LoadGame()
    {
        GameData data = null;
        using (Stream s = File.Open(Path.Combine(gamePath, "gamedata.dat"), FileMode.Open)) {
            BinaryFormatter bf = new BinaryFormatter();
            data = (GameData)bf.Deserialize(s);
        }
        StartCoroutine(LoadScene(data));
    }

    IEnumerator LoadScene(GameData data)
    {
        var asyncOperation = SceneManager.LoadSceneAsync(data.SceneData.First(c => c.IsCurrentScene).name);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
        data.PlayerData.LoadPlayerData();
    }
}