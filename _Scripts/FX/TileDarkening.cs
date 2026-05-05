using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TileDarkening : MonoBehaviour
{
    [System.Serializable]
    public class TileInfo
    {
        public TileBase tile;
        public Vector3Int position;
        public int depth = int.MaxValue;
        public TileInfo(TileBase t, Vector3Int pos)
        {
            this.tile = t;
            this.position = pos;
        }
    }

    [Range(0, 5)] public int maxDepth = 4;
    public List<TileInfo> tiles;
    private Tilemap tilemap;
    private Vector3Int[] directions = new Vector3Int[] {
            new Vector3Int(1, 0, 0),   // right
            new Vector3Int(-1, 0, 0),  // left
            new Vector3Int(0, 1, 0),   // up
            new Vector3Int(0, -1, 0)   // down
        };

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
        tiles = new List<TileInfo>();

        // get every tile
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            Debug.Log(pos);
            TileBase tileBase = tilemap.GetTile(pos);
            if (tileBase is TileBase t)
            {
                TileInfo tI = new TileInfo(t, pos);
                tiles.Add(tI);
            }
        }

        // define edge tiles
        foreach (TileInfo tI in tiles)
        {
            if (IsEdgeTile(tI.position)) { tI.depth = 0; }
        }

        for (int i = 0; i < maxDepth-1; i++)
        {
            foreach (TileInfo tI in tiles)
            {
                if (tI.depth != maxDepth) { continue; }
                foreach (Vector3Int dir in directions)
                {
                    Vector3Int checkPos = tI.position + dir;
                    if (tilemap.HasTile(checkPos))
                    {
                        TileInfo neighbor = tiles.Find(t => t.position == checkPos);
                        if (neighbor.depth > i) { tI.depth = i + 1; }
                    }
                }
            }
        }

        foreach (TileInfo tI in tiles)
        {
            if (tI.depth != int.MaxValue) { continue; }
            tI.depth = maxDepth;
        }

        foreach (TileInfo tI in tiles)
        {
            Color c;
            if (tI.depth == 0) { c = Color.white; }
            else
            {
                float depthFactor = tI.depth / (float)maxDepth;
                float darkenAmount = 1f - depthFactor;
            
                c = new Color(darkenAmount, darkenAmount, darkenAmount, 1f);

                TileFlags flags = tilemap.GetTileFlags(tI.position);
                flags &= ~TileFlags.LockColor;
                tilemap.SetTileFlags(tI.position, flags);
            }
            tilemap.SetColor(tI.position, c);
        }

        tilemap.RefreshAllTiles();
    }

    private bool IsEdgeTile(Vector3Int pos)
    {
        
        bool hit = false;
        foreach (Vector3Int dir in directions)
        {
            Vector3Int checkPos = pos + dir;
            if (!tilemap.HasTile(checkPos))
            {
                hit = true;
                break;
            }
        }

        return hit;
    }
}
