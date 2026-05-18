using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraControlTrigger : MonoBehaviour
{
    private GameObject bound;
    private string targetScene = "Bootstrap";
    private string originalScene;
    public Collider2D _col2d;
    public CinemachineConfiner2D _confiner2D;
    public CinemachineCamera cam;
    private CinemachinePositionComposer _comp;
    private bool hasCam = false;
    private bool stay = false;

    private void Start()
    {
        bound = transform.gameObject;
        originalScene = SceneManager.GetActiveScene().name;
        if (cam != null)
        {
            hasCam = true;
            _comp = cam?.GetComponent<CinemachinePositionComposer>();
            MoveToScene(cam.gameObject, targetScene);
        }
        MoveToScene(bound, targetScene);
    }

    private void Update()
    {
        if (originalScene != SceneManager.GetActiveScene().name)
        {
            if (hasCam) { Destroy(cam.gameObject); }
            Destroy(gameObject);
        }
        else { return; }
    }

    private void FixedUpdate()
    {
        if (!stay) // not in bounds of the collider
        {
            if (_confiner2D.BoundingShape2D == _col2d) { _confiner2D.BoundingShape2D = null; }
            if (hasCam) { cam.Priority = 0; }
        }
        //else { Debug.Log("Player is within " + gameObject.name + " bounds."); }
    }

    public void MoveToScene(GameObject targetObject, string sceneName)
    {
        // 1. Get the destination scene by name
        Scene destinationScene = SceneManager.GetSceneByName(sceneName);

        // 2. Ensure the object is a root object (no parent)
        targetObject.transform.SetParent(null);

        // 3. Move the object
        SceneManager.MoveGameObjectToScene(targetObject, destinationScene);

        // 4. Confiner2D setup
        RebindConfiner();
    }

    private void RebindConfiner()
    {
        Scene destinationScene = SceneManager.GetSceneByName(targetScene);
        if (destinationScene.isLoaded)
        {
            GameObject go = GameObject.Find("CenterPlayerFollowCam");
            _confiner2D = go.GetComponent<CinemachineConfiner2D>();
            _confiner2D.BoundingShape2D = GetComponent<Collider2D>();

            if (hasCam && cam.GetComponent<CinemachinePositionComposer>() != null && cam.GetComponent<CinemachineConfiner2D>() != null)
            {
                CinemachinePositionComposer comp = go.GetComponent<CinemachinePositionComposer>();
                cam.Target = go.GetComponent<CinemachineCamera>().Target;
                //_comp.CameraDistance = comp.CameraDistance;
                //_comp.Damping = comp.Damping;
                //_comp.TargetOffset = new Vector3(comp.TargetOffset.x, comp.TargetOffset.y, comp.TargetOffset.z);
            }
        }

        if (_confiner2D == null) { Invoke(nameof(RebindConfiner), 0.5f); }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _confiner2D != null)
        {
            Debug.Log("Player entered " + gameObject.name + " bounds.");
            stay = true;
            if (hasCam) { cam.Priority = 2; }
            else { _confiner2D.BoundingShape2D = _col2d; }
        }

    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        Debug.Log("Player is staying within " + gameObject.name + " bounds.");
    //        stay = true;
    //        if (hasCam) { cam.Priority = 2; }
    //        else if (_confiner2D != null) { _confiner2D.BoundingShape2D = _col2d; }
    //    }
    //}
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            stay = false;
            if (_confiner2D.BoundingShape2D == _col2d) { _confiner2D.BoundingShape2D = null; }
            if (hasCam) { cam.Priority = 0; }
        }

    }

}