using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class SaveFileManager : MonoBehaviour
{
    public class saveData
    {
        public PlayerKnownSpells playerKnownSpells = null;
        public PlayerMovementStats playerMoveStats = null;
        public SpawnPoint currSpawn = null;
        public string currScene = null;
        public PlayerResourceStats playerResourceStats = null;
        public List<Spell> equippedSpells = null;
    }

    private IDataService DataService = new JsonDataService();
    private saveData gameData = new saveData();
    public int dataSlot;
    public void SaveJson()
    {
        gameData = RecordGameData();

        if (DataService.SaveData("/savefile" + dataSlot.ToString() + ".json", gameData))
        {
            Debug.Log("Data saved successfully.");
        }
        else
        {
            Debug.LogError("Failed to save data.");
        }
    }

    public void LoadJson(bool newGame=false)
    {
        try
        {
            gameData = DataService.LoadData<saveData>("/savefile" + dataSlot.ToString() + ".json");
            Debug.Log("Data loaded successfully.");


        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load data: {e.Message}");
        }
    }

    public saveData RecordGameData()
    {
        saveData newData = new saveData
        {
            playerKnownSpells = ServiceLocator.Get<SpellBook>()._playerKnownSpells,
            equippedSpells = ServiceLocator.Get<SpellBook>()._spell,
            playerMoveStats = ServiceLocator.Get<PlayerMovement>().moveStats,
            currSpawn = ServiceLocator.Get<RoomService>().GetSpawn("default"),
            currScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
            playerResourceStats = ServiceLocator.Get<SpellBook>().playerStats,
        };

        return newData;
    }

    public void UpdateGameData(saveData data)
    {
            ServiceLocator.Get<RoomTransitionManager>().EnterRoom(data.currScene, data.currSpawn.spawnID);
            ServiceLocator.Get<SpellBook>()._playerKnownSpells = data.playerKnownSpells;
            ServiceLocator.Get<SpellBook>()._spell = data.equippedSpells;
            ServiceLocator.Get<PlayerMovement>().moveStats = data.playerMoveStats;
            ServiceLocator.Get<SpellBook>().playerStats = data.playerResourceStats;
    }
}
