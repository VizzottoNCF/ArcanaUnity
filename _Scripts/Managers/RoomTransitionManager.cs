using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomTransitionManager : MonoBehaviour
{
    [SerializeField] private ScreenFader screenFader;
    [SerializeField] private Transform cam;
    public string currSpawn;
    private string currentRoom = "";
    private bool isTransitioning;
    private SpawnPoint s;
    private void Start()
    {
        ServiceLocator.Register<RoomTransitionManager>(this);

        screenFader.StartCoroutine(screenFader.Fade(1, 0, 1.25f));
        EnterRoom("", "");
    }

    public void EnterRoom(string sceneName, string spawnID, bool save=false)
    {
        if (isTransitioning) { return; }
        StartCoroutine(Transition(sceneName, spawnID, save));
    }

    private IEnumerator Transition(string sceneName, string spawnID, bool save)
    {
        //Debug.LogWarning("Reloading scene to spawn point: " + currSpawn);
        if (GameController.Instance.IsDead) { StopCoroutine("Transition"); Debug.LogWarning("Stopped Coroutine because player is dead"); }
        isTransitioning = true;
        GameController.Instance.CanTakeDamage = false;
        GameController.Instance.CanPlayerMove = false;
        GameController.Instance.gameObject.GetComponent<Animator>().SetBool("isRunning", false);
        GameController.Instance.gameObject.GetComponent<Animator>().SetBool("isGrounded", true);

        if (!screenFader.isFading) { yield return screenFader.StartCoroutine(screenFader.Fade(0, 1, 1.25f)); }


        if (!string.IsNullOrEmpty(currentRoom))
        {
            yield return SceneManager.UnloadSceneAsync(currentRoom);
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }

        Scene newScene = SceneManager.GetSceneByName(sceneName);

        if (newScene.IsValid()) { SceneManager.SetActiveScene(newScene); }
        currentRoom = SceneManager.GetActiveScene().name;

        yield return null;
        RoomService service = ServiceLocator.Get<RoomService>();
        SetupRoom(service, spawnID, save);
        ResetParallax(service);
        ResetCameraPosition();

        yield return screenFader.StartCoroutine(screenFader.Fade(1, 0, 1.25f));

        GameController.Instance.CanTakeDamage = true;
        GameController.Instance.CanPlayerMove = true;
        isTransitioning = false;
    }

    private void ResetCameraPosition()
    {
        CameraManager.instance.ResetCameraPosition();
    }

    private void ResetParallax(RoomService service)
    {
        if (service.parallax != null)
        {
            ParallaxManager pm = service.parallax;
            pm.Initialize(CameraManager.instance.camTransform);
        }
    }
    public void TeleportToSpawnPoint()
    {
        if (isTransitioning || GameController.Instance.IsDead) { return; }
        StartCoroutine(TP2Spawn());
    }

    private IEnumerator TP2Spawn()
    {
        //Debug.LogWarning("Teleporting to spawn point: " + currSpawn);
        if (GameController.Instance.IsDead) { StopCoroutine("TP2Spawn"); Debug.LogWarning("Stopped Coroutine because player is dead"); }
        isTransitioning = true;
        GameController.Instance.CanTakeDamage = false;
        GameController.Instance.CanPlayerMove = false;
        GameController.Instance.gameObject.GetComponent<Animator>().SetBool("isRunning", false);
        GameController.Instance.gameObject.GetComponent<Animator>().SetBool("isGrounded", true);
        RoomService service = ServiceLocator.Get<RoomService>();
        yield return screenFader.StartCoroutine(screenFader.Fade(0, 1, 0.25f));
        SetupRoom(service, currSpawn);
        yield return screenFader.StartCoroutine(screenFader.Fade(1, 0, .5f));
        GameController.Instance.CanTakeDamage = true;
        GameController.Instance.CanPlayerMove = true;
        isTransitioning = false;
    }
    private void SetupRoom(RoomService service, string spawnID, bool save = false)
    {
        if (save) { spawnID = "savePoint"; }
        SpawnPoint spawnToUse = service.spawns[0];
        if (!string.IsNullOrEmpty(spawnID)) { spawnToUse = service.GetSpawn(spawnID); }

        transform.position = spawnToUse.transform.position;
        cam.position = spawnToUse.transform.position;

    }
    public string getCurrentRoom() { return currentRoom; }
}
