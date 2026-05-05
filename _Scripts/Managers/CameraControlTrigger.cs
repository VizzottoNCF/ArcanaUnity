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

    private void Start()
    {
        bound = transform.gameObject;
        originalScene = SceneManager.GetActiveScene().name;
        MoveToScene(bound, targetScene);
    }

    private void Update()
    {
        if (_confiner2D != null && _confiner2D.BoundingShape2D == _col2d)
        {
            bool hit = _col2d.bounds.Contains(GameObject.FindGameObjectWithTag("Player").transform.position);
            if (!hit) { _confiner2D.BoundingShape2D = null; }
        }
        
        if (originalScene != SceneManager.GetActiveScene().name) { Destroy(gameObject); }
        else { return; }
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
            _confiner2D = FindFirstObjectByType<CinemachineConfiner2D>();
            _confiner2D.BoundingShape2D = GetComponent<Collider2D>();

            // if CompareTag("Player") is within collider bounds, set confiner to this collider
            if (_col2d.bounds.Contains(GameObject.FindGameObjectWithTag("Player").transform.position)) { _confiner2D.BoundingShape2D = _col2d; }
        }

        if (_confiner2D == null) { Invoke(nameof(RebindConfiner), 0.5f); }    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _confiner2D != null) { _confiner2D.BoundingShape2D = _col2d; }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _confiner2D != null && _confiner2D.BoundingShape2D == null) { _confiner2D.BoundingShape2D = _col2d; }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _confiner2D != null && _confiner2D.BoundingShape2D == _col2d) { _confiner2D.BoundingShape2D = null; }
        
    }
    
}