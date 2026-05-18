using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public string spawnID;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ServiceLocator.Get<RoomTransitionManager>().currSpawn = spawnID;
        }
    }
}
