using UnityEngine;

namespace MakeNewWay.Level
{
    public class MovementInfo
    {
        private Transform objTransform;
        public ObjectType ObjType { get; private set; }
        private Vector3Int prevPos;

        private LevelModel levelModel;

        public MovementInfo(Transform objTransform, ObjectType objType, Vector3Int prevPos, LevelController levelController)
        {
            this.objTransform = objTransform;
            this.ObjType = objType;
            this.prevPos = prevPos;
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
                Vector3Int currentPos = Vector3Int.FloorToInt( objTransform.position );
                levelModel.RemoveObject( currentPos );
                levelModel.RemoveMovable( currentPos );
                levelModel.AddObject( prevPos, ObjType );
                levelModel.AddMovable( prevPos, objTransform );
            }
        }
    }
}
