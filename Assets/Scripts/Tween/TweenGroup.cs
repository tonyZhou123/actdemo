using System.Collections.Generic;

namespace Framework
{
	abstract public class TweenGroup : TweenBase
	{
		protected List<TweenBase> _lst;
		public TweenGroup(List<TweenBase> lst)
		{
			_lst = new List<TweenBase>(lst);
		}

		override public void Reset()
		{
			for (int i = 0, count = _lst.Count; i < count; ++i)
			{
				TweenBase t = _lst[i];
				t.Reset();
			}
		}

		override public void SetSpeed(float speed)
		{
			for (int i = 0, count = _lst.Count; i < count; ++i)
			{
				TweenBase t = _lst[i];
				t.SetSpeed(speed);
			}
		}

		override public void SetEase(System.Func<float, float> ease)
		{
			for (int i = 0, count = _lst.Count; i < count; ++i)
			{
				TweenBase t = _lst[i];
				t.SetEase(ease);
			}
		}

		override public bool IsEnd()
		{
			for(int i = 0, count = _lst.Count; i < count; ++i)
			{
				TweenBase t = _lst[i];
				if (!t.IsEnd())
				{
					return false;
				}
			}

			return true;
		}
	}

}