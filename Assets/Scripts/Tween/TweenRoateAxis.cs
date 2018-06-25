using UnityEngine;

namespace Framework
{
	public class TweenRoateAxis : TweenInterval
	{
		float _v;
		ETweenType _type;

		Quaternion _dest;
		Quaternion _begin;

		Transform _transform;
		Vector3 _axis;
		public TweenRoateAxis(float s, float v,Vector3 axis, ETweenType type)
			: base(s)
		{
			_v = v;
			_type = type;
			_axis = axis;
        }

		override public void OnCreate()
		{
			_transform = go.transform;
		}


		override public void DoTween(float per)
		{
			Quaternion newV = Quaternion.LerpUnclamped(_begin, _dest, per);

			_transform.rotation = newV;
		}

		override public void OnBegin(float time)
		{
			if (_type == ETweenType.By)
			{
				_begin = _transform.rotation;
				_transform.Rotate(_axis, _v);
				_dest = _transform.rotation;
				_transform.rotation = _begin;
			}
			else if (_type == ETweenType.From)
			{
				_begin = Quaternion.AngleAxis(_v, _axis);
				_dest = _transform.rotation;
			}
			else
			{
				_begin = _transform.rotation;
				_dest = Quaternion.AngleAxis(_v, _axis);
			}

			DoTween(0);

			base.OnBegin(time);
		}

		override public TweenBase Reverse()
		{
			if (_type == ETweenType.By)
			{
				return CreateTween(new TweenRoateAxis(_duration, -_v, _axis, _type));
			}
			else
			{
				Debug.LogError("only support ETweenType.By");
				return null;
			}
		}
	}
}
