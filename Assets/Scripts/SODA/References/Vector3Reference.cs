using UnityEngine;

namespace Thicckitty.SODA
{
    [System.Serializable]
    public class Vector3Reference : GenericReference<Vector3>
    {
        [SerializeField]
        private Vector3Variable variable;

        public override bool HasVariable => variable != null;

        public Vector3Variable.VariableChangedDelegate VariableChangedEvent
        {
            get => variable?.VariableChangedEvent;
            set
            {
                if (variable != null)
                {
                    variable.VariableChangedEvent = value;
                }
            }
        }

        protected override Vector3 ReferenceValue
        {
            get => variable != null ? variable.Value : Vector3.zero;
            set
            {
                if (variable != null)
                {
                    variable.Value = value;
                }
            }
        }
    
        public override void Reset()
        {
            if (variable)
            {
                variable.Reset();
            }
        }
    }
}