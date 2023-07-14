using UnityEngine;
using UnityEngine.SceneManagement;

namespace MakeNewWay.UI
{
    public class MenuUIController : MonoBehaviour
    {
        private void Update( )
        {
            if ( Input.GetKeyDown( KeyCode.Return)  || Input.GetKeyDown(KeyCode.KeypadEnter) )
            {
                AudioService.Instance.PlaySound( SoundType.CLICK );
                SceneManager.LoadScene( 1 );
            }
        }
    }
}
