using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Klinch.Settings
{
    public sealed class OptionsAudioView : MonoBehaviour
    {
        [SerializeField] private GameAudioSettings _settings;

        [SerializeField] private Slider _master;
        [SerializeField] private TMP_Text _masterVal;

        [SerializeField] private Slider _music;
        [SerializeField] private TMP_Text _musicVal;

        [SerializeField] private Slider _sfx;
        [SerializeField] private TMP_Text _sfxVal;

        private void OnEnable()
        {
            _settings.Load();
            _master.SetValueWithoutNotify(_settings.master);
            _music.SetValueWithoutNotify(_settings.music);
            _sfx.SetValueWithoutNotify(_settings.sfx);

            _masterVal.SetText($"{Mathf.RoundToInt(_settings.master * 10)}%");
            _musicVal.SetText($"{Mathf.RoundToInt(_settings.music * 10)}%");
            _sfxVal.SetText($"{Mathf.RoundToInt(_settings.sfx * 10)}%");
        }

        public void OnMaster(float v)
        {
            _settings.master = v;
            _settings.Apply();
            _masterVal.SetText($"{Mathf.RoundToInt(v * 10)}%");
        }

        public void OnMusic(float v)
        {
            _settings.music = v;
            _settings.Apply();
            _musicVal.SetText($"{Mathf.RoundToInt(v * 10)}%");
        }

        public void OnSfx(float v)
        {
            _settings.sfx = v;
            _settings.Apply();
            _sfxVal.SetText($"{Mathf.RoundToInt(v * 10)}%");
        }

        public void OnClose(GameObject panel)
        {
            _settings.Save();
            panel.SetActive(false);
        }
    }
}
