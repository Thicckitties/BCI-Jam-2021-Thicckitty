using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Thicckitty
{
    public class SetCursor : MonoBehaviour, IPointerClickHandler
    {
        public Texture2D cursorTexture;
        public Texture2D cursorDownTexture;
        public CursorMode cursorMode = CursorMode.Auto;
        public Vector2 hotSpot = Vector2.zero;

       

        void Start()
        {
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.SetCursor(cursorDownTexture, hotSpot, cursorMode);
            }
            if (Input.GetMouseButtonUp(0))
            {
                Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Cursor.SetCursor(cursorDownTexture, hotSpot, cursorMode);
        }
        
    }
}
