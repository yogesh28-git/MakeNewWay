using UnityEngine;
using UnityEngine.SceneManagement;

namespace MakeNewWay
{
    public class MenuUIController : MonoBehaviour
    {
        private void Update( )
        {
            if ( Input.GetKeyDown( KeyCode.Return)  || Input.GetKeyDown(KeyCode.KeypadEnter) )
            {
                SceneManager.LoadScene( 1 );
            }
        }
    }
}
