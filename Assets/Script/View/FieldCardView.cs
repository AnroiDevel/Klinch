using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SichuanDynasty.UI
{
    public class FieldCardView : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField]
        private int playerIndex;

        [SerializeField]
        private int fieldCardIndex;

        [SerializeField]
        private GameController gameController;

        [SerializeField]
        private Text txtCard;

        [SerializeField]
        private Image imgCard;

        [SerializeField]
        private Sprite[] imgAllCardState;

        [SerializeField]
        private GameObject imgSelectDialog;

        [SerializeField]
        private GameObject imgConfirmSelect;

        [SerializeField]
        private Sprite[] spriteAllConfirmSelect;


        public bool IsSelected { get { return _isSelected; } }


        private bool _isSelected;


        public FieldCardView()
        {
            _isSelected = false;
        }

        public void OnSelect(BaseEventData data)
        {
            imgSelectDialog.SetActive(true);
            imgSelectDialog.GetComponent<Image>().sprite = _isSelected ? spriteAllConfirmSelect[1] : spriteAllConfirmSelect[0];
        }

        public void OnDeselect(BaseEventData data)
        {
            imgSelectDialog.SetActive(false);
        }

        public void HideSelectDialog()
        {
            imgSelectDialog.SetActive(false);
        }

        public void ToggleSelect()
        {
            if(!gameController.IsInteractable)
            {
                gameController.ToggleSelect(fieldCardIndex);
                return;
            }

            if(gameController.CurrentPlayerIndex != playerIndex)
            {
                return;
            }

            _isSelected = !_isSelected;
            gameController.ToggleSelect(fieldCardIndex);

            if(_isSelected && (gameController.CurrentPhase == GameController.Phase.Shuffle))
            {
                gameController.SetInteractable(false);
                StartCoroutine(nameof(ChangeToBattlePhase));
            }

            if(gameController.CurrentPhase != GameController.Phase.Shuffle)
            {
                imgSelectDialog.GetComponent<Image>().sprite = _isSelected ? spriteAllConfirmSelect[1] : spriteAllConfirmSelect[0];
            }
        }

        private void Update()
        {
            if(gameController)
            {

                if(gameController.IsGameInit && gameController.IsGameStart && !gameController.IsGameOver)
                {
                    var cache = playerIndex == 0
                        ? gameController.FieldCardCache_1
                        : gameController.FieldCardCache_2;

                    if(cache != null && fieldCardIndex < cache.Length)
                    {
                        txtCard.text = cache[fieldCardIndex].ToString();

                        if(imgCard != null && !imgCard.enabled)
                        {
                            imgCard.enabled = true;
                        }
                    }
                    else
                    {
                        txtCard.text = string.Empty;

                        if(imgCard != null && imgCard.enabled)
                        {
                            imgCard.enabled = false;
                        }
                    }
                    imgConfirmSelect.SetActive(_isSelected);

                    if(_isSelected)
                    {

                        if(gameController.CurrentPhase == GameController.Phase.Shuffle)
                        {
                            _isSelected = false;

                        }
                    }
                }
            }
        }

        private IEnumerator ChangeToBattlePhase()
        {
            yield return new WaitForSeconds(0.5f);
            gameController.NextPhase();
        }
    }
}
