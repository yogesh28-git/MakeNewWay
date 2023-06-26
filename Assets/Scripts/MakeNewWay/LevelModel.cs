using System.Collections.Generic;
using UnityEngine;

namespace MakeNewWay
{
    [System.Serializable]
    public class LevelModel
    {
        public Dictionary<Vector3Int, ObjectType> gridDict = new Dictionary<Vector3Int, ObjectType>();
        public Dictionary<Vector3Int, Transform> movablesDict = new Dictionary<Vector3Int, Transform>();
        public bool GetObject( Vector3Int pos, out ObjectType obj )
        {
            bool result  = gridDict.TryGetValue( pos, out obj );
            if( result == false )
            {
                obj = ObjectType.NONE;
            }
            return result;
        }

        public void AddObject( Vector3Int pos, ObjectType obj )
        {
            gridDict.Add( pos, obj );
        }

        public void RemoveObject( Vector3Int pos ) 
        {
            gridDict.Remove( pos );
        }

        public bool TryGetMovable( Vector3Int pos, out Transform movable )
        {
            return movablesDict.TryGetValue( pos, out movable );
        }
        public void AddMovable( Vector3Int pos, Transform movable )
        {
            movablesDict.Add( pos, movable );
        }
        public void RemoveMovable( Vector3Int pos )
        {
            movablesDict.Remove( pos );
        }
    }
}

