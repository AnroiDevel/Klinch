using UnityEngine;

namespace Game.Theme
{
    public sealed class ThemeManager : MonoBehaviour
    {
        [SerializeField] private ThemeConfig _config;
        [SerializeField] private ThemeStrings _strings;

        public ThemeConfig Config => _config;
        public ThemeStrings Strings => _strings;

        public static ThemeManager Instance { get; private set; }

        private void Awake()
        {
            if(Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
