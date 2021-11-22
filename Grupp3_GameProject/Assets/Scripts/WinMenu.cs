using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] audioClips;
    private int clipIndex;

    [SerializeField]
    private int sceneToload;

    [SerializeField]
    private TMP_Text collectibleText;

    private void Update()
    {
        collectibleText.text = GameController.GameControllerInstance.CollectibleCount.ToString();
    }

    public void ReturnButton()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene(sceneToload);
    }
    public void SoundOnClick()
    {
        //Get random sound from array (differing pitches)
        clipIndex = Random.Range(1, audioClips.Length);
        AudioClip clip = audioClips[clipIndex];

        EventCallbacks.EventHelper.CreateSoundEvent(gameObject, clip);
    }
    public void QuitGame()
    {
        //Debug.Log("Quit!");
        Application.Quit();
    }
}
