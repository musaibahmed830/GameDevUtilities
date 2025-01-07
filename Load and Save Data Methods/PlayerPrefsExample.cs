using UnityEngine;

public class PlayerPrefsExample : MonoBehaviour
{
    void SaveData()
    {
        PlayerPrefs.SetInt("HighScore", 1000);
        PlayerPrefs.SetString("PlayerName", "Player1");
        PlayerPrefs.Save();
    }

    void LoadData()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        string playerName = PlayerPrefs.GetString("PlayerName", "Guest");
        Debug.Log($"High Score: {highScore}, Player Name: {playerName}");
    }

    void DeleteData()
    {
        PlayerPrefs.DeleteKey("HighScore");
        PlayerPrefs.DeleteKey("PlayerName");
    }
}
