using System;
using toio;
using UnityEngine;
using UnityScript.Steps;

namespace BMProject
{
	public class ToioControllerSimple : ToioController
	{
		public RectTransform leftStick;
		private Touch leftTouch;
		private bool isLeftTouching;
		protected override void UpdateCube(CubeManager cMgr, Cube c)
		{
			base.UpdateCube(cMgr, c);
			Vector2 control;
			if (Application.isMobilePlatform) {
				this.UpdateInputMobile(out control);
			}
			else
			{
				this.UpdateInputPC(out control);
			}
			this.UpdateUI(control);
			if (cMgr != null && cMgr.IsControllable(c) && CubeOrderBalancer.Instance.IsIdle(c))
			{
				int tl, tr;
				GetToioMotor(control, out tl, out tr);
				c.Move(tl,tr, 0, Cube.ORDER_TYPE.Weak);
			}
		}

		private void GetToioMotor(Vector2 param,out int l,out int r)
		{
			float length = param.magnitude;
			param.Normalize();
			float leftF,rightF;

			leftF = param.y + ConvertParam(param.x);
			rightF = param.y - ConvertParam(param.x);

			l = (int)(leftF * 60.0f * length);
			r = (int)(rightF * 60.0f * length); ;
		}

		private static float ConvertParam(float param)
		{
			if( param < 0.0f)
			{
				return -(param * param);
			}
			return param * param;
		}

		private void UpdateUI(Vector2 param)
		{
			this.leftStick.anchoredPosition = param * 60;
		}
		private void UpdateInputMobile(out Vector2 control)
		{
			control = Vector2.zero;
			for (int i = 0; i < Input.touchCount; i++)
			{
				Touch touch = Input.GetTouch(i);
				if (touch.position.x < (float)Screen.width * 0.5f)
				{
					this.UpdateTouch(touch, ref this.leftTouch, ref this.isLeftTouching, out control);
				}
			}
			if (Input.touchCount == 0)
			{
				this.isLeftTouching = false;
			}
		}
		private bool UpdateTouch(Touch current, ref Touch baseTouch, ref bool isTouching, out Vector2 param)
		{
			param = Vector2.zero;
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
				param = vector / Screen.dpi * 3f;
				if (param.sqrMagnitude > 1.0f)
				{
					param.Normalize();
				}
				return true;
			}
			return false;
		}
		private void UpdateInputPC(out Vector2 control)
		{
			control.x = Input.GetAxis("Horizontal");
			control.y = Input.GetAxis("Vertical");
			if (control.sqrMagnitude > 1.0f)
			{
				control.Normalize();
			}
		}
	}
}
