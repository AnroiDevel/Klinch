using UnityEngine;
using UnityEngine.EventSystems;

namespace Klinch.UI
{
    public sealed class UISfx : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
    {
        [SerializeField] private AudioSource _source;
        [SerializeField] private AudioClip _hover;
        [SerializeField] private AudioClip _click;

        private void Awake()
        {
            if(_source == null)
            {
                _source = GetComponent<AudioSource>();
            }

            if(_source == null)
            {
                _source = gameObject.AddComponent<AudioSource>();
            }

            _source.playOnAwake = false;
        }

        private void Reset()
        {
            _source = GetComponent<AudioSource>();
            if(_source == null)
            {
                _source = gameObject.AddComponent<AudioSource>();
            }
            _source.playOnAwake = false;
        }

        public void OnPointerEnter(PointerEventData _) { Play(_hover); }
        public void OnPointerClick(PointerEventData _) { Play(_click); }

        private void Play(AudioClip clip)
        {
            if(clip == null || _source == null)
                return;
            _source.pitch = Random.Range(0.98f, 1.02f);
            _source.PlayOneShot(clip, 0.9f);
        }
    }
}
