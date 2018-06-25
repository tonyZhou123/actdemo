namespace Framework
{
	abstract public class TweenInstant : TweenBase
	{
		override public void OnBegin(float time)
		{
			_isEnd = true;
		}
	} 
}
