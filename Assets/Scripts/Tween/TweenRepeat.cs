using UnityEngine;

namespace Framework
{
	public class TweenRepeat : TweenWrap
	{
		int _n;
		int _curLeft;
		public TweenRepeat(TweenBase inner, int n)
			: base(inner)
		{
			_n = n;
			_curLeft = n;
		}

		override public void Update(float time)
		{
			base.Update(time);

			int maxCount = 100;
			while (_curLeft > 0 && _inner.IsEnd())
			{
				_inner.Reset();
				_inner.OnBegin(time);
				_curLeft--;
				maxCount--;
				//Debug.LogFormat("curLeft = {0} ,maxCount = {1} ,IsEnd = {2}", _curLeft, maxCount, _inner.IsEnd());
				if (maxCount <= 0)
				{
					Debug.Log("Update maxCount <= 0");
					break;
				}
			}
		}


		override public void OnBegin(float time)
		{
			_curLeft = _n;

			int maxCount = 100;
			do
			{
				_inner.Reset();
				_inner.OnBegin(time);
				_curLeft--;
				maxCount--;
				//Debug.LogFormat("curLeft = {0} ,maxCount = {1} ,IsEnd = {2}",_curLeft,maxCount,_inner.IsEnd());
				if(maxCount <= 0)
				{
                    Debug.LogError("OnBegin maxCount <= 0 "+ _inner.IsEnd());
					break;
				}
			} while (_curLeft > 0 && _inner.IsEnd());
		}

		override public bool IsEnd()
		{
			return _curLeft <= 0 && _inner.IsEnd();
		}

		override public TweenBase Reverse()
		{
			return CreateTween(new TweenRepeat(_inner.Reverse(), _n));
		}
	} 
}
