using UnityEngine;
using System.Collections;

namespace Framework
{
	abstract public class TweenBase
	{
		public GameObject go;
		protected bool _isEnd = false;
		protected bool _isStop = false;

		virtual public void Reset()
		{
			_isEnd = false;
        }

		virtual public void OnCreate()
		{

		}

		virtual public void Update(float time)
		{

		}

		virtual public void OnBegin(float time)
		{
		}

		virtual public bool IsEnd()
		{
			return _isEnd;
		}

		virtual public bool IsStop()
		{
			return _isStop;
		}

		virtual public void Stop()
		{
			_isStop = true;
		}

		virtual public TweenBase Reverse()
		{
			Debug.LogError("Reverse not support");
			return null;
		}

		virtual public void SetSpeed(float speed)
		{

		}

		virtual public void SetEase(System.Func<float, float> ease)
		{

		}

		protected TweenBase CreateTween(TweenBase t)
		{
			t.go = go;
			t.OnCreate();
			return t;
		}
	}

}