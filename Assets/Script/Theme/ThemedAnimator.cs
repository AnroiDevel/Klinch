using UnityEngine;

namespace Game.Theme
{
    [RequireComponent(typeof(Animator))]
    public sealed class ThemedAnimator : MonoBehaviour
    {
        [SerializeField] private bool _isLeftCharacter = true;
        private Animator _anim;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            Apply();
        }

        public void Apply()
        {
            var cfg = ThemeManager.Instance.Config;
            _anim.runtimeAnimatorController = _isLeftCharacter ? cfg.leftRobot : cfg.rightRobot;
        }
    }
}
