using UnityEngine;


namespace EventCallbacks
{
    public class DebugListener : MonoBehaviour
    {
        private void OnEnable() => EventSystem<DebugEvent>.RegisterListener(OnUnitDebug);
        private void OnDisable() => EventSystem<DebugEvent>.UnRegisterListener(OnUnitDebug);


        //Might wanna change method name,
        //its a bit confusing since this can be used for other situations than death I guess?
        void OnUnitDebug(DebugEvent unitDebug)
        {
            Debug.Log(unitDebug.EventDescription);
        }
    }
}