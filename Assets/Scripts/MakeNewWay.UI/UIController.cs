using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MakeNewWay.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private Image whiteCover;
        [SerializeField] private Image whiteCover2;
        [SerializeField] private Image blackCover;
        [SerializeField] private TextMeshProUGUI textCompletion;
        [SerializeField] private TextMeshProUGUI textLevelName;

        public void ToggleWhiteCover( )
        {
            whiteCover.enabled = !whiteCover.enabled;
            whiteCover2.enabled = !whiteCover2.enabled;
        }
        public void ToggleBlackCover( ) 
        {
            blackCover.enabled = !blackCover.enabled;
        } 
        public void ToggleTextCompletion( ) 
        {
            textCompletion.enabled = !textCompletion.enabled;
        }

        public void SetTextCompletion( string text)
        {
            textCompletion.text = text;
        }

        public void ChangeLevelNumber(int levelNumber)
        {
            string levelString = levelNumber.ToString( $"D{2}" );
            textLevelName.text = levelString;
        }
    }
}
