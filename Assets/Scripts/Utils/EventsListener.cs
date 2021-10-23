using UnityEngine;

namespace Thicckitty
{
    
    /// <summary>
    /// Used for listening for events.
    /// </summary>
    public abstract class EventsListener : MonoBehaviour
    {

        private bool _eventsHooked = false;

        protected virtual void Start()
        {
            HandleHookEvents();
        }

        protected virtual void OnEnable()
        {
            HandleHookEvents();
        }

        protected virtual void OnDisable()
        {
            HandleUnHookEvents();
        }

        private void HandleHookEvents()
        {
            if(_eventsHooked)
            {
                return;
            }
            _eventsHooked = HookEvents();
        }

        private void HandleUnHookEvents()
        {
            if(!_eventsHooked)
            {
                return;
            }
            _eventsHooked = !UnHookEvents();
        }

        /// <summary>
        /// Function for hooking events.
        /// </summary>
        /// <returns>True if successful, false otherwise.</returns>
        protected abstract bool HookEvents();


        /// <summary>
        /// Function for unhooking events.
        /// </summary>
        /// <returns>True if successful, false otherwise.</returns>
        protected abstract bool UnHookEvents();
    }
}