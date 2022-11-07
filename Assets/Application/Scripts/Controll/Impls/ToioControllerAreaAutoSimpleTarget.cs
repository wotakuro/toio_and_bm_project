using System.Collections;
using toio;
using UnityEngine;

namespace BMProject
{
	public class ToioControllerAreaAutoSimpleTarget : ToioController
	{
		private Vector2Int areaRightFront = new Vector2Int(46,46);
		private Vector2Int areaLeftBack = new Vector2Int(312,239);
		[SerializeField]
		private int rotateTime = 230;
		[SerializeField]
		private int rotateSpeed = 65;


		[SerializeField]
		private int moveSpeed = 50;
		[SerializeField]
		private float moveDistance = 100;
		[SerializeField]
		private float moveDistanceRandom = 0;


		private Coroutine execute;
	    private ToioGroundAdjuster groundAdjuster;


		protected override void OnEnableInput(CubeManager mgr, Cube c)
		{
			base.OnEnableInput(mgr, c);
			this.execute = this.StartCoroutine(Control(mgr,c));

			this.areaRightFront = GlobalGameConfig.currentConfig.areaRightFront;
			this.areaLeftBack = GlobalGameConfig.currentConfig.areaLeftBack;
		}

		protected override void OnDisableInput()
		{
			base.OnDisableInput();
			StopCoroutine(this.execute);
			this.SendMoveCmdCube(0, 0,100);
			StartCoroutine(ToThePosition(1.0f));
		}
		// 仮対応（もとに戻る)
		IEnumerator ToThePosition(float firstWait)
        {
			yield return new WaitForSeconds(firstWait);
			for (int i = 0; i < 5; ++i)
			{
				var initPos = ToioPositionConverter.GetInitializePosition(areaRightFront, areaLeftBack);
				var initRot = ToioPositionConverter.GetInitializeRotation(areaRightFront, areaLeftBack);
				this.MoveToTheInitialPoint(initPos, initRot, 20);

				yield return new WaitForSeconds(0.2f);
			}

		}

		IEnumerator Control(CubeManager mgr, Cube c)
		{
			this.areaRightFront = GlobalGameConfig.currentConfig.areaRightFront;
			this.areaLeftBack = GlobalGameConfig.currentConfig.areaLeftBack;
			while (true)
			{
				var next = NextMovePoint( c.pos, this.areaRightFront, this.areaLeftBack);
				this.TargetMoveAfterRound(next.x, next.y, moveSpeed);

				yield return new WaitForToioMovePosition(c, next);

				if(!c.isGrounded){
					if(groundAdjuster == null){
						groundAdjuster = new ToioGroundAdjuster();
					}
					groundAdjuster.Start(c);
					bool endFlag = false;
					while( !endFlag ){
						endFlag = groundAdjuster.Update() ;
						yield return null;
					}
				}

				if (rotateTime > 0)
				{
					c.Move(-rotateSpeed, rotateSpeed, rotateTime, Cube.ORDER_TYPE.Strong);
					yield return new WaitForSeconds(rotateTime * 0.001f + 0.1f);
				}
				yield return new WaitForSeconds(0.1f);

			}
		}

		private Vector2Int NextMovePoint(Vector2 current, Vector2Int areaLU, Vector2Int areaRD)
        {
			int width = areaRD.x - areaLU.x;
			int height = areaRD.y - areaLU.y;
			int xPos = UnityEngine.Random.Range(areaLU.x, areaRD.x);
			int yPos = UnityEngine.Random.Range(areaLU.y, areaRD.y);
			//Debug.Log("MoveNextPosition " + xPos + "," + yPos +"\n" + areaRD + "\n" + areaLU);
			return new Vector2Int(xPos,yPos);
        }

	}
}
