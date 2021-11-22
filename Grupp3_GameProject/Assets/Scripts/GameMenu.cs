using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    [SerializeField]
    private Image gameMenuImage;

    [SerializeField]
    private TMP_Text hintText;

    [SerializeField] 
    private AudioClip[] audioClips;
    private int clipIndex;

    [SerializeField]
    private AudioMixer audioMixer;

    private SaveAndLoadGame saveAndLoadGame;

    private void Start()
    {
        gameMenuImage.gameObject.SetActive(false);
        saveAndLoadGame = FindObjectOfType<SaveAndLoadGame>();
        hintText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameController.GameControllerInstance.isPaused)//If we press the escape AND the game is PAUSE
            {
                //We want to resume the game ASAP
                Resume();
            }
            else
            {
                //We want to pause the game
                Pause();
            }
        }
    }

    public void Resume()
    {
        gameMenuImage.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
        GameController.GameControllerInstance.isPaused = false;
    }

    private void Pause()
    {
        gameMenuImage.gameObject.SetActive(true);
        Time.timeScale = 0.0f;
        GameController.GameControllerInstance.isPaused = true;
    }
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SaveButton()
    {
        saveAndLoadGame.SaveGame();
        StartCoroutine(DisplayHintCo("SAVED"));
    }

    public void LoadButton()
    {
        saveAndLoadGame.LoadGame();
        Resume();
        StartCoroutine(DisplayHintCo("LOADED"));
    }

    public void QuitButton()
    {
        //Debug.Log("Quit!");
        Application.Quit();
    }

    public void SoundOnClick()
    {
        //Get random sound from array (differing pitches)
        clipIndex = Random.Range(1, audioClips.Length);
        AudioClip clip = audioClips[clipIndex];

        EventCallbacks.EventHelper.CreateSoundEvent(gameObject, clip);
    }

    //THIS FUNCTION WILL BE TRIGGERED WHEN YOU PRESS THE SAVE OR LOAD BUTTON
    IEnumerator DisplayHintCo(string _message)
    {
        //Debug.Log("TTTTT");
        hintText.gameObject.SetActive(true);
        hintText.text = _message;
        yield return new WaitForSeconds(2);
        hintText.gameObject.SetActive(false);
    }
}
