using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakeNewWay
{
    public class test : MonoBehaviour
    { 
        void Start()
        {
            Invoke( nameof( SpawnPlayer ), 3f );
        }

        public void SpawnPlayer()
        {
            Vector3 startPos = this.transform.position;
            startPos.y  += 1;
            Vector3 upPos = new Vector3( startPos.x, startPos.y + 1, startPos.z );

            transform.DOMove( upPos, 0.5f ).OnComplete( ( ) =>
            {
                transform.DOMove( startPos, 0.5f );
            } );

            transform.DORotate( new Vector3( 0, 0, 360 ), 0.3f, RotateMode.FastBeyond360 ).SetLoops( 3, LoopType.Restart );
        }
    }
}
