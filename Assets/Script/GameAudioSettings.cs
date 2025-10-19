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

        const string _kMaster = "gas_master";
        const string _kMusic = "gas_music";
        const string _kSfx = "gas_sfx";

        const string _pMaster = "MasterVolDb";
        const string _pMusic = "MusicVolDb";
        const string _pSfx = "SfxVolDb";

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
            SetDb(_pMaster, master / 10);
            SetDb(_pMusic, music / 10);
            SetDb(_pSfx, sfx / 10);
        }

        void SetDb(string param, float linear01)
        {
            // 0 → -80 dB (почти тишина), 1 → 0 dB
            float db = linear01 <= 0.0001f ? -80f : 20f * Mathf.Log10(Mathf.Clamp01(linear01));
            _mixer.SetFloat(param, db);
        }
    }
}
