using System.Collections.Generic;

namespace Framework
{
	public class TweenSpawn : TweenGroup
	{
		public TweenSpawn(List<TweenBase> lst)
			: base(lst)
		{

		}

		override public void Update(float time)
		{
			for (int i = 0, count = _lst.Count; i < count; ++i)
			{
				TweenBase t = _lst[i];
				t.Update(time);
				if (_isStop) break;
			}
		}

		override public void OnBegin(float time)
		{
			for (int i = 0, count = _lst.Count; i < count; ++i)
			{
				TweenBase t = _lst[i];
				t.OnBegin(time);
			}
		}

		override public TweenBase Reverse()
		{
			List<TweenBase> r = new List<TweenBase>();
			for (int i = _lst.Count - 1; i >= 0; --i)
			{
				r.Add(CreateTween(_lst[i].Reverse()));
			}
			return CreateTween(new TweenSpawn(r));
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
