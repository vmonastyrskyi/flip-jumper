using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Util
{
    public static class PointerEventSystem
    {
        public static bool IsPointerOverGameObject(Vector2 position, string[] tags = null)
        {
            var blockTouch = false;

            var eventDataCurrentPosition = new PointerEventData(EventSystem.current)
            {
                position = position
            };
            var results = new List<RaycastResult>();

            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            if (tags?.Length > 0 && results.Count > 0)
            {
                if (tags.Any(tag => results.Any(result => result.gameObject.CompareTag(tag))))
                    blockTouch = true;
            }
            else if (results.Count > 0)
            {
                blockTouch = true;
            }

            return blockTouch;
        }
    }
}