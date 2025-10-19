using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        private bool _isProcess;
        private bool _isTie;

        private RockPaperScissorState[] _results;


        public DecideFirstPlayerUIView()
        {
            _isHasWinner = false;
            _firstPlayerIndex = 0;
            _isProcess = true;
            _isTie = true;

            _results = new RockPaperScissorState[GameController.MAX_PLAYER_SUPPORT];
            for(int i = 0; i < _results.Length; i++)
            {
                _results[i] = RockPaperScissorState.None;
            }
        }


        private void Update()
        {
            _HandlePlayerInput();
        }

        private void _HandlePlayerInput()
        {
            if(gameController.IsGameInit && _isProcess)
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
                    StartCoroutine(nameof(ShowResult), _isTie);

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

        private void ShowResult(bool isTile)
        {
            for(int i = 0; i < resultPanels.Length; i++)
            {
                gamepadPanels[i].SetActive(false);
                resultPanels[i].SetActive(true);
            }

            for(int i = 0; i < _results.Length; i++)
            {
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

            if(isTile)
            {
                _isTie = false;
                for(int i = 0; i < _results.Length; i++)
                {
                    _results[i] = RockPaperScissorState.None;
                }
                StartCoroutine(nameof(ReShowUI));
            }
        }

        private IEnumerator ReShowUI()
        {
            yield return new WaitForSeconds(0.8f);

            for(int i = 0; i < resultPanels.Length; i++)
            {
                foreach(Transform child in resultPanels[i].transform)
                {
                    child.gameObject.SetActive(false);
                }
            }

            for(int i = 0; i < resultPanels.Length; i++)
            {
                gamepadPanels[i].SetActive(true);
                resultPanels[i].SetActive(false);
            }

            _isProcess = true;
        }

        private IEnumerator NextUI()
        {
            yield return new WaitForSeconds(2.0f);
            gameObject.SetActive(false);
            nextUI.SetActive(true);
            gameController.GameStart(_firstPlayerIndex);
        }
    }
}
