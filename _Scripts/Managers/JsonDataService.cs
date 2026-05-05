using Newtonsoft.Json;
using System.IO;
using System;
using UnityEngine;

public class JsonDataService : IDataService
{
    public bool SaveData<T>(string relativePath, T data)
    {
        string path = Application.persistentDataPath + relativePath;

        if (File.Exists(path))
        {
            try
            {
                Debug.LogWarning($"File at {relativePath} already exists and will be overwritten.");
                File.Delete(path);
                using FileStream fs = File.Create(path);
                fs.Close();

                File.WriteAllText(path, JsonConvert.SerializeObject(data));
                return true;
            }
            catch (Exception c) { Debug.LogError($"Failed to overwrite existing file at {relativePath}: {c.Message}"); return false; }
        }
        else
        {
            try
            {
                Debug.Log($"Creating new file at {relativePath}.");
                using FileStream fs = File.Create(path);
                fs.Close();

                File.WriteAllText(path, JsonConvert.SerializeObject(data));
                return true;
            }
            catch (Exception c) { Debug.LogError($"Failed to create and write file at {relativePath}: {c.Message}"); return false; }
        }
    }

    public T LoadData<T>(string relativePath)
    {
        string path = Application.persistentDataPath + relativePath;
        if (!File.Exists(path))
        {
            Debug.LogError($"File at {relativePath} does not exist. Cannot load data.");
            throw new FileNotFoundException($"File at {relativePath} not found.");
        }

        try
        {
            T data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            return data;
        }
        catch (Exception c)
        {
            Debug.LogError($"Failed to load and deserialize file at {relativePath}: {c.Message}");
            throw new Exception($"Failed to load data from {relativePath}", c);
        }
    }
}
