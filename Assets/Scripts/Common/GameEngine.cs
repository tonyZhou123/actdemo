using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEngine : MonoBehaviour
{

    public static GameEngine instance;

    void Awake()
    {
        instance = this;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartCoroutine(IEnumerator func)
    {
        StartCoroutine(func);
    }

    //当场景被加载时会被调用到
    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        Debug.Log(string.Format("scene :{0}, mode:{1}",scene.name,mode));
        switch (mode)
        {
            case UnityEngine.SceneManagement.LoadSceneMode.Single:
                switch (scene.name)
                {
                    case "login":
                        {
                            //DataManager.instance.CleanPlayer();
                            //AudioManager.instance.PlayBGM("Music/LoginBGM");
                            //if (UIViewManager.instance.IsOpen(UIDefine.UIMain))
                            //{
                            //    UIViewManager.instance.CloseWindow(UIDefine.UIMain);
                            //}
                            //if (!UIViewManager.instance.IsOpen(UIDefine.Login))
                            //{
                            //    UIViewManager.instance.OpenWindow(UIDefine.Login);
                            //}
                        }
                        break;
                    case "BattleScene1":
                        {
                            //DataManager.instance.InitPlayer(1);
                            //AudioManager.instance.PlayBGM("Music/mission1BGM");
                            //if (!UIViewManager.instance.IsOpen(UIDefine.UIMain))
                            //{
                            //    UIViewManager.instance.OpenWindow(UIDefine.UIMain);
                            //}
                            //if (UIViewManager.instance.IsOpen(UIDefine.Login))
                            //{
                            //    UIViewManager.instance.CloseWindow(UIDefine.Login);
                            //}
                        }
                        break;
                    case "BattleScene2":
                        {
                            //DataManager.instance.InitPlayer(2);
                            //AudioManager.instance.PlayBGM("Music/mission2BGM");
                            //if (!UIViewManager.instance.IsOpen(UIDefine.UIMain))
                            //{
                            //    UIViewManager.instance.OpenWindow(UIDefine.UIMain);
                            //}
                            //if (UIViewManager.instance.IsOpen(UIDefine.Login))
                            //{
                            //    UIViewManager.instance.CloseWindow(UIDefine.Login);
                            //}
                        }
                        break;
                }
                break;
            case UnityEngine.SceneManagement.LoadSceneMode.Additive:
                break;
        }
    }
}
