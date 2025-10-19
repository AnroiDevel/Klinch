using UnityEngine;
using UnityEngine.Audio;
using System;

namespace Klinch.Settings
{
    [CreateAssetMenu(menuName = "Klinch/Settings/GameAudioSettings")]
    public sealed class GameAudioSettings : ScriptableObject
    {
        [Header("Mixer")]
        [SerializeField] private AudioMixer _mixer;

        [Header("Values (0..10)")]
        [Range(0f, 10f)] public float master = 1f;
        [Range(0f, 10f)] public float music = 0.8f;
        [Range(0f, 10f)] public float sfx = 0.9f;

        private const string _kMaster = "gas_master";
        private const string _kMusic = "gas_music";
        private const string _kSfx = "gas_sfx";

        private const string _pMaster = "MasterVolDb";
        private const string _pMusic = "MusicVolDb";
        private const string _pSfx = "SfxVolDb";

        public void Load()
        {
            master = PlayerPrefs.GetFloat(_kMaster, master);
            music = PlayerPrefs.GetFloat(_kMusic, music);
            sfx = PlayerPrefs.GetFloat(_kSfx, sfx);
            Apply();
        }

        public void Save()
        {
            PlayerPrefs.SetFloat(_kMaster, master);
            PlayerPrefs.SetFloat(_kMusic, music);
            PlayerPrefs.SetFloat(_kSfx, sfx);
            PlayerPrefs.Save();
        }

        public void Apply()
        {
            if(_mixer == null)
            {
                Debug.LogWarning("GameAudioSettings.Apply called without an assigned AudioMixer.");
                return;
            }

            SetDb(_pMaster, master / 10);
            SetDb(_pMusic, music / 10);
            SetDb(_pSfx, sfx / 10);
        }

        private void SetDb(string param, float linear01)
        {
            // 0 → -80 dB (почти тишина), 1 → 0 dB
            float db = linear01 <= 0.0001f ? -80f : 20f * Mathf.Log10(Mathf.Clamp01(linear01));

            if(!_mixer)
            {
                return;
            }

            _mixer.SetFloat(param, db);
        }
    }
}
