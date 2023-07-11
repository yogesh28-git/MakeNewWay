using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MakeNewWay
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
            levelController = new LevelController( this );
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

            currentPlayerPart.Player.transform.DOMove( upPos, 0.5f ).OnComplete( ( ) =>
            {
                currentPlayerPart.Player.transform.DOMove( startPos, 0.5f );
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
        }

        private void CheckLevelWin( )
        {
            this.currentPlayerNumber++;

            if ( currentPlayerNumber >= numberOfPlayers )
            {
                GameManagerService.Instance.UnlockNextLevel();
                StartCoroutine(NextLevelCoroutine());
            }
            else
            {
                StartCoroutine(NextRoundCoroutine());
            }
        }
        private void PlayerFinishAnimation( )
        {
            Vector3 playerCurrentPos = currentPlayerPart.Player.transform.position;
            float upPos = playerCurrentPos.y + 1;
            currentPlayerPart.Player.transform.DOMoveY( upPos, 0.3f ).OnComplete( ( ) =>
            {
                currentPlayerPart.Player.transform.DOMoveY( playerCurrentPos.y, 0.3f );
            } );
            currentPlayerPart.Player.transform.DORotate( new Vector3( 0, 360, 0 ), 0.1f, RotateMode.FastBeyond360 ).SetLoops( 6, LoopType.Restart );
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
            uiController.SetTextCompletion( "Next " + (levelNumber+1).ToString() + "/10" );
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

        public void CheckRoundWin( )
        {
            if ( currentPlayerPart.Player.transform.position == currentPlayerPart.End.transform.position )
            {
                CheckLevelWin( );
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

        private void OnRoundLose( )
        {
        }
    }
}
