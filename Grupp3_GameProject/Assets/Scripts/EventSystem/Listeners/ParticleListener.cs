using UnityEngine;

namespace EventCallbacks
{
    public class ParticleListener : MonoBehaviour
    {
        private void OnEnable() => EventSystem<ParticleEvent>.RegisterListener(FireParticles);
        private void OnDisable() => EventSystem<ParticleEvent>.UnRegisterListener(FireParticles);

        private void FireParticles(ParticleEvent eve)
        {
            //Disables the unit graphic
            //eve.UnitRenderer.enabled = false;


            //Instantiates the unit particle system
            Instantiate(eve.UnitParticles, eve.Unit.transform.position, eve.Unit.transform.rotation);
        }
    }
}