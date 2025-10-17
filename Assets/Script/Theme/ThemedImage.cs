using UnityEngine;
using UnityEngine.UI;

namespace Game.Theme
{

    [RequireComponent(typeof(Image))]
    public sealed class ThemedImage : MonoBehaviour
    {
        [SerializeField] private ThemeSpriteKey _key;
        [SerializeField] private bool _tintWithPrimary;

        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
            Apply();
        }

        public void Apply()
        {
            var cfg = ThemeManager.Instance.Config;
            Sprite sprite = _key switch
            {
                ThemeSpriteKey.IconAttack => cfg.iconAttack,
                ThemeSpriteKey.IconHeal => cfg.iconHeal,
                ThemeSpriteKey.IconTimer => cfg.iconTimer,
                ThemeSpriteKey.IconPhaseBattle => cfg.iconPhaseBattle,
                ThemeSpriteKey.IconPhaseShuffle => cfg.iconPhaseShuffle,
                ThemeSpriteKey.CardFrame => cfg.cardFrame,
                ThemeSpriteKey.CardBack => cfg.cardBack,
                _ => null
            };
            if(sprite != null)
                _image.sprite = sprite;
            if(_tintWithPrimary)
                _image.color = ThemeManager.Instance.Config.primary;
        }

        public void SetKey(ThemeSpriteKey key)
        {
            _key = key;
            if(_image == null)
                _image = GetComponent<Image>();
            Apply();
        }
    }
}
