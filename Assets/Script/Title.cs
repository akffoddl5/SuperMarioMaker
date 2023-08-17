using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public GameObject obj_marioIdle;
    public GameObject obj_marioJump;
    public GameObject obj_logInPanel;

    public TextMeshProUGUI txt_pressToStart;
    
    bool isLogin = false;

    // Start is called before the first frame update
    void Start()
    {
        //������ �� �ִ� �̹��� �ѱ�, ���� �̹��� ����
        obj_marioIdle.SetActive(true);
        obj_marioJump.SetActive(false);

        //�α��� �г� ����
		obj_logInPanel.SetActive(false);

	}

    // Update is called once per frame
    void Update()
    {
        // ��Ŭ���� �Ǹ� �α��� ȭ�� ����
        if (Input.GetMouseButtonDown(0) && !isLogin)
        {
			obj_logInPanel.SetActive(true);
		}
    }

    public void LogIn()
    {
        // �α��� ����
        isLogin = true;

        // �α����� �Ǹ� �α��� �г��� ����
		obj_logInPanel.SetActive(false);
        // ������ ���ִ� �̹��� ����
        obj_marioIdle.SetActive(false);
        // ���� �̹��� Ű��
        obj_marioJump.SetActive(true);

        // �������� �ڷ�ƾ ���� (�ؽ�Ʈ ���� 1�� �ǵ��� ������ ��)

        // �ؽ�Ʈ �α��� �������� ����
        txt_pressToStart.text = "Log in Complete!!";

        // ������ ���� �̹����� �ǿ��ϰ� �ö󰡴� ��ó�� ���̰�

	}

	// Press to Start ��ư�� ���� �Ÿ��� �ڷ�ƾ

	// ���� 
}