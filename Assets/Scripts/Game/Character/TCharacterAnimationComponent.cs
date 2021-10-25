using UnityEngine;

namespace Thicckitty
{
    [RequireComponent(typeof(Animator))]
    public class TCharacterAnimationComponent : MonoBehaviour
    {
        public delegate void KickAnimationFinishedEventDelegate();
        public KickAnimationFinishedEventDelegate FinishedKickEvent;

        private Animator _animator;

        public Animator Animator
        {
            get
            {
                _animator ??= GetComponent<Animator>();
                return _animator;
            }
        }
        
        public void KickAnimationFinished()
        {
            FinishedKickEvent?.Invoke();            
        }
    }
}