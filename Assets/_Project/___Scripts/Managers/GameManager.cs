using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;


[DefaultExecutionOrder(-10)]


public class GameManager : Singleton<GameManager>
{
    private const string PLAYER_TAG = "Player";
    private const string SOUND_MANAGER_TAG = "SoundManager";
    private const string MAIN_CAMERA_TAG = "MainCamera";

    [HideInInspector] public GameObject MainCamera;
    [HideInInspector] public SoundSystem SoundSystem;

    private void Start()
    {
        MainCamera = GameObject.FindGameObjectWithTag(MAIN_CAMERA_TAG);
        SoundSystem = GameObject.FindGameObjectWithTag(SOUND_MANAGER_TAG).GetComponent<SoundSystem>();
    }

}