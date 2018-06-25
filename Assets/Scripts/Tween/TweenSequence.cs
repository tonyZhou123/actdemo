using System.Collections.Generic;

namespace Framework
{
	public class TweenSequence : TweenGroup
	{
		public TweenSequence(List<TweenBase> lst)
			: base(lst)
		{
			//Debug.Log(_lst.Count.ToString());
		}

		void BeginNext(int idx, float time)
		{
			for (int i = idx, count = _lst.Count; i < count; ++i)
			{
				TweenBase t = _lst[i];
				t.OnBegin(time);
				if (!t.IsEnd())
				{
					break;
				}
			}
		}

		override public void Update(float time)
		{
			for (int i = 0, count = _lst.Count; i < count; ++i)
			{
				TweenBase t = _lst[i];
				if (!t.IsEnd())
				{
					t.Update(time);
					if (t.IsEnd() && !_isStop)
					{
						BeginNext(i + 1, time);
					}
					break;
				}
			}
		}

		override public void OnBegin(float time)
		{
			BeginNext(0, time);
		}

		override public TweenBase Reverse()
		{
			List<TweenBase> r = new List<TweenBase>();
			for (int i = _lst.Count - 1; i >= 0; --i)
			{
				r.Add(CreateTween(_lst[i].Reverse()));
			}
			return CreateTween(new TweenSequence(r));
		}

		override public void Stop()
		{
			_isStop = true;
			for (int i = 0, count = _lst.Count; i < count; ++i)
			{
				TweenBase t = _lst[i];
				t.Stop();
			}
		}
	}
}
