using UnityEngine;

public class FirePoint : MonoBehaviour
{
    [SerializeField] private GameObject _targetObject;
    [SerializeField] private Vector3 _targetOffset;
    [SerializeField] private float _fixedDistance = 1f;
    [SerializeField] private Vector3 _planeNormal = Vector3.forward; // XY plane
    [SerializeField] private float _planeOffset = 0f; // z = 0 plane

    private void Update()
    {
        if (_targetObject == null || GameController.Instance.IsDead) return;

        Vector3 worldPos = _targetObject.transform.position - _targetOffset;

        // Create a plane (XY plane at z = planeOffset)
        Plane plane = new Plane(_planeNormal, new Vector3(0, 0, _planeOffset));

        // Ray from camera through mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Once ray hits plane
        if (plane.Raycast(ray, out float distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);

            // Direction + Position firepoint
            Vector3 direction = (hitPoint - worldPos - _targetOffset).normalized;
            transform.position = worldPos + (direction * _fixedDistance);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}