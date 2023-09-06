using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MARIO_SOUND
{
    INGAME_BGM,//
    LOBBY_BGM,//
    COIN,//
    SELECT,
    JUMP,
    KILL_ENEMY,
    LOGIN_BGM,
    MARIO_DIE,
    POWER_DOWN,
    POWER_UP,
    SELECT2,
    SPRING_JUMP,
    STAGE_CLEAR,
    STOMP,
    STONE,
    CASTLE,
    ONBUTTEM,
    OUTBUTTEM,
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource[] audios;
    public Dictionary<MARIO_SOUND, AudioClip> clip_dictionary = new Dictionary<MARIO_SOUND, AudioClip>();
    public AudioClip[] clips;
    

    public void PlayerOneShot(MARIO_SOUND _clip, bool _loop, int _audio_idx)
    {


        audios[_audio_idx].clip = clip_dictionary[_clip];
        audios[_audio_idx].loop = _loop;
        audios[_audio_idx].Play();
    }

    public void Pause(int _audio_idx)
    {
        audios[_audio_idx].Pause();
    }

    private void Awake()
    {
        if (instance != null) {
            Destroy(this.gameObject);

        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        clip_dictionary[MARIO_SOUND.INGAME_BGM] = clips[0];
        clip_dictionary[MARIO_SOUND.LOBBY_BGM] = clips[1];
        clip_dictionary[MARIO_SOUND.COIN] = clips[3];
        clip_dictionary[MARIO_SOUND.SELECT] = clips[4];
        clip_dictionary[MARIO_SOUND.JUMP] = clips[5];
        clip_dictionary[MARIO_SOUND.KILL_ENEMY] = clips[6];
        clip_dictionary[MARIO_SOUND.LOGIN_BGM] = clips[7];
        clip_dictionary[MARIO_SOUND.MARIO_DIE] = clips[8];
        clip_dictionary[MARIO_SOUND.POWER_DOWN] = clips[9];
        clip_dictionary[MARIO_SOUND.POWER_UP] = clips[10];
        clip_dictionary[MARIO_SOUND.SELECT2] = clips[12];
        clip_dictionary[MARIO_SOUND.SPRING_JUMP] = clips[13];
        clip_dictionary[MARIO_SOUND.STAGE_CLEAR] = clips[14];
        clip_dictionary[MARIO_SOUND.STOMP] = clips[15];
        clip_dictionary[MARIO_SOUND.STONE] = clips[16];
        clip_dictionary[MARIO_SOUND.CASTLE] = clips[17];
        clip_dictionary[MARIO_SOUND.ONBUTTEM] = clips[18];
        clip_dictionary[MARIO_SOUND.OUTBUTTEM] = clips[19];

    }

    private void Start()
    {
        
    }     


}
