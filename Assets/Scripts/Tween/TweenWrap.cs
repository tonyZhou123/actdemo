namespace Framework
{
	abstract public class TweenWrap : TweenBase
	{
		protected TweenBase _inner;
		public TweenWrap(TweenBase inner)
		{
			_inner = inner;
		}

		override public void Update(float time)
		{
			_inner.Update(time);
		}

		override public void Reset()
		{
			_inner.Reset();
		}
	} 
}
