using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakeNewWay.Level
{
    public class UndoController
    {
        private Stack<MovementInfo> undoStack = new Stack<MovementInfo>( );

        private LevelController levelController;

        public UndoController( LevelController levelController)
        {
            this.levelController = levelController;
        }

        public void AddToUndo(Transform objTransform, ObjectType objType, Vector3Int prevPos, Vector3Int nextPos)
        {
            MovementInfo moveInfo = new MovementInfo(objTransform,objType,prevPos,nextPos,levelController);
            undoStack.Push( moveInfo );
        }
        public void Undo( )
        {
            MovementInfo moveInfo;
            bool isNotEmpty = undoStack.TryPop( out moveInfo );
            if ( !isNotEmpty )
                return;
            moveInfo.UpdatePosDictionaries( );
            moveInfo.MoveToPrevPos( );
            

            if (moveInfo.ObjType == ObjectType.MOVABLE )
            {
                Undo( );
            }
        }
    }
}
