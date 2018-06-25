using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Framework
{
    public class LateTween : MonoBehaviour
	{
		GameObject _go;
		List<TweenBase> _lst = new List<TweenBase>();

		TweenBase _final;

		bool _useScaleTime = false;

		public static LateTween Create(GameObject go)
		{
            if (!go)
            {
                go = GameEngine.instance.gameObject;
            }
            LateTween t = go.AddComponent<LateTween>();
            t._go = go;
            return t;
		}
        
		/// <summary>
		/// 延时调用
		/// </summary>
		/// <param name="go"></param>
		/// <param name="func"></param>
		/// <param name="s"></param>
		/// <returns></returns>
		public static LateTween Delay(GameObject go, System.Action func, float s, bool scaled = false)
		{
			return LateTween.Create(go).UseScaleTime(scaled).Delay(s).Func(func).Do();
		}

		/// <summary>
		/// 每帧调用
		/// </summary>
		/// <param name="go"></param>
		/// <param name="func"></param>
		/// <returns></returns>
		public static LateTween Tick(GameObject go, System.Action func)
		{
			return LateTween.Create(go).Tick(func).Do();
		}

		/// <summary>
		/// 每隔interval秒调用一次func
		/// </summary>
		/// <param name="go"></param>
		/// <param name="func"></param>
		/// <param name="interval">时间间隔</param>
		/// <param name="count">总共调用次数，默认无限</param>
		/// <returns></returns>
		public static LateTween Schedule(GameObject go, System.Action func, float interval, int count = int.MaxValue)
		{
			return LateTween.Create(go).Delay(interval).Func(func).Sequence(2).Repeat(count).Do();
		}

		/// <summary>
		/// 删除所有LateTween
		/// </summary>
		/// <param name="go"></param>
		public static void RemoveAll(GameObject go)
		{
			if (go == null) return;

            LateTween [] ts = go.GetComponents<LateTween>();
            for(int i = 0, cnt = ts.Length; i < cnt; ++i)
            {
                Object.Destroy(ts[i]);
            }
		}

        public static void Stop(ref LateTween t)
        {
            if (t != null)
            {
                t.DoStop();
                t = null;
            }
        }

        public bool IsEnd()
		{
			if (_final == null) return true;
			return _final.IsEnd();
		}

		void LateUpdate()
		{
            if (_final == null) return;
            if (_final.IsEnd()) return;
            if (!_useScaleTime)
			{
				_final.Update(Time.unscaledTime);
			}
			else
			{
				_final.Update(Time.time);
			}

            if (_final.IsEnd())
            {
                Object.Destroy(this);
            }
		}

		public LateTween Do()
		{
			if (_lst.Count <= 0) return this;

			if (_lst.Count == 1)
			{
				_final = _lst[0];
			}
			else
			{
				TweenSequence seq = new TweenSequence(_lst);
				seq.go = _go;
				seq.OnCreate();
				_final = seq;
			}

            if (!_useScaleTime)
            {
                _final.OnBegin(Time.unscaledTime);
            }
            else
            {
                _final.OnBegin(Time.time);
            }

            if (_final.IsEnd())
            {
                _final = null;
            }

            _lst.Clear();
			_go = null;

			return this;
		}

		void DoStop()
		{
			if (IsEnd()) return;
			if (_final == null) return;
			if (_final.IsStop()) return;
			_final.Stop();
            Object.Destroy(this);
		}

		public LateTween UseScaleTime(bool v)
		{
			_useScaleTime = v;
			return this;
		}

		void AddTween(TweenBase t)
		{
			t.go = _go;
			t.OnCreate();
			_lst.Add(t);
		}

        /// <summary>
        /// 把前面几个LateTween串起来，一个一个执行(参考cocos）
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
		public LateTween Sequence(int n)
		{
			if (n > _lst.Count) return this;//error

			List<TweenBase> q = _lst.GetRange(_lst.Count - n, n);
			_lst = _lst.GetRange(0, _lst.Count - n);

			TweenSequence seq = new TweenSequence(q);
			AddTween(seq);

			return this;
		}

        /// <summary>
        /// 把前面几个LateTween串起来，同时执行(参考cocos）
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
		public LateTween Spawn(int n)
		{
			if (n > _lst.Count) return this;//error

			List<TweenBase> q = _lst.GetRange(_lst.Count - n, n);
			_lst = _lst.GetRange(0, _lst.Count - n);


			TweenSpawn seq = new TweenSpawn(q);
			AddTween(seq);

			return this;
		}

		//延迟多少帧
		public LateTween Frame(int frame)
		{
			AddTween(new TweenFrame(frame));
			return this;
		}

		public LateTween Tick(System.Action func)
		{
			AddTween(new TweenUpdate(func));
			return this;
		}

		public LateTween RemoveSelf()
		{
			AddTween(new TweenRemoveSelf());
			return this;
		}

		public LateTween Custom(float s, System.Action<float> doTween)
		{
			AddTween(new TweenCustom(s, doTween));
			return this;
		}

		public LateTween Func(System.Action func)
		{
			AddTween(new TweenFunc(func));
			return this;
		}

		public LateTween Speed(float speed)
		{
			if (_lst.Count < 1) return this;

			TweenBase t = _lst[_lst.Count - 1];
			t.SetSpeed(speed);

			return this;
		}

		public LateTween Reverse()
		{
			if (_lst.Count > 0)
			{
				AddTween(_lst[_lst.Count - 1].Reverse());
			}

			return this;
		}

		public LateTween Yoyo()
		{
			Reverse().Sequence(2).RepeatForever();

			return this;
		}

		public LateTween Repeat(int cnt)
		{
			if (_lst.Count < 1) return this;//error

			TweenBase t = _lst[_lst.Count - 1];
			//_lst = _lst.GetRange(0, _lst.Count - 1);
			_lst.RemoveAt(_lst.Count - 1);

			AddTween(new TweenRepeat(t, cnt));

			return this;
		}

		public LateTween RepeatForever()
		{
			return Repeat(int.MaxValue);
		}

		public LateTween MoveXFrom(float s, float v)
		{
			AddTween(new TweenMove(s, new Vector3(v, 0f, 0f), Vector3.right, ETweenType.From));
			return this;
		}

		public LateTween MoveXTo(float s, float v)
		{
			AddTween(new TweenMove(s, new Vector3(v, 0f, 0f), Vector3.right, ETweenType.To));
			return this;
		}

		public LateTween MoveXBy(float s, float v)
		{
			AddTween(new TweenMove(s, new Vector3(v, 0f, 0f), Vector3.right, ETweenType.By));
			return this;
		}

		public LateTween MoveYFrom(float s, float v)
		{
			AddTween(new TweenMove(s, new Vector3(0f, v, 0f), Vector3.up, ETweenType.From));
			return this;
		}

		public LateTween MoveYTo(float s, float v)
		{
			AddTween(new TweenMove(s, new Vector3(0f, v, 0f), Vector3.up, ETweenType.To));
			return this;
		}

		public LateTween MoveYBy(float s, float v)
		{
			AddTween(new TweenMove(s, new Vector3(0f, v, 0f), Vector3.up, ETweenType.By));
			return this;
		}

		public LateTween MoveZFrom(float s, float v)
		{
			AddTween(new TweenMove(s, new Vector3(0f, 0f, v), Vector3.forward, ETweenType.From));
			return this;
		}

		public LateTween MoveZTo(float s, float v)
		{
			AddTween(new TweenMove(s, new Vector3(0f, 0f, v), Vector3.forward, ETweenType.To));
			return this;
		}

		public LateTween MoveZBy(float s, float v)
		{
			AddTween(new TweenMove(s, new Vector3(0f, 0f, v), Vector3.forward, ETweenType.By));
			return this;
		}

		public LateTween MoveXYFrom(float s, float vx, float vy)
		{
			AddTween(new TweenMove(s, new Vector3(vx, vy, 0f), new Vector3(1f, 1f, 0f), ETweenType.From));
			return this;
		}

		public LateTween MoveXYTo(float s, float vx, float vy)
		{
			AddTween(new TweenMove(s, new Vector3(vx, vy, 0f), new Vector3(1f, 1f, 0f), ETweenType.To));
			return this;
		}

		public LateTween MoveXYBy(float s, float vx, float vy)
		{
			AddTween(new TweenMove(s, new Vector3(vx, vy, 0f), new Vector3(1f, 1f, 0f), ETweenType.By));
			return this;
		}

		public LateTween MoveXZFrom(float s, float vx, float vz)
		{
			AddTween(new TweenMove(s, new Vector3(vx, 0f, vz), new Vector3(1f, 0f, 1f), ETweenType.From));
			return this;
		}

		public LateTween MoveXZTo(float s, float vx, float vz)
		{
			AddTween(new TweenMove(s, new Vector3(vx, 0f, vz), new Vector3(1f, 0f, 1f), ETweenType.To));
			return this;
		}

		public LateTween MoveXZBy(float s, float vx, float vz)
		{
			AddTween(new TweenMove(s, new Vector3(vx, 0f, vz), new Vector3(1f, 1f, 1f), ETweenType.By));
			return this;
		}

		public LateTween MoveXYZFrom(float s, float vx, float vy, float vz)
		{
			AddTween(new TweenMove(s, new Vector3(vx, vy, vz), new Vector3(1f, 1f, 1f), ETweenType.From));
			return this;
		}

		public LateTween MoveXYZTo(float s, float vx, float vy, float vz)
		{
			AddTween(new TweenMove(s, new Vector3(vx, vy, vz), new Vector3(1f, 1f, 1f), ETweenType.To));
			return this;
		}

		public LateTween MoveXYZBy(float s, float vx, float vy, float vz)
		{
			AddTween(new TweenMove(s, new Vector3(vx, vy, vz), new Vector3(1f, 1f, 1f), ETweenType.By));
			return this;
		}


		public LateTween ScaleXFrom(float s, float v)
		{
			AddTween(new TweenScale(s, new Vector3(v, 0f, 0f), Vector3.right, ETweenType.From));
			return this;
		}

		public LateTween ScaleXTo(float s, float v)
		{
			AddTween(new TweenScale(s, new Vector3(v, 0f, 0f), Vector3.right, ETweenType.To));
			return this;
		}

		public LateTween ScaleXBy(float s, float v)
		{
			AddTween(new TweenScale(s, new Vector3(v, 0f, 0f), Vector3.right, ETweenType.By));
			return this;
		}

		public LateTween ScaleYFrom(float s, float v)
		{
			AddTween(new TweenScale(s, new Vector3(0f, v, 0f), Vector3.up, ETweenType.From));
			return this;
		}

		public LateTween ScaleYTo(float s, float v)
		{
			AddTween(new TweenScale(s, new Vector3(0f, v, 0f), Vector3.up, ETweenType.To));
			return this;
		}

		public LateTween ScaleYBy(float s, float v)
		{
			AddTween(new TweenScale(s, new Vector3(0f, v, 0f), Vector3.up, ETweenType.By));
			return this;
		}

		public LateTween ScaleZFrom(float s, float v)
		{
			AddTween(new TweenScale(s, new Vector3(0f, 0f, v), Vector3.forward, ETweenType.From));
			return this;
		}

		public LateTween ScaleZTo(float s, float v)
		{
			AddTween(new TweenScale(s, new Vector3(0f, 0f, v), Vector3.forward, ETweenType.To));
			return this;
		}

		public LateTween ScaleZBy(float s, float v)
		{
			AddTween(new TweenScale(s, new Vector3(0f, 0f, v), Vector3.forward, ETweenType.By));
			return this;
		}

		public LateTween ScaleXYFrom(float s, float vx, float vy)
		{
			AddTween(new TweenScale(s, new Vector3(vx, vy, 0f), new Vector3(1f, 1f, 0f), ETweenType.From));
			return this;
		}

		public LateTween ScaleXYTo(float s, float vx, float vy)
		{
			AddTween(new TweenScale(s, new Vector3(vx, vy, 0f), new Vector3(1f, 1f, 0f), ETweenType.To));
			return this;
		}

		public LateTween ScaleXYBy(float s, float vx, float vy)
		{
			AddTween(new TweenScale(s, new Vector3(vx, vy, 0f), new Vector3(1f, 1f, 0f), ETweenType.By));
			return this;
		}

		public LateTween ScaleXZFrom(float s, float vx, float vz)
		{
			AddTween(new TweenScale(s, new Vector3(vx, 0f, vz), new Vector3(1f, 0f, 1f), ETweenType.From));
			return this;
		}

		public LateTween ScaleXZTo(float s, float vx, float vz)
		{
			AddTween(new TweenScale(s, new Vector3(vx, 0f, vz), new Vector3(1f, 0f, 1f), ETweenType.To));
			return this;
		}

		public LateTween ScaleXZBy(float s, float vx, float vz)
		{
			AddTween(new TweenScale(s, new Vector3(vx, 0f, vz), new Vector3(1f, 1f, 1f), ETweenType.By));
			return this;
		}

		public LateTween ScaleXYZFrom(float s, float vx, float vy, float vz)
		{
			AddTween(new TweenScale(s, new Vector3(vx, vy, vz), new Vector3(1f, 1f, 1f), ETweenType.From));
			return this;
		}

		public LateTween ScaleXYZTo(float s, float vx, float vy, float vz)
		{
			AddTween(new TweenScale(s, new Vector3(vx, vy, vz), new Vector3(1f, 1f, 1f), ETweenType.To));
			return this;
		}

		public LateTween ScaleXYZBy(float s, float vx, float vy, float vz)
		{
			AddTween(new TweenScale(s, new Vector3(vx, vy, vz), new Vector3(1f, 1f, 1f), ETweenType.By));
			return this;
		}


		public LateTween RotateXFrom(float s, float v)
		{
			AddTween(new TweenRotate(s, new Vector3(v, 0f, 0f), Vector3.right, ETweenType.From));
			return this;
		}

		public LateTween RotateXTo(float s, float v)
		{
			AddTween(new TweenRotate(s, new Vector3(v, 0f, 0f), Vector3.right, ETweenType.To));
			return this;
		}

		public LateTween RotateXBy(float s, float v)
		{
			AddTween(new TweenRotate(s, new Vector3(v, 0f, 0f), Vector3.right, ETweenType.By));
			return this;
		}

		public LateTween RotateYFrom(float s, float v)
		{
			AddTween(new TweenRotate(s, new Vector3(0f, v, 0f), Vector3.up, ETweenType.From));
			return this;
		}

		public LateTween RotateYTo(float s, float v)
		{
			AddTween(new TweenRotate(s, new Vector3(0f, v, 0f), Vector3.up, ETweenType.To));
			return this;
		}

		public LateTween RotateYBy(float s, float v)
		{
			AddTween(new TweenRotate(s, new Vector3(0f, v, 0f), Vector3.up, ETweenType.By));
			return this;
		}

		public LateTween RotateZFrom(float s, float v)
		{
			AddTween(new TweenRotate(s, new Vector3(0f, 0f, v), Vector3.forward, ETweenType.From));
			return this;
		}

		public LateTween RotateZTo(float s, float v)
		{
			AddTween(new TweenRotate(s, new Vector3(0f, 0f, v), Vector3.forward, ETweenType.To));
			return this;
		}

		public LateTween RotateZBy(float s, float v)
		{
			AddTween(new TweenRotate(s, new Vector3(0f, 0f, v), Vector3.forward, ETweenType.By));
			return this;
		}

		public LateTween RotateFrom(float s, float angle, Vector3 axis)
		{
			AddTween(new TweenRoateAxis(s, angle, axis, ETweenType.From));
			return this;
		}
		public LateTween RotateTo(float s, float angle, Vector3 axis)
		{
			AddTween(new TweenRoateAxis(s, angle, axis, ETweenType.To));
			return this;
		}
		public LateTween RotateBy(float s, float angle, Vector3 axis)
		{
			AddTween(new TweenRoateAxis(s, angle, axis, ETweenType.By));
			return this;
		}

		public LateTween Delay(float s)
		{
			AddTween(new TweenDelay(s));
			return this;
		}

		public LateTween Ease(System.Func<float, float> ease)
		{
			if (_lst.Count < 1) return this;

			TweenBase t = _lst[_lst.Count - 1];
			t.SetEase(ease);

			return this;
		}

		public LateTween Ease(System.Func<float, float, float> ease, float v)
		{
			System.Func<float, float> e = (time) =>
			{
				return ease(time, v);
			};

			return Ease(e);
		}

	}
}
