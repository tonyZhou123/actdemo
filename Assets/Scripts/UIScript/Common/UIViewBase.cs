using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

    public enum EUnloadType
    {
        Destroy,
        Visible,
    }

    public enum EWindowType
    {
        Window,
        MessageTip,
        Loading
    }

    public enum EWindowShowMode
    {
        DoNothing,              //打开时啥事也不干
        HideOther,              //打开时会关闭其它窗口
        SaveTarget,             //打开时会保存当前的全屏大窗口，以便全屏窗口管理
    }

    public enum EWindowLoadState
    {
        None,                  //未加载或已销毁
        Loading,               //正在加载中
        Opening,               //打开中
        Hideing,               //隐藏中
        Shuting,               //正在关闭中
    }

    public enum EWindowMaskType
    {
        None,                  //无遮罩效果
        BlackTransparent,      //黑色半透明遮罩
        WhiteTransparent,      //白色透明遮罩
        //Blur,                  //背景模糊处理遮罩
        //DepthOfField,          //景深遮罩
        //GrayScale,             //灰度背景
    }

    public class  UIViewBase
    {
        public string               ID                 { get; set; }
        public EWindowType          Type               { get; set; }
        public EUnloadType          UnloadType         { get; set; }
        public EWindowMaskType      MaskType           { get; set; }
        public float                MaskAlpha          { get; set; }

        public EWindowShowMode      ShowMode           { get; set; }
        public string               Path               { get; set; }
        public Action               onInitWidgets      { get; set; }
        public Action               onStart            { get; set; }
        public Action               onClose            { get; set; }
        public Action               onDestroy          { get; set; }
        public Action               onLoadSubWindows   { get; set; }

        public Action               onMaskClick        { get; set; }

        public Transform            transform          { get; set; }
        public GameObject           gameObject         { get; set; }
        public float                HideTime           { get; set; } //隐藏时间
        public Transform            Root               { get; set; }
        public int                  Sort               { get; set; } //排序优先级
        public string               TargetID           { get; set; } //绑定的主窗口

        //public UIPanel              Panel
        //{
        //    get
        //    {
        //        if (transform == null)
        //        {
        //            return null;
        //        }
        //        return transform.GetComponent<UIPanel>();
        //    }
        //}

        public bool                 IsActive
        {
            get
            {
                return m_State == EWindowLoadState.Opening;
            }
        }

        public bool                 IsNeedSortZ
        {
            get { return m_IsNeedSortZ; }
            set { m_IsNeedSortZ = value; }
        }

        public bool                 IsResourceAsset
        {
            get { return m_IsResourceAsset; }
            set { m_IsResourceAsset = value; }
        }

        public bool                 IsEnableMaskCollider
        {
            get { return m_IsEnableMaskCollider; }
            set { m_IsEnableMaskCollider = value; }
        }

        private EWindowLoadState      m_State                = EWindowLoadState.None;
        private bool                  m_IsNeedSortZ          = true;
        private bool                  m_IsEnableMaskCollider = true;
        private bool                  m_IsResourceAsset      = true;
        private List<UIViewComponent> m_ViewComponents;


        public virtual void OnInitWidgets()
        {
            if (onInitWidgets != null)
            {
                onInitWidgets();
            }
            if (m_ViewComponents != null)
            {
                for (int i = 0; i < m_ViewComponents.Count; i++)
                {
                    m_ViewComponents[i].OnInitWidgets();
                }
            }
        }

        public virtual void OnStart()
        {
            if (onStart != null)
            {
                onStart();
            }
            if (m_ViewComponents != null)
            {
                for (int i = 0; i < m_ViewComponents.Count; i++)
                {
                    m_ViewComponents[i].OnStart();
                }
            }
        }

        public virtual void OnClose()
        {
            if (onClose != null)
            {
                onClose();
            }
            if (m_ViewComponents != null)
            {
                for (int i = 0; i < m_ViewComponents.Count; i++)
                {
                    m_ViewComponents[i].OnClose();
                }
            }
           
        }

        public virtual void OnDestroy()
        {
            EventCenter.DelListenersByTarget(this);
            if (onDestroy != null)
            {
                onDestroy();
            }
            if (m_ViewComponents != null)
            {
                for (int i = 0; i < m_ViewComponents.Count; i++)
                {
                    m_ViewComponents[i].OnDestroy();
                }
                m_ViewComponents = null;
            }
            m_ViewComponents = null;
        }

        public virtual void OnLoadSubWindows()
        {
            if (onLoadSubWindows != null)
            {
                onLoadSubWindows();
            }
        }

        public virtual void SetLayer()
        {

        }

        public virtual void SetData(params object[] args)
        {

        }

        public         void ShowAsync()
        {
            switch (m_State)
            {
                case EWindowLoadState.None:
                    LoadAsync();
                    break;
                case EWindowLoadState.Hideing:
                    SetActive(true);
                    break;
            }

        }

        public         void HideAsync()
        {
            TargetID = null;
            if (m_State == EWindowLoadState.Opening || 
                m_State == EWindowLoadState.Loading || 
                m_State == EWindowLoadState.Hideing)
            {
                m_State = EWindowLoadState.Shuting;
            }
            switch (UnloadType)
            {
                case EUnloadType.Destroy:
                    OnClose();
                    Destroy();
                    break;
                case EUnloadType.Visible:
                    SetActive(false);
                    break;
            }
        }

        public         void LoadAsync()
        {
            m_State = EWindowLoadState.Loading;
            GameObject go = null;
            if (IsResourceAsset)
            {
                go = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(Path));
            }
            else
            {
                //go = KGame.WorldManager.InstantiatePrefab(Path, Vector3.zero, Quaternion.identity, Vector3.one, Root.gameObject, false);
            }
            if (go == null)
            {
                Debug.LogError(string.Format("不存在窗口 {0} {1}", ID, Path));
                return;
            }
            go.transform.SetParent(Root, false);
            this.gameObject = go;
            this.transform = go.transform;
            this.OnInitWidgets();
            this.SetActive(true);
        }

        public         void Close()
        {
            UIViewManager.instance.CloseWindow(ID);
        }

        public         void Destroy()
        {
            if (m_State == EWindowLoadState.None) return;
            OnDestroy();
            if (gameObject != null)
            {
                gameObject.SetActive(false);
                UnityEngine.Object.Destroy(gameObject);
                transform = null;
                gameObject = null;
            }
            m_State = EWindowLoadState.None;
        }

        public         void SetActive(bool active)
        {
            if (transform == null)
            {
                return;
            }
            if (active)
            {
                if (transform.gameObject.activeSelf == false)
                {
                    transform.gameObject.SetActive(true);
                }
                Vector3 pos = transform.localPosition;
                pos.x = 0;
                pos.y = 0;
                transform.localPosition = pos;
                m_State = EWindowLoadState.Opening;
                OnStart();
                SetLayer();
            }
            else
            {
                Vector3 pos = transform.localPosition;
                pos.x = 20000;
                pos.y = 0;
                transform.localPosition = pos;
                m_State = EWindowLoadState.Hideing;
                OnClose();
            }
        }

        //public        Tween Invoke(float callTime, Action callback, int tick = int.MaxValue)
        //{
        //    return Tween.Schedule(gameObject, callback, callTime, tick);
        //}

        public         T    AddViewComponent<T>(Transform root) where T : UIViewComponent
        {
            if (m_ViewComponents == null)
            {
                m_ViewComponents = new List<UIViewComponent>();
            }
            T viewComponent = System.Activator.CreateInstance<T>();
            m_ViewComponents.Add(viewComponent);
            viewComponent.root = this;
            viewComponent.OnAwake(root);
            return viewComponent;
        }

        public         T    GetViewComponent<T>() where T : UIViewComponent
        {
            for (int i = 0; i < m_ViewComponents.Count; i++)
            {
                if(m_ViewComponents[i] is T)
                {
                    return m_ViewComponents[i] as T;
                }
            }
            return null;
        }
    }