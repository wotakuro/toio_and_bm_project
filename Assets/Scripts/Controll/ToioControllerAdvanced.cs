using System;
using toio;
using UnityEngine;
namespace BMProject
{
	public class ToioControllerAdvanced : ToioController
	{
		public RectTransform leftStick;
		public RectTransform rightStick;
		private Touch leftTouch;
		private Touch rightTouch;
		private bool isLeftTouching;
		private bool isRightTouching;
		protected override void UpdateCube(CubeManager cMgr, Cube c)
		{
			base.UpdateCube(cMgr, c);
			float num = 0f;
			float num2 = 0f;
			if (Application.isMobilePlatform)
			{
				this.UpdateInputMobile(out num, out num2);
			}
			else
			{
				UpdateInputPC(out num, out num2);
			}
			this.UpdateUI(num, num2);
			if (cMgr != null && cMgr.IsControllable(c) && CubeOrderBalancer.Instance.IsIdle(c))
			{
				c.Move((int)(num * 60f), (int)(num2 * 60f), 0, Cube.ORDER_TYPE.Weak);
			}
		}
		private void UpdateUI(float l, float r)
		{
			this.leftStick.anchoredPosition = new Vector2(0f, l * 50f);
			this.rightStick.anchoredPosition = new Vector2(0f, r * 50f);
		}
		private void UpdateInputMobile(out float l, out float r)
		{
			l = 0f;
			r = 0f;
			for (int i = 0; i < Input.touchCount; i++)
			{
				Touch touch = Input.GetTouch(i);
				if (touch.position.x < (float)Screen.width * 0.5f)
				{
					this.UpdateTouch(touch, ref this.leftTouch, ref this.isLeftTouching, out l);
				}
				else
				{
					this.UpdateTouch(touch, ref this.rightTouch, ref this.isRightTouching, out r);
				}
			}
			if (Input.touchCount == 0)
			{
				this.isLeftTouching = false;
				this.isRightTouching = false;
			}
		}
		private bool UpdateTouch(Touch current, ref Touch baseTouch, ref bool isTouching, out float param)
		{
			param = 0f;
			if (current.phase == TouchPhase.Began)
			{
				if (!isTouching)
				{
					isTouching = true;
					baseTouch = current;
					return true;
				}
			}
			else
			{
				if ((current.phase == TouchPhase.Ended || current.phase == TouchPhase.Canceled) && current.fingerId == baseTouch.fingerId)
				{
					isTouching = false;
					return true;
				}
			}
			if (current.fingerId == baseTouch.fingerId)
			{
				Vector2 vector = current.position - baseTouch.position;
				param = vector.y / Screen.dpi * 3f;
				param = Mathf.Clamp(param, -1f, 1f);
				return true;
			}
			param = 0f;
			return false;
		}
		private void UpdateInputPC(out float l, out float r)
		{
			l = Input.GetAxis("Vertical");
			r = Input.GetAxis("Vertical2");
		}
	}
}
