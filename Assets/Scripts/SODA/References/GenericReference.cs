using UnityEngine;

namespace Thicckitty.SODA
{
    [System.Serializable]
    public abstract class GenericReference<T>
    {
        [SerializeField]
        private bool isConstant = false;
        [SerializeField]
        private T constantValue = default(T);

        public abstract bool HasVariable
        {
            get;
        }
        
        public T Value
        {
            get => isConstant ? constantValue : ReferenceValue;
            set 
            {
                if(isConstant)
                {
                    return;
                }
                ReferenceValue = value;
            }
        }

        protected abstract T ReferenceValue { get; set; }

        public abstract void Reset();
    }
}