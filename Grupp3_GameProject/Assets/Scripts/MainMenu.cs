using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] audioClips;
    private int clipIndex;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject nextSceneTrigger;

    public void PlayGame()
    {
        PlayerPrefs.DeleteAll();
        player.transform.position = nextSceneTrigger.transform.position;
    }

    public void QuitGame()
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
}