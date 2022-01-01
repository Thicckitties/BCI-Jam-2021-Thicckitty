using System;
using UnityEngine;

namespace Thicckitty
{
    [System.Serializable]
    public enum SpriteSideType
    {
        [InspectorName("Left Side")]
        TYPE_LEFT_SIDE,
        [InspectorName("Right Side")]
        TYPE_RIGHT_SIDE
    }
    
    [System.Serializable]
    public struct Sprite3DUpdaterData
    {
        [SerializeField]
        public SpriteSideType spriteSide;
        [SerializeField]
        public SpriteRenderer spriteRenderer;
    }
    
    public interface ISprite3DUpdater
    {
        Sprite3DUpdaterData UpdaterData
        {
            get;
        }

        Vector3 MovementDirection
        {
            get;
        }

        Transform Transform
        {
            get;
        }

        Sprite3DUpdaterBehaviour UpdaterBehaviour
        {
            get;
        }
    }
    
    public class Sprite3DUpdaterBehaviour
    {
        private SpriteSideType _currentSpriteType;
        private ISprite3DUpdater _spriteUpdater;

        public Sprite3DUpdaterBehaviour(ISprite3DUpdater spriteUpdater)
        {
            _spriteUpdater = spriteUpdater;
            _currentSpriteType = spriteUpdater.UpdaterData.spriteSide;
        }

        public void Update(float deltaTime)
        {
            float dotProduct = Vector3.Dot(
                _spriteUpdater.MovementDirection.normalized,
                -_spriteUpdater.Transform.right);
            _currentSpriteType = (dotProduct <= 0.0f)
                ? SpriteSideType.TYPE_LEFT_SIDE
                : SpriteSideType.TYPE_RIGHT_SIDE;

            if (_spriteUpdater.UpdaterData.spriteRenderer)
            {
                _spriteUpdater.UpdaterData.spriteRenderer.flipX =
                    _currentSpriteType != _spriteUpdater.UpdaterData.spriteSide;
            }
        }
    }

    public class Sprite3DUpdaterComponent : MonoBehaviour, ISprite3DUpdater
    {

        [SerializeField]
        private Sprite3DUpdaterData updaterData;

        private Sprite3DUpdaterBehaviour _updaterBehaviour;
        private Vector3 _prevPosition = Vector3.zero;
        private Vector3 _movementDirection;

        public Sprite3DUpdaterData UpdaterData => updaterData;
        public Vector3 MovementDirection => _movementDirection;
        public Transform Transform => transform;

        public Sprite3DUpdaterBehaviour UpdaterBehaviour
        {
            get
            {
                _updaterBehaviour ??= new Sprite3DUpdaterBehaviour(this);
                return _updaterBehaviour;
            }
        }


        private void Start()
        {
            _prevPosition = transform.position;
        }

        private void Update()
        {
            UpdaterBehaviour.Update(Time.deltaTime);
        }

        private void LateUpdate()
        {
            Vector3 currentPos = transform.position;
            _movementDirection = (currentPos - _prevPosition).normalized;
            _prevPosition = currentPos;
        }
    }
}