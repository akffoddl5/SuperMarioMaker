using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using static ScriptableMapInfo;

public class BuildSystem : MonoBehaviour
{
    public static BuildSystem instance;

    UI_Editor ui_Editor;
    TilemapManager tilemapManager = new TilemapManager();

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

    [SerializeField] GameObject virtualCamera;

    int backgroundNum = 0;
    float timerCount = 0;
    //int playerLifePoint;
    Vector3 playerStartPos;
    int mapScaleNum = 0;

    [SerializeField] GameObject PlayerPrefab;
    [SerializeField] Tile marioTile;

    [SerializeField] Tiles[] tiles;

    //TileName tileName;

    [SerializeField] private Grid grid;

    [SerializeField] private Tilemap TempTilemap;
    [SerializeField] private Tilemap SetTilemap;
    [SerializeField] private TilemapRenderer SetTilemapRenderer;

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
    List<List<object>> redoList = new List<List<object>>();

    int undoMaxCount = 10;
    int redoMaxCount = 10;

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

    public bool isPlay { get; set; } = false;

    [SerializeField] Sprite[] backgroundSprite;
    [SerializeField] Sprite[] backgroundSkySprite;
    [SerializeField] SpriteRenderer[] background_ground;
    [SerializeField] SpriteRenderer[] background_sky;

    ScriptableMapInfo mapInfo;

    public RenderTexture DrawTexture;

    [SerializeField] GameObject[] mapBoundary;

    [SerializeField] Transform cameraLimitPos_start;
    [SerializeField] Transform[] cameraLimitPos_end;


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

        currentTile[0] = null;

        WIndowManager.instance.mapNum++;

    }

    // Update is called once per frame
    void Update()
    {
        if (isPlay)
        {
            return;
        }

        CameraMove();

        isSetTile = ui_Editor.IsSetTile();

        if (ui_Editor.functionEditMode == UI_Editor.FunctionEditMode.None)
            ClickSetTile();

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
        virtualCamera.transform.Translate(moveX * cameraSpeed * Time.deltaTime,
            moveY * cameraSpeed * Time.deltaTime, 0);
        //Camera.main.transform.Translate(moveX * cameraSpeed * Time.deltaTime,
        //    moveY * cameraSpeed * Time.deltaTime, 0);

        //카메라 리미트 지정
        float posX = virtualCamera.transform.position.x;
        float posY = virtualCamera.transform.position.y;
        if (virtualCamera.transform.position.x <= cameraLimitPos_start.position.x + 11.61f)
            posX = cameraLimitPos_start.position.x + 11.61f;
        else if (virtualCamera.transform.position.x >= cameraLimitPos_end[mapScaleNum].position.x - 11.61f)
            posX = cameraLimitPos_end[mapScaleNum].position.x - 11.61f;

        if (virtualCamera.transform.position.y <= cameraLimitPos_start.position.y + 6.52f)
            posY = cameraLimitPos_start.position.y + 6.52f;
        else if (virtualCamera.transform.position.y >= cameraLimitPos_end[mapScaleNum].position.y - 6.52f)
            posY = cameraLimitPos_end[mapScaleNum].position.y - 6.52f;

        virtualCamera.transform.position = new Vector3(posX, posY, virtualCamera.transform.position.z);
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

                GameObject createObj = null;
                int dirInfo = 0;
                int prefabIndex = 0;

                if (currentTileName == "Mario") //플레이어 배치
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        //플레이어 시작위치는 하나이기 때문에 기존값이 있으면 삭제
                        for (int listIndex = 0; listIndex < objectList.Count; listIndex++)
                        {
                            if ((string)objectList[listIndex][0] == "Mario")
                            {
                                SetTilemap.SetTile((Vector3Int)objectList[listIndex][5], null);
                                Destroy(((GameObject)objectList[listIndex][7]));
                                objectList.RemoveAt(listIndex);

                                break;
                            }
                        }


                        //타일맵에 타일 생성
                        //SetTilemap.SetTile(tilemapMousePosition, currentTile[0]);
                        for (int i = 0; i < tileX; i++)
                        {
                            for (int j = 0; j < tileY; j++)
                            {
                                SetTilemap.SetTile(tilemapMousePosition + new Vector3Int(i, -j), currentTile[i + j * tileX]);
                            }
                        }

                        playerStartPos = createPos;

                        //실제 오브젝트 생성
                        //createObj = Instantiate(currentTileObjectPrefab[prefabIndex], createPos, Quaternion.Euler(0, 0, dirInfo * (-90)));
                        createObj = PhotonNetwork.Instantiate("Prefabs/Mario", createPos, Quaternion.Euler(0, 0, dirInfo * (-90)));
                        createObj.SetActive(false);


                        //리스트에 생성 정보 저장(이름, 월드 생성위치, 그리드 생성위치 시작, 그리드 생성위치 끝, 생성한 게임 오브젝트)
                        //오브젝트 삭제할때도 써먹음
                        //objectList.Add(new List<object> { currentTileName, createPos, tilemapMousePosition,
                        //    tilemapMousePosition + new Vector3Int(tileX - 1, -(tileY - 1)), createObj });
                        objectList.Add(new List<object> { currentTileName, createPos, pipeLinkPos, dirInfo, new List<int>(),
                            tilemapMousePosition, tilemapMousePosition + new Vector3Int(tileX - 1, -(tileY - 1)), createObj });
                    }
                }
                else
                {
                    if (currentTileName == "Pipe") //파이프 배치
                    {
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

                            prefabIndex = 0;
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

                            prefabIndex = 1;
                        }

                        dirInfo = pipeDir;

                    }
                    else //타일 배치
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

                    }


                    //실제 오브젝트 생성
                    createObj = Instantiate(currentTileObjectPrefab[prefabIndex], createPos, Quaternion.Euler(0, 0, dirInfo * (-90)));

                    if (currentTileName != "Pipe" && currentTileName != "Brick" &&
                        currentTileName != "QuestionBrick0" && currentTileName != "IceBrick")
                    {
                        createObj.SetActive(false);
                    }

                    if (currentTileName == "Pipe" && createObj.GetComponent<Pipe_top>() != null)
                        createObj.GetComponent<Pipe_top>().dirInfo = dirInfo;

                    //리스트에 생성 정보 저장(이름, 월드 생성위치, 그리드 생성위치 시작, 그리드 생성위치 끝, 생성한 게임 오브젝트)
                    //오브젝트 삭제할때도 써먹음
                    //objectList.Add(new List<object> { currentTileName, createPos, tilemapMousePosition,
                    //    tilemapMousePosition + new Vector3Int(tileX - 1, -(tileY - 1)), createObj });
                    objectList.Add(new List<object> { currentTileName, createPos, pipeLinkPos, dirInfo, new List<int>(),
                    tilemapMousePosition, tilemapMousePosition + new Vector3Int(tileX - 1, -(tileY - 1)), createObj });

                    UndoListInput(objectList[objectList.Count - 1], true);
                }

                //생성 또는 삭제 시 redoList 클리어
                RedoListClear();
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

                        UndoListInput(objectList[listIndex], false);

                        //생성된 오브젝트 삭제
                        Destroy((GameObject)objectList[listIndex][7]);
                        //리스트 요소에서 제거
                        objectList.RemoveAt(listIndex);

                        //생성 또는 삭제 시 redoList 클리어
                        RedoListClear();
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
        if (_tileName == "Mario")
        {
            currentTile = new Tile[1] { marioTile };
            //currentTile[0] = marioTile;
            currentTileName = _tileName;
            currentTileObjectPrefab = new GameObject[1] { PlayerPrefab };
        }
        else
        {
            currentTile = tiles[tilesDictionary[_tileName]].tile;
            currentTileName = _tileName;
            currentTileObjectPrefab = tiles[tilesDictionary[_tileName]].objectPrefab;
        }


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

    //타일 설치가 가능한지 확인
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

    public void UndoListInput(List<object> _objList, bool _create_true_delete_false)
    {
        undoList.Add(_objList);
        if (undoList[undoList.Count - 1].Count == 8)
            undoList[undoList.Count - 1].Add(_create_true_delete_false);
        else
            undoList[undoList.Count - 1][8] = _create_true_delete_false;

        if (undoList.Count > undoMaxCount)
        {
            undoList.RemoveAt(0);
        }


    }

    public void Undo()
    {
        if (undoList.Count > 0)
        {
            Debug.Log("bool : " + (bool)undoList[undoList.Count - 1][8]);
            Debug.Log("count : " + undoList.Count);
            //undo로 생성한 것을 지울 때
            if ((bool)undoList[undoList.Count - 1][8])
            {
                Vector3Int startSearchPoint = ((Vector3Int)undoList[undoList.Count - 1][5]);
                Vector3Int endSearchPoint = ((Vector3Int)undoList[undoList.Count - 1][6]);
                //배치된 타일 삭제
                for (int i = 0; i <= endSearchPoint.x - startSearchPoint.x; i++)
                {
                    for (int j = 0; j <= -(endSearchPoint.y - startSearchPoint.y); j++)
                    {
                        SetTilemap.SetTile(startSearchPoint + new Vector3Int(i, -j), null);
                    }
                }

                RedoListInput(undoList[undoList.Count - 1]);
                undoList.RemoveAt(undoList.Count - 1);


                //오브젝트 비활성화
                ((GameObject)objectList[objectList.Count - 1][7]).SetActive(false);
                //리스트 요소에서 제거
                objectList.RemoveAt(objectList.Count - 1);

            }
            else //undo로 지운 것을 다시 되돌릴 때
            {
                Debug.Log("되돌려줘");
                int tileX = 1;
                int tileY = 1;
                //현재 타일에 맞게 x,y값 설정
                if ((string)undoList[undoList.Count - 1][0] == "Pipe")
                {
                    if ((int)undoList[undoList.Count - 1][3] == 0 || (int)undoList[undoList.Count - 1][3] == 2)
                    {
                        tileX = 2;
                        tileY = 1;
                    }
                    else if ((int)undoList[undoList.Count - 1][3] == 1 || (int)undoList[undoList.Count - 1][3] == 3)
                    {
                        tileX = 1;
                        tileY = 2;
                    }
                }
                else if ((string)undoList[undoList.Count - 1][0] == "StoneMonster")
                {
                    tileX = 2;
                    tileY = 2;
                }
                else if ((string)undoList[undoList.Count - 1][0] == "castle")
                {
                    tileX = 5;
                    tileY = 5;
                }
                else
                {
                    tileX = 1;
                    tileY = 1;
                }

                //타일맵에 타일 생성
                for (int i = 0; i < tileX; i++)
                {
                    for (int j = 0; j < tileY; j++)
                    {
                        SetTilemap.SetTile((Vector3Int)undoList[undoList.Count - 1][5] + new Vector3Int(i, -j),
                            tiles[tilesDictionary[(string)undoList[undoList.Count - 1][0]]].tile[i + j * tileX]);
                    }
                }

                if ((string)undoList[undoList.Count - 1][0] == "Pipe")
                {
                    //파이트 머리 일 때
                    if ((Vector3)undoList[undoList.Count - 1][2] != defaultPipeTopPosition)
                    {
                        undoList[undoList.Count - 1][7] =
                            Instantiate(tiles[tilesDictionary[(string)undoList[undoList.Count - 1][0]]].objectPrefab[0],
                            (Vector3)undoList[undoList.Count - 1][1],
                            Quaternion.Euler(0, 0, (int)undoList[undoList.Count - 1][3] * (-90)));

                        ((GameObject)undoList[undoList.Count - 1][7]).GetComponent<Pipe_top>().linkObjectPos =
                            (Vector3)undoList[undoList.Count - 1][2];
                    }
                    else
                    {
                        undoList[undoList.Count - 1][7] =
                            Instantiate(tiles[tilesDictionary[(string)undoList[undoList.Count - 1][0]]].objectPrefab[1],
                            (Vector3)undoList[undoList.Count - 1][1],
                            Quaternion.Euler(0, 0, (int)undoList[undoList.Count - 1][3] * (-90)));
                    }

                    ((GameObject)undoList[undoList.Count - 1][7]).SetActive(true);
                }
                else if (currentTileName == "Brick" || currentTileName == "QuestionBrick0" || currentTileName == "IceBrick")
                {
                    undoList[undoList.Count - 1][7] =
                            Instantiate(tiles[tilesDictionary[(string)undoList[undoList.Count - 1][0]]].objectPrefab[0],
                            (Vector3)undoList[undoList.Count - 1][1],
                            Quaternion.Euler(0, 0, (int)undoList[undoList.Count - 1][3] * (-90)));

                    ((GameObject)undoList[undoList.Count - 1][7]).GetComponent<Box>().
                        Add_Item_Num((List<int>)undoList[undoList.Count - 1][4]);

                    ((GameObject)undoList[undoList.Count - 1][7]).SetActive(true);
                }
                else
                {
                    undoList[undoList.Count - 1][7] =
                            Instantiate(tiles[tilesDictionary[(string)undoList[undoList.Count - 1][0]]].objectPrefab[0],
                            (Vector3)undoList[undoList.Count - 1][1],
                            Quaternion.Euler(0, 0, (int)undoList[undoList.Count - 1][3] * (-90)));

                    ((GameObject)undoList[undoList.Count - 1][7]).SetActive(false);
                }

                objectList.Add(undoList[undoList.Count - 1]);

                RedoListInput(undoList[undoList.Count - 1]);
                undoList.RemoveAt(undoList.Count - 1);
            }
        }
    }

    public void RedoListInput(List<object> _undoList)
    {
        redoList.Add(_undoList);

        if (redoList.Count > redoMaxCount)
        {
            if ((bool)redoList[0][8])
            {
                Destroy((GameObject)redoList[0][7]);
            }
            redoList.RemoveAt(0);
        }
    }

    public void RedoListClear()
    {
        for (int i = 0; i < redoList.Count; i++)
        {
            if ((bool)redoList[i][8])
            {
                Destroy((GameObject)redoList[i][7]);
            }
        }
        redoList.Clear();
    }

    public void Redo()
    {
        if (redoList.Count > 0)
        {
            //redo로 지운 것을 생성할 때
            if ((bool)redoList[redoList.Count - 1][8])
            {
                int tileX = 1;
                int tileY = 1;
                //현재 타일에 맞게 x,y값 설정
                if ((string)redoList[redoList.Count - 1][0] == "Pipe")
                {
                    if ((int)redoList[redoList.Count - 1][3] == 0 || (int)redoList[redoList.Count - 1][3] == 2)
                    {
                        tileX = 2;
                        tileY = 1;
                    }
                    else if ((int)redoList[redoList.Count - 1][3] == 1 || (int)redoList[redoList.Count - 1][3] == 3)
                    {
                        tileX = 1;
                        tileY = 2;
                    }
                }
                else if ((string)redoList[redoList.Count - 1][0] == "StoneMonster")
                {
                    tileX = 2;
                    tileY = 2;
                }
                else if ((string)redoList[redoList.Count - 1][0] == "castle")
                {
                    tileX = 5;
                    tileY = 5;
                }
                else
                {
                    tileX = 1;
                    tileY = 1;
                }

                //타일맵에 타일 생성
                for (int i = 0; i < tileX; i++)
                {
                    for (int j = 0; j < tileY; j++)
                    {
                        SetTilemap.SetTile((Vector3Int)redoList[redoList.Count - 1][5] + new Vector3Int(i, -j),
                            tiles[tilesDictionary[(string)redoList[redoList.Count - 1][0]]].tile[i + j * tileX]);
                    }
                }

                if ((string)redoList[redoList.Count - 1][0] == "Pipe")
                {
                    //파이프 머리 일 때
                    if ((Vector3)redoList[redoList.Count - 1][2] != defaultPipeTopPosition)
                    {
                        ((GameObject)redoList[redoList.Count - 1][7]).SetActive(true);
                    }
                }
                else if (currentTileName == "Brick" || currentTileName == "QuestionBrick0" || currentTileName == "IceBrick")
                    ((GameObject)redoList[redoList.Count - 1][7]).SetActive(true);

                redoList[redoList.Count - 1].RemoveAt(redoList[redoList.Count - 1].Count - 1);
                objectList.Add(redoList[redoList.Count - 1]);

                redoList.RemoveAt(redoList.Count - 1);

                UndoListInput(objectList[objectList.Count - 1], true);
            }
            else //redo로 생성한 것을 지울 때
            {

            }
        }
    }

    //파이프 연결 설정
    public void PipeLinkPos_ObjectListInput(GameObject _pipeLinkObject_0, GameObject _pipeLinkObject_1)
    {
        for (int listIndex = 0; listIndex < objectList.Count; listIndex++)
        {
            if (_pipeLinkObject_0 == (GameObject)objectList[listIndex][7])
            {
                objectList[listIndex][2] = _pipeLinkObject_1.GetComponent<Pipe_top>().myTransform.position;
            }

            if (_pipeLinkObject_1 == (GameObject)objectList[listIndex][7])
            {
                objectList[listIndex][2] = _pipeLinkObject_0.GetComponent<Pipe_top>().myTransform.position;
            }
        }
    }

    //블럭 아이템 설정, 블럭에 들어가있는 아이템 갯수 리턴
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

    //블럭 아이템 삭제, 블럭에 들어가있는 아이템 갯수 리턴
    //(안쓸듯?)
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

    //블럭에 들어가있는 아이템 갯수 리턴
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


    public void PlayButtonOn()
    {
        isPlay = true;
        PastTempTileClear();

        for (int i = 0; i < objectList.Count; i++)
        {
            ((GameObject)objectList[i][7]).SetActive(true);

            if (((GameObject)objectList[i][7]).GetComponent<Box>() != null)
                ((GameObject)objectList[i][7]).GetComponent<Box>().Add_Item_Num((List<int>)objectList[i][4]);
        }

        SetTilemapRenderer.enabled = false;

        //virtualCamera.SetActive(true);
    }

    public void StopButtonOn()
    {
        isPlay = false;

        for (int i = 0; i < objectList.Count; i++)
        {
            if ((string)objectList[i][0] == "Mario")
            {
                GameObject player;
                player = PhotonNetwork.Instantiate("Prefabs/Mario", playerStartPos, Quaternion.identity);
                player.SetActive(false);
                Destroy((GameObject)objectList[i][7]);
                objectList[i][7] = player;

            }

            if ((string)objectList[i][0] != "Pipe" && (string)objectList[i][0] != "Brick" &&
                        (string)objectList[i][0] != "QuestionBrick0" && (string)objectList[i][0] != "IceBrick")
            {
                ((GameObject)objectList[i][7]).SetActive(false);
            }
        }

        SetTilemapRenderer.enabled = true;

        //virtualCamera.SetActive(false);
    }


    public void SaveMap()
    {
        StartCoroutine(TakeScreenShot());

        mapInfo = new ScriptableMapInfo();

        mapInfo.name = PhotonNetwork.NickName;
        //mapInfo.levelIndex = WIndowManager.instance.mapNum;
        mapInfo.levelIndex = 0;
        mapInfo.backgroundNum = backgroundNum;
        mapInfo.timerCount = 500;
        mapInfo.playerLifePoint = 1;
        mapInfo.playerStartPos = playerStartPos;
        mapInfo.mapScaleNum = 0;
        for (int i = 0; i < objectList.Count; i++)
        {
            if ((string)objectList[i][0] != "Mario")
                mapInfo.createObjectInfoList.Add(new CreateObjectInfo((string)objectList[i][0], (Vector3)objectList[i][1], (Vector3)objectList[i][2], (int)objectList[i][3], (List<int>)objectList[i][4]));
        }
        tilemapManager.SaveMap(mapInfo);
    }

    public void LoadMap()
    {
        //tilemapManager.LoadMap();
    }

    public void MakeMap(string _name, int _levelIndex)
    {
        //tilemapManager.LoadMap(WIndowManager.instance.mapNum, out mapInfo);
        tilemapManager.LoadMap(_name, _levelIndex, out mapInfo);

        //배경 설정
        for (int i = 0; i < background_ground.Length; i++)
        {
            background_ground[i].sprite = backgroundSprite[mapInfo.backgroundNum];
        }
        for (int i = 0; i < background_sky.Length; i++)
        {
            background_sky[i].sprite = backgroundSkySprite[mapInfo.backgroundNum];
        }

        //타이머 설정

        //플레이어 시작위치 설정

        //맵 크기 설정

        //new Vector3(0, 0, -100);
        //오브젝트 생성
        List<CreateObjectInfo> creatObjList = mapInfo.createObjectInfoList;
        for (int i = 0; i < creatObjList.Count; i++)
        {
            if (creatObjList[i].objectName == "Pipe")
            {
                if (creatObjList[i].pipeLinkPos != new Vector3(0, 0, -100))
                {
                    GameObject createPipe = Instantiate(tiles[tilesDictionary["Pipe"]].objectPrefab[0], creatObjList[i].createPos, Quaternion.Euler(0, 0, creatObjList[i].dirInfo * (-90)));
                    createPipe.GetComponent<Pipe_top>().linkObjectPos = creatObjList[i].pipeLinkPos;

                    createPipe.GetComponent<Pipe_top>().dirInfo = creatObjList[i].dirInfo;

                    createPipe.GetComponent<Pipe_top>().lineActive = true;
                    createPipe.GetComponent<Pipe_top>().isActive = true;
                }
                else
                {
                    Instantiate(tiles[tilesDictionary["Pipe"]].objectPrefab[1], creatObjList[i].createPos, Quaternion.Euler(0, 0, creatObjList[i].dirInfo * (-90)));
                }
            }
            else if (creatObjList[i].objectName == "Brick" || creatObjList[i].objectName == "QuestionBrick0" || creatObjList[i].objectName == "QuestionBrick1" || creatObjList[i].objectName == "IceBrick")
            {
                Instantiate(tiles[tilesDictionary[creatObjList[i].objectName]].objectPrefab[0], creatObjList[i].createPos,
                    Quaternion.Euler(0, 0, creatObjList[i].dirInfo * (-90))).GetComponent<Box>().Add_Item_Num(creatObjList[i].brickListInfo);
                //Debug.Log("아이템 리스트 : " + creatObjList[i].brickListInfo[0] + creatObjList[i].brickListInfo[1] + creatObjList[i].brickListInfo[2]);
            }
            else
            {
                Instantiate(tiles[tilesDictionary[creatObjList[i].objectName]].objectPrefab[0], creatObjList[i].createPos, Quaternion.Euler(0, 0, creatObjList[i].dirInfo * (-90)));

                //for (int j = 0; j < tiles.Length; j++)
                //{
                //    if (creatObjList[i].objectName == tiles[j].tileName)
                //    {
                //        Instantiate(tiles[j].objectPrefab[0], creatObjList[i].createPos, Quaternion.Euler(0, 0, creatObjList[i].dirInfo * (-90)));

                //        break;
                //    }
                //}
            }
        }
    }


    public void BackgroundSet(int _backgroundNum)
    {
        backgroundNum = _backgroundNum;

        for (int i = 0; i < background_ground.Length; i++)
        {
            background_ground[i].sprite = backgroundSprite[backgroundNum];
        }
        for (int i = 0; i < background_sky.Length; i++)
        {
            background_sky[i].sprite = backgroundSkySprite[backgroundNum];
        }
    }

    public void TimerSet(float _timerCount)
    {
        timerCount = _timerCount;
    }


    IEnumerator TakeScreenShot()
    {
        Debug.Log("shot");
        yield return new WaitForEndOfFrame();
        string screenShotName = DateTime.Now.ToString("yyyyMMddHHmmss");

        //var width = Screen.width;
        //var height = Screen.height;

        RenderTexture.active = DrawTexture;
        var texture2D = new Texture2D(DrawTexture.width, DrawTexture.height);
        texture2D.ReadPixels(new Rect(0, 0, DrawTexture.width, DrawTexture.height), 0, 0);
        texture2D.Apply();
        var data = texture2D.EncodeToPNG();
        Directory.CreateDirectory(Application.dataPath + "/../ScreenShot");
        File.WriteAllBytes($"{Application.dataPath}/../ScreenShot/{screenShotName}.png", data);
        //var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        //var tex = renderTexture.EncodeToPNG


        //tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        //tex.Apply();
        //Debug.Log(Application.dataPath);


        //pc

        //Application.dataPath는 해당 프로젝트 Assets 폴더.

        //해당 경로에 NewDirectory라는 이름을 가진 폴더 생성

        //Directory.CreateDirectory(Application.dataPath + "/../ScreenShot");


        //File.WriteAllBytes($"{Application.dataPath}/../ScreenShot/{screenShotName}.png", renderTexture.);

        Debug.Log("shot end");
    }
}
