namespace MakeNewWay
{
    public class SingletonGeneric<T> where T: SingletonGeneric<T> , new() 
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if ( instance == null )
                {
                    instance = new T( );
                }
                return instance;
            }
            private set { }
        }
    }

}