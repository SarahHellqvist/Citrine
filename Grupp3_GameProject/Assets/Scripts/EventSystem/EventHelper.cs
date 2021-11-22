using UnityEngine;

//Author: Molly Röle
//Todo: Singleton or static??
namespace EventCallbacks
{
    public static class EventHelper 
    {
        public static void CreateSoundEvent(GameObject targetGameObject, AudioClip targetAudioClip)
        {
            SoundEvent soundEvent = new SoundEvent();
            soundEvent.Unit = targetGameObject;
            soundEvent.UnitSound = targetAudioClip;
            EventSystem<SoundEvent>.FireEvent(soundEvent);
        }

        public static void CreateSoundEvent(GameObject targetGameObject, AudioClip targetAudioClip, bool isCancel)
        {
            SoundEvent soundEvent = new SoundEvent();
            soundEvent.Unit = targetGameObject;
            soundEvent.UnitSound = targetAudioClip;
            soundEvent.isCancel = isCancel;
            EventSystem<SoundEvent>.FireEvent(soundEvent);
        }

        public static void CreateParticleEvent(GameObject targetGameObject, ParticleSystem targetParticles) 
        {
            ParticleEvent particleEvent = new ParticleEvent();
            particleEvent.Unit = targetGameObject;
            particleEvent.UnitParticles = targetParticles;
            EventSystem<ParticleEvent>.FireEvent(particleEvent);
            //Eventuellt: ta in renderern som argument och sätt renderern här, ifall man vill göra ngt med den 
        }

        public static void CreateDeathEvent(GameObject targetGameObject)
        {
            DeathEvent deathEvent = new DeathEvent();
            deathEvent.Unit = targetGameObject;
            EventSystem<DeathEvent>.FireEvent(deathEvent);
        }

        public static void CreateDebugEvent(GameObject targetGameObject, string eventDescription)
        {
            DebugEvent debugEvent = new DebugEvent();
            debugEvent.EventDescription = eventDescription;
            debugEvent.Unit = targetGameObject;
            EventSystem<DebugEvent>.FireEvent(debugEvent);
        }
    }
}
