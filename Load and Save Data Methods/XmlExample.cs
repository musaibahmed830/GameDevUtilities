using UnityEngine;
using System.IO;
using System.Xml.Serialization;

[System.Serializable]
public class PlayerDataXml
{
    public string playerName;
    public int score;
}

public class XmlExample : MonoBehaviour
{
    void SaveData()
    {
        PlayerDataXml playerData = new PlayerDataXml { playerName = "Player1", score = 1000 };
        XmlSerializer serializer = new XmlSerializer(typeof(PlayerDataXml));
        using (FileStream fs = new FileStream("playerData.xml", FileMode.Create))
        {
            serializer.Serialize(fs, playerData);
        }
    }

    void LoadData()
    {
        if (File.Exists("playerData.xml"))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PlayerDataXml));
            using (FileStream fs = new FileStream("playerData.xml", FileMode.Open))
            {
                PlayerDataXml playerData = (PlayerDataXml)serializer.Deserialize(fs);
                Debug.Log($"Player Name: {playerData.playerName}, Score: {playerData.score}");
            }
        }
    }

    void DeleteData()
    {
        if (File.Exists("playerData.xml"))
        {
            File.Delete("playerData.xml");
        }
    }
}
