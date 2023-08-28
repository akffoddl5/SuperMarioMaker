using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildSystem : MonoBehaviour
{
    public static BuildSystem instance;

    UI_Editor ui_Editor;

    [Serializable]
    class Tiles
    {
        [SerializeField] Tile[] Tile;// { get; private set; }
        public Tile[] tile { get { return Tile; } }

        [SerializeField] string TileName;
        public string tileName { get { return TileName; } }

        [SerializeField] GameObject[] ObjectPrefab;
        public GameObject[] objectPrefab { get { return ObjectPrefab; } }
    }

    //public enum TileName
    //{
    //    ground,
    //    brick,
    //    castle,
    //    coin,
    //    flag,
    //    flower,
    //    hardBrick,
    //    iceBrick,
    //    iceCoin,
    //    MushroomRed,
    //    MushroomGreen,
    //    nomalBrick,
    //    pipe,
    //    questionBrick0,
    //    questionBrick1,
    //    star,
    //    stoneMonster,
    //    thornsBrick,
    //    goomba,
    //    turtle,
    //    boo
    //}

    [SerializeField] public int AAA { get; private set; }

    [SerializeField]
    Tiles[] tiles;

    //TileName tileName;

    [SerializeField] private Grid grid;

    [SerializeField] private Tilemap TempTilemap;
    [SerializeField] private Tilemap SetTilemap;

    int tileNum = 0;
    //[SerializeField] private Tile deleteTile;
    [SerializeField] private List<Tile> tileList;
    //Dictionary<TileName, Tile> tiles = new Dictionary<TileName, Tile>();
    Dictionary<string, int> tilesDictionary = new Dictionary<string, int>();

    public Tile[] currentTile { get; private set; } = new Tile[1];
    //[SerializeField] public TileName currentTileName { get; private set; }
    public string currentTileName { get; private set; } = null;
    GameObject[] currentTileObjectPrefab;


    List<List<object>> objectList = new List<List<object>>();
    List<List<object>> undoList = new List<List<object>>();

    //[Serializable]
    //public class MyTiles
    //{
    //    public Tile[] tiles;
    //}
    //[SerializeField] private MyTiles[] tiles;


    [SerializeField] bool isSetTile = false;
    int tileX = 1;
    int tileY = 1;


    Vector3Int pastMousePosition;


    [SerializeField] float cameraSpeed;
    [SerializeField] float cameraMoveTriggerPos;





    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        ui_Editor = UI_Editor.instance;

        //for (int i = 0; i < tileList.Count; i++)
        //{
        //    tiles.Add((TileName)i, tileList[i]);
        //}

        for (int i = 0; i < tiles.Length; i++)
        {
            tilesDictionary.Add(tiles[i].tileName, i);
        }
        Debug.Log(tiles.Length);

        currentTile[0] = null;

    }

    // Update is called once per frame
    void Update()
    {
        CameraMove();

        //SetCurrentTile(currentTileName);

        isSetTile = ui_Editor.IsSetTile();

        ClickSetTile();


        //리스트 테스트용 
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Instantiate((GameObject)objectList[objectList.Count - 1][4], (Vector3)objectList[objectList.Count - 1][1], Quaternion.identity);
        }

    }

    private void CameraMove()
    {
        //뷰포트 좌표
        Vector2 mousePositionInCamera = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        //좌표 (0 ~ 1) => (-1 ~ 1) 변환
        mousePositionInCamera = new Vector2(mousePositionInCamera.x * 2 - 1,
            mousePositionInCamera.y * 2 - 1);

        float moveX = 0;
        float moveY = 0;
        if (mousePositionInCamera.x >= cameraMoveTriggerPos)
            moveX = 1;
        else if (mousePositionInCamera.x <= -cameraMoveTriggerPos)
            moveX = -1;

        if (mousePositionInCamera.y >= cameraMoveTriggerPos)
            moveY = 1;
        else if (mousePositionInCamera.y <= -cameraMoveTriggerPos)
            moveY = -1;

        //카메라 이동
        Camera.main.transform.Translate(moveX * cameraSpeed * Time.deltaTime,
            moveY * cameraSpeed * Time.deltaTime, 0);
    }

    private void ClickSetTile()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Debug.Log("마우스 위치 : " + mousePosition);
        //Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        Vector3Int tilemapMousePosition = SetTilemap.WorldToCell(mousePosition);

        //if (pastMousePosition != tilemapMousePosition)

        //현재 타일에 맞게 x,y값 설정
        if (currentTileName == "Pipe")
        {
            tileX = 2;
        }
        else if (currentTileName == "StoneMonster")
        {
            tileX = 2;
            tileY = 2;
        }
        else if (currentTileName == "castle")
        {
            tileX = 5;
            tileY = 5;
        }
        else
        {
            tileX = 1;
            tileY = 1;
        }

        //임시 타일맵에 그려진 이전 위치 타일 지워줌
        for (int i = 0; i < tileX; i++)
        {
            for (int j = 0; j < tileY; j++)
            {
                TempTilemap.SetTile(pastMousePosition + new Vector3Int(i, -j), null);
            }
        }
        //임시 타일맵에 배치할 타일 표시
        for (int i = 0; i < tileX; i++)
        {
            for (int j = 0; j < tileY; j++)
            {
                TempTilemap.SetTile(tilemapMousePosition + new Vector3Int(i, -j), currentTile[i + j * tileX]);
            }
        }

        //이전 마우스 위치 저장
        pastMousePosition = tilemapMousePosition;

        //타일 배치
        if (Input.GetMouseButton(0) && isSetTile)
        {
            if (SetTilemap.GetTile(tilemapMousePosition) == null)
            {
                //타일맵에 타일 생성
                //SetTilemap.SetTile(tilemapMousePosition, currentTile[0]);
                for (int i = 0; i < tileX; i++)
                {
                    for (int j = 0; j < tileY; j++)
                    {
                        SetTilemap.SetTile(tilemapMousePosition + new Vector3Int(i, -j), currentTile[i + j * tileX]);
                    }
                }

                //실제 오브젝트 생성
                //(생성할 오브젝트, 생성위치 리스트에 저장해놓고 한번에 생성할듯?)
                Vector3 tilemapToWorldPoint = SetTilemap.CellToWorld(tilemapMousePosition);
                Vector3 createPos = new Vector3(tilemapToWorldPoint.x + grid.cellSize.x / 2,
                    tilemapToWorldPoint.y + grid.cellSize.x / 2);
                GameObject createObj = Instantiate(currentTileObjectPrefab[0], createPos, Quaternion.identity);

                //리스트에 생성 정보 저장(이름, 월드 생성위치, 그리드 생성위치 시작, 그리드 생성위치 끝, 생성한 게임 오브젝트)
                objectList.Add(new List<object> { currentTileName, createPos, tilemapMousePosition,
                    tilemapMousePosition + new Vector3Int(tileX - 1, -(tileY - 1)), createObj });
            }



        }
        else if (Input.GetMouseButton(1) && isSetTile)
        {
            //SetTilemap.SetTile(tilemapMousePosition, null);
            //tilemap.DeleteCells(gridPosition, tilemapPosition);

            if (SetTilemap.GetTile(tilemapMousePosition) != null)
            {
                for (int listIndex = 0; listIndex < objectList.Count; listIndex++)
                {
                    Vector3Int startSearchPoint = ((Vector3Int)objectList[listIndex][2]);
                    Vector3Int endSearchPoint = ((Vector3Int)objectList[listIndex][3]);
                    if (tilemapMousePosition.x >= startSearchPoint.x &&
                        tilemapMousePosition.x <= endSearchPoint.x &&
                        tilemapMousePosition.y <= startSearchPoint.y &&
                        tilemapMousePosition.y >= endSearchPoint.y)
                    {
                        for (int i = 0; i <= endSearchPoint.x - startSearchPoint.x; i++)
                        {
                            for (int j = 0; j <= -(endSearchPoint.y - startSearchPoint.y); j++)
                            {
                                SetTilemap.SetTile(startSearchPoint + new Vector3Int(i, -j), null);
                            }
                        }
                        //((GameObject)objectList[listIndex][4]).SetActive(false);
                        Destroy((GameObject)objectList[listIndex][4]);
                        objectList.RemoveAt(listIndex);
                    }
                }
            }

        }
    }

    public void SetCurrentTile(string _tileName)
    {
        for (int i = 0; i < tileX; i++)
        {
            for (int j = 0; j < tileY; j++)
            {
                TempTilemap.SetTile(pastMousePosition + new Vector3Int(i, -j), null);
            }
        }

        currentTile = tiles[tilesDictionary[_tileName]].tile;
        currentTileName = _tileName;
        currentTileObjectPrefab = tiles[tilesDictionary[_tileName]].objectPrefab;
    }

    public void UndoListInput()
    {

    }

    public void Undo()
    {

    }
}
