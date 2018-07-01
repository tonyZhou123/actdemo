using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 控制场景切换的脚本
/// </summary>
public class SceneLoadControl : Singleton<SceneLoadControl> {

    private Action m_callback = null;
    private float _timer;
    public string CurrentSceneName
    {
        get
        {
            return SceneManager.GetActiveScene().name;
        }
    }

    public void LoadScene(string SceneName,Action CallBack = null, float timer = 0f)
	{
	    m_callback = CallBack;
	    _timer = timer;
        GameEngine.instance.StartCoroutine(LoadSceneAsync(SceneName));
	}

	IEnumerator LoadSceneAsync(string name)
	{
	    TimeControl.instance.Set(this);
        yield return null;
	    UIViewManager.instance.OpenWindow(UIDefine.Loading);
        yield return null;
	    ChangeImage(name);
        AsyncOperation ao = SceneManager.LoadSceneAsync (name);
		while (!ao.isDone) {
			yield return null;
            //EventCenter.Notify(EventID.LoadPanel_SetSlider,ao.progress);
        }
	    if (_timer > 0f)
	    {
            Tween.Create(GameEngine.instance.gameObject).Custom(_timer, (per) =>
            {
                //EventCenter.Notify(EventID.LoadPanel_SetSlider, per);
                if (per >= 1f)
                {
                    CloseLoading();
                }
            }).Do();
        }
	    else
	    {
	        CloseLoading();
	    }
	}

    void CloseLoading()
    {
        UIViewManager.instance.CloseWindow(UIDefine.Loading);
        if (m_callback != null)
        {
            m_callback();
        }
        TimeControl.instance.Remove(this);
    }

    void ChangeImage(string name)
    {
        Texture image = Resources.Load<Texture>("Texture/" + name);
        if (image != null)
        {
            //EventCenter.Notify(EventID.LoadPanel_SetImage, image);
        }
    }
}
