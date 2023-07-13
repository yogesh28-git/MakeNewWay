using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MakeNewWay.UI;

namespace MakeNewWay.Level
{
    public class LevelView : MonoBehaviour
    {
        public PlayerParts CurrentPlayerPart { get { return currentPlayerPart; } private set { } }
        public int LevelNumber { get { return levelNumber; } private set { } }

        [SerializeField] private Transform[] Obstacles;
        [SerializeField] private Transform[] Movables;
        [SerializeField] private PlayerParts[] playerParts;
        [SerializeField] private UIController uiController;
        [SerializeField] private int levelNumber;

        private int maxLevelNumber = 10;
        private LevelController levelController;
        private PlayerParts currentPlayerPart = null;
        private int numberOfPlayers;
        private int currentPlayerNumber = 0;
        private bool IsInputInterrupted = false;


        private void Awake( )
        {
            levelController = new LevelController( this, uiController);
            numberOfPlayers = playerParts.Length;
        }
        private void Start( )
        {
            foreach ( var obstacle in Obstacles )
            {
                levelController.LevelModel.AddObject( Vector3Int.FloorToInt( obstacle.position ), ObjectType.OBSTACLE );
            }

            foreach ( var movable in Movables )
            {
                levelController.LevelModel.AddObject( Vector3Int.FloorToInt( movable.position ), ObjectType.MOVABLE );
                levelController.LevelModel.AddMovable( Vector3Int.FloorToInt( movable.position ), movable );
            }
            SpawnPlayer( 0 );
        }

        private void SpawnPlayer( int playerIndex )
        {
            if(currentPlayerPart != null )
            {
                //currentPlayerPart.Player.SetActive( false );
                //currentPlayerPart.End.SetActive( false );
            }

            this.currentPlayerPart = playerParts[ playerIndex ];

            Vector3 startPos = currentPlayerPart.StartingPoint.transform.position;
            Vector3 upPos = new Vector3(startPos.x, startPos.y+1, startPos.z);

            AudioService.Instance.PlaySound( SoundType.JUMP );

            IsInputInterrupted = true;
            currentPlayerPart.Player.transform.DOMove( upPos, 0.5f ).OnComplete( ( ) =>
            {
                currentPlayerPart.Player.transform.DOMove( startPos, 0.5f ).OnComplete( ( ) =>
                {
                    IsInputInterrupted = false;
                } );
            } );

            currentPlayerPart.Player.transform.DORotate( new Vector3( 0, 0, 360 ), 0.3f, RotateMode.FastBeyond360 ).SetLoops( 3, LoopType.Restart );
        }

        private void Update( )
        {
            if (!IsInputInterrupted)
            {
                InputHandler( );
            }
        }

        private void InputHandler( )
        {
            if ( Input.GetKeyDown( KeyCode.A ) || Input.GetKeyDown( KeyCode.LeftArrow ) )
            {
                levelController.Move( MoveDirection.LEFT );
            }
            else if ( Input.GetKeyDown( KeyCode.D ) || Input.GetKeyDown( KeyCode.RightArrow ) )
            {
                levelController.Move( MoveDirection.RIGHT );
            }
            else if ( Input.GetKeyDown( KeyCode.W ) || Input.GetKeyDown( KeyCode.UpArrow ) )
            {
                levelController.Move( MoveDirection.UP );
            }
            else if ( Input.GetKeyDown( KeyCode.S ) || Input.GetKeyDown( KeyCode.DownArrow ) )
            {
                levelController.Move( MoveDirection.DOWN );
            }

            if ( Input.GetKeyDown( KeyCode.Z ) )
            {
                levelController.UndoGame( );
            }
            if ( Input.GetKeyDown( KeyCode.Q ) || Input.GetKeyDown(KeyCode.Escape) )
            {
                AudioService.Instance.PlaySound( SoundType.CLICK );
                QuitToMenu( );
            }
            if(Input.GetKeyDown( KeyCode.R ) )
            {
                AudioService.Instance.PlaySound( SoundType.CLICK );
                ReloadScene( );
            }
        }

        private void CheckLevelWin( )
        {
            this.currentPlayerNumber++;

            if ( currentPlayerNumber >= numberOfPlayers )
            {
                if(levelNumber < maxLevelNumber )
                {
                    GameManagerService.Instance.UnlockNextLevel( levelNumber );
                }
                StartCoroutine(NextLevelCoroutine());
            }
            else
            {
                StartCoroutine(NextRoundCoroutine());
            }
        }
        private void PlayerFinishAnimation( )
        {
            try
            {
                AudioService.Instance.PlaySound( SoundType.WIN );
                Vector3 playerCurrentPos = currentPlayerPart.Player.transform.position;
                float upPos = playerCurrentPos.y + 1;
                currentPlayerPart.Player.transform.DOMoveY( upPos, 0.3f ).OnComplete( ( ) =>
                {
                    currentPlayerPart.Player.transform.DOMoveY( playerCurrentPos.y, 0.3f );
                } );
                currentPlayerPart.Player.transform.DORotate( new Vector3( 0, 360, 0 ), 0.1f, RotateMode.FastBeyond360 ).SetLoops( 6, LoopType.Restart );
            }
            catch
            {
                Debug.Log( "Error here" );
            }
            
        }

        private IEnumerator NextLevelCoroutine( )
        {
            IsInputInterrupted = true;

            PlayerFinishAnimation( );

            yield return new WaitForSeconds( 0.6f );

            //Display completion text
            uiController.ToggleWhiteCover( );
            yield return new WaitForSeconds( 0.5f );
            uiController.SetTextCompletion( "Completed" );
            uiController.ToggleTextCompletion( );
            yield return new WaitForSeconds( 1.5f );
            if( levelNumber < maxLevelNumber )
            {
                uiController.SetTextCompletion( "Next " + ( levelNumber + 1 ).ToString( ) + "/10" );
            }
            else
            {
                uiController.SetTextCompletion( "Congratulations!");
            }
                
            yield return new WaitForSeconds( 1.5f );

            //Cleanup
            uiController.ToggleWhiteCover( );
            uiController.ToggleTextCompletion( );
            IsInputInterrupted = false;

            //Load Next Level
            if(levelNumber < maxLevelNumber )
            {
                SceneManager.LoadScene( "Level" + (levelNumber+1).ToString() );
            }
            else
            {
                SceneManager.LoadScene( 0 );
            }
        }

        public bool CheckRoundWin( )
        {
            Vector3Int playerPos = Vector3Int.FloorToInt( currentPlayerPart.Player.transform.position );
            Vector3Int endPos = Vector3Int.FloorToInt( currentPlayerPart.End.transform.position );
            if ( playerPos == endPos )
            {
                CheckLevelWin( );
                return true;
            }
            else
            {
                return false;
            }
        }

        private IEnumerator NextRoundCoroutine( )
        {
            IsInputInterrupted = true;
            PlayerFinishAnimation( );
            yield return new WaitForSeconds( 0.6f );

            uiController.ToggleWhiteCover();
            yield return new WaitForSeconds( 0.5f );
            uiController.SetTextCompletion( "End Is The New Beginning" );
            uiController.ToggleTextCompletion( );
            yield return new WaitForSeconds( 2f );

            uiController.ToggleWhiteCover( );
            uiController.ToggleTextCompletion( );

            SpawnPlayer( currentPlayerNumber );
            IsInputInterrupted = false;
        }

        private void QuitToMenu( )
        {
            SceneManager.LoadScene( 0 );
        }

        private void ReloadScene( )
        {
            SceneManager.LoadScene( SceneManager.GetActiveScene( ).buildIndex );
        }

    }
}
