using UnityEngine;
using System.IO;

[System.Serializable]
public class PlayerData
{
    public string playerName;
    public int score;
}

public class JsonExample : MonoBehaviour
{
    void SaveData()
    {
        PlayerData playerData = new PlayerData { playerName = "Player1", score = 1000 };
        string json = JsonUtility.ToJson(playerData);
        File.WriteAllText("playerData.json", json);
    }

    void LoadData()
    {
        if (File.Exists("playerData.json"))
        {
            string json = File.ReadAllText("playerData.json");
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log($"Player Name: {playerData.playerName}, Score: {playerData.score}");
        }
    }

    void DeleteData()
    {
        if (File.Exists("playerData.json"))
        {
            File.Delete("playerData.json");
        }
    }
}
