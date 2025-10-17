using UnityEngine;
using TMPro;

namespace Game.Theme
{
    [CreateAssetMenu(menuName = "CardGame/Theme/ThemeConfig")]
    public sealed class ThemeConfig : ScriptableObject
    {
        [Header("Icons")]
        public Sprite iconAttack;          // молот/искры
        public Sprite iconHeal;            // ключ/ремонт
        public Sprite iconTimer;           // шестерня с секундным диском
        public Sprite iconPhaseBattle;     // микрочип/крест саблей
        public Sprite iconPhaseShuffle;    // конвейер/роборука

        [Header("Cards")]
        public Sprite cardFrame;           // рамка лицевой стороны
        public Sprite cardBack;            // обратка

        [Header("Characters")]
        public RuntimeAnimatorController leftRobot;   // Idle/Hit/Win/Lose
        public RuntimeAnimatorController rightRobot;

        [Header("Fonts & Colors")]
        public TMP_FontAsset font;         // тех-гротеск для UI
        public Color primary = new(0.85f, 0.85f, 0.9f);
        public Color accent = new(0f, 0.9f, 0.8f);
        public Color warning = new(1f, 0.45f, 0.1f);
    }
}
