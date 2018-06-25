using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;

public class MaskGUI
{
    public GameObject gameObject;
    public Transform transform;
    public Action ClickAction;
    public int Sort;

    public MaskGUI() { }

    public MaskGUI(string path)
    {
        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab == null)
        {
            Debug.LogError(string.Format("not exists Path = [{0}]", path));
            return;
        }
        this.gameObject = GameObject.Instantiate<GameObject>(prefab);
        this.gameObject.SetActive(false);
        UIEventListener.Get(this.gameObject).onClick = (go) =>
        {
            if (ClickAction != null)
            {
                ClickAction();
            }
        };
        this.transform = this.gameObject.transform;
    }
}

public class UIViewManager : Singleton<UIViewManager>
{
    private Dictionary<string, UIViewBase> m_AllWindows = new Dictionary<string, UIViewBase>();
    private Dictionary<EWindowType, int> m_PreMinDepths = new Dictionary<EWindowType, int>();//给每种UI层级设置的最小深度
    private Dictionary<EWindowType, float> m_PreMinZ = new Dictionary<EWindowType, float>();//给每种UI层级设置的最大Z轴
    private List<UIViewBase> m_OpenWindows = new List<UIViewBase>();
    private List<UIViewBase> m_MutexStacks = new List<UIViewBase>();//互斥窗口
    private Camera m_UICamera;
    private Canvas m_UICanvas;
    //private int                                       m_Sort = 0;
    private HashSet<GameObject> m_FingerUpEventIgnoreObjects = new HashSet<GameObject>();//手势弹起触发事件条件（忽略对象）
    private List<Action> m_FingerUpEvents = new List<Action>();       //手势弹起触发事件列表
    private List<string> m_FingerUpEventHideWindows = new List<string>();       //手势弹起关闭的窗口

    private MaskGUI m_MaskBlackTransparent;

    MaskGUI MaskBlackTransparent
    {
        get
        {
            if (m_MaskBlackTransparent == null)
            {
                m_MaskBlackTransparent = new MaskGUI(UIDefine.BlackMaskPath);
                m_MaskBlackTransparent.transform.SetParent(CanvasRoot.transform,false);
                m_MaskBlackTransparent.transform.localPosition = Vector3.zero;
                m_MaskBlackTransparent.transform.localScale = Vector3.one;
            }
            return m_MaskBlackTransparent;
        }
    }
    private MaskGUI m_MaskWhiteTransparent;

    MaskGUI MaskWhiteTransparent
    {
        get
        {
            if (m_MaskWhiteTransparent == null)
            {
                m_MaskWhiteTransparent = new MaskGUI(UIDefine.WhiteMaskPath);
                m_MaskWhiteTransparent.transform.SetParent(CanvasRoot.transform);
                m_MaskWhiteTransparent.transform.localPosition = Vector3.zero;
                m_MaskWhiteTransparent.transform.localScale = Vector3.one;
            }
            return m_MaskWhiteTransparent;
        }
    }

    //private MaskGUI m_MaskPostProcess;
    private bool m_InitClipPlane;

    public Camera UICamera
    {
        get
        {
            if (m_UICamera == null)
            {
                m_UICamera = CanvasRoot.GetComponentInChildren<Camera>();
                //m_UICamera.depth = 50;

            }
            //if (m_InitClipPlane == false)
            //{
            //    m_UICamera.farClipPlane = 10000;
            //    m_UICamera.nearClipPlane = -10;
            //    m_InitClipPlane = true;
            //}
            return m_UICamera;
        }
    }

    public Canvas CanvasRoot
    {
        get
        {
            if (m_UICanvas == null)
            {
                m_UICanvas = GameObject.FindObjectOfType<Canvas>().GetComponent<Canvas>();
            }
            return m_UICanvas;
        }
    }

    public void Init()
    {
        //foreach (var current in Enum.GetNames(typeof(EWindowType)))
        //{
        //    string enumName = current;
        //    EWindowType type = (EWindowType)Enum.Parse(typeof(EWindowType), enumName);
        //    m_PreMinDepths[type] = 100 + (int)type * 600;
        //}
        m_PreMinDepths[EWindowType.Window] = 100;
        m_PreMinDepths[EWindowType.MessageTip] = 1500;
        m_PreMinDepths[EWindowType.Loading] = 3000;

        m_PreMinZ[EWindowType.Window] = 9000;
        m_PreMinZ[EWindowType.MessageTip] = 1000;
        m_PreMinZ[EWindowType.Loading] = 500;
        //UICamera.onClick += OnGlobalClick;
        RegisterWindows();

        //UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        //UnityEngine.SceneManagement.SceneManager.sceneUnloaded += OnSceneUnloaded;
    }


    //void OnSceneLoadedEnd(string scenename)
    //{
    //    if (scenename.Contains("Login"))
    //    {
    //        Hot.HotFix.Close();
    //    }
    //}
    public override void Destroy()
    {
        //UICamera.onClick -= OnGlobalClick;
        ReleaseEx();
        //UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        //UnityEngine.SceneManagement.SceneManager.sceneUnloaded -= OnSceneUnloaded;
        //WorldCollectionManager.instance.onEndWorldLoaded -= OnSceneLoadedEnd;
    }

    //void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    //{
    //    Debug.Log("+++++++++++ OnSceneLoaded " + scene.name);
    //    Debug.Log("+++++++++++ OnSceneLoadSceneMode " + mode);
    //    switch (mode)
    //    {
    //        case UnityEngine.SceneManagement.LoadSceneMode.Single:
    //            switch (scene.name)
    //            {
    //                case "login":
    //                    {
    //                        if (IsOpen(UIDefine.UIMain))
    //                        {
    //                            CloseWindow(UIDefine.UIMain);
    //                        }
    //                        OpenWindow(UIDefine.Login);
    //                    }
    //                    break;
    //                case "BattleScene":
    //                    {
    //                        OpenWindow(UIDefine.UIMain);
    //                    }
    //                    break;
    //            }

    //            break;
    //        case UnityEngine.SceneManagement.LoadSceneMode.Additive:
    //            break;
    //    }
    //}

    //void OnSceneUnloaded(UnityEngine.SceneManagement.Scene scene)
    //{
    //    Debug.Log("----------- OnSceneUnloaded " + scene.name);
    //}

    void RegisterWindows()
    {
        //成就
        //Register(UIDefine.Loading, new LoadingPanel());
        //Register(UIDefine.Login, new LoginPanel());
        //Register(UIDefine.test, new testPanel());
        //Register(UIDefine.KeyCodePanel, new KeyCodePanel());
        //Register(UIDefine.Setting1, new Setting1());
        //Register(UIDefine.Setting2, new Setting2());
        //Register(UIDefine.Setting3, new Setting3());
        //Register(UIDefine.Notice, new NoticePanel());
        //Register(UIDefine.UIMain, new UIMain());
        //Register(UIDefine.AboutPanel, new AboutPanel());
        //Register(UIDefine.CheatPanel, new CheatPanel());
        //Register(UIDefine.WinPanel, new WinPanel());
        //Register(UIDefine.FailPanel, new FailPanel());
        //Register(UIDefine.UIAchievementMain, new UIAchievementMain());
        //Register(UIDefine.UIAchievementDashi, new UIAchievementDashi());
        //

    }

    /// <summary>
    /// Lua代码使用，注册一个UI
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public UIViewBase Register(string id)
    {
        UIViewBase viewBase = null;
        if (m_AllWindows.TryGetValue(id, out viewBase))
        {
            Debug.LogError("重复注册的UI的ID = " + id);
            return viewBase;
        }
        else
        {
            viewBase = new UIViewBase();
            viewBase.ID = id;
            m_AllWindows[id] = viewBase;
            return viewBase;
        }
    }

    /// <summary>
    /// C#代码使用，注册一个UI
    /// </summary>
    /// <param name="id"></param>
    /// <param name="view"></param>
    /// <returns></returns>
    public UIViewBase Register(string id, UIViewBase view)
    {
        if (m_AllWindows.ContainsKey(id))
        {
            Debug.LogError("重复注册的UI的ID = " + id);
            return null;
        }
        else
        {
            view.ID = id;
            m_AllWindows[id] = view;
            return view;
        }
    }

    public UIViewBase OpenWindow(string windowID)
    {
        return OpenWindow(windowID, null);
    }

    public UIViewBase OpenWindow(string windowID, params object[] args)
    {
        if (!m_AllWindows.ContainsKey(windowID))
        {
            return null;
        }
        UIViewBase window = m_AllWindows[windowID];
        //m_Sort++;
        m_PreMinDepths[window.Type] += 2;
        int m_Sort = m_PreMinDepths[window.Type];
        window.Sort = m_Sort;
        DealWindowStack(window, true);
        window.Root = CanvasRoot.transform;
        if (window.MaskType != EWindowMaskType.None)
        {
            //TODO:要做事情
            //EventCenter.Notify(EventID.PlayerEvent_StopJoystick);
        }
        if (window.ShowMode == EWindowShowMode.SaveTarget && m_MutexStacks.Count > 0)
        {
            UIViewBase w = m_MutexStacks[m_MutexStacks.Count - 1];
            window.TargetID = w.ID;
        }
        if (m_OpenWindows.Contains(window) == false)
        {
            m_OpenWindows.Add(window);
        }
        window.SetData(args);
        window.ShowAsync();

        if (window.transform == null)
        {
            return null;
        }

        //RefreshDepth(window);
        RefreshMask();
        window.OnLoadSubWindows();
        return window;
    }

    public void CloseWindow(string windowID, bool IsPlayAudio = false)
    {
        if (IsPlayAudio)
        {
            //是否播放关闭音乐
            //UIFunc.OnClickBtnNormalClose();
        }
        if (string.IsNullOrEmpty(windowID))
        {
            return;
        }
        UIViewBase window = null;
        if (m_AllWindows.TryGetValue(windowID, out window) == false)
        {
            return;
        }
        window.HideAsync();
        m_OpenWindows.Remove(window);
        RefreshMask();
        DealWindowStack(window, false);
    }

    public UIViewBase GetWindow(string windowID)
    {
        UIViewBase window = null;
        m_AllWindows.TryGetValue(windowID, out window);
        return window;
    }

    public UIViewBase GetOpenWindow(string windowID)
    {
        UIViewBase window = m_OpenWindows.Find(obj => obj.ID == windowID);
        return window;
    }

    public bool IsOpen(string windowID)
    {
        return GetOpenWindow(windowID) != null;
    }

    public void Release()
    {
        foreach (KeyValuePair<string, UIViewBase> pair in m_AllWindows)
        {
            if (pair.Value.Type != EWindowType.Loading)
            {
                pair.Value.Destroy();
            }
        }
        //TODO:遮罩问题
        //if (m_MaskPostProcess != null)
        //{
        //    m_MaskPostProcess.Release();
        //}
        //if (m_MaskWhiteTransparent != null)
        //{
        //    m_MaskWhiteTransparent.Release();
        //}
        //if (m_MaskBlackTransparent != null)
        //{
        //    m_MaskBlackTransparent.Release();
        //}
        m_MutexStacks.Clear();
        m_OpenWindows.Clear();
        m_FingerUpEventIgnoreObjects.Clear();
        m_FingerUpEventHideWindows.Clear();
    }

    public void ReleaseEx()
    {
        foreach (KeyValuePair<string, UIViewBase> pair in m_AllWindows)
        {
            pair.Value.Destroy();
        }
        //TODO:遮罩问题
        //if (m_MaskPostProcess != null)
        //{
        //    m_MaskPostProcess.Release();
        //}
        //if (m_MaskWhiteTransparent != null)
        //{
        //    m_MaskWhiteTransparent.Release();
        //}
        //if (m_MaskBlackTransparent != null)
        //{
        //    m_MaskBlackTransparent.Release();
        //}
        m_MutexStacks.Clear();
        m_OpenWindows.Clear();
        m_FingerUpEventIgnoreObjects.Clear();
        m_FingerUpEventHideWindows.Clear();
    }

    public void SetChildActive(string childName, bool visible)
    {

    }

    //public void SetEventReceiveActive(bool active)
    //{
    //    if (active == false)
    //    {
    //        NGUICamera.GetComponent<UICamera>().eventReceiverMask = 1 << KGameUtility.layerCutsceneUI;
    //        NGUICamera.cullingMask = 1 << KGameUtility.layerCutsceneUI;
    //    }
    //    else
    //    {
    //        NGUICamera.GetComponent<UICamera>().eventReceiverMask = 1 << LayerMask.NameToLayer("UI");
    //        NGUICamera.cullingMask = 1 << LayerMask.NameToLayer("UI");
    //    }
    //}

    public void SetCameraVisable(bool active)
    {

    }

    //void FindPanels(UIViewBase window, List<UIPanel> panels)
    //{
    //    if (window == null || window.Root == null || window.transform == null)
    //    {
    //        return;
    //    }
    //    panels.AddRange(window.transform.GetComponentsInChildren<UIPanel>(true));
    //}


    //TODO：窗口深度
    /// <summary>
    /// 窗口深度排序
    /// </summary>
    /// <param name="window"></param>
    //    void RefreshDepth(UIViewBase window)
    //    {
    //    EWindowType type = window.Type;
    //    List<UIPanel> pList = new List<UIPanel>();
    //    FindPanels(window, pList);

    //    float minZ = m_PreMinZ[type];
    //    Int32 stDepth = m_PreMinDepths[type];
    //    Int32 maxDepth = stDepth;


    //    for (int i = 0; i < m_OpenWindows.Count; i++)
    //    {
    //        UIViewBase w = m_OpenWindows[i];
    //        if (w == null || w.transform == null)
    //        {
    //            Debug.LogError("不正常销毁的" + w.GetType());
    //            continue;
    //        }
    //        if (w.Type != type)
    //        {
    //            continue;
    //        }
    //        if (w == window)
    //        {
    //            continue;
    //        }

    //        List<UIPanel> list = new List<UIPanel>();
    //        FindPanels(w, list);
    //        UIPanel m = w.Panel;
    //        for (int k = 0; k < list.Count; k++)
    //        {
    //            UIPanel childPanel = list[k];
    //            if (maxDepth < childPanel.depth)
    //            {
    //                maxDepth = childPanel.depth;
    //            }
    //            float zOffset = childPanel.transform.position.z - m.transform.position.z;
    //            zOffset *= (1 / NGUIRoot.transform.localScale.z);
    //            zOffset += m.transform.localPosition.z;
    //            if (minZ > zOffset && zOffset > 0)
    //            {
    //                minZ = zOffset;
    //            }
    //        }
    //    }


    //    if (pList.Count >= 2)
    //    {
    //        pList.Sort(UIPanel.CompareFunc);
    //    }


    //    UIPanel mainPanel = window.Panel;
    //    for (int i = 0; i < pList.Count; i++)
    //    {
    //        pList[i].depth = maxDepth + i + 2;
    //        pList[i].useSortingOrder = true;
    //        pList[i].sortingOrder = pList[i].depth;
    //        pList[i].renderQueue = UIPanel.RenderQueue.Automatic;

    //        if (window.IsNeedSortZ)
    //        {
    //            if (mainPanel == pList[i])
    //            {
    //                Vector3 pos = pList[i].transform.localPosition;
    //                pos.z = -i * 400 + minZ;
    //                pList[i].transform.localPosition = pos;
    //            }
    //            else
    //            {
    //                Vector3 pos = pList[i].transform.localPosition;
    //                pos.z = -i * 400;
    //                pList[i].transform.localPosition = pos;
    //            }
    //        }
    //        else
    //        {
    //            Vector3 pos = pList[i].transform.localPosition;
    //            if (mainPanel == pList[i])
    //            {

    //                pos.z = minZ;
    //                pList[i].transform.localPosition = pos;
    //            }
    //            else
    //            {
    //                pos.z = 0;
    //                pList[i].transform.localPosition = pos;
    //            }
    //        }
    //    }

    //    EEffectRenderQueue[] eEffectRenderQueues = window.transform.gameObject.GetComponentsInChildren<EEffectRenderQueue>();
    //    for (int i = 0; i < eEffectRenderQueues.Length; i++)
    //    {
    //        eEffectRenderQueues[i].Calculate();
    //    }
    //}


    /// <summary>
    /// 处理窗口导航、互斥关系
    /// </summary>
    /// <param name="win"></param>
    /// <param name="open"></param>
    void DealWindowStack(UIViewBase win, bool open)
    {
        if (win.ShowMode != EWindowShowMode.HideOther)
        {
            return;
        }
        if (open)
        {
            if (m_MutexStacks.Count > 0)
            {
                UIViewBase w = m_MutexStacks[m_MutexStacks.Count - 1];
                for (int i = 0; i < m_OpenWindows.Count; i++)
                {
                    UIViewBase view = m_OpenWindows[i];
                    if (view.ShowMode == EWindowShowMode.SaveTarget && w.ID == view.TargetID)
                    {
                        view.SetActive(false);
                    }
                    if (view.Type == EWindowType.Window && view.ShowMode == EWindowShowMode.DoNothing)
                    {
                        view.Close();
                    }
                }
                w.SetActive(false);
            }
            m_MutexStacks.Add(win);

        }
        else
        {
            m_MutexStacks.Remove(win);
            for (int i = m_OpenWindows.Count - 1; i >= 0; i--)
            {
                UIViewBase view = m_OpenWindows[i];
                if (view.ShowMode == EWindowShowMode.SaveTarget && view.TargetID == win.ID)
                {
                    CloseWindow(view.ID);
                }
            }

            if (m_MutexStacks.Count > 0)
            {
                UIViewBase last = m_MutexStacks[m_MutexStacks.Count - 1];
                last.SetActive(true);
                for (int i = 0; i < m_OpenWindows.Count; i++)
                {
                    UIViewBase view = m_OpenWindows[i];
                    if (view.ShowMode == EWindowShowMode.SaveTarget && view.TargetID == last.ID)
                    {
                        view.SetActive(true);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 刷新Mask
    /// </summary>
    void RefreshMask()
    {
        List<UIViewBase> windows = m_OpenWindows;
        UIViewBase needBlackView = null;
        UIViewBase needWhiteView = null;
        //UIViewBase needPostProcessView = null;
        for (int i = 0; i < windows.Count; i++)
        {
            UIViewBase view = windows[i];
            if (view.IsActive == false)
            {
                continue;
            }
            switch (view.MaskType)
            {
                case EWindowMaskType.BlackTransparent:
                    if (needBlackView == null)
                    {
                        needBlackView = view;
                    }
                    else
                    {
                        if (view != needBlackView && view.Sort > needBlackView.Sort)
                        {
                            needBlackView = view;
                        }
                    }
                    break;
                case EWindowMaskType.WhiteTransparent:
                    if (needWhiteView == null)
                    {
                        needWhiteView = view;
                    }
                    else
                    {
                        if (view != needWhiteView && view.Sort > needWhiteView.Sort)
                        {
                            needWhiteView = view;
                        }
                    }
                    break;
                //case EWindowMaskType.Blur:
                //case EWindowMaskType.DepthOfField:
                //case EWindowMaskType.GrayScale:
                //    if (needPostProcessView == null)
                //    {
                //        needPostProcessView = view;
                //    }
                //    else
                //    {
                //        if (view != needPostProcessView && view.Sort > needPostProcessView.Sort)
                //        {
                //            needPostProcessView = view;
                //        }
                //    }
                //    break;
            }
        }

        //ShowMask(needBlackView, ref m_MaskBlackTransparent, "assets/kgame/ui/prefabs/mask/maskblacktransparent.prefab");
        //ShowMask(needWhiteView, ref m_MaskWhiteTransparent, "assets/kgame/ui/prefabs/mask/maskwhitetransparent.prefab");
        ShowMask(needBlackView, needWhiteView);
        //ShowMask(needPostProcessView, ref m_MaskPostProcess, "");
    }

    //刷新mask+排序
    void ShowMask(UIViewBase needBlackView, UIViewBase needWhiteView)
    {
        if (needBlackView != null && needBlackView.transform != null)
        {
            MaskBlackTransparent.Sort = needBlackView.Sort - 1;
            MaskBlackTransparent.gameObject.SetActive(true);
            MaskBlackTransparent.ClickAction = needBlackView.onMaskClick;
        }
        else
        {
            MaskBlackTransparent.gameObject.SetActive(false);
        }
        if (needWhiteView != null && needWhiteView.transform != null)
        {
            MaskWhiteTransparent.Sort = needWhiteView.Sort - 1;
            MaskWhiteTransparent.gameObject.SetActive(true);
            MaskWhiteTransparent.ClickAction = needWhiteView.onMaskClick;
        }
        else
        {
            MaskWhiteTransparent.gameObject.SetActive(false);
        }
        m_OpenWindows.Sort((viewBase1, viewBase2) => viewBase1.Sort.CompareTo(viewBase2.Sort));//升序
        bool blackFlag = MaskBlackTransparent.gameObject.activeSelf;
        bool WhiteFlag = MaskWhiteTransparent.gameObject.activeSelf;
        for (int i = 0; i < m_OpenWindows.Count; i++)
        {
            if (blackFlag && MaskBlackTransparent.Sort < m_OpenWindows[i].Sort)
            {
                blackFlag = !blackFlag;
                MaskBlackTransparent.transform.SetAsLastSibling();
            }
            if (WhiteFlag && MaskWhiteTransparent.Sort < m_OpenWindows[i].Sort)
            {
                WhiteFlag = !WhiteFlag;
                MaskWhiteTransparent.transform.SetAsLastSibling();
            }
            m_OpenWindows[i].transform.SetAsLastSibling();
        }
    }


    //void ShowMask(UIViewBase targetView, ref MaskGUI maskGUI, string path)
    //{
    //    if (targetView != null && targetView.transform != null)
    //    {
    //        if (maskGUI == null)
    //        {
    //            GameObject prefab = Resources.Load<GameObject>(path);
    //            if (prefab == null)
    //            {
    //                Debug.LogError(string.Format("not exists Path = [{0}]", path));
    //                return;
    //            }
    //            maskGUI = new MaskGUI();
    //            maskGUI.gameObject = GameObject.Instantiate<GameObject>(prefab);
    //            maskGUI.transform = maskGUI.gameObject.transform;
    //            maskGUI.Sort = targetView.Sort - 1;
    //            maskGUI.transform.SetParent(CanvasRoot.transform);
    //            maskGUI.transform.localPosition = Vector3.zero;
    //            maskGUI.transform.localScale = Vector3.one;
    //            //GameObject go = KGame.WorldManager.InstantiatePrefab(path, Vector3.zero, Quaternion.identity, Vector3.one, NGUIRoot.gameObject, false);
    //            //maskGUI = System.Activator.CreateInstance<T>();
    //            //maskGUI.InitWidgets(go.transform);
    //        }
    //        maskGUI.gameObject.SetActive(true);
    //        //maskGUI.SetTargetWindow(targetView);
    //    }
    //    else
    //    {
    //        if (maskGUI != null && maskGUI.gameObject != null)
    //        {
    //            maskGUI.gameObject.SetActive(false);
    //        }
    //    }
    //}

    public void AddFingerUpEventIgnoreObject(GameObject obj)
    {
        if (m_FingerUpEventIgnoreObjects.Contains(obj) == false)
        {
            m_FingerUpEventIgnoreObjects.Add(obj);
        }
    }

    public void DelFingerUpEventIgnoreObject(GameObject obj)
    {
        if (obj == null)
        {
            return;
        }
        m_FingerUpEventIgnoreObjects.Remove(obj);
    }

    public void AddFingerUpEvent(Action callback)
    {
        if (callback == null)
        {
            return;
        }
        if (m_FingerUpEvents.Contains(callback) == false)
        {
            m_FingerUpEvents.Add(callback);
        }
    }

    public void DelFingerUpEvent(Action callback)
    {
        if (callback == null)
        {
            return;
        }
        m_FingerUpEvents.Remove(callback);
    }

    public void AddFingerUpHideWindow(string windowID)
    {
        if (m_FingerUpEventHideWindows.Contains(windowID) == false)
        {
            m_FingerUpEventHideWindows.Add(windowID);
        }
    }

    //void OnGlobalClick(GameObject go)
    //{
    //    if (go == NGUIRoot.gameObject)
    //    {
    //        OnFingerUpEvents();
    //    }
    //    if (m_FingerUpEventIgnoreObjects.Contains(go))
    //    {
    //        return;
    //    }
    //    OnFingerUpEvents();
    //}

    //手势抬起事件触发
    //void OnFingerUpEvents()
    //{
    //    if (m_FingerUpEvents.Count > 0)
    //    {
    //        for (int i = 0; i < m_FingerUpEvents.Count; i++)
    //        {
    //            Action callback = m_FingerUpEvents[i];
    //            if (callback != null)
    //            {
    //                callback();
    //            }
    //        }
    //    }
    //    if (m_FingerUpEventHideWindows.Count > 0)
    //    {
    //        for (int i = 0; i < m_FingerUpEventHideWindows.Count; i++)
    //        {
    //            string windowID = m_FingerUpEventHideWindows[i];
    //            CloseWindow(windowID);
    //        }
    //    }
    //}
}
