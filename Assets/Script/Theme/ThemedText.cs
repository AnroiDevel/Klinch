using UnityEngine;
using TMPro;

namespace Game.Theme
{

    [RequireComponent(typeof(TextMeshProUGUI))]
    public sealed class ThemedText : MonoBehaviour
    {
        [SerializeField] private ThemeTextKey _key;
        [SerializeField] private bool _useAccentColor;

        private TextMeshProUGUI _tmp;

        private void Awake()
        {
            _tmp = GetComponent<TextMeshProUGUI>();
            Apply();
        }

        public void Apply()
        {
            var t = ThemeManager.Instance;
            _tmp.font = t.Config.font;

            _tmp.text = _key switch
            {
                ThemeTextKey.BattlePhase => t.Strings.battlePhase,
                ThemeTextKey.ShufflePhase => t.Strings.shufflePhase,
                ThemeTextKey.Attack => t.Strings.attack,
                ThemeTextKey.Heal => t.Strings.heal,
                ThemeTextKey.Ready => t.Strings.ready,
                ThemeTextKey.YouWin => t.Strings.youWin,
                ThemeTextKey.YouLose => t.Strings.youLose,
                _ => _tmp.text
            };

            if(_useAccentColor)
                _tmp.color = t.Config.accent;
        }

        public void SetKey(ThemeTextKey key)
        {
            _key = key;
            if(_tmp == null)
                _tmp = GetComponent<TextMeshProUGUI>();
            Apply();
        }
    }
}
