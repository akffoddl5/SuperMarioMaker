using System;
using UnityEngine;
using UnityEngine.Tilemaps;

//AssetMeun ��ü�� ����
[CreateAssetMenu(fileName = "New Level Tile", menuName = "2D/Tiles/Level Tile")]

public class LevelTile : Tile
{
    public TileType Type;
}

//inspector ����
[Serializable]
public enum TileType
{ 
    Field = 0,
    Brick,
    Ice,
    Grid,
    Castle
}
