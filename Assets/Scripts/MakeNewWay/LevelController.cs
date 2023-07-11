using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MakeNewWay
{
    public class LevelController
    {
        public LevelModel LevelModel { get { return levelModel; } private set { } }

        private LevelView levelView;
        private LevelModel levelModel;
        private bool isMoving = false;
        

        private UndoController undoController;

        public LevelController( LevelView levelView)
        {
            this.levelView = levelView;
            this.levelModel = new LevelModel( );

            undoController = new UndoController( this );
        }

        

        public void Move( MoveDirection direction )
        {
            if ( isMoving )
            {
                return;
            }
            Transform playerTransform = levelView.CurrentPlayerPart.Player.transform;
            Vector3 playerCurrentPos = playerTransform.position;
            Vector3Int intCurrentPos = Vector3Int.FloorToInt( playerCurrentPos );
            Vector3 nextPos = CalculateNextPos( playerCurrentPos, direction );
            Vector3Int intNextPos = Vector3Int.FloorToInt( nextPos );

            if ( !CanItMove( nextPos, direction ) )
            {
                return;
            }

            isMoving = true;

            playerTransform.DOMove( nextPos, 0.5f ).OnComplete( ( ) => {
                isMoving = false;
                undoController.AddToUndo(playerTransform, ObjectType.NONE, intCurrentPos, intNextPos );
                GroundCheckAndFall( playerTransform, ObjectType.NONE );
                levelView.CheckRoundWin( );
            } );
            
            
            ObjectType nextObj = ObjectType.NONE;
            levelModel.GetObject( intNextPos, out nextObj );
            if ( nextObj == ObjectType.MOVABLE )
            {
                MoveTheMovable( nextPos, direction );
            }
        }

        private void GroundCheckAndFall( Transform itemTransform , ObjectType type)
        {
            Vector3Int itemPos = Vector3Int.FloorToInt( itemTransform.position );
            Vector3Int downPos = new Vector3Int( itemPos.x, itemPos.y - 1, itemPos.z );
            ObjectType groundObj = ObjectType.NONE;
            LevelModel.GetObject( downPos, out groundObj );

            if ( groundObj == ObjectType.NONE)
            {
                if( itemPos.y > -5 )
                {
                    isMoving = true;
                    itemTransform.DOMoveY( downPos.y, 0.2f ).SetEase(Ease.OutQuad).OnComplete( ( ) =>
                    {
                        isMoving = false;
                        GroundCheckAndFall( itemTransform, type );
                    } );

                    if ( type == ObjectType.MOVABLE )
                    {
                        levelModel.RemoveObject( itemPos );
                    }
                }
                /*else
                {
                    itemTransform.gameObject.SetActive( false );
                }*/
            }
            
        }

        private void MoveTheMovable( Vector3 movableCurrentPos, MoveDirection direction )
        {
            Vector3Int intMovableCurrentPos = Vector3Int.FloorToInt( movableCurrentPos );
            Vector3 movableNextPos = CalculateNextPos( movableCurrentPos, direction );
            Vector3Int intMovableNextPos = Vector3Int.FloorToInt( movableNextPos );

            Transform movableTransform;
            levelModel.TryGetMovable( intMovableCurrentPos, out movableTransform );

            isMoving = true;
            movableTransform.DOMove( movableNextPos, 0.6f ).OnComplete( ( ) =>
            {
                isMoving = false;
                undoController.AddToUndo( movableTransform, ObjectType.MOVABLE, intMovableCurrentPos, intMovableNextPos );
                GroundCheckAndFall( movableTransform, ObjectType.MOVABLE );
            } );

            levelModel.RemoveObject( intMovableCurrentPos );
            levelModel.RemoveMovable( intMovableCurrentPos );

            levelModel.AddObject( intMovableNextPos, ObjectType.MOVABLE );
            levelModel.AddMovable( intMovableNextPos, movableTransform );
        }

        private Vector3 CalculateNextPos( Vector3 currentPos, MoveDirection direction )
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

        private bool CanItMove( Vector3 target, MoveDirection direction )
        {
            Vector3Int intTarget = Vector3Int.FloorToInt( target );
            ObjectType nextObj = ObjectType.NONE;
            levelModel.GetObject( intTarget, out nextObj );

            switch ( nextObj )
            {
                case ObjectType.OBSTACLE:
                    return false;
                case ObjectType.MOVABLE:
                    Vector3 movableNextPos = CalculateNextPos( target, direction );
                    intTarget = Vector3Int.FloorToInt( movableNextPos );
                    ObjectType obj2 = ObjectType.NONE;
                    levelModel.GetObject( intTarget, out obj2 );
                    if ( obj2 == ObjectType.NONE )
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                default:
                    //ObjectType.NONE
                    return true;
            }
        }

        public void UndoGame( )
        {
            if ( isMoving )
            {
                return;
            }
            undoController.Undo( );
        }

    }
}
