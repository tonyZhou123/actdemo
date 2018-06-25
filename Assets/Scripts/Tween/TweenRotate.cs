using UnityEngine;

namespace Framework
{
	public class TweenRotate : TweenInterval
	{
		Vector3 _v;
		ETweenType _type;

		Vector3 _dest;
		Vector3 _begin;

		Transform _transform;

		Vector3 _ctrl = Vector3.zero;


		public TweenRotate(float s, Vector3 v, Vector3 ctrl, ETweenType type)
			: base(s)
		{
			_v = v;
			_ctrl = ctrl;
			_type = type;
		}

		override public void OnCreate()
		{
			_transform = go.transform;
		}

		override public void DoTween(float per)
		{
			Vector3 p = _transform.eulerAngles;
			if (_ctrl.x != 0)
			{
				p.x = Mathf.LerpUnclamped(_begin.x, _dest.x, per);
			}
			if (_ctrl.y != 0)
			{
				p.y = Mathf.LerpUnclamped(_begin.y, _dest.y, per);
			}
			if (_ctrl.z != 0)
			{
				p.z = Mathf.LerpUnclamped(_begin.z, _dest.z, per);
			}
			_transform.eulerAngles = p;
		}

		override public void OnBegin(float time)
		{
			if (_type == ETweenType.By)
			{
				_begin = _transform.eulerAngles;
				_dest = _begin + _v;
			}
			else if (_type == ETweenType.From)
			{
				_begin = _v;
				_dest = _transform.eulerAngles;
			}
			else
			{
				_begin = _transform.eulerAngles;
				_dest = _v;
			}

			DoTween(0);

			base.OnBegin(time);
		}

		override public TweenBase Reverse()
		{
			if (_type == ETweenType.By)
			{
				return CreateTween(new TweenRotate(_duration, -_v, _ctrl, _type));
			}
			else
			{
				Debug.LogError("only support ETweenType.By");
				return null;
			}
		}
	}
}
