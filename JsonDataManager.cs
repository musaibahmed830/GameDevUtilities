using UnityEngine;
using System.IO;

public static class JsonDataManager
{
    private static string basePath = Application.persistentDataPath + "/";
    public static void SaveData<T>(string fileName, T data)
    {
        string filePath = basePath + fileName;

        string jsonData = JsonUtility.ToJson(data, true);

        try
        {
            File.WriteAllText(filePath, jsonData);
            Debug.Log("Data saved to: " + filePath);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error saving data: " + ex.Message);
        }
    }

    public static T LoadData<T>(string fileName)
    {
        string filePath = basePath + fileName;

        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            try
            {
                T data = JsonUtility.FromJson<T>(jsonData);
                Debug.Log("Data loaded from: " + filePath);
                return data;
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error loading data: " + ex.Message);
            }
        }
        else
        {
            Debug.LogWarning("File not found: " + filePath);
        }
        return default(T);
    }
    public static void DeleteData(string fileName)
    {
        string filePath = basePath + fileName;

        if (File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
                Debug.Log("Data deleted from: " + filePath);
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error deleting data: " + ex.Message);
            }
        }
        else
        {
            Debug.LogWarning("File not found: " + filePath);
        }
    }
    public static bool DataExists(string fileName)
    {
        string filePath = basePath + fileName;
        return File.Exists(filePath);
    }
}
