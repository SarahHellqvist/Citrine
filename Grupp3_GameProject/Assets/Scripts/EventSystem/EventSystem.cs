using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCallbacks
{
    public static class EventSystem<TEvent> where TEvent : GameEvent
    {
        private static Dictionary<Type, Action<TEvent>> typeEventListeners;

        public static void RegisterListener(Action<TEvent> listener)
        {
            RegisterType();
            typeEventListeners[typeof(TEvent)] += listener;
        }

        public static void RegisterType()
        {
            if (typeEventListeners == null)
            {
                typeEventListeners = new Dictionary<Type, Action<TEvent>>();
            }

            if (!typeEventListeners.ContainsKey(typeof(TEvent)))
            {
                //lägg till en tom entry
                typeEventListeners.Add(typeof(TEvent), null);
            }

            //Debug.Log("registered");
        }

        public static void UnRegisterListener(Action<TEvent> listener)
        {
            typeEventListeners[typeof(TEvent)] -= listener;
            //Debug.Log("Unregistered");
        }

        public static void FireEvent(TEvent eve)
        {
            typeEventListeners[typeof(TEvent)]?.Invoke(eve);
        }
    }

}
