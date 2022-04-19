using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using toio;
using TMPro;
using Cinemachine;
using System.Threading;

namespace BMProject
{
    public class ToioEventController : MonoBehaviour
    {
        public PlayingScoreBoard scoreBoard;

        public GameObject hitPrefab;
        private ParticleSystem hitParticle;
        private AudioSource hitAudio;

        private CinemachineCollisionImpulseSource impulseSource;


        private int hitNum = 0;

        private float tm = 0.0f;
        private float lastHitTime = -10.0f;

        private void Awake()
        {
            var gmo = GameObject.Instantiate(hitPrefab);
            this.hitAudio = gmo.GetComponentInChildren<AudioSource>(true);
            this.hitParticle = gmo.GetComponentInChildren<ParticleSystem>(true);

            this.impulseSource = this.GetComponent<CinemachineCollisionImpulseSource>();
        }



        private void Update()
        {
            this.tm += Time.deltaTime;

        }

        // Start is called before the first frame update
        public void InitCube(CubeManager mgr, Cube c)
        {
            // 衝突レベルセット
            c.ConfigCollisionThreshold(7);
            c.collisionCallback.AddListener("ToioEventCtrl", OnCubeHit);
        }

        public void EndEvent(CubeManager mgr, Cube c)
        {
            c.collisionCallback.RemoveListener("ToioEventCtrl");
        }

        // キューブがヒットした時の処理
        private void OnCubeHit(Cube c)
        {
            if( this.tm - this.lastHitTime < 0.15f)
            {
                return;
            }
            if (this.hitParticle)
            {
                this.hitParticle.Emit(100);
            }
            if (this.hitAudio)
            {
                this.hitAudio.Play();
            }
            if (impulseSource)
            {
                impulseSource.GenerateImpulseAt(Vector3.zero, Vector3.forward);
            }

            c.PlayPresetSound(0,255,Cube.ORDER_TYPE.Strong);
            c.TurnLedOn(255, 200, 200, 120,Cube.ORDER_TYPE.Weak);
            ++hitNum;
            this.lastHitTime = this.tm;
            scoreBoard.AddScore(1,LeftTimer.Instance.GetTimeFromStart());
        }

    }
}