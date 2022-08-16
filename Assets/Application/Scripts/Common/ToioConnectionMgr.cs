using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using toio;
using Cysharp.Threading.Tasks;

namespace BMProject
{
    public class ToioConnectionMgr
    {
        public static ToioConnectionMgr Instance
        {
            private set; get;
        } = new ToioConnectionMgr();

        public CubeManager cubeManager { get; set; }
        private List<Cube> availableCubes;


        private ToioConnectionMgr()
        {
            Init(ConnectType.Real);
        }

        private void Init(ConnectType connectType)
        {
            this.cubeManager = new CubeManager(connectType);
            this.availableCubes = new List<Cube>();
        }
        public async UniTask<Cube> ConnectCube()
        {
            this.RemoveDisconnectedCubes();
            var c = GetAvailableCube();
            if( c != null) { return c; }

            c = await cubeManager.SingleConnect();
            return c;
        }
        public void ReleaseCube(Cube cube)
        {
            if(cube != null){
                this.availableCubes.Add(cube);
            }
        }

        private Cube GetAvailableCube()
        {
            for(int i = 0;i<availableCubes.Count;++i)
            {
                var c = availableCubes[i];
                if (c.isConnected)
                {
                    availableCubes.RemoveAt(i);
                    return c;
                }
            }
            return null;
        }
        private void RemoveDisconnectedCubes()
        {
            for (int i = 0; i < availableCubes.Count; ++i)
            {
                if(!availableCubes[i].isConnected)
                {
                    cubeManager.Disconnect(availableCubes[i]);
                    availableCubes.RemoveAt(i);
                    --i;
                }
            }
        }

    }
}