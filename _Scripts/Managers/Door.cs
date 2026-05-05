using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private string targetScene;
    [SerializeField] private string targetSpawnID;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        RoomTransitionManager manager = collision.GetComponent<RoomTransitionManager>();

        if (manager != null) { manager.EnterRoom(targetScene, targetSpawnID); }
    }
}
