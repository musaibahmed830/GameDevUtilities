using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseExample : MonoBehaviour
{
    DatabaseReference reference;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            reference = FirebaseDatabase.DefaultInstance.RootReference;
        });
    }

    void SaveData()
    {
        string json = "{\"playerName\": \"Player1\", \"score\": 1000}";
        reference.Child("players").Child("player1").SetRawJsonValueAsync(json);
    }

    void LoadData()
    {
        reference.Child("players").Child("player1").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string playerName = snapshot.Child("playerName").Value.ToString();
                int score = int.Parse(snapshot.Child("score").Value.ToString());
                Debug.Log($"Player Name: {playerName}, Score: {score}");
            }
        });
    }

    void DeleteData()
    {
        reference.Child("players").Child("player1").RemoveValueAsync();
    }
}
