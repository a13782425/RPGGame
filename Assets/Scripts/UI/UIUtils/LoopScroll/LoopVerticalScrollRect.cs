using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace RPGGame.UI
{
    [AddComponentMenu("UI/Loop Vertical Scroll Rect", 51)]
    [DisallowMultipleComponent]
    public class LoopVerticalScrollRect : LoopScrollRect
    {
        protected override float GetSize(RectTransform item)
        {
            float size = ContentSpacing;
            if (_gridLayout != null)
            {
                size += _gridLayout.cellSize.y;
            }
            else
            {
                size += LayoutUtility.GetPreferredHeight(item);
            }
            return size;
        }

        protected override float GetDimension(Vector2 vector)
        {
            return vector.y;
        }

        protected override Vector2 GetVector(float value)
        {
            return new Vector2(0, value);
        }

        protected override void Awake()
        {
            base.Awake();
            _directionSign = -1;

            GridLayoutGroup layout = Content.GetComponent<GridLayoutGroup>();
            if (layout != null && layout.constraint != GridLayoutGroup.Constraint.FixedColumnCount)
            {
                Debug.LogError("[LoopHorizontalScrollRect] unsupported GridLayoutGroup constraint");
            }
        }

        protected override bool UpdateItems(Bounds viewBounds, Bounds contentBounds)
        {
            bool changed = false;
            if (viewBounds.min.y < contentBounds.min.y + 1)
            {
                float size = NewItemAtEnd();
                if (size > 0)
                {
                    if (Threshold < size)
                    {
                        Threshold = size * 1.1f;
                    }
                    changed = true;
                }
            }
            else if (viewBounds.min.y > contentBounds.min.y + Threshold)
            {
                float size = DeleteItemAtEnd();
                if (size > 0)
                {
                    changed = true;
                }
            }
            if (viewBounds.max.y > contentBounds.max.y - 1)
            {
                float size = NewItemAtStart();
                if (size > 0)
                {
                    if (Threshold < size)
                    {
                        Threshold = size * 1.1f;
                    }
                    changed = true;
                }
            }
            else if (viewBounds.max.y < contentBounds.max.y - Threshold)
            {
                float size = DeleteItemAtStart();
                if (size > 0)
                {
                    changed = true;
                }
            }
            return changed;
        }
    }
}