using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

[DisallowMultipleComponent]
public sealed class ButtonJuice :
    MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
    IPointerDownHandler, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    [SerializeField] private Image _imgFill;
    [SerializeField] private Image _imgFocus;
    [SerializeField] private float _hoverScale = 1.06f;
    [SerializeField] private float _hoverDuration = 0.12f;
    [SerializeField] private float _clickPunch = 0.06f;
    [SerializeField] private float _clickDuration = 0.18f;

    private Transform _t;
    private Tween _tween;
    private bool _isHovered;
    private bool _isSelected;
    private Color _fillBase;

    private void Awake()
    {
        _t = transform;
        if(_imgFill != null)
            _fillBase = _imgFill.color;
        if(_imgFocus != null)
            _imgFocus.enabled = false;
    }

    public void OnPointerEnter(PointerEventData e) { _isHovered = true; ApplyState(); }
    public void OnPointerExit(PointerEventData e) { _isHovered = false; ApplyState(); }
    public void OnSelect(BaseEventData e) { _isSelected = true; ApplyState(); }
    public void OnDeselect(BaseEventData e) { _isSelected = false; ApplyState(); }

    public void OnPointerDown(PointerEventData e)
    {
        // press: лёгкое притухание "глубины"
        if(_imgFill != null)
            _imgFill.color = _fillBase * 0.9f;
        _t.DOKill();
        _t.localScale = Vector3.one * (_isHovered || _isSelected ? _hoverScale : 1f);
        _t.DOPunchScale(new Vector3(-_clickPunch, -_clickPunch, 0f), _clickDuration, 8, 0.8f)
         .OnComplete(() => { if(_imgFill != null) _imgFill.color = _fillBase; });
    }

    public void OnSubmit(BaseEventData e) { OnPointerDown(null); }

    private void ApplyState()
    {
        float target = (_isHovered || _isSelected) ? _hoverScale : 1f;
        _tween?.Kill();
        _tween = _t.DOScale(target, _hoverDuration)
                   .SetEase(target > 1f ? Ease.OutBack : Ease.OutQuad);

        if(_imgFocus != null)
            _imgFocus.enabled = (_isHovered || _isSelected);
    }
}
