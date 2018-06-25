namespace Framework
{
	public class TweenDelay : TweenInterval
	{
		public TweenDelay(float s)
			: base(s)
		{

		}

		override public TweenBase Reverse()
		{
			return CreateTween(new TweenDelay(_duration));
		}

	}
}
