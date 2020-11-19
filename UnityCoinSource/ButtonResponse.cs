using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class ButtonResponse
{
    public Tilemap tilemap;
    public TileBase beforeTile;
    public TileBase afterTile;
    public Vector3Int tilePos;
    public int ZRotation;
    public int BeforeZRotation;
    public Transform TransformTilePos;
    [HideInInspector]
    public Vector3Int TempPos;
}
