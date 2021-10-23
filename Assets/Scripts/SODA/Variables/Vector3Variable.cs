using UnityEngine;

namespace Thicckitty.SODA
{
    [CreateAssetMenu(fileName = "Vector3 Variable", menuName = "SODA/Vector3 Variable")]
    public class Vector3Variable : GenericVariable<Vector3>
    {
        [SerializeField, Min(0.0f)]
        private float minimumDistanceAmount = 0.0f;
        
        protected override void HandleVariableChanged(Vector3 oldValue, Vector3 newValue)
        {
            if (minimumDistanceAmount <= 0.0f)
            {
                base.HandleVariableChanged(oldValue, newValue);
                return;
            }
            
            float squareDistance = (newValue - oldValue).sqrMagnitude;
            if (squareDistance <= minimumDistanceAmount)
            {
                return;
            }
            VariableChangedEvent?.Invoke(oldValue, newValue);
        }
    }
}
