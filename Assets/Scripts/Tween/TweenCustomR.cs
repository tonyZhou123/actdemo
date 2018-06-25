namespace Framework
{
	public class TweenCustomR : TweenInterval
	{
		System.Action<float> _doTween;
        public TweenCustomR(float s, System.Action<float> doTween)
			: base(s)
		{
			_doTween = doTween;
		}

		override public void DoTween(float per)
		{
			if (_doTween != null)
			{
				_doTween(1 - per);
			}
		}

		override public void OnBegin(float time)
		{
			base.OnBegin(time);
		}

		override public TweenBase Reverse()
		{
			return CreateTween(new TweenCustom(_duration, _doTween));
		}
	}

}