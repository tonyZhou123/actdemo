namespace Framework
{
	public class TweenCustom : TweenInterval
	{
		System.Action<float> _doTween;
        public TweenCustom(float s, System.Action<float> doTween)
			: base(s)
		{
			_doTween = doTween;
		}

		override public void DoTween(float per)
		{
			if (_doTween != null)
			{
				_doTween(per);
			}
		}

		override public void OnBegin(float time)
		{
			base.OnBegin(time);
		}

		override public TweenBase Reverse()
		{
			return CreateTween(new TweenCustomR(_duration, _doTween));
		}
	}

}