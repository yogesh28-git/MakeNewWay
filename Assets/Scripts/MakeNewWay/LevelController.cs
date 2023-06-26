using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakeNewWay
{
    public class LevelController
    {
        public LevelModel LevelModel { get { return levelModel; } private set { } }

        private LevelView levelView;
        private LevelModel levelModel;
        private bool isMoving = false;
        private int numberOfPlayers;

        public LevelController( LevelView levelView, int numberOfPlayers )
        {
            this.levelView = levelView;
            this.levelModel = new LevelModel();
            this.numberOfPlayers = numberOfPlayers;
        }

        private void GameWinCheck( )
        {
            if ( levelView.CurrentPlayerPart.Player.transform.position == levelView.CurrentPlayerPart.End.transform.position )
            {
                //Round Win
            }
        }

        private void GameLoseCheck( )
        {
            if(levelView.CurrentPlayerPart.Player.transform.position.y < -3 )
            {
                //Level Lost
            }
        }

        public void Move( MoveDirection direction )
        {
            if ( isMoving )
            {
                return;
            }
            Transform playerTransform = levelView.CurrentPlayerPart.Player.transform;
            Vector3 playerCurrentPos = playerTransform.position;
            Vector3 nextPos = CalculateNextPos( playerCurrentPos, direction );
            Debug.Log( nextPos );
            if ( !CanItMove( nextPos, direction ) )
            {
                return;
            }

            isMoving = true;

            playerTransform.DOMove( nextPos, 0.5f ).OnComplete( ( ) => { isMoving = false; } );
            Vector3Int intNextPos = Vector3Int.FloorToInt( nextPos );
            ObjectType nextObj = ObjectType.NONE;
            levelModel.GetObject( intNextPos, out nextObj );
            if(nextObj == ObjectType.MOVABLE )
            {
                MoveTheMovable( nextPos, direction );
            }
        }

        private void MoveTheMovable(Vector3 movableCurrentPos, MoveDirection direction )
        {
            Debug.Log( "Moving the movable" );
            Vector3Int intMovableCurrentPos = Vector3Int.FloorToInt( movableCurrentPos );
            Vector3 movableNextPos = CalculateNextPos( movableCurrentPos, direction );
            Vector3Int intMovableNextPos = Vector3Int.FloorToInt( movableNextPos );

            Transform movableTransform;
            levelModel.TryGetMovable( intMovableCurrentPos, out movableTransform );
            Debug.Log( movableTransform );
            movableTransform.DOMove( movableNextPos, 0.5f );

            levelModel.RemoveObject( intMovableCurrentPos);
            levelModel.RemoveMovable( intMovableCurrentPos );

            levelModel.AddObject( intMovableNextPos, ObjectType.MOVABLE );
            levelModel.AddMovable( intMovableNextPos, movableTransform );
        }

        private Vector3 CalculateNextPos(Vector3 currentPos, MoveDirection direction )
        {
            switch ( direction )
            {
                case MoveDirection.LEFT:
                    currentPos.x -= 1;
                    break;
                case MoveDirection.RIGHT:
                    currentPos.x += 1;
                    break;
                case MoveDirection.UP:
                    currentPos.z += 1;
                    break;
                case MoveDirection.DOWN:
                    currentPos.z -= 1;
                    break;
            }
            return currentPos;
        }

        private bool CanItMove(Vector3 target, MoveDirection direction)
        {
            Vector3Int intTarget = Vector3Int.FloorToInt( target );
            ObjectType nextObj = ObjectType.NONE;
            levelModel.GetObject( intTarget, out nextObj );
            if( nextObj == ObjectType.OBSTACLE)
            {
                return false;
            }
            else if ( nextObj == ObjectType.MOVABLE )
            {
                Vector3 movableNextPos = CalculateNextPos(target, direction );
                intTarget = Vector3Int.FloorToInt( movableNextPos );
                ObjectType obj2 = ObjectType.NONE;
                levelModel.GetObject( intTarget, out obj2 );
                if(obj2 == ObjectType.NONE )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

    }
}
