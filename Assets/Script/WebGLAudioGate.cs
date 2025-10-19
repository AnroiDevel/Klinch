using UnityEngine;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices;

/// Вешай на объект в главном меню. Привяжи AudioSource (Music).
public sealed class WebGLAudioGate : MonoBehaviour
{
    [SerializeField] private AudioSource _music;   // Output -> Music (AudioMixer)
    [SerializeField] private bool _autoLoop = true;

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")] private static extern void KL_UnlockAudio();
#endif

    private bool _armed;

    private void Awake()
    {
        if(_music == null)
            _music = gameObject.AddComponent<AudioSource>();
        _music.playOnAwake = false;
        _music.loop = _autoLoop;
        _armed = false;
    }

    private void OnEnable()
    {
        // На всякий случай ставим паузу, чтобы точно не проигрывалось до жеста
        if(_music.isPlaying)
            _music.Stop();
    }

    private void Update()
    {
        if(_armed)
            return;

        // Любой пользовательский жест: клик, тап, клавиша
        bool gesture =
            Input.GetMouseButtonDown(0) ||
            Input.touchCount > 0 ||
            Input.anyKeyDown;

        if(!gesture)
            return;

        _armed = true;

#if UNITY_WEBGL && !UNITY_EDITOR
        // Разблокируем WebAudio (особенно актуально для iOS/Safari)
        try { KL_UnlockAudio(); } catch { /* no-op */ }
#endif
        // Теперь запускаем музыку через AudioSource (WebAudio), без системного плеера
        if(_music.clip != null && !_music.isPlaying)
            _music.Play();

        // Больше не нужен Update — отключим компонент (чисто)
        enabled = false;
    }

    private void OnDisable()
    {
        // Ничего не делаем — музыка пусть играет сквозь паузу меню, если нужно
        // Если хочешь останавливать при закрытии меню — раскомментируй:
        // if (_music != null && _music.isPlaying) _music.Stop();
    }
}
