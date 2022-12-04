using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

namespace Timers
{
    // The TimerManager manages all scheduled timers. This includes both the regular execution of timers, as well as the cleanup of timers after garbage collection.
    [DisallowMultipleComponent]
    public class TimersManager : MonoBehaviour
    {
        // Ensure we only have a single instance of the TimersManager loaded (singleton pattern).
        private static TimersManager m_instance = null;

        // A map of weak references. When an object is garbage collected, all its timers are automatically removed.
        private static IDictionary<WeakReference, Timer> m_Timers = new Dictionary<WeakReference, Timer>();

        // Whether the game is paused
        private static bool m_bPaused = false;

        void Awake()
        {
            if (m_instance != null)
            {
#if UNITY_EDITOR
                Debug.LogWarning("An instance of Timer has already been loaded. Multiple instances are not necessary and will be destroyed.");
#endif
                Destroy(this);
            }

            DontDestroyOnLoad(this);
            DontDestroyOnLoad(gameObject);
            m_instance = this;
        }

        private static void FindAndRemove(UnityAction UnityAction)
        {
            foreach (KeyValuePair<WeakReference, Timer> elem in m_Timers)
            {
                if (elem.Value.Delegate() == UnityAction)
                {
                    m_Timers.Remove(elem.Key);
                    break;
                }
            }
        }

        private void Update()
        {
            if (m_bPaused)
                return;

            List<WeakReference> Keys = new List<WeakReference>(m_Timers.Keys);
            foreach (WeakReference key in Keys)
            {
                Timer timer = null;
                if (m_Timers.TryGetValue(key, out timer))
                {
                    if (key.Target != null && !key.Target.Equals(null))
                        timer.UpdateTimer();

                    if (key.Target == null || key.Target.Equals(null) || timer == null || timer.ShouldClear())
                        m_Timers.Remove(key);
                }
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            m_bPaused = pauseStatus;
        }


        /// <summary>
        /// Set timer
        /// </summary>
        /// <param name="Owner">The object that contains the timer. Required in order to remove the timer if the object is destroyed.</param>
        /// <param name="timer">Timer to add</param>
        public static void SetTimer(object Owner, Timer timer)
        {
            if (timer.Delegate() != null && timer.Interval() > 0f && Owner != null && timer.LoopsCount() > 0)
            {
                ClearTimer(timer.Delegate());
                m_Timers.Add(new WeakReference(Owner), timer);
            }
        }

        /// <summary>
        /// Set a timer that loops LoopCount times
        /// </summary>
        /// <param name="Owner">The object that contains the timer. Required in order to remove the timer if the object is destroyed.</param>
        /// <param name="interval">Interval(in seconds) between loops</param>
        /// <param name="LoopsCount">How many times to loop</param>
        /// <param name="UnityAction">Delegate</param>
        public static void SetTimer(object Owner, float interval, uint LoopsCount, UnityAction unityAction)
        {
            LoopsCount = System.Math.Max(LoopsCount, 1);
            if (unityAction != null && interval > 0f && Owner != null && LoopsCount > 0)
            {
                ClearTimer(unityAction);
                m_Timers.Add(new WeakReference(Owner), new Timer(interval, LoopsCount, unityAction));
            }
        }

        /// <summary>
        /// Set a timer that activates only once.
        /// </summary>
        /// <param name="Owner">The object that contains the timer. Required in order to remove the timer if the object is destroyed.</param>
        /// <param name="interval">Interval(in seconds) between loops</param>
        /// <param name="UnityAction">Delegate</param>
        public static void SetTimer(object Owner, float interval, UnityAction unityAction)
        {
            if (unityAction != null && interval > 0f && Owner != null)
            {
                ClearTimer(unityAction);
                m_Timers.Add(new WeakReference(Owner), new Timer(interval, 1, unityAction));
            }
        }

        /// <summary>
        /// Set an infinitely loopable timer
        /// </summary>
        /// <param name="Owner">The object that contains the timer. Required in order to remove the timer if the object is destroyed.</param>
        /// <param name="interval">Interval(in seconds)</param>
        /// <param name="unityAction">Delegate</param>
        public static void SetLoopableTimer(object Owner, float interval, UnityAction unityAction)
        {
            if (unityAction != null && interval > 0f && Owner != null)
            {
                ClearTimer(unityAction);
                m_Timers.Add(new WeakReference(Owner), new Timer(interval, Timer.INFINITE, unityAction));
            }
        }

        /// <summary>
        /// Add a list of timers. Works great with List<Timer> in inspector. See 'TimersList.cs' for an example.
        /// </summary>
        /// <param name="Owner">Owner of timers. This should be the object that have these timers. Required in order to remove the timers if the object is destroyed.</param>
        /// <param name="Timers">Timers list</param>
        public static void AddTimers(object Owner, List<Timer> Timers)
        {
            if (Owner == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning("Owner is null. Aborted.");
#endif
                return;
            }

            foreach (Timer timer in Timers)
            {
                if (timer.Interval() > 0f && Owner != null && timer.LoopsCount() > 0)
                {
                    timer.UpdateActionFromEvent();
                    m_Timers.Add(new WeakReference(Owner), timer);
                }
            }
        }

        /// <summary>
        /// Remove a certain timer
        /// </summary>
        /// <param name="UnityAction">Delegate name</param>
        public static void ClearTimer(UnityAction UnityAction)
        {
            if (UnityAction != null)
                FindAndRemove(UnityAction);
        }

        /// <summary>
        /// Get timer by name (which is the delegate's name)
        /// </summary>
        /// <param name="UnityAction">Delegate name</param>
        public static Timer GetTimerByName(UnityAction UnityAction)
        {
            foreach (KeyValuePair<WeakReference, Timer> elem in m_Timers)
                if (elem.Value.Delegate() == UnityAction)
                    return elem.Value;

            return null;
        }

        /// <summary>
        /// Get timer interval. Returns 0 if not found.
        /// </summary>
        /// <param name="unityAction">Delegate name</param>
        public static float Interval(UnityAction unityAction) { Timer timer = GetTimerByName(unityAction); return timer == null ? 0f : timer.Interval(); }

        /// <summary>
        /// Get total loops count (INFINITE (which is uint.MaxValue) if is constantly looping) 
        /// </summary>
        /// <param name="unityAction">Delegate name</param>
        public static uint LoopsCount(UnityAction unityAction) { Timer timer = GetTimerByName(unityAction); return timer == null ? 0 : timer.LoopsCount(); }

        /// <summary>
        /// Get how many loops were completed
        /// </summary>
        /// <param name="unityAction">Delegate name</param>
        public static uint CurrentLoopsCount(UnityAction unityAction) { Timer timer = GetTimerByName(unityAction); return timer == null ? 0 : timer.CurrentLoopsCount(); }

        /// <summary>
        /// Get how many loops remained to completion
        /// </summary>
        /// <param name="unityAction">Delegate name</param>
        public static uint RemainingLoopsCount(UnityAction unityAction) { Timer timer = GetTimerByName(unityAction); return timer == null ? 0 : timer.RemainingLoopsCount(); }

        /// <summary>
        /// Get total remaining time
        /// </summary>
        /// <param name="unityAction">Delegate name</param>
        public static float RemainingTime(UnityAction unityAction) { Timer timer = GetTimerByName(unityAction); return timer == null ? -1f : timer.RemainingTime(); }

        /// <summary>
        /// Get total elapsed time
        /// </summary>
        /// <param name="unityAction">Delegate name</param>
        public static float ElapsedTime(UnityAction unityAction) { Timer timer = GetTimerByName(unityAction); return timer == null ? -1f : timer.ElapsedTime(); }

        /// <summary>
        /// Get elapsed time in current loop
        /// </summary>
        /// <param name="unityAction">Delegate name</param>
        public static float CurrentCycleElapsedTime(UnityAction unityAction) { Timer timer = GetTimerByName(unityAction); return timer == null ? -1f : timer.CurrentCycleElapsedTime(); }

        /// <summary>
        /// Get remaining time in current loop
        /// </summary>
        /// <param name="unityAction">Delegate name</param>
        public static float CurrentCycleRemainingTime(UnityAction unityAction) { Timer timer = GetTimerByName(unityAction); return timer == null ? -1f : timer.CurrentCycleRemainingTime(); }

        /// <summary>
        /// Verifies whether the timer exits
        /// </summary>
        /// <param name="unityAction">Delegate name</param>
        public static bool IsTimerActive(UnityAction unityAction) { Timer timer = GetTimerByName(unityAction); return timer != null; }

        /// <summary>
        /// Checks if the timer is paused
        /// </summary>
        /// <param name="unityAction">Delegate name</param>
        public static bool IsTimerPaused(UnityAction unityAction) { Timer timer = GetTimerByName(unityAction); return timer == null ? false : timer.IsPaused(); }

        /// <summary>
        /// Pause / Unpause timer
        /// </summary>
        /// <param name="unityAction">Delegate name</param>
        ///  <param name="bPause">true - pause, false - unpause</param>
        public static void SetPaused(UnityAction unityAction, bool bPause) { Timer timer = GetTimerByName(unityAction); if (timer != null) timer.SetPaused(bPause); }

        /// <summary>
        /// Get total duration, (INFINITE if it's constantly looping)
        /// </summary>
        /// <param name="unityAction">Delegate name</param>
        public static float Duration(UnityAction unityAction) { Timer timer = GetTimerByName(unityAction); return timer == null ? 0f : timer.Duration(); }
    }
}