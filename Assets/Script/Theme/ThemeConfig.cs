using UnityEngine;
using TMPro;

namespace Game.Theme
{
    [CreateAssetMenu(menuName = "CardGame/Theme/ThemeConfig")]
    public sealed class ThemeConfig : ScriptableObject
    {
        [Header("Icons")]
        public Sprite iconAttack;          // �����/�����
        public Sprite iconHeal;            // ����/������
        public Sprite iconTimer;           // �������� � ��������� ������
        public Sprite iconPhaseBattle;     // ��������/����� ������
        public Sprite iconPhaseShuffle;    // ��������/��������

        [Header("Cards")]
        public Sprite cardFrame;           // ����� ������� �������
        public Sprite cardBack;            // �������

        [Header("Characters")]
        public RuntimeAnimatorController leftRobot;   // Idle/Hit/Win/Lose
        public RuntimeAnimatorController rightRobot;

        [Header("Fonts & Colors")]
        public TMP_FontAsset font;         // ���-������� ��� UI
        public Color primary = new(0.85f, 0.85f, 0.9f);
        public Color accent = new(0f, 0.9f, 0.8f);
        public Color warning = new(1f, 0.45f, 0.1f);
    }
}
