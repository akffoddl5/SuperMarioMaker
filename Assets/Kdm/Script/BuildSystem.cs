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

    //Pipe 방향 설정(위쪽 : 0, 오른쪽 : 1, 아래쪽 : 2, 왼쪽 : 3)
    public int pipeDir { get; set; } = 0;
    Vector3Int pipeTopPosition = new Vector3Int(0, 0, -100);
    Vector3Int defaultPipeTopPosition = new Vector3Int(0, 0, -100);

    Vector3 pipeLinkPos = new Vector3(0, 0, -100);
    int dirInfo = 0;
    //List<int> brickItemListInfo = new List<int>();

    //brick


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

        //타일 이름으로 타일과 프리팹을 찾기위해 딕셔너리에 이름과 인덱스 저장
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

        isSetTile = ui_Editor.IsSetTile();

        //if (!ui_Editor.pipeLinkMode)
        if (ui_Editor.functionEditMode == UI_Editor.FunctionEditMode.None)
            ClickSetTile();

        //Debug.Log("????????");
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

        ////카메라 이동
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
            if (pipeDir == 0 || pipeDir == 2)
            {
                tileX = 2;
                tileY = 1;
            }
            else if (pipeDir == 1 || pipeDir == 3)
            {
                tileX = 1;
                tileY = 2;
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
                    TempTilemap.SetTile(tilemapMousePosition + new Vector3Int(i, -j), currentTile[i + j + pipeDir * 4]);
                }
            }
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

        if (currentTileName == "Pipe")
        {

        }
        else
        {
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
        }


        //이전 마우스 위치 저장
        pastMousePosition = tilemapMousePosition;

        //타일 배치
        if (Input.GetMouseButton(0) && isSetTile)
        {

            if (PossibleSetTile(tilemapMousePosition))
            {
                //오브젝트를 생성할 월드포지션을 계산
                Vector3 tilemapToWorldPoint = SetTilemap.CellToWorld(tilemapMousePosition);

                Vector3 createPos = new Vector3(tilemapToWorldPoint.x + grid.cellSize.x / 2,
                    tilemapToWorldPoint.y + grid.cellSize.x / 2);

                GameObject createObj;
                int dirInfo = 0;

                //파이프 배치
                if (currentTileName == "Pipe")
                {
                    int pipePrefabIndex;

                    //첫 클릭만 파이프 입구 생성
                    if (pipeTopPosition == defaultPipeTopPosition)
                    {
                        pipeTopPosition = tilemapMousePosition;

                        //타일맵에 타일 생성
                        for (int i = 0; i < tileX; i++)
                        {
                            for (int j = 0; j < tileY; j++)
                            {
                                SetTilemap.SetTile(tilemapMousePosition + new Vector3Int(i, -j), currentTile[i + j + pipeDir * 4]);
                            }
                        }

                        if (pipeDir == 2)
                            createPos += new Vector3(grid.cellSize.x, 0);
                        else if (pipeDir == 3)
                            createPos += new Vector3(0, -grid.cellSize.y);

                        pipePrefabIndex = 0;
                    }
                    else //파이프 몸통 생성
                    {
                        //파이프 몸통이 입구 밑으로만 만들어지도록 제한
                        if (pipeDir == 0 || pipeDir == 2)
                        {
                            tilemapMousePosition = new Vector3Int(pipeTopPosition.x, tilemapMousePosition.y);
                            if (pipeDir == 0 && tilemapMousePosition.y > pipeTopPosition.y)
                                return;
                            else if (pipeDir == 2 && tilemapMousePosition.y < pipeTopPosition.y)
                                return;
                        }
                        else if (pipeDir == 1 || pipeDir == 3)
                        {
                            tilemapMousePosition = new Vector3Int(tilemapMousePosition.x, pipeTopPosition.y);
                            if (pipeDir == 1 && tilemapMousePosition.x > pipeTopPosition.x)
                                return;
                            else if (pipeDir == 3 && tilemapMousePosition.x < pipeTopPosition.x)
                                return;
                        }
                        if (!PossibleSetTile(tilemapMousePosition))
                        {
                            return;
                        }

                        //타일맵에 타일 생성
                        for (int i = 0; i < tileX; i++)
                        {
                            for (int j = 0; j < tileY; j++)
                            {
                                SetTilemap.SetTile(tilemapMousePosition + new Vector3Int(i, -j), currentTile[i + j + 2 + pipeDir * 4]);
                            }
                        }

                        tilemapToWorldPoint = SetTilemap.CellToWorld(tilemapMousePosition);
                        createPos = new Vector3(tilemapToWorldPoint.x + grid.cellSize.x / 2,
                            tilemapToWorldPoint.y + grid.cellSize.x / 2);

                        if (pipeDir == 2)
                            createPos += new Vector3(grid.cellSize.x, 0);
                        else if (pipeDir == 3)
                            createPos += new Vector3(0, -grid.cellSize.y);

                        pipePrefabIndex = 1;
                    }

                    dirInfo = pipeDir;

                    //실제 오브젝트 생성
                    createObj = Instantiate(currentTileObjectPrefab[pipePrefabIndex], createPos, Quaternion.Euler(0, 0, pipeDir * (-90)));
                }
                else
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

                    createObj = Instantiate(currentTileObjectPrefab[0], createPos, Quaternion.identity);
                }

                //리스트에 생성 정보 저장(이름, 월드 생성위치, 그리드 생성위치 시작, 그리드 생성위치 끝, 생성한 게임 오브젝트)
                //오브젝트 삭제할때도 써먹음
                //objectList.Add(new List<object> { currentTileName, createPos, tilemapMousePosition,
                //    tilemapMousePosition + new Vector3Int(tileX - 1, -(tileY - 1)), createObj });
                objectList.Add(new List<object> { currentTileName, createPos, pipeLinkPos, dirInfo, new List<int>(),
                    tilemapMousePosition, tilemapMousePosition + new Vector3Int(tileX - 1, -(tileY - 1)), createObj });


            }



        }
        //타일 삭제
        else if (Input.GetMouseButton(1) && isSetTile)
        {
            //SetTilemap.SetTile(tilemapMousePosition, null);
            //tilemap.DeleteCells(gridPosition, tilemapPosition);

            //빈 타일인지 확인
            if (SetTilemap.GetTile(tilemapMousePosition) != null)
            {
                //리스트에서 삭제해야할 오브젝트 검색
                for (int listIndex = 0; listIndex < objectList.Count; listIndex++)
                {
                    Vector3Int startSearchPoint = ((Vector3Int)objectList[listIndex][5]);
                    Vector3Int endSearchPoint = ((Vector3Int)objectList[listIndex][6]);
                    //마우스 위치에 해당하는 오브젝트 검색
                    if (tilemapMousePosition.x >= startSearchPoint.x &&
                        tilemapMousePosition.x <= endSearchPoint.x &&
                        tilemapMousePosition.y <= startSearchPoint.y &&
                        tilemapMousePosition.y >= endSearchPoint.y)
                    {
                        //배치된 타일 삭제
                        for (int i = 0; i <= endSearchPoint.x - startSearchPoint.x; i++)
                        {
                            for (int j = 0; j <= -(endSearchPoint.y - startSearchPoint.y); j++)
                            {
                                SetTilemap.SetTile(startSearchPoint + new Vector3Int(i, -j), null);
                            }
                        }
                        //((GameObject)objectList[listIndex][4]).SetActive(false);

                        //생성된 오브젝트 삭제
                        Destroy((GameObject)objectList[listIndex][7]);
                        //리스트 요소에서 제거
                        objectList.RemoveAt(listIndex);
                    }
                }
            }

        }

        //마우스 뗄 때
        if (Input.GetMouseButtonUp(0))
        {
            //파이프 타일 배치가 끝나면 pipeTopPosition 초기화
            if (currentTileName == "Pipe")
            {
                pipeTopPosition = defaultPipeTopPosition;



                //(파이프 끝 부분 HardBrick으로 마무리(미완성))
                ////타일맵에 타일 생성
                //for (int i = 0; i < tileX; i++)
                //{
                //    for (int j = 0; j < tileY; j++)
                //    {
                //        SetTilemap.SetTile(tilemapMousePosition + new Vector3Int(i, -j), tiles[tilesDictionary["HardBrick"]].tile[0]);
                //    }
                //}
            }
        }

    }

    //현재 타일 설정
    public void SetCurrentTile(string _tileName)
    {
        PastTempTileClear();

        //현재 타일 설정
        currentTile = tiles[tilesDictionary[_tileName]].tile;
        currentTileName = _tileName;
        currentTileObjectPrefab = tiles[tilesDictionary[_tileName]].objectPrefab;

    }

    //타일 변경 시 임시 타일맵에 그려진 이전 위치 타일 지워줌
    public void PastTempTileClear()
    {
        for (int i = 0; i < tileX; i++)
        {
            for (int j = 0; j < tileY; j++)
            {
                TempTilemap.SetTile(pastMousePosition + new Vector3Int(i, -j), null);
            }
        }
    }

    bool PossibleSetTile(Vector3Int _tilemapMousePosition)
    {
        for (int i = 0; i < tileX; i++)
        {
            for (int j = 0; j < tileY; j++)
            {
                if (SetTilemap.GetTile(_tilemapMousePosition + new Vector3Int(i, -j)) != null)
                    return false;
            }
        }

        return true;
    }

    public void UndoListInput()
    {

    }

    public void Undo()
    {

    }

    public void PipeLinkPos_ObjectListInput(GameObject _pipeLinkObject_0, GameObject _pipeLinkObject_1)
    {
        for (int listIndex = 0; listIndex < objectList.Count; listIndex++)
        {
            if (_pipeLinkObject_0 == (GameObject)objectList[listIndex][7])
            {
                objectList[listIndex][2] = _pipeLinkObject_1.transform.position;
            }

            if (_pipeLinkObject_1 == (GameObject)objectList[listIndex][7])
            {
                objectList[listIndex][2] = _pipeLinkObject_0.transform.position;
            }
        }
    }

    public int BrickItemSet_ObjectListInput(GameObject _itemSetBrick, int _brickItemNum)
    {
        for (int listIndex = 0; listIndex < objectList.Count; listIndex++)
        {
            if (_itemSetBrick == (GameObject)objectList[listIndex][7])
            {
                ((List<int>)objectList[listIndex][4]).Add(_brickItemNum);

                return ((List<int>)objectList[listIndex][4]).Count;
            }
        }
        return 0;
    }

    public int BrickItemSet_ObjectListOutput(GameObject _itemSetBrick, int _brickItemNum)
    {
        for (int listIndex = 0; listIndex < objectList.Count; listIndex++)
        {
            if (_itemSetBrick == (GameObject)objectList[listIndex][7])
            {
                ((List<int>)objectList[listIndex][4]).RemoveAt(((List<int>)objectList[listIndex][4]).Count - 1);

                return ((List<int>)objectList[listIndex][4]).Count;
            }
        }
        return 0;
    }

    public int BrickItemSet_ObjectListCount(GameObject _itemSetBrick)
    {
        for (int listIndex = 0; listIndex < objectList.Count; listIndex++)
        {
            if (_itemSetBrick == (GameObject)objectList[listIndex][7])
            {
                return ((List<int>)objectList[listIndex][4]).Count;
            }
        }
        return 0;
    }
}
