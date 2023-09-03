using System;
using UnityEngine;
using UnityEngine.Tilemaps;

//AssetMeun 자체를 생성
[CreateAssetMenu(fileName = "New Level Tile", menuName = "2D/Tiles/Level Tile")]

public class LevelTile : Tile
{
    public TileType Type;
}

//inspector 노출
[Serializable]
public enum TileType
{ 
    Field = 0,
    Brick,
    Ice,
    Grid,
    Castle
}
