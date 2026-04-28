using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public Transform layer;
        [Range(0,1)] public float parallaxFactor;
    }

    public ParallaxLayer[] layers;
    public Transform camTransform;
    private Vector3 lastCamPos;

    public void Initialize(Transform camera)
    {
        camTransform = camera;
        lastCamPos = camTransform.position;
    }

    private void LateUpdate()
    {
        if (camTransform == null) return;

        Vector3 cameraDelta = camTransform.position - lastCamPos;

        foreach (ParallaxLayer l in layers)
        {
            float moveX = cameraDelta.x * l.parallaxFactor;
            float moveY = cameraDelta.y * l.parallaxFactor;

            l.layer.position += new Vector3(moveX, moveY, 0);
        }

        lastCamPos = camTransform.position;
    }
}
