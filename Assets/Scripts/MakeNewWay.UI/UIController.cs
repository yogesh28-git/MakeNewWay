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
        [SerializeField] private Image blackCover;
        [SerializeField] private TextMeshProUGUI textCompletion;

        public void ToggleWhiteCover( )
        {
            whiteCover.enabled = !whiteCover.enabled;
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
    }
}
