using UnityEngine;

namespace Game.Theme
{
    [CreateAssetMenu(menuName = "CardGame/Theme/ThemeStrings")]
    public sealed class ThemeStrings : ScriptableObject
    {
        [Header("UI Labels")]
        public string battlePhase = "�����";
        public string shufflePhase = "�����������";
        public string attack = "�����";
        public string heal = "������";
        public string ready = "�����";
        public string youWin = "������";
        public string youLose = "���������";
    }
}
