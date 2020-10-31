using System;
using System.Collections;
using toio;
using UnityEngine;

namespace BMProject
{
	public class ToioControllerAutoSimpleTarget : ToioController
	{
		private Coroutine execute;

		public int moveSpeed = 40;
		public int moveMsec= 1500;

		public int rotateSpeed = 65;
		public int rotateTime = 230;

		protected override void OnEnableInput(CubeManager mgr, Cube c)
		{
			base.OnEnableInput(mgr, c);
			this.execute = this.StartCoroutine(Control(mgr, c));
		}

		protected override void OnDisableInput()
		{
			base.OnDisableInput();
			StopCoroutine(this.execute);
		}

		IEnumerator Control(CubeManager cMgr, Cube c)
		{
			while (true)
			{
				c.Move(moveSpeed, moveSpeed, moveMsec, Cube.ORDER_TYPE.Strong);
				yield return new WaitForSeconds(moveMsec * 0.001f + 0.1f);
				c.Move(-rotateSpeed, rotateSpeed, rotateTime, Cube.ORDER_TYPE.Strong);
				yield return new WaitForSeconds(rotateTime * 0.001f + 0.3f);
			}
		}

	}
}
