using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace RPGGame.UI
{
    [AddComponentMenu("UI/Loop Horizontal Scroll Rect", 50)]
    [DisallowMultipleComponent]
    public class LoopHorizontalScrollRect : LoopScrollRect
    {
        protected override float GetSize(RectTransform item)
        {
            float size = ContentSpacing;
            if (_gridLayout != null)
            {
                size += _gridLayout.cellSize.x;
            }
            else
            {
                size += LayoutUtility.GetPreferredWidth(item);
            }
            return size;
        }

        protected override float GetDimension(Vector2 vector)
        {
            return vector.x;
        }

        protected override Vector2 GetVector(float value)
        {
            return new Vector2(-value, 0);
        }

        protected override void Awake()
        {
            base.Awake();
            _directionSign = 1;

            GridLayoutGroup layout = Content.GetComponent<GridLayoutGroup>();
            if (layout != null && layout.constraint != GridLayoutGroup.Constraint.FixedRowCount)
            {
                Debug.LogError("[LoopHorizontalScrollRect] unsupported GridLayoutGroup constraint");
            }
        }

        protected override bool UpdateItems(Bounds viewBounds, Bounds contentBounds)
        {
            bool changed = false;
            if (viewBounds.max.x > contentBounds.max.x)
            {
                float size = NewItemAtEnd();
                if (size > 0)
                {
                    if (Threshold < size)
                    {
                        // Preventing new and delete repeatly...
                        Threshold = size * 1.1f;
                    }
                    changed = true;
                }
            }
            else if (viewBounds.max.x < contentBounds.max.x - Threshold)
            {
                float size = DeleteItemAtEnd();
                if (size > 0)
                {
                    changed = true;
                }
            }

            if (viewBounds.min.x < contentBounds.min.x)
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
            else if (viewBounds.min.x > contentBounds.min.x + Threshold)
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