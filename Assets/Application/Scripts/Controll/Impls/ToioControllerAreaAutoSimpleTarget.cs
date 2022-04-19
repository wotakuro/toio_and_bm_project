using System;
using System.Collections;
using toio;
using UnityEngine;

namespace BMProject
{
	public class ToioControllerAreaAutoSimpleTarget : ToioController
	{
		private Coroutine execute;
		[SerializeField]
		private Vector2Int leftUpper = new Vector2Int(46,46);
		[SerializeField]
		private Vector2Int rightDowner = new Vector2Int(312,239);
		[SerializeField]
		private float rotateTime = 1.0f;

		[SerializeField]
		private float moveDistance = 100;
		[SerializeField]
		private float moveDistanceRandom = 0;


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
			}
		}

	}
}
