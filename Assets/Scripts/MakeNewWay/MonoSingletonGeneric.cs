using UnityEngine;

namespace MakeNewWay
{
    public class MonoSingletonGeneric<T> : MonoBehaviour where T: MonoSingletonGeneric<T>
    {
        public static T Instance { get {return instance; } set { } }
        
        private static T instance;

        protected virtual void Awake( )
        {
            if (instance == null )
            {
                instance = this as T;
                DontDestroyOnLoad( this.gameObject );
            }
            else
            {
                Destroy( this.gameObject );
            }
        }
    }
}
