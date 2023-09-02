using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//스크립트 오브젝트화
public class ScriptableLevel : ScriptableObject
{
    public int levelIndex;
    public List<SavedTile> GroundTiles;
    public List<SavedTile> UnitTiles;
}

[Serializable]
public class SavedTile
{
    public Vector3Int Position;
    public LevelTile Tile;
}

