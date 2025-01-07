using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class PlayerDataBinary
{
    public string playerName;
    public int score;
}

public class BinaryExample : MonoBehaviour
{
    void SaveData()
    {
        PlayerDataBinary playerData = new PlayerDataBinary { playerName = "Player1", score = 1000 };
        FileStream file = File.Create("playerData.dat");
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, playerData);
        file.Close();
    }

    void LoadData()
    {
        if (File.Exists("playerData.dat"))
        {
            FileStream file = File.Open("playerData.dat", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            PlayerDataBinary playerData = (PlayerDataBinary)bf.Deserialize(file);
            file.Close();
            Debug.Log($"Player Name: {playerData.playerName}, Score: {playerData.score}");
        }
    }

    void DeleteData()
    {
        if (File.Exists("playerData.dat"))
        {
            File.Delete("playerData.dat");
        }
    }
}
