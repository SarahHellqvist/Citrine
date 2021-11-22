using UnityEngine;

namespace EventCallbacks
{
    public class DeathListener : MonoBehaviour
    {
        private void OnEnable() => EventSystem<DeathEvent>.RegisterListener(OnUnitDeath);
        private void OnDisable() => EventSystem<DeathEvent>.UnRegisterListener(OnUnitDeath);

        void OnUnitDeath(DeathEvent unitDeath)
        {
            //Destroy(unitDeath.Unit);
            unitDeath.Unit.SetActive(false);
            //Debug.Log("Unit deactivated: " + unitDeath.Unit.name);
        }
    }
}
