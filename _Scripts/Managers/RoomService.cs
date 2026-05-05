using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomService : MonoBehaviour
{
    public ParallaxManager parallax;
    public List<SpawnPoint> spawns;

    private void Awake()
    {
        ServiceLocator.Register<RoomService>(this);
    }

    public SpawnPoint GetSpawn(string id)
    {
        return spawns.Find(spawn => spawn.spawnID == id);
    }
}
