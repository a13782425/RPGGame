using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using RPGGame.Attr;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RPGGame.UI
{
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public abstract class LoopScrollRect : UIBehaviour, IInitializePotentialDragHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IScrollHandler, ICanvasElement, ILayoutElement, ILayoutGroup
    {
        [BField("Id")]
        private string _scrollId = "";
        public string ScrollId
        {
            get
            {
                if (string.IsNullOrEmpty(_scrollId))
                {
                    _scrollId = Guid.NewGuid().ToString();
                }
                return _scrollId;
            }
        }

        private BelongPool _currentPool = null;

        [SerializeField, BField("预制物体")]
        private GameObject _cellObject = null;
        public GameObject CellObject { get { return _cellObject; } }

        [SerializeField, BField("数据数量")]
        private int _totalCount = 0;
        public int TotalCount { get { return _totalCount; } set { _totalCount = value; } }

        [SerializeField, BField("数据集合")]
        private List<object> _dataList = new List<object>();

        public List<object> DataList { get { return _dataList; } }
        [SerializeField, BField("初始化个数")]
        private int _initCount = 5;
        public int InitCount { get { return _initCount; } }

        [SerializeField, BField("预加载阈值")]
        private float _threshold = 100;
        public float Threshold { get { return _threshold; } set { _threshold = value; } }
        [SerializeField, BField("反向拖动")]
        private bool _reverseDirection = false;

        public bool ReverseDirection { get { return _reverseDirection; } set { _reverseDirection = value; } }
        [SerializeField, Tooltip("Rubber scale for outside")]//外用橡皮垢
        private float _rubberScale = 1;
        public float RubberScale { get { return _rubberScale; } set { _rubberScale = value; } }

        protected int _itemTypeStart = 0;
        protected int _itemTypeEnd = 0;

        protected abstract float GetSize(RectTransform item);
        protected abstract float GetDimension(Vector2 vector);
        protected abstract Vector2 GetVector(float value);
        protected int _directionSign = 0;

        private float _contentSpacing = -1;
        protected GridLayoutGroup _gridLayout = null;
        protected float ContentSpacing
        {
            get
            {
                if (_contentSpacing >= 0)
                {
                    return _contentSpacing;
                }
                _contentSpacing = 0;
                if (Content != null)
                {
                    HorizontalOrVerticalLayoutGroup layout1 = Content.GetComponent<HorizontalOrVerticalLayoutGroup>();
                    if (layout1 != null)
                    {
                        _contentSpacing = layout1.spacing;
                    }
                    _gridLayout = Content.GetComponent<GridLayoutGroup>();
                    if (_gridLayout != null)
                    {
                        _contentSpacing = GetDimension(_gridLayout.spacing);
                    }
                }
                return _contentSpacing;
            }
        }

        private int _contentConstraintCount = 0;
        protected int ContentConstraintCount
        {
            get
            {
                if (_contentConstraintCount > 0)
                {
                    return _contentConstraintCount;
                }
                _contentConstraintCount = 1;
                if (Content != null)
                {
                    GridLayoutGroup layout2 = Content.GetComponent<GridLayoutGroup>();
                    if (layout2 != null)
                    {
                        if (layout2.constraint == GridLayoutGroup.Constraint.Flexible)
                        {
                            Debug.LogWarning("[LoopScrollRect] Flexible not supported yet");
                        }
                        _contentConstraintCount = layout2.constraintCount;
                    }
                }
                return _contentConstraintCount;
            }
        }

        protected virtual bool UpdateItems(Bounds viewBounds, Bounds contentBounds) { return false; }
        //==========LoopScrollRect==========
        [Serializable]
        public class ScrollRectEvent : UnityEvent<Vector2> { }

        [SerializeField, BField("内容节点")]
        private RectTransform _content;
        public RectTransform Content { get { return _content; } set { _content = value; } }

        [SerializeField, BField("水平滑动")]
        private bool _horizontal = true;
        public bool Horizontal { get { return _horizontal; } set { _horizontal = value; } }

        [SerializeField, BField("垂直滑动")]
        private bool _vertical = true;
        public bool Vertical { get { return _vertical; } set { _vertical = value; } }

        [SerializeField, BField("移动方式")]
        private ScrollRect.MovementType _movementType = ScrollRect.MovementType.Elastic;
        public ScrollRect.MovementType MovementType { get { return _movementType; } set { _movementType = value; } }

        [SerializeField,]
        private float _elasticity = 0.1f; // Only used for MovementType.Elastic
        public float Elasticity { get { return _elasticity; } set { _elasticity = value; } }

        [SerializeField, BField("惯性")]
        private bool _inertia = true;
        public bool Inertia { get { return _inertia; } set { _inertia = value; } }

        [SerializeField, BField("减速率")]
        private float _decelerationRate = 0.135f; // Only used when inertia is enabled
        public float DecelerationRate { get { return _decelerationRate; } set { _decelerationRate = value; } }

        [SerializeField, BField("滚动灵敏度")]
        private float _scrollSensitivity = 1.0f;
        public float ScrollSensitivity { get { return _scrollSensitivity; } set { _scrollSensitivity = value; } }

        [SerializeField]
        private RectTransform _viewport;
        public RectTransform Viewport { get { return _viewport; } set { _viewport = value; SetDirtyCaching(); } }

        [SerializeField, BField("水平滚动条")]
        private Scrollbar _horizontalScrollbar;
        public Scrollbar HorizontalScrollbar
        {
            get
            {
                return _horizontalScrollbar;
            }
            set
            {
                if (_horizontalScrollbar)
                    _horizontalScrollbar.onValueChanged.RemoveListener(SetHorizontalNormalizedPosition);
                _horizontalScrollbar = value;
                if (_horizontalScrollbar)
                    _horizontalScrollbar.onValueChanged.AddListener(SetHorizontalNormalizedPosition);
                SetDirtyCaching();
            }
        }

        [SerializeField, BField("垂直滚动条")]
        private Scrollbar _verticalScrollbar;
        public Scrollbar VerticalScrollbar
        {
            get
            {
                return _verticalScrollbar;
            }
            set
            {
                if (_verticalScrollbar)
                    _verticalScrollbar.onValueChanged.RemoveListener(SetVerticalNormalizedPosition);
                _verticalScrollbar = value;
                if (_verticalScrollbar)
                    _verticalScrollbar.onValueChanged.AddListener(SetVerticalNormalizedPosition);
                SetDirtyCaching();
            }
        }

        [SerializeField, BField("水平滚动条可见性")]
        private ScrollRect.ScrollbarVisibility _horizontalScrollbarVisibility;
        public ScrollRect.ScrollbarVisibility HorizontalScrollbarVisibility { get { return _horizontalScrollbarVisibility; } set { _horizontalScrollbarVisibility = value; SetDirtyCaching(); } }

        [SerializeField, BField("垂直滚动条可见性")]
        private ScrollRect.ScrollbarVisibility _verticalScrollbarVisibility;
        public ScrollRect.ScrollbarVisibility VerticalScrollbarVisibility { get { return _verticalScrollbarVisibility; } set { _verticalScrollbarVisibility = value; SetDirtyCaching(); } }

        [SerializeField, BField("水平滚动条间距")]
        private float _horizontalScrollbarSpacing;
        public float HorizontalScrollbarSpacing { get { return _horizontalScrollbarSpacing; } set { _horizontalScrollbarSpacing = value; SetDirty(); } }

        [SerializeField, BField("垂直滚动条间距")]
        private float _verticalScrollbarSpacing;
        public float VerticalScrollbarSpacing { get { return _verticalScrollbarSpacing; } set { _verticalScrollbarSpacing = value; SetDirty(); } }

        [SerializeField]
        private ScrollRectEvent _onValueChanged = new ScrollRectEvent();
        public ScrollRectEvent OnValueChanged { get { return _onValueChanged; } set { _onValueChanged = value; } }

        // The offset from handle position to mouse down position
        private Vector2 _pointerStartLocalCursor = Vector2.zero;
        private Vector2 _contentStartPosition = Vector2.zero;

        private RectTransform _viewRect;

        protected RectTransform ViewRect
        {
            get
            {
                if (_viewRect == null)
                    _viewRect = _viewport;
                if (_viewRect == null)
                    _viewRect = (RectTransform)transform;
                return _viewRect;
            }
        }

        private Bounds _contentBounds;
        private Bounds _viewBounds;

        private Vector2 _velocity;
        public Vector2 Velocity { get { return _velocity; } set { _velocity = value; } }

        private bool _dragging;

        private Vector2 _prevPosition = Vector2.zero;
        private Bounds _prevContentBounds;
        private Bounds _prevViewBounds;
        [NonSerialized]
        private bool _hasRebuiltLayout = false;

        private bool _hSliderExpand;
        private bool _vSliderExpand;
        private float _hSliderHeight;
        private float _vSliderWidth;

        [System.NonSerialized]
        private RectTransform _currentRect;
        private RectTransform CurrentRect
        {
            get
            {
                if (_currentRect == null)
                    _currentRect = GetComponent<RectTransform>();
                return _currentRect;
            }
        }

        private RectTransform _horizontalScrollbarRect;
        private RectTransform _verticalScrollbarRect;

        private DrivenRectTransformTracker _tracker;

        protected LoopScrollRect()
        {
            flexibleWidth = -1;

        }

        protected override void Awake()
        {
            if (CellObject != null)
            {
                GameObject pool = new GameObject("Pool");
                _currentPool = pool.AddComponent<BelongPool>();
                _currentPool.PoolName = ScrollId;
                BelongPrefabPool b1 = new BelongPrefabPool(CellObject);
                b1.InitCount = InitCount;
                b1.MaxCount = 10000;
                b1.ShowName = "ScrollItem";
                _currentPool.CreatePrefabPool(b1);

            }
            base.Awake();
        }



        //==========LoopScrollRect==========
        private void ReturnObjectAndSendMessage(Transform go)
        {
            //todo 扔会对象池
            //go.SendMessage("ScrollCellReturn", SendMessageOptions.DontRequireReceiver);
            //SG.ResourceManager.Instance.ReturnObjectToPool(go.gameObject);
            _currentPool.PutBack(go);
        }
        /// <summary>
        /// 清理cell
        /// </summary>
        public void ClearCells()
        {
            if (Application.isPlaying)
            {
                _itemTypeStart = 0;
                _itemTypeEnd = 0;
                TotalCount = 0;
                DataList.Clear();
                //objectsToFill = null;
                for (int i = Content.childCount - 1; i >= 0; i--)
                {
                    ReturnObjectAndSendMessage(Content.GetChild(i));
                }
            }
        }

        public void RefreshCells()
        {
            if (Application.isPlaying && this.isActiveAndEnabled)
            {
                _itemTypeEnd = _itemTypeStart;
                // recycle items if we can
                for (int i = 0; i < Content.childCount; i++)
                {
                    if (_itemTypeEnd < TotalCount)
                    {
                        //todo 更改cell 上的数据
                        ILoopScrollData data = Content.GetChild(i).GetComponent<ILoopScrollData>();
                        if (data != null)
                        {
                            data.ProvideData(_itemTypeEnd);
                        }
                        //dataSource.ProvideData(content.GetChild(i), itemTypeEnd);
                        _itemTypeEnd++;
                    }
                    else
                    {
                        ReturnObjectAndSendMessage(Content.GetChild(i));
                        i--;
                    }
                }
            }
        }

        public void RefillCellsFromEnd(int offset = 0)
        {
            //TODO: unsupported for Infinity or Grid yet
            if (!Application.isPlaying || TotalCount < 0 || ContentConstraintCount > 1)
                return;

            StopMovement();
            _itemTypeEnd = ReverseDirection ? offset : TotalCount - offset;
            _itemTypeStart = _itemTypeEnd;

            for (int i = _content.childCount - 1; i >= 0; i--)
            {
                ReturnObjectAndSendMessage(_content.GetChild(i));
            }

            float sizeToFill = 0, sizeFilled = 0;
            if (_directionSign == -1)
                sizeToFill = ViewRect.rect.size.y;
            else
                sizeToFill = ViewRect.rect.size.x;

            while (sizeToFill > sizeFilled)
            {
                float size = ReverseDirection ? NewItemAtEnd() : NewItemAtStart();
                if (size <= 0) break;
                sizeFilled += size;
            }

            Vector2 pos = _content.anchoredPosition;
            float dist = Mathf.Max(0, sizeFilled - sizeToFill);
            if (ReverseDirection)
                dist = -dist;
            if (_directionSign == -1)
                pos.y = dist;
            else if (_directionSign == 1)
                pos.x = dist;
            _content.anchoredPosition = pos;
        }

        public void RefillCells(int offset = 0)
        {
            if (!Application.isPlaying)
                return;

            StopMovement();
            _itemTypeStart = ReverseDirection ? TotalCount - offset : offset;
            _itemTypeEnd = _itemTypeStart;

            // Don't `Canvas.ForceUpdateCanvases();` here, or it will new/delete cells to change itemTypeStart/End
            for (int i = _content.childCount - 1; i >= 0; i--)
            {
                ReturnObjectAndSendMessage(_content.GetChild(i));
            }

            float sizeToFill = 0, sizeFilled = 0;
            // m_ViewBounds may be not ready when RefillCells on Start
            if (_directionSign == -1)
                sizeToFill = ViewRect.rect.size.y;
            else
                sizeToFill = ViewRect.rect.size.x;

            while (sizeToFill > sizeFilled)
            {
                float size = ReverseDirection ? NewItemAtStart() : NewItemAtEnd();
                if (size <= 0) break;
                sizeFilled += size;
            }

            Vector2 pos = _content.anchoredPosition;
            if (_directionSign == -1)
                pos.y = 0;
            else if (_directionSign == 1)
                pos.x = 0;
            _content.anchoredPosition = pos;
        }

        protected float NewItemAtStart()
        {
            if (TotalCount >= 0 && _itemTypeStart - ContentConstraintCount < 0)
            {
                return 0;
            }
            float size = 0;
            for (int i = 0; i < ContentConstraintCount; i++)
            {
                _itemTypeStart--;
                RectTransform newItem = InstantiateNextItem(_itemTypeStart);
                newItem.SetAsFirstSibling();
                size = Mathf.Max(GetSize(newItem), size);
            }

            if (!ReverseDirection)
            {
                Vector2 offset = GetVector(size);
                Content.anchoredPosition += offset;
                _prevPosition += offset;
                _contentStartPosition += offset;
            }
            return size;
        }

        protected float DeleteItemAtStart()
        {
            // special case: when moving or dragging, we cannot simply delete start when we've reached the end
            if (((_dragging || _velocity != Vector2.zero) && TotalCount >= 0 && _itemTypeEnd >= TotalCount - 1)
                || Content.childCount == 0)
            {
                return 0;
            }

            float size = 0;
            for (int i = 0; i < ContentConstraintCount; i++)
            {
                RectTransform oldItem = Content.GetChild(i) as RectTransform;
                size = Mathf.Max(GetSize(oldItem), size);
                ReturnObjectAndSendMessage(oldItem);

                _itemTypeStart++;

                if (Content.childCount == 0)
                {
                    break;
                }
            }

            if (!ReverseDirection)
            {
                Vector2 offset = GetVector(size);
                Content.anchoredPosition -= offset;
                _prevPosition -= offset;
                _contentStartPosition -= offset;
            }
            return size;
        }


        protected float NewItemAtEnd()
        {
            if (TotalCount >= 0 && _itemTypeEnd >= TotalCount)
            {
                return 0;
            }
            float size = 0;
            // issue 4: fill lines to end first
            int count = ContentConstraintCount - (Content.childCount % ContentConstraintCount);
            for (int i = 0; i < count; i++)
            {
                RectTransform newItem = InstantiateNextItem(_itemTypeEnd);
                size = Mathf.Max(GetSize(newItem), size);
                _itemTypeEnd++;
                if (TotalCount >= 0 && _itemTypeEnd >= TotalCount)
                {
                    break;
                }
            }

            if (ReverseDirection)
            {
                Vector2 offset = GetVector(size);
                Content.anchoredPosition -= offset;
                _prevPosition -= offset;
                _contentStartPosition -= offset;
            }
            return size;
        }

        protected float DeleteItemAtEnd()
        {
            if (((_dragging || _velocity != Vector2.zero) && TotalCount >= 0 && _itemTypeStart < ContentConstraintCount)
                || Content.childCount == 0)
            {
                return 0;
            }

            float size = 0;
            for (int i = 0; i < ContentConstraintCount; i++)
            {
                RectTransform oldItem = Content.GetChild(Content.childCount - 1) as RectTransform;
                size = Mathf.Max(GetSize(oldItem), size);
                ReturnObjectAndSendMessage(oldItem);

                _itemTypeEnd--;
                if (_itemTypeEnd % ContentConstraintCount == 0 || Content.childCount == 0)
                {
                    break;  //just delete the whole row
                }
            }

            if (ReverseDirection)
            {
                Vector2 offset = GetVector(size);
                Content.anchoredPosition += offset;
                _prevPosition += offset;
                _contentStartPosition += offset;
            }
            return size;
        }

        private RectTransform InstantiateNextItem(int itemIdx)
        {
            //todo 加载新的cell
            //GameObject cell = GameObject.Instantiate(CellObject);
            Transform cell = _currentPool.Extract("ScrollItem");
            RectTransform nextItem = cell.GetComponent<RectTransform>();
            nextItem.transform.SetParent(Content, false);
            nextItem.gameObject.SetActive(true);
            //todo 更新cell数据
            ILoopScrollData data = cell.GetComponent<ILoopScrollData>();
            if (data != null)
            {
                data.ProvideData(itemIdx);
            }
            //dataSource.ProvideData(nextItem, itemIdx);
            return nextItem;
        }
        //==========LoopScrollRect==========

        public virtual void Rebuild(CanvasUpdate executing)
        {
            if (executing == CanvasUpdate.Prelayout)
            {
                UpdateCachedData();
            }

            if (executing == CanvasUpdate.PostLayout)
            {
                UpdateBounds(false);
                UpdateScrollbars(Vector2.zero);
                UpdatePrevData();

                _hasRebuiltLayout = true;
            }
        }

        public virtual void LayoutComplete()
        { }

        public virtual void GraphicUpdateComplete()
        { }

        void UpdateCachedData()
        {
            Transform transform = this.transform;
            _horizontalScrollbarRect = HorizontalScrollbar == null ? null : HorizontalScrollbar.transform as RectTransform;
            _verticalScrollbarRect = VerticalScrollbar == null ? null : VerticalScrollbar.transform as RectTransform;

            // These are true if either the elements are children, or they don't exist at all.
            bool viewIsChild = (ViewRect.parent == transform);
            bool hScrollbarIsChild = (!_horizontalScrollbarRect || _horizontalScrollbarRect.parent == transform);
            bool vScrollbarIsChild = (!_verticalScrollbarRect || _verticalScrollbarRect.parent == transform);
            bool allAreChildren = (viewIsChild && hScrollbarIsChild && vScrollbarIsChild);

            _hSliderExpand = allAreChildren && _horizontalScrollbarRect && HorizontalScrollbarVisibility == ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
            _vSliderExpand = allAreChildren && _verticalScrollbarRect && VerticalScrollbarVisibility == ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
            _hSliderHeight = (_horizontalScrollbarRect == null ? 0 : _horizontalScrollbarRect.rect.height);
            _vSliderWidth = (_verticalScrollbarRect == null ? 0 : _verticalScrollbarRect.rect.width);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (HorizontalScrollbar)
                HorizontalScrollbar.onValueChanged.AddListener(SetHorizontalNormalizedPosition);
            if (VerticalScrollbar)
                VerticalScrollbar.onValueChanged.AddListener(SetVerticalNormalizedPosition);

            CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
        }

        protected override void OnDisable()
        {
            CanvasUpdateRegistry.UnRegisterCanvasElementForRebuild(this);

            if (HorizontalScrollbar)
                HorizontalScrollbar.onValueChanged.RemoveListener(SetHorizontalNormalizedPosition);
            if (VerticalScrollbar)
                VerticalScrollbar.onValueChanged.RemoveListener(SetVerticalNormalizedPosition);

            _hasRebuiltLayout = false;
            _tracker.Clear();
            _velocity = Vector2.zero;
            LayoutRebuilder.MarkLayoutForRebuild(CurrentRect);
            base.OnDisable();
        }

        public override bool IsActive()
        {
            return base.IsActive() && _content != null;
        }

        private void EnsureLayoutHasRebuilt()
        {
            if (!_hasRebuiltLayout && !CanvasUpdateRegistry.IsRebuildingLayout())
                Canvas.ForceUpdateCanvases();
        }

        public virtual void StopMovement()
        {
            _velocity = Vector2.zero;
        }

        public virtual void OnScroll(PointerEventData data)
        {
            if (!IsActive())
                return;

            EnsureLayoutHasRebuilt();
            UpdateBounds();

            Vector2 delta = data.scrollDelta;
            // Down is positive for scroll events, while in UI system up is positive.
            delta.y *= -1;
            if (Vertical && !Horizontal)
            {
                if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                    delta.y = delta.x;
                delta.x = 0;
            }
            if (Horizontal && !Vertical)
            {
                if (Mathf.Abs(delta.y) > Mathf.Abs(delta.x))
                    delta.x = delta.y;
                delta.y = 0;
            }

            Vector2 position = _content.anchoredPosition;
            position += delta * _scrollSensitivity;
            if (_movementType == ScrollRect.MovementType.Clamped)
                position += CalculateOffset(position - _content.anchoredPosition);

            SetContentAnchoredPosition(position);
            UpdateBounds();
        }

        public virtual void OnInitializePotentialDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            _velocity = Vector2.zero;
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            if (!IsActive())
                return;

            UpdateBounds();

            _pointerStartLocalCursor = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(ViewRect, eventData.position, eventData.pressEventCamera, out _pointerStartLocalCursor);
            _contentStartPosition = _content.anchoredPosition;
            _dragging = true;
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            _dragging = false;
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            if (!IsActive())
                return;

            Vector2 localCursor;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(ViewRect, eventData.position, eventData.pressEventCamera, out localCursor))
                return;

            UpdateBounds();

            var pointerDelta = localCursor - _pointerStartLocalCursor;
            Vector2 position = _contentStartPosition + pointerDelta;

            // Offset to get content into place in the view.
            Vector2 offset = CalculateOffset(position - _content.anchoredPosition);
            position += offset;
            if (_movementType == ScrollRect.MovementType.Elastic)
            {
                //==========LoopScrollRect==========
                if (offset.x != 0)
                    position.x = position.x - RubberDelta(offset.x, _viewBounds.size.x) * RubberScale;
                if (offset.y != 0)
                    position.y = position.y - RubberDelta(offset.y, _viewBounds.size.y) * RubberScale;
                //==========LoopScrollRect==========
            }

            SetContentAnchoredPosition(position);
        }

        protected virtual void SetContentAnchoredPosition(Vector2 position)
        {
            if (!_horizontal)
                position.x = _content.anchoredPosition.x;
            if (!_vertical)
                position.y = _content.anchoredPosition.y;

            if (position != _content.anchoredPosition)
            {
                _content.anchoredPosition = position;
                UpdateBounds();
            }
        }

        protected virtual void LateUpdate()
        {
            if (!_content)
                return;

            EnsureLayoutHasRebuilt();
            UpdateScrollbarVisibility();
            UpdateBounds();
            float deltaTime = Time.unscaledDeltaTime;
            Vector2 offset = CalculateOffset(Vector2.zero);
            if (!_dragging && (offset != Vector2.zero || _velocity != Vector2.zero))
            {
                Vector2 position = _content.anchoredPosition;
                for (int axis = 0; axis < 2; axis++)
                {
                    // Apply spring physics if movement is elastic and content has an offset from the view.
                    if (_movementType == ScrollRect.MovementType.Elastic && offset[axis] != 0)
                    {
                        float speed = _velocity[axis];
                        position[axis] = Mathf.SmoothDamp(_content.anchoredPosition[axis], _content.anchoredPosition[axis] + offset[axis], ref speed, _elasticity, Mathf.Infinity, deltaTime);
                        _velocity[axis] = speed;
                    }
                    // Else move content according to velocity with deceleration applied.
                    else if (_inertia)
                    {
                        _velocity[axis] *= Mathf.Pow(_decelerationRate, deltaTime);
                        if (Mathf.Abs(_velocity[axis]) < 1)
                            _velocity[axis] = 0;
                        position[axis] += _velocity[axis] * deltaTime;
                    }
                    // If we have neither elaticity or friction, there shouldn't be any velocity.
                    else
                    {
                        _velocity[axis] = 0;
                    }
                }

                if (_velocity != Vector2.zero)
                {
                    if (_movementType == ScrollRect.MovementType.Clamped)
                    {
                        offset = CalculateOffset(position - _content.anchoredPosition);
                        position += offset;
                    }

                    SetContentAnchoredPosition(position);
                }
            }

            if (_dragging && _inertia)
            {
                Vector3 newVelocity = (_content.anchoredPosition - _prevPosition) / deltaTime;
                _velocity = Vector3.Lerp(_velocity, newVelocity, deltaTime * 10);
            }

            if (_viewBounds != _prevViewBounds || _contentBounds != _prevContentBounds || _content.anchoredPosition != _prevPosition)
            {
                UpdateScrollbars(offset);
                _onValueChanged.Invoke(normalizedPosition);
                UpdatePrevData();
            }
        }

        private void UpdatePrevData()
        {
            if (_content == null)
                _prevPosition = Vector2.zero;
            else
                _prevPosition = _content.anchoredPosition;
            _prevViewBounds = _viewBounds;
            _prevContentBounds = _contentBounds;
        }

        private void UpdateScrollbars(Vector2 offset)
        {
            if (HorizontalScrollbar)
            {
                //==========LoopScrollRect==========
                if (_contentBounds.size.x > 0 && TotalCount > 0)
                {
                    HorizontalScrollbar.size = Mathf.Clamp01((_viewBounds.size.x - Mathf.Abs(offset.x)) / _contentBounds.size.x * (_itemTypeEnd - _itemTypeStart) / TotalCount);
                }
                //==========LoopScrollRect==========
                else
                    HorizontalScrollbar.size = 1;

                HorizontalScrollbar.value = horizontalNormalizedPosition;
            }

            if (VerticalScrollbar)
            {
                //==========LoopScrollRect==========
                if (_contentBounds.size.y > 0 && TotalCount > 0)
                {
                    VerticalScrollbar.size = Mathf.Clamp01((_viewBounds.size.y - Mathf.Abs(offset.y)) / _contentBounds.size.y * (_itemTypeEnd - _itemTypeStart) / TotalCount);
                }
                //==========LoopScrollRect==========
                else
                    VerticalScrollbar.size = 1;

                VerticalScrollbar.value = verticalNormalizedPosition;
            }
        }

        public Vector2 normalizedPosition
        {
            get
            {
                return new Vector2(horizontalNormalizedPosition, verticalNormalizedPosition);
            }
            set
            {
                SetNormalizedPosition(value.x, 0);
                SetNormalizedPosition(value.y, 1);
            }
        }

        public float horizontalNormalizedPosition
        {
            get
            {
                UpdateBounds();
                //==========LoopScrollRect==========
                if (TotalCount > 0 && _itemTypeEnd > _itemTypeStart)
                {
                    //TODO: consider contentSpacing
                    float elementSize = _contentBounds.size.x / (_itemTypeEnd - _itemTypeStart);
                    float totalSize = elementSize * TotalCount;
                    float offset = _contentBounds.min.x - elementSize * _itemTypeStart;

                    if (totalSize <= _viewBounds.size.x)
                        return (_viewBounds.min.x > offset) ? 1 : 0;
                    return (_viewBounds.min.x - offset) / (totalSize - _viewBounds.size.x);
                }
                else
                    return 0.5f;
                //==========LoopScrollRect==========
            }
            set
            {
                SetNormalizedPosition(value, 0);
            }
        }

        public float verticalNormalizedPosition
        {
            get
            {
                UpdateBounds();
                //==========LoopScrollRect==========
                if (TotalCount > 0 && _itemTypeEnd > _itemTypeStart)
                {
                    //TODO: consider contentSpacinge
                    float elementSize = _contentBounds.size.y / (_itemTypeEnd - _itemTypeStart);
                    float totalSize = elementSize * TotalCount;
                    float offset = _contentBounds.max.y + elementSize * _itemTypeStart;

                    if (totalSize <= _viewBounds.size.y)
                        return (offset > _viewBounds.max.y) ? 1 : 0;
                    return (offset - _viewBounds.max.y) / (totalSize - _viewBounds.size.y);
                }
                else
                    return 0.5f;
                //==========LoopScrollRect==========
            }
            set
            {
                SetNormalizedPosition(value, 1);
            }
        }

        private void SetHorizontalNormalizedPosition(float value) { SetNormalizedPosition(value, 0); }
        private void SetVerticalNormalizedPosition(float value) { SetNormalizedPosition(value, 1); }

        private void SetNormalizedPosition(float value, int axis)
        {
            //==========LoopScrollRect==========
            if (TotalCount <= 0 || _itemTypeEnd <= _itemTypeStart)
                return;
            //==========LoopScrollRect==========

            EnsureLayoutHasRebuilt();
            UpdateBounds();

            //==========LoopScrollRect==========
            Vector3 localPosition = _content.localPosition;
            float newLocalPosition = localPosition[axis];
            if (axis == 0)
            {
                float elementSize = _contentBounds.size.x / (_itemTypeEnd - _itemTypeStart);
                float totalSize = elementSize * TotalCount;
                float offset = _contentBounds.min.x - elementSize * _itemTypeStart;

                newLocalPosition += _viewBounds.min.x - value * (totalSize - _viewBounds.size[axis]) - offset;
            }
            else if (axis == 1)
            {
                float elementSize = _contentBounds.size.y / (_itemTypeEnd - _itemTypeStart);
                float totalSize = elementSize * TotalCount;
                float offset = _contentBounds.max.y + elementSize * _itemTypeStart;

                newLocalPosition -= offset - value * (totalSize - _viewBounds.size.y) - _viewBounds.max.y;
            }
            //==========LoopScrollRect==========

            if (Mathf.Abs(localPosition[axis] - newLocalPosition) > 0.01f)
            {
                localPosition[axis] = newLocalPosition;
                _content.localPosition = localPosition;
                _velocity[axis] = 0;
                UpdateBounds();
            }
        }

        private static float RubberDelta(float overStretching, float viewSize)
        {
            return (1 - (1 / ((Mathf.Abs(overStretching) * 0.55f / viewSize) + 1))) * viewSize * Mathf.Sign(overStretching);
        }

        protected override void OnRectTransformDimensionsChange()
        {
            SetDirty();
        }

        private bool hScrollingNeeded
        {
            get
            {
                if (Application.isPlaying)
                    return _contentBounds.size.x > _viewBounds.size.x + 0.01f;
                return true;
            }
        }
        private bool vScrollingNeeded
        {
            get
            {
                if (Application.isPlaying)
                    return _contentBounds.size.y > _viewBounds.size.y + 0.01f;
                return true;
            }
        }

        public virtual void CalculateLayoutInputHorizontal() { }
        public virtual void CalculateLayoutInputVertical() { }

        public virtual float minWidth { get { return -1; } }
        public virtual float preferredWidth { get { return -1; } }
        public virtual float flexibleWidth { get; private set; }

        public virtual float minHeight { get { return -1; } }
        public virtual float preferredHeight { get { return -1; } }
        public virtual float flexibleHeight { get { return -1; } }

        public virtual int layoutPriority { get { return -1; } }

        public virtual void SetLayoutHorizontal()
        {
            _tracker.Clear();

            if (_hSliderExpand || _vSliderExpand)
            {
                _tracker.Add(this, ViewRect,
                    DrivenTransformProperties.Anchors |
                    DrivenTransformProperties.SizeDelta |
                    DrivenTransformProperties.AnchoredPosition);

                // Make view full size to see if content fits.
                ViewRect.anchorMin = Vector2.zero;
                ViewRect.anchorMax = Vector2.one;
                ViewRect.sizeDelta = Vector2.zero;
                ViewRect.anchoredPosition = Vector2.zero;

                // Recalculate content layout with this size to see if it fits when there are no scrollbars.
                LayoutRebuilder.ForceRebuildLayoutImmediate(Content);
                _viewBounds = new Bounds(ViewRect.rect.center, ViewRect.rect.size);
                _contentBounds = GetBounds();
            }

            // If it doesn't fit vertically, enable vertical scrollbar and shrink view horizontally to make room for it.
            if (_vSliderExpand && vScrollingNeeded)
            {
                ViewRect.sizeDelta = new Vector2(-(_vSliderWidth + _verticalScrollbarSpacing), ViewRect.sizeDelta.y);

                // Recalculate content layout with this size to see if it fits vertically
                // when there is a vertical scrollbar (which may reflowed the content to make it taller).
                LayoutRebuilder.ForceRebuildLayoutImmediate(Content);
                _viewBounds = new Bounds(ViewRect.rect.center, ViewRect.rect.size);
                _contentBounds = GetBounds();
            }

            // If it doesn't fit horizontally, enable horizontal scrollbar and shrink view vertically to make room for it.
            if (_hSliderExpand && hScrollingNeeded)
            {
                ViewRect.sizeDelta = new Vector2(ViewRect.sizeDelta.x, -(_hSliderHeight + HorizontalScrollbarSpacing));
                _viewBounds = new Bounds(ViewRect.rect.center, ViewRect.rect.size);
                _contentBounds = GetBounds();
            }

            // If the vertical slider didn't kick in the first time, and the horizontal one did,
            // we need to check again if the vertical slider now needs to kick in.
            // If it doesn't fit vertically, enable vertical scrollbar and shrink view horizontally to make room for it.
            if (_vSliderExpand && vScrollingNeeded && ViewRect.sizeDelta.x == 0 && ViewRect.sizeDelta.y < 0)
            {
                ViewRect.sizeDelta = new Vector2(-(_vSliderWidth + _verticalScrollbarSpacing), ViewRect.sizeDelta.y);
            }
        }

        public virtual void SetLayoutVertical()
        {
            UpdateScrollbarLayout();
            _viewBounds = new Bounds(ViewRect.rect.center, ViewRect.rect.size);
            _contentBounds = GetBounds();
        }

        void UpdateScrollbarVisibility()
        {
            if (VerticalScrollbar && VerticalScrollbarVisibility != ScrollRect.ScrollbarVisibility.Permanent && VerticalScrollbar.gameObject.activeSelf != vScrollingNeeded)
                VerticalScrollbar.gameObject.SetActive(vScrollingNeeded);

            if (HorizontalScrollbar && _horizontalScrollbarVisibility != ScrollRect.ScrollbarVisibility.Permanent && HorizontalScrollbar.gameObject.activeSelf != hScrollingNeeded)
                HorizontalScrollbar.gameObject.SetActive(hScrollingNeeded);
        }

        void UpdateScrollbarLayout()
        {
            if (_vSliderExpand && HorizontalScrollbar)
            {
                _tracker.Add(this, _horizontalScrollbarRect,
                              DrivenTransformProperties.AnchorMinX |
                              DrivenTransformProperties.AnchorMaxX |
                              DrivenTransformProperties.SizeDeltaX |
                              DrivenTransformProperties.AnchoredPositionX);
                _horizontalScrollbarRect.anchorMin = new Vector2(0, _horizontalScrollbarRect.anchorMin.y);
                _horizontalScrollbarRect.anchorMax = new Vector2(1, _horizontalScrollbarRect.anchorMax.y);
                _horizontalScrollbarRect.anchoredPosition = new Vector2(0, _horizontalScrollbarRect.anchoredPosition.y);
                if (vScrollingNeeded)
                    _horizontalScrollbarRect.sizeDelta = new Vector2(-(_vSliderWidth + _verticalScrollbarSpacing), _horizontalScrollbarRect.sizeDelta.y);
                else
                    _horizontalScrollbarRect.sizeDelta = new Vector2(0, _horizontalScrollbarRect.sizeDelta.y);
            }

            if (_hSliderExpand && VerticalScrollbar)
            {
                _tracker.Add(this, _verticalScrollbarRect,
                              DrivenTransformProperties.AnchorMinY |
                              DrivenTransformProperties.AnchorMaxY |
                              DrivenTransformProperties.SizeDeltaY |
                              DrivenTransformProperties.AnchoredPositionY);
                _verticalScrollbarRect.anchorMin = new Vector2(_verticalScrollbarRect.anchorMin.x, 0);
                _verticalScrollbarRect.anchorMax = new Vector2(_verticalScrollbarRect.anchorMax.x, 1);
                _verticalScrollbarRect.anchoredPosition = new Vector2(_verticalScrollbarRect.anchoredPosition.x, 0);
                if (hScrollingNeeded)
                    _verticalScrollbarRect.sizeDelta = new Vector2(_verticalScrollbarRect.sizeDelta.x, -(_hSliderHeight + HorizontalScrollbarSpacing));
                else
                    _verticalScrollbarRect.sizeDelta = new Vector2(_verticalScrollbarRect.sizeDelta.x, 0);
            }
        }

        private void UpdateBounds(bool updateItems = true)
        {
            _viewBounds = new Bounds(ViewRect.rect.center, ViewRect.rect.size);
            _contentBounds = GetBounds();

            if (_content == null)
                return;

            // ============LoopScrollRect============
            // Don't do this in Rebuild
            if (Application.isPlaying && updateItems && UpdateItems(_viewBounds, _contentBounds))
            {
                Canvas.ForceUpdateCanvases();
                _contentBounds = GetBounds();
            }
            // ============LoopScrollRect============

            // Make sure content bounds are at least as large as view by adding padding if not.
            // One might think at first that if the content is smaller than the view, scrolling should be allowed.
            // However, that's not how scroll views normally work.
            // Scrolling is *only* possible when content is *larger* than view.
            // We use the pivot of the content rect to decide in which directions the content bounds should be expanded.
            // E.g. if pivot is at top, bounds are expanded downwards.
            // This also works nicely when ContentSizeFitter is used on the content.
            Vector3 contentSize = _contentBounds.size;
            Vector3 contentPos = _contentBounds.center;
            Vector3 excess = _viewBounds.size - contentSize;
            if (excess.x > 0)
            {
                contentPos.x -= excess.x * (_content.pivot.x - 0.5f);
                contentSize.x = _viewBounds.size.x;
            }
            if (excess.y > 0)
            {
                contentPos.y -= excess.y * (_content.pivot.y - 0.5f);
                contentSize.y = _viewBounds.size.y;
            }

            _contentBounds.size = contentSize;
            _contentBounds.center = contentPos;
        }

        private readonly Vector3[] m_Corners = new Vector3[4];
        private Bounds GetBounds()
        {
            if (_content == null)
                return new Bounds();

            var vMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var vMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            var toLocal = ViewRect.worldToLocalMatrix;
            _content.GetWorldCorners(m_Corners);
            for (int j = 0; j < 4; j++)
            {
                Vector3 v = toLocal.MultiplyPoint3x4(m_Corners[j]);
                vMin = Vector3.Min(v, vMin);
                vMax = Vector3.Max(v, vMax);
            }

            var bounds = new Bounds(vMin, Vector3.zero);
            bounds.Encapsulate(vMax);
            return bounds;
        }

        private Vector2 CalculateOffset(Vector2 delta)
        {
            Vector2 offset = Vector2.zero;
            if (_movementType == ScrollRect.MovementType.Unrestricted)
                return offset;

            Vector2 min = _contentBounds.min;
            Vector2 max = _contentBounds.max;

            if (_horizontal)
            {
                min.x += delta.x;
                max.x += delta.x;
                if (min.x > _viewBounds.min.x)
                    offset.x = _viewBounds.min.x - min.x;
                else if (max.x < _viewBounds.max.x)
                    offset.x = _viewBounds.max.x - max.x;
            }

            if (_vertical)
            {
                min.y += delta.y;
                max.y += delta.y;
                if (max.y < _viewBounds.max.y)
                    offset.y = _viewBounds.max.y - max.y;
                else if (min.y > _viewBounds.min.y)
                    offset.y = _viewBounds.min.y - min.y;
            }

            return offset;
        }

        protected void SetDirty()
        {
            if (!IsActive())
                return;

            LayoutRebuilder.MarkLayoutForRebuild(CurrentRect);
        }

        protected void SetDirtyCaching()
        {
            if (!IsActive())
                return;

            CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
            LayoutRebuilder.MarkLayoutForRebuild(CurrentRect);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            SetDirtyCaching();
        }
#endif
    }

}