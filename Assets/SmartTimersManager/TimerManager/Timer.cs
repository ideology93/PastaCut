using UnityEngine;using UnityEngine.Events;using System.Runtime.CompilerServices;[assembly: InternalsVisibleToAttribute("TimersManager")]namespace Timers {    [System.Serializable]    public class Timer    {        public const uint INFINITE = uint.MaxValue;

        [SerializeField]
        private float m_Interval = 0;        [SerializeField]
        private bool m_InfiniteLoops = false;        [SerializeField]        [Range(1, 10000)]        private uint m_LoopsCount = 1;

        [SerializeField]
        private UnityEvent m_Event = null;

        private UnityAction m_Delegate = null;        private bool m_bIsPaused = false;        private uint m_CurrentLoopsCount = 0;        private float m_ElapsedTime = 0;        private float m_CurrentCycleElapsedTime = 0;

        internal void UpdateActionFromEvent()
        {
            if (m_InfiniteLoops)
                m_LoopsCount = INFINITE;

            if (m_Event != null)
                m_Delegate = delegate { m_Event.Invoke(); };
        }        internal void UpdateTimer()        {            if (m_bIsPaused)                return;            if (m_Delegate == null || m_Interval < 0)
            {
                m_Interval = 0;                return;            }            if (m_CurrentLoopsCount >= m_LoopsCount && m_LoopsCount != INFINITE)            {                m_ElapsedTime = m_Interval * m_LoopsCount;                m_CurrentCycleElapsedTime = m_Interval;            }            else
            {
                m_ElapsedTime += Time.deltaTime;
                m_CurrentCycleElapsedTime = m_ElapsedTime - m_CurrentLoopsCount * m_Interval;

                if (m_CurrentCycleElapsedTime > m_Interval)
                {
                    m_CurrentCycleElapsedTime -= m_Interval;
                    m_CurrentLoopsCount++;
                    m_Delegate.Invoke();
                }            }
        }        public Timer(float Interval, uint LoopsCount, UnityAction UnityAction)        {            if (m_Interval < 0)                m_Interval = 0;            m_Interval = Interval;            m_LoopsCount = System.Math.Max(LoopsCount, 1);            m_Delegate = UnityAction;        }

        ~Timer()
        {
            m_Delegate = null;
            m_Event = null;
        }

        /// <summary>
        /// Get interval
        /// </summary>        public float Interval() { return m_Interval; }

        /// <summary>
        /// Get total loops count (INFINITE (which is uint.MaxValue) if is constantly looping) 
        /// </summary>
        public uint LoopsCount() { return m_LoopsCount; }

        /// <summary>
        /// Get how many loops were completed
        /// </summary>
        public uint CurrentLoopsCount() { return m_CurrentLoopsCount; }        /// <summary>
        /// Get how many loops remained to completion
        /// </summary>
        public uint RemainingLoopsCount() { return m_LoopsCount - m_CurrentLoopsCount; }

        /// <summary>
        /// Get total duration, (INFINITE if it's constantly looping)
        /// </summary>        public float Duration() { return (m_LoopsCount == INFINITE) ? INFINITE : (m_LoopsCount * m_Interval); }

        /// <summary>
        /// Get the delegate to execute
        /// </summary>        public UnityAction Delegate() { return m_Delegate; }

        /// <summary>
        /// Get total remaining time
        /// </summary>        public float RemainingTime() { return (m_LoopsCount == INFINITE && m_Interval > 0f) ? INFINITE : Mathf.Max(m_LoopsCount * m_Interval - m_ElapsedTime, 0f); }

        /// <summary>
        /// Get total elapsed time
        /// </summary>
        public float ElapsedTime() { return m_ElapsedTime; }

        /// <summary>
        /// Get elapsed time in current loop
        /// </summary>        public float CurrentCycleElapsedTime() { return m_CurrentCycleElapsedTime; }

        /// <summary>
        /// Get remaining time in current loop
        /// </summary>
        public float CurrentCycleRemainingTime() { return Mathf.Max(m_Interval - m_CurrentCycleElapsedTime, 0); }

        /// <summary>
        /// Checks whether this timer is ok to be removed
        /// </summary>        public bool ShouldClear() { return (m_Delegate == null || RemainingTime() == 0); }

        /// <summary>
        /// Checks if the timer is paused
        /// </summary>        public bool IsPaused() { return m_bIsPaused; }

        /// <summary>
        /// Pause / Inpause timer
        /// </summary>        public void SetPaused(bool bPause) { m_bIsPaused = bPause; }        
        

        /// <summary>
        ///     Compare frequency (calls per second)
        /// </summary>
        public static bool operator >(Timer A, Timer B)     { return (A == null || B == null) ? true : A.Interval() < B.Interval(); }

        /// <summary>
        ///     Compare frequency (calls per second)
        /// </summary>        public static bool operator <(Timer A, Timer B)     { return (A == null || B == null) ? true : A.Interval() > B.Interval(); }

        /// <summary>
        ///     Compare frequency (calls per second)
        /// </summary>        public static bool operator >=(Timer A, Timer B)    { return (A == null || B == null) ? true : A.Interval() <= B.Interval(); }

        /// <summary>
        ///     Compare frequency (calls per second)
        /// </summary>        public static bool operator <=(Timer A, Timer B)    { return (A == null || B == null) ? true : A.Interval() >= B.Interval(); }    }}