using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Activities
{
    [Serializable]
    public class Activity
    {
        public UnityEvent OnActive;
        public UnityEvent OnRelease;
        public UnityEvent OnActiveTimeThreshold;
        public float ActiveTime;
        public float MaximumActiveTime = -1f;
        public float NextAllowedStartTime;
        public string Name;

        [SerializeField]
        private bool _active;
        public bool Active
        {
            get => _active;
            set
            {
                if (value && _active == false)
                {
                    if (Time.time < NextAllowedStartTime)
                        return;

                    _active = true;
                    OnActive?.Invoke();
                }
                else if (value == false && _active)
                {
                    _active = false;
                    OnRelease?.Invoke();
                }
            }
        }

        public Activity(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Pauses the activity for a certain period of time
        /// </summary>
        public void Pause(float duration)
        {
            Active = false;
            NextAllowedStartTime = Time.time + duration;
        }

        /// <summary>
        /// Increases the ActiveTime variable when active
        /// </summary>
        public void AccumulateActiveTime()
        {
            if (Active == false)
            {
                return;
            }

            //Deactivate the activity when the MaximumActiveTime is reached.
            if (MaximumActiveTime > 0 && ActiveTime > MaximumActiveTime)
            {
                Active = false;
                OnActiveTimeThreshold?.Invoke();
            }
            else
            {
                ActiveTime += Time.deltaTime;
            }
        }
    }
}
