using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public string sceneName;
    public string enemyID;
    public bool dead;
}

[System.Serializable]
public class EnemySaveFile
{
    public List<EnemyData> enemies = new List<EnemyData>();
}

public static class EnemySaveSystem
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "enemies.json");

    private static EnemySaveFile saveData = new EnemySaveFile();

    // =========================
    // LOAD
    // =========================
    public static void Load()
    {
        if (!File.Exists(SavePath))
        {
            saveData = new EnemySaveFile();
            return;
        }

        string json = File.ReadAllText(SavePath);
        saveData = JsonUtility.FromJson<EnemySaveFile>(json);

        if (saveData == null)
            saveData = new EnemySaveFile();
    }

    // =========================
    // SAVE
    // =========================
    public static void Save()
    {
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(SavePath, json);
    }

    // =========================
    // MARK ENEMY DEAD
    // =========================
    public static void SetEnemyDead(string sceneName, string enemyID)
    {
        EnemyData enemy = saveData.enemies.Find(e => e.sceneName == sceneName && e.enemyID == enemyID);

        if (enemy == null)
        {
            enemy = new EnemyData
            {
                sceneName = sceneName,
                enemyID = enemyID,
                dead = true
            };

            saveData.enemies.Add(enemy);
        }
        else
        {
            enemy.dead = true;
        }

        Save();
    }

    // =========================
    // CHECK IF DEAD
    // =========================
    public static bool IsEnemyDead(string sceneName, string enemyID)
    {
        EnemyData enemy = saveData.enemies.Find(e => e.sceneName == sceneName && e.enemyID == enemyID);

        return enemy != null && enemy.dead;
    }

    // =========================
    // RESET DEAD ENEMIES
    // =========================
    public static void ResetSave()
    {
        saveData = new EnemySaveFile();
        Save();
    }
}