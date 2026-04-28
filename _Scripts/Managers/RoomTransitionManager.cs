using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomTransitionManager : MonoBehaviour
{
    [SerializeField] private ScreenFader screenFader;
    [SerializeField] private Transform cam;
    private string currentRoom = "";
    private void Start()
    {
        screenFader.StartCoroutine(screenFader.Fade(1, 0, 1.5f));
        EnterRoom("", "");
    }
    public void EnterRoom(string sceneName, string spawnID)
    {
        StartCoroutine(Transition(sceneName, spawnID));
    }

    private IEnumerator Transition(string sceneName, string spawnID)
    {
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

        if (newScene.IsValid())
        {
            SceneManager.SetActiveScene(newScene);
        }
        currentRoom = SceneManager.GetActiveScene().name;


        SetupRoom(spawnID);
        ResetParallax();


        yield return screenFader.StartCoroutine(screenFader.Fade(1, 0, 1.5f));
        GameController.Instance.CanPlayerMove = true;
    }

    private void ResetParallax()
    {
        ParallaxManager pm = FindFirstObjectByType<ParallaxManager>();
        if (pm != null) { pm.Initialize(CameraManager.instance.camTransform); }

    }
    private void SetupRoom(string spawnID)
    {
        SpawnPoint[] spawns = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);
        SpawnPoint spawnToUse = spawns[0];
        //Debug.Log(spawnToUse);
        if (!string.IsNullOrEmpty(spawnID))
        {
            foreach (SpawnPoint s in spawns)
            {
                if (s.spawnID == spawnID)
                {

                    spawnToUse = s;
                    //Debug.Log(spawnToUse);
                    break;
                }
            }
        }
        transform.position = spawnToUse.transform.position;
        cam.position = spawnToUse.transform.position;
    }

    private void ChangeSpawn()
    {

    }
}
