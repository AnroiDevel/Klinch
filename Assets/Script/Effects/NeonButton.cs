using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Klinch.UI
{
    [RequireComponent(typeof(Image))]
    public sealed class NeonButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Image _image;
        [SerializeField] private bool _interactable = true;

        [Header("Colors")]
        [SerializeField] private Color _normal = new(0.25f, 0.9f, 1f, 1f);   // циан
        [SerializeField] private Color _hover = new(0.45f, 1f, 1f, 1f);
        [SerializeField] private Color _pressed = new(0.18f, 0.8f, 1f, 1f);
        [SerializeField] private Color _disabled = new(0.35f, 0.35f, 0.35f, 0.6f);

        [Header("Scale")]
        [SerializeField, Range(0.95f, 1.2f)] private float _hoverScale = 1.04f;
        [SerializeField, Range(0.9f, 1.2f)] private float _pressedScale = 0.97f;
        [SerializeField, Range(0.05f, 0.5f)] private float _lerp = 0.15f;

        private Vector3 _baseScale;

        private void Reset()
        {
            _image = GetComponent<Image>();
        }

        private void Awake()
        {
            if(_image == null)
                _image = GetComponent<Image>();
            _baseScale = transform.localScale;
            ApplyState(_interactable ? _normal : _disabled, 1f);
        }

        public void SetInteractable(bool interactable)
        {
            _interactable = interactable;
            ApplyState(_interactable ? _normal : _disabled, 1f);
        }

        public void OnPointerEnter(PointerEventData _) { if(_interactable) ApplyState(_hover, _hoverScale); }
        public void OnPointerExit(PointerEventData _) { if(_interactable) ApplyState(_normal, 1f); }
        public void OnPointerDown(PointerEventData _) { if(_interactable) ApplyState(_pressed, _pressedScale); }
        public void OnPointerUp(PointerEventData _) { if(_interactable) ApplyState(_hover, _hoverScale); }

        private void ApplyState(Color targetColor, float scaleMul)
        {
            StopAllCoroutines();
            StartCoroutine(LerpColorAndScale(targetColor, scaleMul));
        }

        private System.Collections.IEnumerator LerpColorAndScale(Color target, float scaleMul)
        {
            var startC = _image.color;
            var startS = transform.localScale;
            var endS = _baseScale * scaleMul;

            float t = 0f;
            float dur = Mathf.Max(0.0001f, _lerp);
            while(t < 1f)
            {
                t += Time.unscaledDeltaTime / dur;
                float e = Mathf.SmoothStep(0f, 1f, t);
                _image.color = Color.Lerp(startC, target, e);
                transform.localScale = Vector3.Lerp(startS, endS, e);
                yield return null;
            }
            _image.color = target;
            transform.localScale = endS;
        }
    }
}
