namespace Framework
{
	public class TweenFunc : TweenInstant
	{
		System.Action _func;
		public TweenFunc(System.Action func)
		{
			_func = func;
		}


		override public void OnBegin(float time)
		{
			base.OnBegin(time);
			_func();
		}

		override public TweenBase Reverse()
		{
			return CreateTween(new TweenFunc(_func));
		}
	} 
}
