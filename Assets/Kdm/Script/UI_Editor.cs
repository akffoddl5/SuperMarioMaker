using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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


    int currentOpenButtonPanelNumber = -1;

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


    }

    // Update is called once per frame
    void Update()
    {

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

    //Ÿ�� ���� �г� ��ư Ŭ��
    public void TileSetPanelButtonClick(int _buttonNum)
    {
        //if (currentOpenButtonPanelNumber < tileSetButtonEdge.Length)
        //    tileSetButtonEdge[currentOpenButtonPanelNumber].SetActive(false);
        ButtonPanel_OnOff(_buttonNum);
        //tileSetButtonEdge[currentOpenButtonPanelNumber].SetActive(false);
    }

    //�� ��ư Ŭ��
    public void MapButtonClick()
    {
        ButtonPanel_OnOff(buttonPanel.Length - 1);
    }

    //��ư �г� On, Off
    void ButtonPanel_OnOff(int _buttonNum)
    {
        if (_buttonNum < 0 || _buttonNum >= buttonPanel.Length)
        {
            Debug.Log("�迭 ������ ���");
            return;
        }

        //Ŭ���� ��ư�� �ٸ� �г��� �����ְų� �����ִ� �г��� ���� ��� 
        if (currentOpenButtonPanelNumber != _buttonNum)
        {
            //�����ִ� �г��� ���� ��� ��
            if (currentOpenButtonPanelNumber != -1)
                ButtonPanelSetActive_FALSE(currentOpenButtonPanelNumber);

            //Ŭ���� ��ư �г��� �Ҵ�
            currentOpenButtonPanelNumber = _buttonNum;
            ButtonPanelSetActive_TRUE(currentOpenButtonPanelNumber);
        }
        else //���� �����ִ� �г��� ��ư�� ������ �г��� ��
        {
            ButtonPanelSetActive_FALSE(currentOpenButtonPanelNumber);
            currentOpenButtonPanelNumber = -1;
        }
    }

    //��ư �г� On
    void ButtonPanelSetActive_TRUE(int _buttonNum)
    {
        buttonPanel[_buttonNum].SetActive(true);
        buttonEdge[_buttonNum].SetActive(true);
    }

    //��ư �г� Off
    void ButtonPanelSetActive_FALSE(int _buttonNum)
    {
        buttonPanel[_buttonNum].SetActive(false);
        buttonEdge[_buttonNum].SetActive(false);
    }


    //Ÿ�� ����
    public void TileButtonClick(string _tileName)
    {
        buildSystem.SetCurrentTile(_tileName);

        //Ÿ�� ���� �̹��� ����
        tileSetButtonImage[currentOpenButtonPanelNumber].sprite =
            buildSystem.currentTile[buildSystem.currentTile.Length - 1].sprite;

        ButtonPanel_OnOff(currentOpenButtonPanelNumber);
    }


    #endregion
}
