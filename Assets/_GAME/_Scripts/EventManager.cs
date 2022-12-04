using System;
using System.Collections.Generic;
using UnityEngine;

public enum GameEvents
{
    FinishCheeseGrating,
    Disappear,
}

namespace Aezakmi
{
    public class EventManager : MonoBehaviour
    {
        private Dictionary<GameEvents, Action<Dictionary<string, object>>> eventDictionary;

        private static EventManager eventManager;

        public static EventManager Instance
        {
            get
            {
                if (!eventManager)
                {
                    eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                    if (!eventManager)
                    {
                        Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene.");
                    }
                    else
                    {
                        eventManager.Init();

                        //  Sets this to not be destroyed when reloading scene
                        DontDestroyOnLoad(eventManager);
                    }
                }
                return eventManager;
            }
        }

        private void Init()
        {
            if (eventDictionary == null)
            {
                eventDictionary = new Dictionary<GameEvents, Action<Dictionary<string, object>>>();
            }
        }

        public static void StartListening(GameEvents eventName, Action<Dictionary<string, object>> listener)
        {
            Action<Dictionary<string, object>> thisEvent;

            if (EventManager.Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent += listener;
                EventManager.Instance.eventDictionary[eventName] = thisEvent;
            }
            else
            {
                thisEvent += listener;
                EventManager.Instance.eventDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StopListening(GameEvents eventName, Action<Dictionary<string, object>> listener)
        {
            if (EventManager.Instance == null) return;
            Action<Dictionary<string, object>> thisEvent;
            if (EventManager.Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent -= listener;
                EventManager.Instance.eventDictionary[eventName] = thisEvent;
            }
        }

        public static void TriggerEvent(GameEvents eventName, Dictionary<string, object> message)
        {
            Action<Dictionary<string, object>> thisEvent = null;
            if (EventManager.Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke(message);
            }
        }
    }
}

// http://bernardopacheco.net/using-an-event-manager-to-decouple-your-game-in-unity
// Edited to use Enums