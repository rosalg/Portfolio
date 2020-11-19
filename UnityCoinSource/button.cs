using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Sprites;
using UnityEditor;

public class button : MonoBehaviour
{
    public ButtonResponse[] AffectedTiles;
    private Grid grid;

    private void Start()
    {
        grid = FindObjectOfType<Grid>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        for (int i = 0; i < AffectedTiles.Length; i++)
        {
            if (AffectedTiles[i].TransformTilePos)
            {
                Vector3Int pos = grid.WorldToCell(AffectedTiles[i].TransformTilePos.position);
                AffectedTiles[i].TempPos = pos;
                AffectedTiles[i].tilemap.SetTile(pos, AffectedTiles[i].afterTile);
                AffectedTiles[i].tilemap.SetTransformMatrix(pos, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, AffectedTiles[i].ZRotation), Vector3.one));
                continue;
            }
            AffectedTiles[i].tilemap.SetTile(AffectedTiles[i].tilePos, AffectedTiles[i].afterTile);
            AffectedTiles[i].tilemap.SetTransformMatrix(AffectedTiles[i].tilePos, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, AffectedTiles[i].ZRotation), Vector3.one));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        for (int i = AffectedTiles.Length - 1; i >= 0; i--)
        {
            if (AffectedTiles[i].TransformTilePos)
            {
                Vector3Int left = new Vector3Int(AffectedTiles[i].TempPos.x - 1, AffectedTiles[i].TempPos.y, AffectedTiles[i].TempPos.z);
                Vector3Int right = new Vector3Int(AffectedTiles[i].TempPos.x + 1, AffectedTiles[i].TempPos.y, AffectedTiles[i].TempPos.z);
                AffectedTiles[i].tilemap.SetTile(AffectedTiles[i].TempPos, AffectedTiles[i].beforeTile);
                AffectedTiles[i].tilemap.SetTransformMatrix(AffectedTiles[i].TempPos, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, AffectedTiles[i].BeforeZRotation), Vector3.one));

                AffectedTiles[i].tilemap.SetTile(left, AffectedTiles[i].beforeTile);
                AffectedTiles[i].tilemap.SetTransformMatrix(left, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, AffectedTiles[i].BeforeZRotation), Vector3.one));

                AffectedTiles[i].tilemap.SetTile(right, AffectedTiles[i].beforeTile);
                AffectedTiles[i].tilemap.SetTransformMatrix(right, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, AffectedTiles[i].BeforeZRotation), Vector3.one));
                continue;
            }
            AffectedTiles[i].tilemap.SetTile(AffectedTiles[i].tilePos, AffectedTiles[i].beforeTile);
            AffectedTiles[i].tilemap.SetTransformMatrix(AffectedTiles[i].tilePos, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, AffectedTiles[i].BeforeZRotation), Vector3.one));
        }
    }
}
