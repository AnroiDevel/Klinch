using UnityEngine;

namespace Klinch.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class LogoIntro : MonoBehaviour
    {
        [SerializeField] private float _fade = 0.35f;
        [SerializeField] private float _overshoot = 1.05f;
        [SerializeField] private float _scaleDur = 0.28f;

        private CanvasGroup _cg;
        private Vector3 _base;

        private void Awake()
        {
            _cg = GetComponent<CanvasGroup>();
            _base = transform.localScale;
            _cg.alpha = 0f;
            transform.localScale = _base * 0.92f;
        }

        private void OnEnable()
        {
            StopAllCoroutines();
            StartCoroutine(Play());
        }

        private System.Collections.IEnumerator Play()
        {
            // Fade in
            float t = 0f;
            while(t < 1f)
            {
                t += Time.unscaledDeltaTime / Mathf.Max(0.0001f, _fade);
                _cg.alpha = Mathf.SmoothStep(0f, 1f, t);
                yield return null;
            }
            // Scale overshoot
            t = 0f;
            Vector3 from = _base * 0.92f;
            Vector3 to = _base * _overshoot;
            while(t < 1f)
            {
                t += Time.unscaledDeltaTime / Mathf.Max(0.0001f, _scaleDur);
                float e = Mathf.SmoothStep(0f, 1f, t);
                transform.localScale = Vector3.Lerp(from, to, e);
                yield return null;
            }
            // Settle back
            t = 0f;
            from = transform.localScale;
            to = _base;
            while(t < 1f)
            {
                t += Time.unscaledDeltaTime / 0.18f;
                float e = Mathf.SmoothStep(0f, 1f, t);
                transform.localScale = Vector3.Lerp(from, to, e);
                yield return null;
            }
        }
    }
}
