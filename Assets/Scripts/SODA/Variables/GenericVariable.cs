using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thicckitty.SODA
{
    public abstract class GenericVariable<T> : ScriptableObject
    {
        public delegate void VariableChangedDelegate(T oldValue, T newValue);

        public VariableChangedDelegate VariableChangedEvent;
        public VariableChangedDelegate VariableResetEvent;


        [SerializeField] private T originalValue = default(T);
        [SerializeField] private T value = default(T);

        private T _prevValue = default(T);

        public T Value
        {
            get => value;
            set
            {
                _prevValue = this.value;
                HandleVariableChanged(_prevValue, value);
                this.value = value;
            }
        }

        private void OnValidate()
        {
            HandleVariableChanged(_prevValue, value);
        }

        protected virtual void HandleVariableChanged(T oldValue, T newValue)
        {
            if (oldValue != null
                && newValue != null
                && !oldValue.Equals(newValue))
            {
                VariableChangedEvent?.Invoke(oldValue, newValue);
            }
            else if (oldValue == null
                     && newValue != null)
            {
                VariableChangedEvent?.Invoke(oldValue, newValue);
            }
            else if (oldValue != null
                     && newValue == null)
            {
                VariableChangedEvent?.Invoke(oldValue, newValue);
            }
        }

        public void Reset()
        {
            _prevValue = value;
            value = originalValue;
            VariableResetEvent?.Invoke(_prevValue, value);
        }
    }
}
