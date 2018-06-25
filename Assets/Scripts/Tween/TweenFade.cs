using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace Framework
{
	public class TweenFade : TweenInterval
	{
        public TweenFade(float s, float v, ETweenType type)
            : base(s)
        {

        }
        public TweenFade(float s, float from, float to) : base(s)
        {

        }

        //float _v;
        //ETweenType _type;
        //float _dest;
        //float _begin;

        ////GControl _g;
        //CanvasGroup _cg;
        //Graphic _g;

        //public TweenFade(float s, float v, ETweenType type)
        //	: base(s)
        //{
        //	_v = v;
        //	_type = type;
        //}
        //public TweenFade(float s, float from, float to):base(s)
        //{
        //	_begin = from;
        //	_dest = to;
        //	_type = ETweenType.From_To;
        //}

        //float alpha
        //{
        //	get
        //	{
        //		if(_cg != null)
        //		{
        //			return _cg.alpha;
        //		}
        //		else
        //		{
        //			return _g.color.a;
        //		}
        //	}
        //	set
        //	{
        //		if (_cg != null)
        //		{
        //			_cg.alpha = value; 
        //		}
        //		else
        //		{
        //			Color c = _g.color;
        //			c.a = value;
        //			_g.color = c;
        //		}
        //	}
        //}

        //override public void OnCreate()
        //{
        //          _cg = Shell.CompUtil.GetComponent_CanvasGroup(go);
        //          if (_cg == null)
        //          {
        //              _g = Shell.CompUtil.GetComponent_Graphic(go);
        //              if (_g == null)
        //              {
        //                  _cg = Shell.CompUtil.AddComponent_CanvasGroup(go);
        //              }
        //          }
        //      }


        //override public void DoTween(float per)
        //{
        //          float newV = Util.Lerp(_begin, _dest, per);

        //          alpha = newV;
        //      }

        //override public void OnBegin(float time)
        //{
        //	if (_type == ETweenType.By)
        //	{
        //		_begin = alpha;
        //		_dest = _begin + _v;
        //	}
        //	else if (_type == ETweenType.From)
        //	{
        //		_begin = _v;
        //		_dest = alpha;
        //	}
        //	else if (_type == ETweenType.To)
        //	{
        //		_begin = alpha;
        //		_dest = _v;
        //	}		
        //	DoTween(0);

        //	base.OnBegin(time);
        //}

        //override public TweenBase Reverse()
        //{
        //	if (_type == ETweenType.By)
        //	{
        //		return CreateTween(new TweenFade(_duration, -_v, _type));
        //	}
        //	else
        //	{
        //		Debug.LogError("only support ETweenType.By");
        //		return null;
        //	}
        //}
    }

}