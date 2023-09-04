using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
//using static ScriptableMapInfo;
//using static UnityEditor.PlayerSettings;

public class TilemapManager// : MonoBehaviour
{
    ////inspector 노출
    [SerializeField] private Tilemap _gridMap, _tempMap, _setTilemap;
    [SerializeField] public int _levelIndex = 0;

    public int _backgroundNum;
    public float _timerCount;
    public int _playerLifePoint;
    public Vector3 _playerStartPos;
    public int _mapScaleNum;

    public static void SaveLevelFile(ScriptableLevel level)
    {
        
        //newlevel을 asset으로 저장하기, 파일명은 수정가능, 리소스는 필수임
        AssetDatabase.CreateAsset(level, $"Assets/Resources/Levels/{level.name}.asset");
        //asset 저장 확정
        AssetDatabase.SaveAssets();
        //Unity Project를 최신 상태로 갱신 즉 프로젝트 전체의 파일 구성을 체크
        AssetDatabase.Refresh();
    }

    public static void SaveObjFile(ScriptableMapInfo obj)
    {
        //newlevel을 asset으로 저장하기, 파일명은 수정가능, 리소스는 필수임
        AssetDatabase.CreateAsset(obj, $"Assets/Resources/Levels/{obj.name}.asset");
        //asset 저장 확정
        AssetDatabase.SaveAssets();
        //Unity Project를 최신 상태로 갱신 즉 프로젝트 전체의 파일 구성을 체크
        AssetDatabase.Refresh();
    }

    //List<CreateObjectInfo> _createObjectInfoList;

    //맵 저장하는 메서드
    public void SaveMap(ScriptableMapInfo _mapInfo)
    {
        var newObj = ScriptableObject.CreateInstance<ScriptableMapInfo>();

        newObj.name = $"Level Obj {_mapInfo.levelIndex}";

        //값 긁어오는거 필요
        newObj.levelIndex = _mapInfo.levelIndex;
        newObj.backgroundNum = _mapInfo.backgroundNum;
        newObj.timerCount = _mapInfo.timerCount;
        newObj.playerLifePoint = _mapInfo.playerLifePoint;
        newObj.playerStartPos = _mapInfo.playerStartPos;
        newObj.mapScaleNum = _mapInfo.mapScaleNum;

        //foreach (var obj in _createObjectInfoList)
        //{
        //    yield return new CreateObjectInfo()
        //    {

        //    };
        //}

        newObj.createObjectInfoList = _mapInfo.createObjectInfoList;



        //태그 이름 변경
        //GameObject[] obj = GameObject.FindGameObjectsWithTag("Object");

        //IEnumerable<CreateObjectInfo> GetObjFromMap(Tilemap map)
        //{
        //    foreach (var pos in obj)
        //    {
        //        yield return new CreateObjectInfo()
        //        {
        //            objectName = pos.name,
        //            createPos = pos.transform.position,
        //        };
        //    }
        //}

        SaveObjFile(newObj);


        //스크립트 오브젝트화 한 ScriptableLevel을 newLevel에 대입시킴
        var newLevel = ScriptableObject.CreateInstance<ScriptableLevel>();

        newLevel.levelIndex = _mapInfo.levelIndex;
        newLevel.name = $"Level {_mapInfo.levelIndex}";

        ////GetTilesFromMap에서 적용한 리스트에 영향을 주지 않고 타일값만 가져오기
        //newLevel.GroundTiles = GetTilesFromMap(_gridMap).ToList();
        //newLevel.UnitTiles = GetTilesFromMap(_tempMap).ToList();

        //json화 수정 필요함
        var json = JsonUtility.ToJson(newLevel);
        //에셋에 저장
        SaveLevelFile(newLevel);

        ////foreach를 사용하기 위해 IEnumerable 사용, 결과를 반환
        //IEnumerable<SavedTile> GetTilesFromMap(Tilemap map)
        //{
        //    //cellbounds : 타일맵의 경계를 셀 크기로 반환
        //    foreach (var pos in map.cellBounds.allPositionsWithin)
        //    {
        //        //pos에 해당하는 타일이 존재하는지 아닌지 체크
        //        if (map.HasTile(pos))
        //        {
        //            //만약 타일이 존재한다면 levelTile로 반환
        //            var levelTile = map.GetTile<LevelTile>(pos);
        //            //위 계산값을 대입, SavedTile로 전환
        //            yield return new SavedTile()
        //            {
        //                Position = pos,
        //                Tile = levelTile
        //            };
        //        }
        //    }
        //}
    }

    //public void ClearMap()
    //{
    //    //지역변수 var maps는 Tilemap 오브젝트
    //    var maps = FindObjectsOfType<Tilemap>();
    //    var obj = GameObject.FindGameObjectsWithTag("Object");

    //    //maps에 대해 밑에 적힌 작업을 시작함
    //    foreach (var tilemap in maps)
    //    {
    //        //타일맵 제거
    //        tilemap.ClearAllTiles();
    //    }

    //    foreach (var Obj in obj)
    //    {
    //        DestroyImmediate(Obj);
    //    }
    //}

    public void LoadMap(int _levelIndex, out ScriptableMapInfo _scriptableMapInfo)
    {
        //Resources 폴더에서 스크립터블 오브젝트인 ScriptableLevel에
        //Level *.asset 파일을 level에 덮어씌움
        //var level = Resources.Load<ScriptableLevel>($"Levels/Level {_levelIndex}");
        var obj = Resources.Load<ScriptableMapInfo>($"Levels/Level Obj {_levelIndex}");

        _scriptableMapInfo = obj;

        //만약 파일이 없었을 경우
        if (obj == null)// || level == null)
        {
            //디버그 로그를 띄운 뒤 함수 종료
            Debug.LogError($"Level {_levelIndex} does now exist.");
            return;
        }

        

        ////불러오기 전 맵 정리
        ////ClearMap();

        //CreateObjectInfo createObjectInfo = new CreateObjectInfo();

        //Vector3 playerStartPos = obj.playerStartPos;

        //Debug.Log(playerStartPos);

        //foreach (var savedObj in obj.createObjectInfoList)
        //{
        //    switch (savedObj)
        //    {

        //        default:
        //            throw new ArgumentOutOfRangeException();
        //    }

        //}



        ////땅 타일, 한개씩 반복작업 용 foreach
        //foreach (var savedTile in level.GroundTiles)
        //{
        //    //switch에 따라 처리 바꾸기, level에 있는 enum 참고
        //    switch (savedTile.Tile.Type)
        //    {
        //        case TileType.Field:
        //        case TileType.Brick:
        //        case TileType.Ice:
        //        case TileType.Grid:
        //        case TileType.Castle:
        //            //_groundMap에 타일들 처리해주기
        //            SetTile(_gridMap, savedTile);
        //            break;
        //        default:
        //            //만약 처리 중 값이 상정범위 내를 넘어갔을 경우 에러 발생
        //            throw new ArgumentOutOfRangeException();
        //    }
        //}

        ////유닛타일, 위 땅타일하고 똑같음
        //foreach (var savedTile in level.UnitTiles)
        //{
        //    switch (savedTile.Tile.Type)
        //    {
        //        case TileType.Field:
        //        case TileType.Brick:
        //        case TileType.Ice:
        //        case TileType.Grid:
        //        case TileType.Castle:
        //            SetTile(_tempMap, savedTile);
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException();
        //    }
        //}

        //void SetTile(Tilemap map, SavedTile tile)
        //{
        //    //타일을 포지션 및 타일 번호에 따라서 쭉 깔아주는 메서드
        //    map.SetTile(tile.Position, tile.Tile);
        //}

    }

}


    

//level 구조체 시작
public struct Level
{
    public int LevelIndex;
    public List<SavedTile> GroundTiles;
    public List<SavedTile> UnitTiles;

    //데이터를 직렬화하여 문자열로 변환
    public string Serialize()
    {
        //string을 써도 됐지만 stringBuilder를 쓴 이유는 문자열 변경이 잦기 때문에 사용함
        var builder = new StringBuilder();
        //문자열 시작에 g[ cnrkgka
        builder.Append("g[");

        //foreach 반복문
        foreach (var groundTile in GroundTiles)
        {
            //땅타일의 type을 숫자로 변환, position은 문자열로 변환 후 stringBuilder에 추가함
            builder.Append($"{(int)groundTile.Tile.Type}({groundTile.Position.x}," +
                $" {groundTile.Position.y})");
        }
        //데이터의 끝
        builder.Append("]");

        //추가했던 정보들을 문자열로 변환
        return builder.ToString();
    }
}

