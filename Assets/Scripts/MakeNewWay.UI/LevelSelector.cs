using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MakeNewWay.UI
{
    public class LevelSelector : MonoBehaviour
    {
        [SerializeField] private LevelButton[] levelButtons;

        private void Start( )
        {
            GameManagerService.Instance.SetLevelStatus( levelButtons[0].LevelSceneName, LevelStatus.UNLOCKED );
            RefreshLevelSelectUI( );
        }

        private void RefreshLevelSelectUI( )
        {
            foreach (var level in levelButtons )
            {
                //Updating UI of level buttons
                string levelName = level.LevelSceneName;
                LevelStatus levelStatus = GameManagerService.Instance.GetLevelStatus( levelName );
                switch(levelStatus )
                {
                    case LevelStatus.LOCKED:
                        level.ButtonText.enabled = false;
                        level.LockImage.enabled = true;
                        break;
                    case LevelStatus.UNLOCKED:
                        level.ButtonText.enabled = true;
                        level.LockImage.enabled = false;
                        break;
                }

                //Subscribing to function
                level.LvlButton.onClick.AddListener( delegate { LoadLevel( level.LevelSceneName ); } );
            }
            
        }

        private void LoadLevel( string levelName )
        {
            LevelStatus levelStatus = GameManagerService.Instance.GetLevelStatus( levelName );
            if(levelStatus == LevelStatus.LOCKED )
            {
                //playsound
            }
            else
            {
                SceneManager.LoadScene( levelName );
            }
        }
    }
}
