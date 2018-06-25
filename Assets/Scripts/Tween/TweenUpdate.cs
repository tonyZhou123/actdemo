
namespace Framework
{
	public class TweenUpdate : TweenBase
	{
		System.Action _func;

		public TweenUpdate(System.Action func)
		{
			_func = func;
		}

		override public void Update(float time)
		{
			_func();
		}
	} 
}
