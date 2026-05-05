using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomTransitionManager : MonoBehaviour
{
    [SerializeField] private ScreenFader screenFader;
    [SerializeField] private Transform cam;
    private string currentRoom = "";
    private bool isTransitioning;
    private void Start()
    {
        ServiceLocator.Register<RoomTransitionManager>(this);

        screenFader.StartCoroutine(screenFader.Fade(1, 0, 1.5f));
        EnterRoom("", "");
    }
    public void EnterRoom(string sceneName, string spawnID)
    {
        if (isTransitioning) { return; }
        StartCoroutine(Transition(sceneName, spawnID));
    }

    private IEnumerator Transition(string sceneName, string spawnID)
    {
        isTransitioning = true;
        GameController.Instance.CanPlayerMove = false;
        GameController.Instance.gameObject.GetComponent<Animator>().SetBool("isRunning", false);
        GameController.Instance.gameObject.GetComponent<Animator>().SetBool("isGrounded", true);

        if (!screenFader.isFading) { yield return screenFader.StartCoroutine(screenFader.Fade(0, 1, 0.5f)); }

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
        SetupRoom(service, spawnID);
        ResetParallax(service);

        yield return screenFader.StartCoroutine(screenFader.Fade(1, 0, 0.5f));

        GameController.Instance.CanPlayerMove = true;
        isTransitioning = false;
    }

    private void ResetParallax(RoomService service)
    {
        if (service.parallax != null)
        {
            ParallaxManager pm = service.parallax;
            pm.Initialize(CameraManager.instance.camTransform);
        }
    }
    private void SetupRoom(RoomService service, string spawnID)
    {
        SpawnPoint spawnToUse = service.spawns[0];
        if (!string.IsNullOrEmpty(spawnID)) { spawnToUse = service.GetSpawn(spawnID); }

        transform.position = spawnToUse.transform.position;
        cam.position = spawnToUse.transform.position;

    }

    private void ChangeSpawn()
    {

    }
}
