using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using static UI_Editor;

public class UI_Editor : MonoBehaviour
{
    public static UI_Editor instance;

    BuildSystem buildSystem;

    [Header("Panel")]
    [SerializeField] RectTransform upPanel;
    [SerializeField] RectTransform leftPanel;
    [SerializeField] RectTransform rightPanel;

    [Header("UpPanel")]
    [SerializeField] GameObject[] buttonEdge;
    [SerializeField] Image[] tileSetButtonImage;

    [Header("LeftPanel")]
    [SerializeField] Image backgroundSetImage;
    [SerializeField] Sprite[] backgroundSprite;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] Slider timerSlider;
    [SerializeField] TextMeshProUGUI lifeCountText;

    [Header("RightPanel")]
    [SerializeField] GameObject characterSetEdge;

    [Header("ButtonPanel")]
    [SerializeField] GameObject[] buttonPanel;
    [SerializeField] GameObject PipePanel;

    [SerializeField] GameObject brickSetPanel;

    [SerializeField] RectTransform stopButton;

    int currentOpenButtonPanelNumber = -1;

    public enum FunctionEditMode
    {
        None,
        PipeLinkMode,
        BrickItemSetMode
    }
    public FunctionEditMode functionEditMode { get; private set; } = FunctionEditMode.None;
    //public bool pipeLinkMode { get; private set; } = false;
    GameObject[] pipeLinkObject = new GameObject[2];
    List<GameObject> alreadyPipeLinkObject = new List<GameObject>();


    [SerializeField] GameObject[] itemPrefab;
    GameObject currentSetBrick = null;
    [SerializeField] TextMeshProUGUI itemCountText;
    int itemCount = 0;


    bool isPanelOutIn = false;
    bool isPanelIn = false;
    Vector3 upPanelPos;
    Vector3 leftPanelPos;
    Vector3 rightPanelPos;
    Vector3 stopButtonPos;

    int backgroundNum = 0;


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
        buildSystem = BuildSystem.instance;

        upPanelPos = upPanel.position;
        leftPanelPos = leftPanel.position;
        rightPanelPos = rightPanel.position;
        stopButtonPos = stopButton.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (functionEditMode != FunctionEditMode.BrickItemSetMode && brickSetPanel.activeSelf)
        {
            BrickSetPanel_Off();
        }
        FunctionEditMode_On();
        //Debug.Log(functionEditMode);
    }

    private void FunctionEditMode_On()
    {
        //파이프 연결 모드
        if (functionEditMode == FunctionEditMode.PipeLinkMode)
        {
            //마우스 클릭
            if (Input.GetMouseButtonDown(0))
            {
                //클릭한 대상 확인
                GameObject tempObject = RaycastHitObject();
                Debug.Log(tempObject);
                if (tempObject != null)
                {
                    PipeLink(tempObject);
                }

            }
        }
        else if (functionEditMode == FunctionEditMode.BrickItemSetMode)
        {
            //마우스 클릭
            if (Input.GetMouseButtonDown(0))
            {
                //클릭한 대상 확인
                GameObject tempObject = RaycastHitObject();
                if (tempObject != null)
                {
                    BrickSet(tempObject);
                }
            }
            //Debug.Log(currentSetBrick);

        }
    }

    void BrickSet(GameObject _tempObject)
    {
        //클릭한 대상이 블럭일 때
        if (_tempObject.GetComponent<Box>() != null || _tempObject.GetComponentInParent<Box>() != null)
        {
            currentSetBrick = _tempObject;

            itemCountText.text = buildSystem.BrickItemSet_ObjectListCount(currentSetBrick).ToString();
        }
    }


    void PipeLink(GameObject _tempObject)
    {
        //클릭한 대상이 파이프일 때
        if (_tempObject.GetComponent<Pipe_top>() != null)
        {
            //이미 연결된 파이프인지 확인
            for (int i = 0; i < alreadyPipeLinkObject.Count; i++)
            {
                if (alreadyPipeLinkObject[i] == _tempObject)
                {
                    //Debug.Log("이미 연결된 파이프");
                    return;
                }
            }
            //첫번째와 같은 파이프인지 확인(자기 자신에 연결 X)
            if (_tempObject == pipeLinkObject[0])
            {
                //Debug.Log("첫번째와 같은 파이프");
                return;
            }

            int pipeLinkPosIndex;
            if (pipeLinkObject[0] == null)
            {
                pipeLinkPosIndex = 0;
            }
            else if (pipeLinkObject[1] == null)
            {
                pipeLinkPosIndex = 1;
            }
            else
            {
                return;
            }


            pipeLinkObject[pipeLinkPosIndex] = _tempObject;

            if (pipeLinkPosIndex == 0)
            {
                pipeLinkObject[pipeLinkPosIndex].GetComponent<Pipe_top>().lineActive = true;
            }
            else if (pipeLinkPosIndex == 1)
            {
                buildSystem.PipeLinkPos_ObjectListInput(pipeLinkObject[0], pipeLinkObject[1]);
                //Debug.Log("파이프 연결");
                pipeLinkObject[0].GetComponent<Pipe_top>().linkObjectPos =
                    pipeLinkObject[1].GetComponent<Pipe_top>().myTransform.position;
                pipeLinkObject[1].GetComponent<Pipe_top>().linkObjectPos =
                    pipeLinkObject[0].GetComponent<Pipe_top>().myTransform.position;

                alreadyPipeLinkObject.Add(pipeLinkObject[0]);
                alreadyPipeLinkObject.Add(pipeLinkObject[1]);

                pipeLinkObject[0] = null;
                pipeLinkObject[1] = null;
            }
        }
    }

    GameObject RaycastHitObject()
    {
        Vector3 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(ray, transform.forward, 15);

        if (hit)
        {
            return hit.collider.gameObject;
        }

        return null;
    }


    public bool IsSetTile()
    {
        Vector2 mousePos = Input.mousePosition;
        if ((mousePos.x >= upPanel.position.x && mousePos.x <= upPanel.position.x + upPanel.rect.width &&
            mousePos.y >= upPanel.position.y && mousePos.y <= upPanel.position.y + upPanel.rect.height) ||
            (mousePos.x >= leftPanel.position.x && mousePos.x <= leftPanel.position.x + leftPanel.rect.width &&
            mousePos.y >= leftPanel.position.y && mousePos.y <= leftPanel.position.y + leftPanel.rect.height) ||
            (mousePos.x >= rightPanel.position.x && mousePos.x <= rightPanel.position.x + rightPanel.rect.width &&
            mousePos.y >= rightPanel.position.y && mousePos.y <= rightPanel.position.y + rightPanel.rect.height) ||
            buildSystem.currentTileName == null || currentOpenButtonPanelNumber != -1)
            return false;

        return true;
    }


    #region Button

    //타일 선택 패널 버튼 클릭
    public void TileSetPanelButtonClick(int _buttonNum)
    {
        //if (currentOpenButtonPanelNumber < tileSetButtonEdge.Length)
        //    tileSetButtonEdge[currentOpenButtonPanelNumber].SetActive(false);
        ButtonPanel_OnOff(_buttonNum);
        //tileSetButtonEdge[currentOpenButtonPanelNumber].SetActive(false);
    }

    //맵 버튼 클릭
    public void MapButtonClick()
    {
        ButtonPanel_OnOff(buttonPanel.Length - 1);
    }

    //버튼 패널 On, Off
    void ButtonPanel_OnOff(int _buttonNum)
    {
        if (_buttonNum < 0 || _buttonNum >= buttonPanel.Length)
        {
            //Debug.Log("배열 범위를 벗어남");
            return;
        }

        //클릭한 버튼과 다른 패널이 켜져있거나 켜져있는 패널이 없는 경우 
        if (currentOpenButtonPanelNumber != _buttonNum)
        {
            //켜져있는 패널이 있을 경우 끔
            if (currentOpenButtonPanelNumber != -1)
                ButtonPanelSetActive_FALSE(currentOpenButtonPanelNumber);

            //클릭한 버튼 패널을 켠다
            currentOpenButtonPanelNumber = _buttonNum;
            ButtonPanelSetActive_TRUE(currentOpenButtonPanelNumber);
        }
        else //현재 켜져있는 패널의 버튼을 누르면 패널을 끔
        {
            ButtonPanelSetActive_FALSE(currentOpenButtonPanelNumber);
            currentOpenButtonPanelNumber = -1;
        }

        if (functionEditMode == FunctionEditMode.BrickItemSetMode)
        {
            functionEditMode = FunctionEditMode.None;
        }
    }

    //버튼 패널 On
    void ButtonPanelSetActive_TRUE(int _buttonNum)
    {
        buttonPanel[_buttonNum].SetActive(true);
        buttonEdge[_buttonNum].SetActive(true);
    }

    //버튼 패널 Off
    void ButtonPanelSetActive_FALSE(int _buttonNum)
    {
        buttonPanel[_buttonNum].SetActive(false);
        buttonEdge[_buttonNum].SetActive(false);
    }


    //타일 선택
    public void TileButtonClick(string _tileName)
    {
        buildSystem.SetCurrentTile(_tileName);

        if (_tileName != "Mario")
        {
            //타일 선택 이미지 변경
            tileSetButtonImage[currentOpenButtonPanelNumber].sprite =
                buildSystem.currentTile[buildSystem.currentTile.Length - 1].sprite;
        }

        ButtonPanel_OnOff(currentOpenButtonPanelNumber);

        //pipeLinkMode = false;
        functionEditMode = FunctionEditMode.None;
    }


    //파이프 패널 OFF
    public void PipePanel_Off()
    {
        PipePanel.SetActive(false);
    }

    //파이프 선택 버튼(방향)
    public void PipeTileButtonClick(int _pipeDir)
    {
        buildSystem.pipeDir = _pipeDir;
        TileButtonClick("Pipe");

        PipePanel_Off();
    }

    //파이프 연결 버튼
    public void PipeLinkButtonClick()
    {
        //pipeLinkMode = true;
        ButtonPanel_OnOff(currentOpenButtonPanelNumber);

        functionEditMode = FunctionEditMode.PipeLinkMode;

        buildSystem.PastTempTileClear();

        PipePanel_Off();
    }


    //블럭 아이템 편집 버튼
    public void BrickItemSetButtonClick()
    {
        buildSystem.PastTempTileClear();

        ButtonPanel_OnOff(currentOpenButtonPanelNumber);

        functionEditMode = FunctionEditMode.BrickItemSetMode;

        //buildSystem.PastTempTileClear();

        brickSetPanel.SetActive(true);

    }

    //아이템 편집 기능 OFF
    public void BrickSetPanel_Off()
    {
        functionEditMode = FunctionEditMode.None;

        brickSetPanel.SetActive(false);
    }

    //블럭에 넣을 아이템 선택
    public void BrickItemButtonClick(int _brickItemNum)
    {
        int itemCount = buildSystem.BrickItemSet_ObjectListInput(currentSetBrick, _brickItemNum);

        itemCountText.text = itemCount.ToString();
    }


    //플레이 버튼 클릭
    public void PlayButtonClick()
    {
        buildSystem.PlayButtonOn();

        PipePanel_Off();
        BrickSetPanel_Off();
        ButtonPanel_OnOff(currentOpenButtonPanelNumber);

        isPanelOutIn = true;
        StartCoroutine(PanelOut());
    }

    //스탑 버튼 클릭
    public void StopButtonClick()
    {
        buildSystem.StopButtonOn();

        isPanelOutIn = false;
        StartCoroutine(PanelIn());
    }

    IEnumerator PanelOut()
    {
        float upPanelStopPos = upPanelPos.y + upPanel.rect.height;
        float leftPanelStopPos = leftPanelPos.x - leftPanel.rect.width;
        float rightPanelStopPos = rightPanelPos.x + rightPanel.rect.width;
        float stopButtonStopPos = -stopButtonPos.y;

        float moveSpeed = 5f;

        while (true)
        {
            if (!isPanelOutIn)
            {
                yield break;
            }

            upPanel.position = (Vector3.Lerp(upPanel.position,
                new Vector3(0, upPanelStopPos), moveSpeed * Time.deltaTime));
            leftPanel.position = (Vector3.Lerp(leftPanel.position,
                new Vector3(leftPanelStopPos, 0), moveSpeed * Time.deltaTime));
            rightPanel.position = (Vector3.Lerp(rightPanel.position,
                new Vector3(rightPanelStopPos, 0), moveSpeed * Time.deltaTime));
            stopButton.position = (Vector3.Lerp(stopButton.position,
                new Vector3(stopButton.position.x, stopButtonStopPos), moveSpeed * Time.deltaTime));

            if (upPanel.position.y >= upPanelStopPos - 1)
            {
                upPanel.position = new Vector3(0, upPanelStopPos);
                leftPanel.position = new Vector3(leftPanelStopPos, 0);
                rightPanel.position = new Vector3(rightPanelStopPos, 0);
                stopButton.position = new Vector3(stopButton.position.x, stopButtonStopPos);

                yield break;
            }

            yield return null;
        }
    }

    IEnumerator PanelIn()
    {
        float upPanelStopPos = upPanelPos.y;
        float leftPanelStopPos = leftPanelPos.x;
        float rightPanelStopPos = rightPanelPos.x;
        float stopButtonStopPos = stopButtonPos.y;

        float moveSpeed = 5f;

        while (true)
        {
            if (isPanelOutIn)
            {
                yield break;
            }

            upPanel.position = (Vector3.Lerp(upPanel.position,
                new Vector3(0, upPanelStopPos), moveSpeed * Time.deltaTime));
            leftPanel.position = (Vector3.Lerp(leftPanel.position,
                new Vector3(leftPanelStopPos, 0), moveSpeed * Time.deltaTime));
            rightPanel.position = (Vector3.Lerp(rightPanel.position,
                new Vector3(rightPanelStopPos, 0), moveSpeed * Time.deltaTime));
            stopButton.position = (Vector3.Lerp(stopButton.position,
                new Vector3(stopButton.position.x, stopButtonStopPos), moveSpeed * Time.deltaTime));

            if (upPanel.position.y <= upPanelStopPos + 1)
            {
                upPanel.position = new Vector3(0, upPanelStopPos);
                leftPanel.position = new Vector3(leftPanelStopPos, 0);
                rightPanel.position = new Vector3(rightPanelStopPos, 0);
                stopButton.position = new Vector3(stopButton.position.x, stopButtonStopPos);

                yield break;
            }

            yield return null;
        }
    }

    public void SaveButtonClick()
    {
        buildSystem.SaveMap();
    }

    public void LoadButtonClick()
    {
        buildSystem.LoadMap();
    }

    public void BackgroundUpBotton()
    {
        backgroundNum++;
        if (backgroundNum >= backgroundSprite.Length)
        {
            backgroundNum = 0;
        }

        backgroundSetImage.sprite = backgroundSprite[backgroundNum];

        buildSystem.BackgroundSet(backgroundNum);
    }

    public void BackgroundDownBotton()
    {
        backgroundNum--;
        if (backgroundNum < 0)
        {
            backgroundNum = 2;
        }

        backgroundSetImage.sprite = backgroundSprite[backgroundNum];

        buildSystem.BackgroundSet(backgroundNum);
    }

    public void TimerSliderSet()
    {
        buildSystem.TimerSet(timerSlider.value);
        timerText.text = timerSlider.value.ToString();
    }

    public void ExitButtonClick()
    {
        //PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("LobbyScene");
        
    }


    public void MapMakeButtonClick()
    {
        buildSystem.MakeMap();
    }

    #endregion
}
