using UnityEngine;

namespace Framework
{
	public class TweenRemoveSelf : TweenInstant
	{

		public TweenRemoveSelf()
		{

		}


		override public void OnBegin(float time)
		{
			base.OnBegin(time);
            go.SetActive(false);
            Object.Destroy(go);
		}

	} 
}
