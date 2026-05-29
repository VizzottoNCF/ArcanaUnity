using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string sceneToLoad;
    private ScreenFader sf;

    private void Start() => sf = this?.GetComponent<ScreenFader>();

    public void ChangeSceneNow() { SceneManager.LoadScene(sceneToLoad); }

    public void FadeIntoSceneChange()
    {
        StartCoroutine(sf.Fade(0f, 1f, .5f));

        Invoke("ChangeSceneNow", 2f);
    }
    public void Fade()
    {
        StartCoroutine(sf.Fade(1f, 0f, .25f));
    }
}
