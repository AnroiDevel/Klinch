using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace SichuanDynasty.UI
{
    public class DecideFirstPlayerUIView : MonoBehaviour
    {
        [SerializeField]
        private GameController gameController;

        [SerializeField]
        private GameObject nextUI;

        [SerializeField]
        private GameObject[] gamepadPanels;

        [SerializeField]
        private GameObject[] resultPanels;


        private enum RockPaperScissorState
        {
            None,
            Rock,
            Paper,
            Scissor
        }


        private int _firstPlayerIndex;

        private bool _isHasWinner;
        private bool _isProcess = true;
        private bool _isTie = true;

        private RockPaperScissorState[] _results;


        private void Awake()
        {
            _isHasWinner = false;
            _firstPlayerIndex = 0;

            _results = new RockPaperScissorState[GameController.MAX_PLAYER_SUPPORT];

            ResetResults();
        }

        private void OnEnable()
        {
            _isHasWinner = false;
            _isProcess = true;
            _isTie = true;
            ResetResults();

            if(gamepadPanels != null)
            {
                foreach(var panel in gamepadPanels)
                {
                    if(panel != null)
                    {
                        panel.SetActive(true);
                    }
                }
            }

            if(resultPanels != null)
            {
                foreach(var panel in resultPanels)
                {
                    if(panel == null)
                    {
                        continue;
                    }

                    foreach(Transform child in panel.transform)
                    {
                        child.gameObject.SetActive(false);
                    }

                    panel.SetActive(false);
                }
            }
        }


        private void Update()
        {
            HandlePlayerInput();
        }


        private void HandlePlayerInput()
        {
            if(gameController != null && gameController.IsGameInit && _isProcess)
            {

                if(Input.GetButtonDown("Player1_X") || Input.GetKeyDown(KeyCode.Q))
                {
                    _results[0] = RockPaperScissorState.Rock;

                }
                else if(Input.GetButtonDown("Player1_Y"))
                {
                    _results[0] = RockPaperScissorState.Scissor;

                }
                else if(Input.GetButtonDown("Player1_B"))
                {
                    _results[0] = RockPaperScissorState.Paper;

                }

                if(Input.GetButtonDown("Player2_X"))
                {
                    _results[1] = RockPaperScissorState.Rock;

                }
                else if(Input.GetButtonDown("Player2_Y") || Input.GetKeyDown(KeyCode.O))
                {
                    _results[1] = RockPaperScissorState.Scissor;

                }
                else if(Input.GetButtonDown("Player2_B"))
                {
                    _results[1] = RockPaperScissorState.Paper;

                }

                CheckWinner();
            }
        }


        private void CheckWinner()
        {
            if((_results[0] != RockPaperScissorState.None) && (_results[1] != RockPaperScissorState.None))
            {

                if(_results[0] == _results[1])
                {
                    _isTie = true;
                    _isHasWinner = false;
                    ShowResult(_isTie);

                }
                else
                {

                    switch(_results[0])
                    {
                        case RockPaperScissorState.Rock:
                            if(_results[1] == RockPaperScissorState.Paper)
                            {
                                _firstPlayerIndex = 1;

                            }
                            else if(_results[1] == RockPaperScissorState.Scissor)
                            {
                                _firstPlayerIndex = 0;

                            }
                            break;

                        case RockPaperScissorState.Paper:
                            if(_results[1] == RockPaperScissorState.Scissor)
                            {
                                _firstPlayerIndex = 1;

                            }
                            else if(_results[1] == RockPaperScissorState.Rock)
                            {
                                _firstPlayerIndex = 0;

                            }
                            break;

                        case RockPaperScissorState.Scissor:
                            if(_results[1] == RockPaperScissorState.Rock)
                            {
                                _firstPlayerIndex = 1;

                            }
                            else if(_results[1] == RockPaperScissorState.Paper)
                            {
                                _firstPlayerIndex = 0;

                            }
                            break;

                        default:
                            break;
                    }

                    _isTie = false;
                    ShowResult(_isTie);
                    _isHasWinner = true;
                }
            }

            if(_isHasWinner)
            {
                _isProcess = false;
                StartCoroutine(nameof(NextUI));
            }
        }

        private void ShowResult(bool isTie)
        {
            if(gamepadPanels == null || resultPanels == null || _results == null)
            {
                Debug.LogWarning("DecideFirstPlayerUIView is missing panel assignments.", this);
                return;
            }

            var length = Mathf.Min(Mathf.Min(gamepadPanels.Length, resultPanels.Length), _results.Length);

            for(int i = 0; i < length; i++)
            {
                if(gamepadPanels[i] != null)
                {
                    gamepadPanels[i].SetActive(false);
                }

                if(resultPanels[i] != null)
                {
                    resultPanels[i].SetActive(true);
                }
            }

            for(int i = 0; i < length; i++)
            {
                if(resultPanels[i] == null)
                {
                    continue;
                }

                switch(_results[i])
                {
                    case RockPaperScissorState.Rock:
                        resultPanels[i].transform.GetChild(0).gameObject.SetActive(true);

                        break;

                    case RockPaperScissorState.Paper:
                        resultPanels[i].transform.GetChild(1).gameObject.SetActive(true);

                        break;

                    case RockPaperScissorState.Scissor:
                        resultPanels[i].transform.GetChild(2).gameObject.SetActive(true);

                        break;

                    default:
                        break;
                }
            }

            if(isTie)
                if(isTie)
                {
                    _isTie = false;
                    ResetResults();
                    StartCoroutine(nameof(ReShowUI));
                }
        }

        private void ResetResults()
        {
            if(_results == null)
            {
                return;
            }

            for(int i = 0; i < _results.Length; i++)
            {
                _results[i] = RockPaperScissorState.None;
            }
        }

        private IEnumerator ReShowUI()
        {
            yield return new WaitForSeconds(0.8f);

            if(resultPanels != null)
            {
                for(int i = 0; i < resultPanels.Length; i++)
                {
                    if(resultPanels[i] == null)
                    {
                        continue;
                    }

                    foreach(Transform child in resultPanels[i].transform)
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }

            var length = Mathf.Min(gamepadPanels != null ? gamepadPanels.Length : 0, resultPanels != null ? resultPanels.Length : 0);

            for(int i = 0; i < length; i++)
            {
                if(gamepadPanels != null && gamepadPanels[i] != null)
                {
                    gamepadPanels[i].SetActive(true);
                }

                if(resultPanels != null && resultPanels[i] != null)
                {
                    resultPanels[i].SetActive(false);
                }
            }

            _isProcess = true;
        }

        private IEnumerator NextUI()
        {
            yield return new WaitForSeconds(2.0f);
            gameObject.SetActive(false);
            if(nextUI != null)
            {
                nextUI.SetActive(true);
            }
            if(gameController != null)
            {
                gameController.GameStart(_firstPlayerIndex);
            }
        }
    }
}
