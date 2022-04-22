using System;
using System.Collections;
using toio;
using UnityEngine;

namespace BMProject
{
	public class ToioControllerAutoSimpleTarget : ToioController
	{
		private Coroutine execute;

		[SerializeField]
		private int moveSpeed = 40;
		[SerializeField]
		private int moveMsec= 1500;

		[SerializeField]
		private int rotateSpeed = 65;
		[SerializeField]
		private int rotateTime = 230;

		protected override void OnEnableInput(CubeManager mgr, Cube c)
		{
			base.OnEnableInput(mgr, c);
			this.execute = this.StartCoroutine(Control(mgr,c));
		}

		protected override void OnDisableInput()
		{
			base.OnDisableInput();
			StopCoroutine(this.execute);
			this.SendMoveCmdCube(0, 0,100);
		}

		IEnumerator Control(CubeManager mgr, Cube c)
		{
			while (true)
			{
				this.SendMoveCmdCube(moveSpeed, moveSpeed, moveMsec);
				yield return null;
				this.SendMoveCmdCube(moveSpeed, moveSpeed, moveMsec);
				yield return new WaitForSeconds(moveMsec * 0.001f + 0.1f);
				this.SendMoveCmdCube(-rotateSpeed, rotateSpeed, rotateTime);
				yield return new WaitForSeconds(rotateTime * 0.001f + 0.3f);
			}
		}

	}
}
