using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MakeNewWay
{
    public class GameManagerService : MonoSingletonGeneric<GameManagerService>
    {
        public LevelStatus GetLevelStatus(string levelName )
        {
            return (LevelStatus)PlayerPrefs.GetInt( levelName, 0);
        }
        public void SetLevelStatus(string levelName, LevelStatus status) 
        {
            PlayerPrefs.SetInt( levelName, ( int ) status );
        }
        public void UnlockNextLevel(int levelNumber)
        {
            string nextLevelName = "Level" + (levelNumber+1).ToString();
            if ( GetLevelStatus( nextLevelName ) == LevelStatus.LOCKED )
            {
                SetLevelStatus( nextLevelName, LevelStatus.UNLOCKED );
            }
        }
    }
}
