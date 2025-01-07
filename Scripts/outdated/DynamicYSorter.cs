using System;
using UnityEngine;

public class DynamicYSorter : MonoBehaviour
{
    private int _baseSortingOrder;
    private float _ySortingOffset;
    [SerializeField] SortableSprite[] _sortableSprites;
    [SerializeField] Transform _sortOffsetMarker;

    private void Start()
    {
        _ySortingOffset = _sortOffsetMarker.position.y;
    }

    private void Update()
    {
        _baseSortingOrder = transform.GetSortingOrder(_ySortingOffset);

        foreach (var sortableSprite in _sortableSprites)
        {
            sortableSprite._spriteRenderer.sortingOrder = _baseSortingOrder + sortableSprite._relativeOrder;
        }
    }

    [Serializable]
    public struct SortableSprite
    {
        public SpriteRenderer _spriteRenderer;
        public int _relativeOrder;
    }
}