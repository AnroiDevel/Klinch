using UnityEngine;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices;

/// ����� �� ������ � ������� ����. ������� AudioSource (Music).
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
        // �� ������ ������ ������ �����, ����� ����� �� ������������� �� �����
        if(_music.isPlaying)
            _music.Stop();
    }

    private void Update()
    {
        if(_armed)
            return;

        // ����� ���������������� ����: ����, ���, �������
        bool gesture =
            Input.GetMouseButtonDown(0) ||
            Input.touchCount > 0 ||
            Input.anyKeyDown;

        if(!gesture)
            return;

        _armed = true;

#if UNITY_WEBGL && !UNITY_EDITOR
        // ������������ WebAudio (�������� ��������� ��� iOS/Safari)
        try { KL_UnlockAudio(); } catch { /* no-op */ }
#endif
        // ������ ��������� ������ ����� AudioSource (WebAudio), ��� ���������� ������
        if(_music.clip != null && !_music.isPlaying)
            _music.Play();

        // ������ �� ����� Update � �������� ��������� (�����)
        enabled = false;
    }

    private void OnDisable()
    {
        // ������ �� ������ � ������ ����� ������ ������ ����� ����, ���� �����
        // ���� ������ ������������� ��� �������� ���� � ��������������:
        // if (_music != null && _music.isPlaying) _music.Stop();
    }
}
