using UnityEngine;

namespace Game.Theme
{
    [CreateAssetMenu(menuName = "CardGame/Theme/ThemeStrings")]
    public sealed class ThemeStrings : ScriptableObject
    {
        [Header("UI Labels")]
        public string battlePhase = "юпемю";
        public string shufflePhase = "оепеярпнийю";
        public string attack = "юрюйю";
        public string heal = "пелнмр";
        public string ready = "цнрнб";
        public string youWin = "онаедю";
        public string youLose = "онпюфемхе";
    }
}
