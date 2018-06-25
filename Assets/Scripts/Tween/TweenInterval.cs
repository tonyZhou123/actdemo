namespace Framework
{
	abstract public class TweenInterval : TweenBase
	{
		protected float _beginTime = 0f;
		System.Func<float, float> _ease = EaseFunc.Linear;

		float _speed = 1.0f;

		protected float _duration;
		public TweenInterval(float duration)
		{
			_duration = duration;
		}

		virtual public void DoTween(float per)
		{

		}

		override public void SetSpeed(float speed)
		{
			_speed = speed;
		}

		override public void SetEase(System.Func<float, float> ease)
		{
			_ease = ease;
		}

		override public void Update(float time)
		{
			if (IsEnd()) return;

			float dt = (time - _beginTime) * _speed;

			if (dt >= _duration)
			{
				_isEnd = true;
			}

			float per = dt / _duration;

			per = UnityEngine.Mathf.Clamp01(per);

			//Debug.Log("before " + per);
			per = _ease(per);
			//Debug.Log("after " + per);

			DoTween(per);
		}

		override public void OnBegin(float time)
		{
			_beginTime = time;
			if (_duration <= 0f)
			{
				DoTween(1f);
				_isEnd = true;
			}
		}
	} 
}
