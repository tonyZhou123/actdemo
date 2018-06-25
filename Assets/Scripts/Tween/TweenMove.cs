using UnityEngine;

namespace Framework
{
	public class TweenMove : TweenInterval
	{
		Vector3 _v;
		Vector3 _ctrl;
		ETweenType _type;

		Vector3 _dest;
		Vector3 _begin;

		Transform _transform;
		RectTransform _rt;

		public TweenMove(float s, Vector3 v, Vector3 ctrl, ETweenType type)
			: base(s)
		{
			_v = v;
			_ctrl = ctrl;
			_type = type;
		}

		override public void OnCreate()
		{
			_transform = go.transform;
			if (_transform is RectTransform)
			{
				_rt = _transform as RectTransform;
			}
		}


		override public void DoTween(float per)
		{
			if (_rt != null)
			{
				Vector2 p = _rt.anchoredPosition;
				if (_ctrl.x != 0)
				{
					p.x = Mathf.LerpUnclamped(_begin.x, _dest.x, per);
				}

				if (_ctrl.y != 0)
				{
					p.y = Mathf.LerpUnclamped(_begin.y, _dest.y, per);
				}

				_rt.anchoredPosition = p;
			}
			else
			{
				Vector3 p = _transform.localPosition;
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

				_transform.localPosition = p;
			}
		}

		override public void OnBegin(float time)
		{
			if (_type == ETweenType.By)
			{
				if (_rt != null)
				{
					_begin = _rt.anchoredPosition;
				}
				else
				{
					_begin = _transform.localPosition;
				}
				_dest = _begin + _v;
			}
			else if (_type == ETweenType.From)
			{
				_begin = _v;

				if (_rt != null)
				{
					_dest = _rt.anchoredPosition;
				}
				else
				{
					_dest = _transform.localPosition;
				}
			}
			else
			{
				if (_rt != null)
				{
					_begin = _rt.anchoredPosition;
				}
				else
				{
					_begin = _transform.localPosition;
				}
				_dest = _v;
			}

			DoTween(0);

			base.OnBegin(time);
		}

		override public TweenBase Reverse()
		{
			if (_type == ETweenType.By)
			{
				return CreateTween(new TweenMove(_duration, -_v, _ctrl, _type));
			}
			else
			{
				Debug.LogError("only support ETweenType.By");
				return null;
			}
		}
	}
}
