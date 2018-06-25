namespace Framework
{
	public class TweenFrame : TweenBase
	{
		int _frame;
		int _left = 0;
		public TweenFrame(int frame)
		{
			_frame = frame;
		}

		override public void Update(float time)
		{
			if (IsEnd()) return;

			_left--;
			if(_left <= 0)
			{
				_isEnd = true;
			}
		}

		override public void OnBegin(float time)
		{
			_left = _frame;
        }
	} 
}
