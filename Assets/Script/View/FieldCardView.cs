using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
            if (gameController.IsInteractable) {
                _isSelected = !_isSelected;
                gameController.ToggleSelect(fieldCardIndex);

                if (_isSelected && (gameController.CurrentPhase == GameController.Phase.Shuffle)) {
                    gameController.SetInteractable(false);
                    StartCoroutine(nameof(ChangeToBattlePhase));
                }

                if (gameController.CurrentPhase != GameController.Phase.Shuffle) {
                    imgSelectDialog.GetComponent<Image>().sprite = _isSelected ? spriteAllConfirmSelect[1] : spriteAllConfirmSelect[0];
                }
            }
        }


        private void Update()
        {
            if (gameController) {

                if (gameController.IsGameInit && gameController.IsGameStart && !gameController.IsGameOver) {

                    if (playerIndex == 0) {
                        txtCard.text = gameController.FieldCardCache_1[fieldCardIndex].ToString();

                    } else if (playerIndex == 1) {
                        txtCard.text = gameController.FieldCardCache_2[fieldCardIndex].ToString();

                    }

                    imgConfirmSelect.SetActive(_isSelected);

                    if (_isSelected) {

                        if (gameController.CurrentPhase == GameController.Phase.Shuffle) {
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
