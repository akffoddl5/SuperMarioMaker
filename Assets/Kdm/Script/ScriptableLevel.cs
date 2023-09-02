using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��ũ��Ʈ ������Ʈȭ
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

