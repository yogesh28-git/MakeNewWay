using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MakeNewWay.UI
{
    [System.Serializable]
    public class LevelButton
    {
        public Button LvlButton { get { return lvlButton; } private set{ } }
        public Image LockImage { get { return lockImage; } private set { } }
        public TextMeshProUGUI ButtonText { get { return buttonText; } private set { } }
        public string LevelSceneName { get { return levelSceneName; } private set { } }

        [SerializeField] private Button lvlButton;
        [SerializeField] private Image lockImage;
        [SerializeField] private TextMeshProUGUI buttonText;
        [SerializeField] private string levelSceneName;
    }
}
