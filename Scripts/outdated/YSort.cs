using UnityEngine;

public class YSort : MonoBehaviour
{
    void Start()
    {
        var _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sortingOrder = -(int)(transform.position.y * 100);
    }
}
