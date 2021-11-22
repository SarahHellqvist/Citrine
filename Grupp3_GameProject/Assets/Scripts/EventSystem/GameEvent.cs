using UnityEngine;

namespace EventCallbacks
{
    public abstract class GameEvent
    {
        public GameObject Unit; //Put unit here since all subclasses needs access to it,
                                //better than having a separate version in each subclass
        public string EventDescription;
    }

    public class DebugEvent : GameEvent
    {

    }

    public class DeathEvent : GameEvent
    {
        //Info about cause of death, our killer, etc
        //Could be a struct, readonly, etc
    }

    public class ParticleEvent : GameEvent
    {
        public ParticleSystem UnitParticles;
        public MeshRenderer UnitRenderer;
    }

    public class SoundEvent : GameEvent
    {
        public AudioClip UnitSound;
        public bool isCancel = false;
    }
}
