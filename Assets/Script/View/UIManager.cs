using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SichuanDynasty.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private GameController gameController;

        [SerializeField]
        private GameObject pausePanel;

        [SerializeField]
        private GameObject[] disableDecksView;

        [SerializeField]
        private Text[] txtCardNum;

        [SerializeField]
        private GameObject gameOverUI;

        [SerializeField]
        private GameObject gameplayUI;

        [SerializeField]
        private EventSystem[] allEventSystem;

        [SerializeField]
        private GameObject btnRestart;

        [SerializeField]
        private GameObject[] player1Cards;

        [SerializeField]
        private GameObject[] player2Cards;

        [SerializeField]
        private Image imgCurrentPlayerTurn;

        [SerializeField]
        private Sprite[] spritePlayerTurn;

        [SerializeField]
        private GameObject[] selectDialogs;

        [SerializeField]
        private GameObject[] imgPhases;

        [SerializeField]
        private Image imgPlayerTurn;

        [SerializeField]
        private Sprite[] spriteAllPlayerTurn;

        [SerializeField]
        private GameObject[] imgWarning;

        [SerializeField]
        private StatusView[] statusViews;

        [SerializeField]
        private Animator[] anims;

        [SerializeField]
        private GameObject[] imgCriticals;

        [SerializeField]
        private GameObject btnMainMenuInPauseMenu;


        private bool _isInitShowGameOver;

        private bool _isInitHandle;
        private bool _isHandlingSelectingCards;
        private bool _isAxisInUse;

        private List<GameObject> _currentAvailableButton;
        private int _currentSelectIndex;

        private GameObject _previousSelectedObj;


        public UIManager()
        {
            _isInitShowGameOver = false;
            _isInitHandle = false;
            _isHandlingSelectingCards = false;
            _isAxisInUse = false;
            _currentAvailableButton = new List<GameObject>();
            _currentSelectIndex = 0;
        }

        public void InitHandleSelectingCards(int playerIndex)
        {
            _isInitHandle = true;
            UpdateAvailableButton(playerIndex);
        }

        public void DisableHandlingSelectingCards()
        {
            _isInitHandle = false;
            _isHandlingSelectingCards = false;
            _currentAvailableButton.Clear();
            _currentSelectIndex = 0;
            _isAxisInUse = false;
        }

        public void UpdateAvailableButton(int playerIndex)
        {
            _currentAvailableButton.Clear();
            _currentSelectIndex = 0;

            if (playerIndex == 0) {
                foreach (GameObject obj in player1Cards) {
                    if (obj.activeSelf) {
                        _currentAvailableButton.Add(obj);
                    }
                }

            } else if (playerIndex == 1) {
                foreach (GameObject obj in player2Cards) {
                    if (obj.activeSelf) {
                        _currentAvailableButton.Add(obj);
                    }
                }
            }
        }

        public void HideAllSelectDialog()
        {
            foreach (GameObject obj in selectDialogs) {
                obj.SetActive(false);
            }
        }

        public void AlertCurrentPhase(GameController.Phase phase)
        {
            switch (phase) {
                case GameController.Phase.Shuffle:
                    imgPlayerTurn.sprite = gameController.Players[0].IsTurn ? spriteAllPlayerTurn[0] : spriteAllPlayerTurn[1];
                    StartCoroutine(nameof(ShowShufflePhaseAlert));
                break;

                case GameController.Phase.Battle:
                    StartCoroutine(nameof(ShowBattlePhaseAlert));
                break;
            }
        }

        public void AlertWarning(int cause)
        {
            ClearWarning();
            if (cause < imgWarning.Length) {
                imgWarning[cause].SetActive(true);
            }
        }

        public void ClearWarning()
        {
            foreach (GameObject obj in imgWarning) {
                obj.SetActive(false);
            }
        }

        public void ShowPointStatus(int targetPlayerIndex, string sign, int totalPoint) {
            statusViews[targetPlayerIndex].ShowStatus(sign, totalPoint);
        }

        public void SelectFirstButtonInPauseMenu()
        {
            if (gameController.CurrentPlayerIndex == 0) {
                _previousSelectedObj = player1Cards[0];

            } else if (gameController.CurrentPlayerIndex == 1) {
                _previousSelectedObj = player2Cards[0];

            }

            allEventSystem[1].gameObject.SetActive(false);
            allEventSystem[2].gameObject.SetActive(false);

            allEventSystem[0].gameObject.SetActive(true);
            allEventSystem[0].SetSelectedGameObject(btnMainMenuInPauseMenu);
        }

        public void SelectPreviousButton()
        {
            allEventSystem[0].gameObject.SetActive(false);

            if (gameController.CurrentPlayerIndex == 0) {
                allEventSystem[1].gameObject.SetActive(true);
                allEventSystem[2].gameObject.SetActive(false);

            } else {
                allEventSystem[1].gameObject.SetActive(false);
                allEventSystem[2].gameObject.SetActive(true);

            }

            allEventSystem[gameController.CurrentPlayerIndex + 1].SetSelectedGameObject(_previousSelectedObj);

        }


        public bool IsFieldCardsEmpty()
        {
            var isEmpty = true;

            switch (gameController.CurrentPlayerIndex) {
                case 0:
                    foreach (GameObject obj in player1Cards) {
                        if (obj.activeSelf) {
                            isEmpty = false;
                            break;

                        } 
                    }
                break;

                case 1:
                    foreach (GameObject obj in player2Cards) {
                        if (obj.activeSelf) {
                            isEmpty = false;
                            break;

                        } 
                    }
                break;
            }

            return isEmpty;
        }


        private void Update()
        {
            if (gameController) {
                if (gameController.IsGameInit && gameController.IsGameStart) {

                    if (!gameController.IsGameOver) {

                        imgCurrentPlayerTurn.sprite = gameController.Players[0].IsTurn ? spritePlayerTurn[0] : spritePlayerTurn[1];

                        imgCriticals[0].SetActive(gameController.Players[0].Health.Current <= GameController.MAX_CRITICAL_STACK);
                        imgCriticals[1].SetActive(gameController.Players[1].Health.Current <= GameController.MAX_CRITICAL_STACK);

                        if (Input.GetButtonDown("Player_Pause")) {

                            gameController.ToggleGamePause();
                            pausePanel.SetActive(gameController.IsGamePause);

                            if (pausePanel.gameObject.activeSelf) {
                                SelectFirstButtonInPauseMenu();

                            } else {
                                SelectPreviousButton();

                            }
                        }

                        for (int i = 0; i < disableDecksView.Length; i++) {

                            if (gameController.Players[i].DisableDeck.Cards.Count > 0) {
                                disableDecksView[i].SetActive(true);
                                txtCardNum[i].text = gameController.Players[i].DisableDeck.Cards[0].ToString();

                            } else {
                                disableDecksView[i].SetActive(false);

                            }
                        }

                        if (_isInitHandle) {

                            for (int i = 0; i < gameController.Players.Length; i++) {
                                if (gameController.Players[i].IsTurn) {
                                    _HandleSelectCards(i);
                                    break;
                                }
                            }

                            _isInitHandle = false;
                        }

                        if (_isHandlingSelectingCards) {
                            HandleSelectCardsFromPlayer(gameController.CurrentPlayerIndex);
                            
                        }

                    } else {
                        if (gameOverUI && gameplayUI) {
                            if (!_isInitShowGameOver) {
                                anims[0].SetBool("IsHurt", false);
                                anims[1].SetBool("IsHurt", false);
                                StartCoroutine(nameof(ShowGameOverUI));
                                _isInitShowGameOver = true;

                            }
                        }
                    }
                }
            }
        }

        private void _HandleSelectCards(int playerIndex)
        {
            var isNeedHandle = _IsNeedHandleSelecting(playerIndex);
            if (isNeedHandle) {

                if (playerIndex == 0) {
                    foreach (GameObject obj in player1Cards) {
                        if (obj.activeSelf) {
                            allEventSystem[1].SetSelectedGameObject(obj);
                        }
                        break;
                    }

                } else if (playerIndex == 1) {
                    foreach (GameObject obj in player2Cards) {
                        if (obj.activeSelf) {
                            allEventSystem[2].SetSelectedGameObject(obj);
                        }
                        break;
                    }

                }

                _isHandlingSelectingCards = true;
            }
        }

        private bool _IsNeedHandleSelecting(int playerIndex)
        {
            var isNeed = false;

            if (playerIndex == 0) {
                foreach (GameObject obj in player1Cards) {
                    if (!obj.activeSelf) {
                        isNeed = true;
                        break;
                    }
                }

            } else if (playerIndex == 1) {
                foreach (GameObject obj in player2Cards) {
                    if (!obj.activeSelf) {
                        isNeed = true;
                        break;
                    }
                }
            }

            return isNeed;
        }

        private void HandleSelectCardsFromPlayer(int playerIndex)
        {
            if (_currentAvailableButton.Count > 0) {
                if (playerIndex == 0) {

                    var axis = Input.GetAxisRaw("Player1_Vertical");
                    if (!_isAxisInUse) {
                        if (axis == 1) {
                            _currentSelectIndex = (_currentSelectIndex - 1) < 0 ? 0 : _currentSelectIndex - 1;
                            allEventSystem[1].SetSelectedGameObject(_currentAvailableButton[_currentSelectIndex]);
                            _isAxisInUse = true;

                        } else if (axis == -1) {
                            _currentSelectIndex = (_currentSelectIndex + 1) > (_currentAvailableButton.Count - 1) ? (_currentAvailableButton.Count - 1) : _currentSelectIndex + 1;
                            allEventSystem[1].SetSelectedGameObject(_currentAvailableButton[_currentSelectIndex]);
                            _isAxisInUse = true;
                        }
                    } else {
                        if (axis == 0) {
                            _isAxisInUse = false;

                        }
                    }

                } else if (playerIndex == 1) {
                    var axis = Input.GetAxisRaw("Player2_Vertical");
                    if (!_isAxisInUse) {
                        if (axis == 1) {
                            _currentSelectIndex = (_currentSelectIndex - 1) < 0 ? 0 : _currentSelectIndex - 1;
                            allEventSystem[2].SetSelectedGameObject(_currentAvailableButton[_currentSelectIndex]);
                            _isAxisInUse = true;

                        } else if (axis == -1) {
                            _currentSelectIndex = (_currentSelectIndex + 1) > (_currentAvailableButton.Count - 1) ? (_currentAvailableButton.Count - 1) : _currentSelectIndex + 1;
                            allEventSystem[2].SetSelectedGameObject(_currentAvailableButton[_currentSelectIndex]);
                            _isAxisInUse = true;
                        }
                    } else {
                        if (axis == 0) {
                            _isAxisInUse = false;

                        }
                    }
                }
            }
        }


        private IEnumerator ShowGameOverUI()
        {
            yield return new WaitForSeconds(1.3f);
            gameplayUI.SetActive(false);
            gameOverUI.SetActive(true);

            for (int i = 0; i < allEventSystem.Length; i++) {
                allEventSystem[i].gameObject.SetActive(false);
            }

            allEventSystem[0].gameObject.SetActive(true);
            allEventSystem[0].SetSelectedGameObject(btnRestart);
        }

        private IEnumerator ShowShufflePhaseAlert()
        {
            imgPlayerTurn.gameObject.SetActive(true);
            imgPhases[0].SetActive(true);
            yield return new WaitForSeconds(1.0f);
            imgPhases[0].SetActive(false);
            imgPlayerTurn.gameObject.SetActive(false);
        }

        private IEnumerator ShowBattlePhaseAlert()
        {
            imgPhases[1].SetActive(true);
            yield return new WaitForSeconds(1.0f);
            imgPhases[1].SetActive(false);
        }
    }
}
