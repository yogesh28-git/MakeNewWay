using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakeNewWay
{
    public class LevelView : MonoBehaviour
    {
        public PlayerParts CurrentPlayerPart { get { return currentPlayerPart; } private set { } }

        [SerializeField] private Transform[] Obstacles;
        [SerializeField] private Transform[] Movables;
        [SerializeField] private PlayerParts[] playerParts;


        private LevelController levelController;
        private PlayerParts currentPlayerPart;


        private void Awake( )
        {
            levelController = new LevelController( this, playerParts.Length );
        }
        private void Start( )
        {
            foreach ( var obstacle in Obstacles )
            {
                levelController.LevelModel.AddObject( Vector3Int.FloorToInt( obstacle.position ), ObjectType.OBSTACLE );
            }

            foreach ( var movable in Movables )
            {
                levelController.LevelModel.AddObject( Vector3Int.FloorToInt( movable.position ), ObjectType.MOVABLE );
                levelController.LevelModel.AddMovable( Vector3Int.FloorToInt( movable.position ), movable );
            }

            SpawnPlayer( 0 );
        }

        public void SpawnPlayer( int playerIndex )
        {
            if(currentPlayerPart != null )
            {
                currentPlayerPart.Player.SetActive( false );
                currentPlayerPart.End.SetActive( false );
            }

            this.currentPlayerPart = playerParts[ playerIndex ];

            Vector3 startPos = currentPlayerPart.StartingPoint.transform.position;
            Vector3 upPos = new Vector3(startPos.x, startPos.y+1, startPos.z);

            currentPlayerPart.Player.transform.DOMove( upPos, 0.5f ).OnComplete( ( ) =>
            {
                currentPlayerPart.Player.transform.DOMove( startPos, 0.5f );
            } );

            currentPlayerPart.Player.transform.DORotate( new Vector3( 0, 0, 360 ), 0.3f, RotateMode.FastBeyond360 ).SetLoops( 3, LoopType.Restart );
        }

        private void Update( )
        {
            InputHandler( );
        }

        private void InputHandler( )
        {
            if ( Input.GetKeyDown( KeyCode.A ) || Input.GetKeyDown( KeyCode.LeftArrow ) )
            {
                levelController.Move( MoveDirection.LEFT );
            }
            else if ( Input.GetKeyDown( KeyCode.D ) || Input.GetKeyDown( KeyCode.RightArrow ) )
            {
                levelController.Move( MoveDirection.RIGHT );
            }
            else if ( Input.GetKeyDown( KeyCode.W ) || Input.GetKeyDown( KeyCode.UpArrow ) )
            {
                levelController.Move( MoveDirection.UP );
            }
            else if ( Input.GetKeyDown( KeyCode.S ) || Input.GetKeyDown( KeyCode.DownArrow ) )
            {
                levelController.Move( MoveDirection.DOWN );
            }

            if ( Input.GetKeyDown( KeyCode.Z ) )
            {
                levelController.UndoGame( );
            }
        }
    }
}
