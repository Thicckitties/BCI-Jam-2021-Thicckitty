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

        Sprite3DUpdaterComponent UpdaterComponent
        {
            get;
        }
    }
    
    public class Sprite3DUpdaterComponent
    {
        private SpriteSideType _currentSpriteType;
        private ISprite3DUpdater _spriteUpdater;

        public Sprite3DUpdaterComponent(ISprite3DUpdater spriteUpdater)
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
}