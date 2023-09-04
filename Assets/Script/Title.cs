using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public GameObject obj_marioIdle;
    public GameObject obj_marioJump;
    public GameObject obj_logInPanel;

    public TextMeshProUGUI txt_pressToStart;
    
    public string lobbySceneName;
    
    bool isTextInvisible = false;

    bool isLogin = false;

    Coroutine blinkTxt;
    Coroutine marioJump;

    public AudioSource main_audioSource;
    public AudioSource jump_audioSource;
    public AudioSource select_audioSource;
    

	void Start()
    {
        AudioManager.instance.PlayerOneShot(MARIO_SOUND.LOGIN_BGM,true, 0);
        blinkTxt = StartCoroutine(blinkStart());
        ShowBeforeLogin();
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
        // ���̾�̽����� ����������ؼ� �����ϸ�
        // �α��� ����
        isLogin = true;
		ShowAfterLogin();

		// �������� �ڷ�ƾ ���� (�ؽ�Ʈ ���� 1�� �ǵ��� ������ ��)
		StopCoroutine(blinkTxt);
        if (txt_pressToStart.color.a <= 1) txt_pressToStart.color = Color.white;

		txt_pressToStart.text = "Login Complete!!";

        // ������ ���� �̹����� �ǿ��ϰ� �ö󰡴� ��ó�� ���̰�
        marioJump = StartCoroutine(JumpMario());
	}

    void ShowBeforeLogin()
    {
		//�α��� �г� ����
		obj_logInPanel.SetActive(false);
       // AudioManager.instance.PlayerOneShot(MARIO_SOUND.OUTBUTTEM, false, 0);
        //������ �� �ִ� �̹��� �ѱ�, ���� �̹��� ����
        obj_marioIdle.SetActive(true);
		obj_marioJump.SetActive(false);

	}
    void ShowAfterLogin()
    {
		// �α����� �Ǹ� �α��� �г��� ����
		obj_logInPanel.SetActive(false);
       // AudioManager.instance.PlayerOneShot(MARIO_SOUND.OUTBUTTEM, false, 0);
        // ������ ���ִ� �̹��� ����, ���� �̹��� Ű��
        obj_marioIdle.SetActive(false);
		obj_marioJump.SetActive(true);
	}

	// Press to Start ��ư�� ���� �Ÿ��� �ڷ�ƾ
    IEnumerator blinkStart()
    {
        while (true)
        {
            Color invisibleWhite = new Color(1, 1, 1, 0.2f);
            // �ؽ�Ʈ�� ���̴� ���� isTextInvisible = false�� ����
            if (!isTextInvisible)
            {
                txt_pressToStart.color -= invisibleWhite;
                yield return new WaitForSeconds(0.1f);
		        // �ؽ�Ʈ�� �� ���̰� �Ǹ� isTextInvisible=true�� �������ֱ�
		        if (txt_pressToStart.color.a <= 0)
                {
                    isTextInvisible = true;
                }
            }
            // �ؽ�Ʈ�� �� ���̴� ���¶�� 
            else
            {
			    txt_pressToStart.color += invisibleWhite;
                yield return new WaitForSeconds(0.1f);
			    // �ؽ�Ʈ�� ���̰� �Ǹ� isTextInvisible = false �� �������ֱ�
			    if (txt_pressToStart.color.a >= 1)
			    {
				    isTextInvisible = false;
			    }
		    }
        }
	}

	// ������ ����
	IEnumerator JumpMario()
    {
        AudioManager.instance.PlayerOneShot(MARIO_SOUND.JUMP,false,0);
        
        yield return new WaitForSeconds(0.28f);
		Vector3 a = new Vector3(obj_marioJump.transform.localPosition.x , obj_marioJump.transform.localPosition.y , obj_marioJump.transform.localPosition.z);
		Vector3 b = new Vector3(obj_marioJump.transform.localPosition.x, obj_marioJump.transform.localPosition.y +140f, obj_marioJump.transform.localPosition.z);
        // -160 -> 14���� ����
        while (obj_marioJump.transform.localPosition.y < b.y - 0.1f)
        {
            obj_marioJump.transform.localPosition = Vector3.Lerp(obj_marioJump.transform.localPosition, b, 0.5f);
            yield return new WaitForSeconds(0.08f);
		}
        while (obj_marioJump.transform.localPosition.y > a.y+3f)
        {
			obj_marioJump.transform.localPosition = Vector3.Lerp(obj_marioJump.transform.localPosition, a, 0.2f);
			yield return new WaitForSeconds(0.08f);
		}

		obj_marioJump.gameObject.SetActive(false);
        obj_marioIdle.gameObject.SetActive(true);


        // �� �ű��
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(lobbySceneName);
	}
}