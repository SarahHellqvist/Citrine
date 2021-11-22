using UnityEngine;

namespace EventCallbacks
{
    public class SoundListener : MonoBehaviour
    {
        [SerializeField]
        private AudioSource AudioSource;
        private void OnEnable() => EventSystem<SoundEvent>.RegisterListener(PlaySound);
        private void OnDisable() => EventSystem<SoundEvent>.UnRegisterListener(PlaySound);
        private void PlaySound(SoundEvent eve)
        {
            if (eve.isCancel)
            {
                AudioSource.clip = eve.UnitSound;
                AudioSource.Stop();
            }
            else
            {
                if (AudioSource.clip != eve.UnitSound || AudioSource.isPlaying == false)
                {
                    AudioSource.clip = eve.UnitSound;
                    AudioSource.PlayOneShot(eve.UnitSound);
                }
                // else if (!AudioSource.isPlaying || eve.UnitSound != AudioSource.clip)
                // {
                //     AudioSource.PlayOneShot(eve.UnitSound);
                // }
            }
        }
    }
}