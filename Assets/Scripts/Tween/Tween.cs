using UnityEngine;
using System.Collections.Generic;

namespace Framework
{
    //Tween.Create(cube).MoveXTo(3, 1).Do();
    //Tween.Create(cube).MoveYTo(3, 1).Do();
    //Tween.Create(cube).MoveXTo(3, 1).MoveYTo(3, 1).DelayTime(1).MoveXTo(3, 2).Do();
    //Tween.Create(cube).MoveXTo(1, 3).Ease(EaseFunc.BackEaseOut).MoveYBy(1, 1).Spawn(2).Show(false).DelayTime(2).Show(true).Do();
    //Tween.Create(cube).MoveXTo(1, 3).Ease(EaseFunc.BackEaseOut).Do();
    //Tween.Create(cube).MoveXTo(1, 3).Ease(EaseFunc.EaseIn, 2).MoveXFrom(1, 0).Do();
    //Tween.Create(cube).MoveXBy(1, 1).Speed(2f).Ease(EaseFunc.EaseIn, 2).Func(() => { Debug.Log(" func  in tween"); }).Sequence(2).Repeat(2).Do();
    //Tween.Create(cube).MoveXBy(1, 1).Reverse().Sequence(2).Speed(0.1f).RepeatForever().Do();
    //Tween.Create(cube).MoveXBy(1, 1).Repeat(2).Yoyo().Do();

    public enum ETweenType
    {
        From = 1,
        To,
        By,
        From_To
    }

    public class Tween : MonoBehaviour
	{
		GameObject _go;
		List<TweenBase> _lst = new List<TweenBase>();

		TweenBase _final;

		bool _useScaleTime = false;

		public static Tween Create(GameObject go)
		{
            if (!go)
            {
                go = GameEngine.instance.gameObject;
            }
            Tween t = go.AddComponent<Tween>();
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
		public static Tween Delay(GameObject go, System.Action func, float s, bool scaled = false)
		{
			return Tween.Create(go).UseScaleTime(scaled).Delay(s).Func(func).Do();
		}

		/// <summary>
		/// 每帧调用
		/// </summary>
		/// <param name="go"></param>
		/// <param name="func"></param>
		/// <returns></returns>
		public static Tween Tick(GameObject go, System.Action func)
		{
			return Tween.Create(go).Tick(func).Do();
		}

		/// <summary>
		/// 每隔interval秒调用一次func
		/// </summary>
		/// <param name="go"></param>
		/// <param name="func"></param>
		/// <param name="interval">时间间隔</param>
		/// <param name="count">总共调用次数，默认无限</param>
		/// <returns></returns>
		public static Tween Schedule(GameObject go, System.Action func, float interval, int count = int.MaxValue)
		{
			return Tween.Create(go).Delay(interval).Func(func).Sequence(2).Repeat(count).Do();
		}

		/// <summary>
		/// 删除所有Tween
		/// </summary>
		/// <param name="go"></param>
		public static void RemoveAll(GameObject go)
		{
			if (go == null) return;

            Tween [] ts = go.GetComponents<Tween>();
            for(int i = 0, cnt = ts.Length; i < cnt; ++i)
            {
                Object.Destroy(ts[i]);
            }
		}

        public static void Stop(ref Tween t)
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

		void Update()
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

		public Tween Do()
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
                if (Application.isPlaying) Object.Destroy(this);
                else Object.DestroyImmediate(this);
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

		public Tween UseScaleTime(bool v)
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
        /// 把前面几个Tween串起来，一个一个执行(参考cocos）
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
		public Tween Sequence(int n)
		{
			if (n > _lst.Count) return this;//error

			List<TweenBase> q = _lst.GetRange(_lst.Count - n, n);
			_lst = _lst.GetRange(0, _lst.Count - n);

			TweenSequence seq = new TweenSequence(q);
			AddTween(seq);

			return this;
		}

        /// <summary>
        /// 把前面几个Tween串起来，同时执行(参考cocos）
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
		public Tween Spawn(int n)
		{
			if (n > _lst.Count) return this;//error

			List<TweenBase> q = _lst.GetRange(_lst.Count - n, n);
			_lst = _lst.GetRange(0, _lst.Count - n);


			TweenSpawn seq = new TweenSpawn(q);
			AddTween(seq);

			return this;
		}

		//延迟多少帧
		public Tween Frame(int frame)
		{
			AddTween(new TweenFrame(frame));
			return this;
		}

		public Tween Tick(System.Action func)
		{
			AddTween(new TweenUpdate(func));
			return this;
		}

		public Tween RemoveSelf()
		{
			AddTween(new TweenRemoveSelf());
			return this;
		}

		public Tween Custom(float s, System.Action<float> doTween)
		{
			AddTween(new TweenCustom(s, doTween));
			return this;
		}


        public Tween Func(System.Action func)
		{
			AddTween(new TweenFunc(func));
			return this;
		}

		public Tween Speed(float speed)
		{
			if (_lst.Count < 1) return this;

			TweenBase t = _lst[_lst.Count - 1];
			t.SetSpeed(speed);

			return this;
		}

		public Tween Reverse()
		{
			if (_lst.Count > 0)
			{
				AddTween(_lst[_lst.Count - 1].Reverse());
			}

			return this;
		}

		public Tween Yoyo()
		{
			Reverse().Sequence(2).RepeatForever();

			return this;
		}

		public Tween Repeat(int cnt)
		{
			if (_lst.Count < 1) return this;//error

			TweenBase t = _lst[_lst.Count - 1];
			//_lst = _lst.GetRange(0, _lst.Count - 1);
			_lst.RemoveAt(_lst.Count - 1);

			AddTween(new TweenRepeat(t, cnt));

			return this;
		}

		public Tween RepeatForever()
		{
			return Repeat(int.MaxValue);
		}

		public Tween MoveXFrom(float s, float v)
		{
			AddTween(new TweenMove(s, new Vector3(v, 0f, 0f), Vector3.right, ETweenType.From));
			return this;
		}

		public Tween MoveXTo(float s, float v)
		{
			AddTween(new TweenMove(s, new Vector3(v, 0f, 0f), Vector3.right, ETweenType.To));
			return this;
		}

		public Tween MoveXBy(float s, float v)
		{
			AddTween(new TweenMove(s, new Vector3(v, 0f, 0f), Vector3.right, ETweenType.By));
			return this;
		}

		public Tween MoveYFrom(float s, float v)
		{
			AddTween(new TweenMove(s, new Vector3(0f, v, 0f), Vector3.up, ETweenType.From));
			return this;
		}

		public Tween MoveYTo(float s, float v)
		{
			AddTween(new TweenMove(s, new Vector3(0f, v, 0f), Vector3.up, ETweenType.To));
			return this;
		}

		public Tween MoveYBy(float s, float v)
		{
			AddTween(new TweenMove(s, new Vector3(0f, v, 0f), Vector3.up, ETweenType.By));
			return this;
		}

		public Tween MoveZFrom(float s, float v)
		{
			AddTween(new TweenMove(s, new Vector3(0f, 0f, v), Vector3.forward, ETweenType.From));
			return this;
		}

		public Tween MoveZTo(float s, float v)
		{
			AddTween(new TweenMove(s, new Vector3(0f, 0f, v), Vector3.forward, ETweenType.To));
			return this;
		}

		public Tween MoveZBy(float s, float v)
		{
			AddTween(new TweenMove(s, new Vector3(0f, 0f, v), Vector3.forward, ETweenType.By));
			return this;
		}

		public Tween MoveXYFrom(float s, float vx, float vy)
		{
			AddTween(new TweenMove(s, new Vector3(vx, vy, 0f), new Vector3(1f, 1f, 0f), ETweenType.From));
			return this;
		}

		public Tween MoveXYTo(float s, float vx, float vy)
		{
			AddTween(new TweenMove(s, new Vector3(vx, vy, 0f), new Vector3(1f, 1f, 0f), ETweenType.To));
			return this;
		}

		public Tween MoveXYBy(float s, float vx, float vy)
		{
			AddTween(new TweenMove(s, new Vector3(vx, vy, 0f), new Vector3(1f, 1f, 0f), ETweenType.By));
			return this;
		}

		public Tween MoveXZFrom(float s, float vx, float vz)
		{
			AddTween(new TweenMove(s, new Vector3(vx, 0f, vz), new Vector3(1f, 0f, 1f), ETweenType.From));
			return this;
		}

		public Tween MoveXZTo(float s, float vx, float vz)
		{
			AddTween(new TweenMove(s, new Vector3(vx, 0f, vz), new Vector3(1f, 0f, 1f), ETweenType.To));
			return this;
		}

		public Tween MoveXZBy(float s, float vx, float vz)
		{
			AddTween(new TweenMove(s, new Vector3(vx, 0f, vz), new Vector3(1f, 1f, 1f), ETweenType.By));
			return this;
		}

		public Tween MoveXYZFrom(float s, float vx, float vy, float vz)
		{
			AddTween(new TweenMove(s, new Vector3(vx, vy, vz), new Vector3(1f, 1f, 1f), ETweenType.From));
			return this;
		}

		public Tween MoveXYZTo(float s, float vx, float vy, float vz)
		{
			AddTween(new TweenMove(s, new Vector3(vx, vy, vz), new Vector3(1f, 1f, 1f), ETweenType.To));
			return this;
		}

		public Tween MoveXYZBy(float s, float vx, float vy, float vz)
		{
			AddTween(new TweenMove(s, new Vector3(vx, vy, vz), new Vector3(1f, 1f, 1f), ETweenType.By));
			return this;
		}


		public Tween ScaleXFrom(float s, float v)
		{
			AddTween(new TweenScale(s, new Vector3(v, 0f, 0f), Vector3.right, ETweenType.From));
			return this;
		}

		public Tween ScaleXTo(float s, float v)
		{
			AddTween(new TweenScale(s, new Vector3(v, 0f, 0f), Vector3.right, ETweenType.To));
			return this;
		}

		public Tween ScaleXBy(float s, float v)
		{
			AddTween(new TweenScale(s, new Vector3(v, 0f, 0f), Vector3.right, ETweenType.By));
			return this;
		}

		public Tween ScaleYFrom(float s, float v)
		{
			AddTween(new TweenScale(s, new Vector3(0f, v, 0f), Vector3.up, ETweenType.From));
			return this;
		}

		public Tween ScaleYTo(float s, float v)
		{
			AddTween(new TweenScale(s, new Vector3(0f, v, 0f), Vector3.up, ETweenType.To));
			return this;
		}

		public Tween ScaleYBy(float s, float v)
		{
			AddTween(new TweenScale(s, new Vector3(0f, v, 0f), Vector3.up, ETweenType.By));
			return this;
		}

		public Tween ScaleZFrom(float s, float v)
		{
			AddTween(new TweenScale(s, new Vector3(0f, 0f, v), Vector3.forward, ETweenType.From));
			return this;
		}

		public Tween ScaleZTo(float s, float v)
		{
			AddTween(new TweenScale(s, new Vector3(0f, 0f, v), Vector3.forward, ETweenType.To));
			return this;
		}

		public Tween ScaleZBy(float s, float v)
		{
			AddTween(new TweenScale(s, new Vector3(0f, 0f, v), Vector3.forward, ETweenType.By));
			return this;
		}

		public Tween ScaleXYFrom(float s, float vx, float vy)
		{
			AddTween(new TweenScale(s, new Vector3(vx, vy, 0f), new Vector3(1f, 1f, 0f), ETweenType.From));
			return this;
		}

		public Tween ScaleXYTo(float s, float vx, float vy)
		{
			AddTween(new TweenScale(s, new Vector3(vx, vy, 0f), new Vector3(1f, 1f, 0f), ETweenType.To));
			return this;
		}

		public Tween ScaleXYBy(float s, float vx, float vy)
		{
			AddTween(new TweenScale(s, new Vector3(vx, vy, 0f), new Vector3(1f, 1f, 0f), ETweenType.By));
			return this;
		}

		public Tween ScaleXZFrom(float s, float vx, float vz)
		{
			AddTween(new TweenScale(s, new Vector3(vx, 0f, vz), new Vector3(1f, 0f, 1f), ETweenType.From));
			return this;
		}

		public Tween ScaleXZTo(float s, float vx, float vz)
		{
			AddTween(new TweenScale(s, new Vector3(vx, 0f, vz), new Vector3(1f, 0f, 1f), ETweenType.To));
			return this;
		}

		public Tween ScaleXZBy(float s, float vx, float vz)
		{
			AddTween(new TweenScale(s, new Vector3(vx, 0f, vz), new Vector3(1f, 1f, 1f), ETweenType.By));
			return this;
		}

		public Tween ScaleXYZFrom(float s, float vx, float vy, float vz)
		{
			AddTween(new TweenScale(s, new Vector3(vx, vy, vz), new Vector3(1f, 1f, 1f), ETweenType.From));
			return this;
		}

		public Tween ScaleXYZTo(float s, float vx, float vy, float vz)
		{
			AddTween(new TweenScale(s, new Vector3(vx, vy, vz), new Vector3(1f, 1f, 1f), ETweenType.To));
			return this;
		}

		public Tween ScaleXYZBy(float s, float vx, float vy, float vz)
		{
			AddTween(new TweenScale(s, new Vector3(vx, vy, vz), new Vector3(1f, 1f, 1f), ETweenType.By));
			return this;
		}


		public Tween RotateXFrom(float s, float v)
		{
			AddTween(new TweenRotate(s, new Vector3(v, 0f, 0f), Vector3.right, ETweenType.From));
			return this;
		}

		public Tween RotateXTo(float s, float v)
		{
			AddTween(new TweenRotate(s, new Vector3(v, 0f, 0f), Vector3.right, ETweenType.To));
			return this;
		}

		public Tween RotateXBy(float s, float v)
		{
			AddTween(new TweenRotate(s, new Vector3(v, 0f, 0f), Vector3.right, ETweenType.By));
			return this;
		}

		public Tween RotateYFrom(float s, float v)
		{
			AddTween(new TweenRotate(s, new Vector3(0f, v, 0f), Vector3.up, ETweenType.From));
			return this;
		}

		public Tween RotateYTo(float s, float v)
		{
			AddTween(new TweenRotate(s, new Vector3(0f, v, 0f), Vector3.up, ETweenType.To));
			return this;
		}

		public Tween RotateYBy(float s, float v)
		{
			AddTween(new TweenRotate(s, new Vector3(0f, v, 0f), Vector3.up, ETweenType.By));
			return this;
		}

		public Tween RotateZFrom(float s, float v)
		{
			AddTween(new TweenRotate(s, new Vector3(0f, 0f, v), Vector3.forward, ETweenType.From));
			return this;
		}

		public Tween RotateZTo(float s, float v)
		{
			AddTween(new TweenRotate(s, new Vector3(0f, 0f, v), Vector3.forward, ETweenType.To));
			return this;
		}

		public Tween RotateZBy(float s, float v)
		{
			AddTween(new TweenRotate(s, new Vector3(0f, 0f, v), Vector3.forward, ETweenType.By));
			return this;
		}

		public Tween RotateFrom(float s, float angle, Vector3 axis)
		{
			AddTween(new TweenRoateAxis(s, angle, axis, ETweenType.From));
			return this;
		}
		public Tween RotateTo(float s, float angle, Vector3 axis)
		{
			AddTween(new TweenRoateAxis(s, angle, axis, ETweenType.To));
			return this;
		}
		public Tween RotateBy(float s, float angle, Vector3 axis)
		{
			AddTween(new TweenRoateAxis(s, angle, axis, ETweenType.By));
			return this;
		}

		public Tween Delay(float s)
		{
			AddTween(new TweenDelay(s));
			return this;
		}

		public Tween Ease(System.Func<float, float> ease)
		{
			if (_lst.Count < 1) return this;

			TweenBase t = _lst[_lst.Count - 1];
			t.SetEase(ease);

			return this;
		}

		public Tween Ease(System.Func<float, float, float> ease, float v)
		{
			System.Func<float, float> e = (time) =>
			{
				return ease(time, v);
			};

			return Ease(e);
		}

	}
}
