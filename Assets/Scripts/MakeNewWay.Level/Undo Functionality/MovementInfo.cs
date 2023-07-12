using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakeNewWay.Level
{
    public class MovementInfo
    {
        private Transform objTransform;
        public ObjectType ObjType { get; private set; }
        private Vector3Int prevPos;
        private Vector3Int newPos;

        private LevelModel levelModel;

        public MovementInfo(Transform objTransform, ObjectType objType, Vector3Int prevPos, Vector3Int newPos, LevelController levelController)
        {
            this.objTransform = objTransform;
            this.ObjType = objType;
            this.prevPos = prevPos;
            this.newPos = newPos;
            this.levelModel = levelController.LevelModel;
        }

        public void MoveToPrevPos( )
        {
            objTransform.position = prevPos;
        }

        public void UpdatePosDictionaries()
        {
            if(ObjType == ObjectType.MOVABLE )
            {
                levelModel.RemoveObject( newPos );
                levelModel.RemoveMovable( newPos );
                levelModel.AddObject( prevPos, ObjType );
                levelModel.AddMovable( prevPos, objTransform );
            }
        }
    }
}
