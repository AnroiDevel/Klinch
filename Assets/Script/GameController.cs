using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using SichuanDynasty.UI;

namespace SichuanDynasty
{
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] firstSelectedCards;

        [SerializeField]
        private EventSystem[] eventSystem;

        [SerializeField]
        private GameObject[] parentCards;

        [SerializeField]
        private UIManager uiManager;

        [SerializeField]
        private Animator[] anims;


        public const int MAX_PLAYER_SUPPORT = 2;
        public const int MAX_PLAYER_HEALTH_PER_GAME = 31;
        public const int MAX_PHASE_PER_PLAYER = 2;
        public const int MAX_FIELD_CARD_PER_GAME = 4;
        public const int MAX_HEAL_CARD = 2;
        public const int MAX_CRITICAL_STACK = 2;
        public const int MAX_CRITICAL_TURN = 2;

        public const float MAX_TIME_PER_PHASE = 60.0f;


        public bool IsGameInit { get { return _isGameInit; } }
        public bool IsGameStart { get { return _isGameStart; } }
        public bool IsGameOver { get { return _isGameOver; } }
        public bool IsGamePause { get { return _isGamePause; } }
        public bool IsInteractable { get { return _isInteractable; } }
        public bool IsExceedHealCard { get { return _isExceedHealCard; } }

        public int TotalTurn { get { return _totalTurn; } }
        public int CurrentPlayerIndex { get { return _currentPlayerIndex; } }
        public Phase CurrentPhase { get { return _currentPhase; } }

        public Player[] Players { get { return _players; } }

        public int[] FieldCardCache_1 { get { return _fieldCache_1; } }
        public int[] FieldCardCache_2 { get { return _fieldCache_2; } }

        public int TotalHealPoint { get { return _totalHealPoint; } }
        public int TotalAttackPoint { get { return _totalAttackPoint; } }


        public enum Phase
        {
            Shuffle,
            Battle,
            None
        }


        private int _firstPlayerIndex;
        private int _currentPlayerIndex;
        private int _totalTurn;

        private bool _isGameInit;
        private bool _isGameStart;
        private bool _isGameOver;
        private bool _isGamePause;

        private bool _isNextTurn;
        private bool _isHasWinner;

        private bool _isInitNextTurn;
        private bool _isInteractable;

        private bool _isExceedHealCard;

        private Phase _currentPhase;

        private Player[] _players;
        private Timer _timer;


        private List<int> _currentSelectedCardCache;

        private int[] _fieldCache_1;
        private int[] _fieldCache_2;

        private int _healCardStack;

        private int _totalHealPoint;
        private int _totalAttackPoint;

        private bool _isInitDelayNextTurn;


        public GameController()
        {
            _currentPlayerIndex = 0;
            _totalTurn = 0;
            _isGameInit = false;
            _isGameStart = false;
            _isGameOver = true;
            _isGamePause = false;
            _isNextTurn = false;
            _isHasWinner = false;
            _isInitNextTurn = false;
            _isInteractable = true;
            _isExceedHealCard = false;
            _currentPhase = Phase.None;
            _players = new Player[MAX_PLAYER_SUPPORT];
            _currentSelectedCardCache = new List<int>();
            _fieldCache_1 = new int[MAX_FIELD_CARD_PER_GAME];
            _fieldCache_2 = new int[MAX_FIELD_CARD_PER_GAME];
            _healCardStack = 0;
            _totalHealPoint = 0;
            _totalAttackPoint = 0;
            _isInitDelayNextTurn = false;
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void Restart()
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void GameReset()
        {
            _isGameInit = false;
            _isGameStart = false;
            _isGameOver = true;
        }

        public void GameInit()
        {
            _isGameOver = false;
            _isGameInit = true;
        }

        public void GameStart(int firstPlayerIndex)
        {
            _currentPlayerIndex = firstPlayerIndex;
            _players[firstPlayerIndex].SetTurn(true);
            _currentPhase = Phase.Shuffle;

            foreach(Player player in _players)
            {
                player.FirstDraw(MAX_FIELD_CARD_PER_GAME);
            }

            eventSystem[0].gameObject.SetActive(false);

            eventSystem[_currentPlayerIndex].gameObject.SetActive(true);
            eventSystem[_currentPlayerIndex].SetSelectedGameObject(firstSelectedCards[_currentPlayerIndex]);

            _fieldCache_1 = _players[0].FieldDeck.Cards.ToArray();
            _fieldCache_2 = _players[1].FieldDeck.Cards.ToArray();

            uiManager.AlertCurrentPhase(_currentPhase);

            _isGameStart = true;
            _isNextTurn = true;
        }

        public void GameOver()
        {
            _isGameOver = true;
        }

        public void ToggleGamePause()
        {
            _isGamePause = !_isGamePause;
            Time.timeScale = (_isGamePause) ? 0.0f : 1.0f;
        }

        public void NextPhase()
        {
            if(_currentPhase == Phase.Shuffle)
            {
                _currentPhase = Phase.Battle;
                uiManager.AlertCurrentPhase(_currentPhase);
            }

            SetInteractable(true);
            _currentSelectedCardCache.Clear();
            _players[_currentPlayerIndex].SelectedDeck.Cards.Clear();
        }

        public void ToggleSelect(int index)
        {
            var targetPlayer = _players[_currentPlayerIndex];
            var targetCard = 0;

            if(_currentPlayerIndex == 0)
            {
                targetCard = _fieldCache_1[index];

            }
            else if(_currentPlayerIndex == 1)
            {
                targetCard = _fieldCache_2[index];

            }

            switch(_currentPhase)
            {
                case Phase.Shuffle:
                    targetPlayer.NormalDeck.Cards.Add(targetCard);
                    targetPlayer.FieldDeck.Cards.Remove(targetCard);

                    var newCard = targetPlayer.NormalDeck.Cards[0];

                    targetPlayer.FieldDeck.Cards.Add(newCard);
                    targetPlayer.NormalDeck.Cards.Remove(newCard);

                    if(_currentPlayerIndex == 0)
                    {
                        _fieldCache_1 = targetPlayer.FieldDeck.Cards.ToArray();

                    }
                    else if(_currentPlayerIndex == 1)
                    {
                        _fieldCache_2 = targetPlayer.FieldDeck.Cards.ToArray();

                    }

                    break;

                case Phase.Battle:
                    if(_currentSelectedCardCache.Contains(targetCard))
                    {
                        _currentSelectedCardCache.Remove(targetCard);

                    }
                    else
                    {
                        _currentSelectedCardCache.Add(targetCard);

                    }

                    break;

                default:
                    break;
            }
        }

        public void SetInteractable(bool value)
        {
            _isInteractable = value;
        }


        private void Awake()
        {
            for(int i = 0; i < _players.Length; i++)
            {
                _players[i] = gameObject.AddComponent(typeof(Player)) as Player;
            }

            _timer = gameObject.AddComponent(typeof(Timer)) as Timer;
            _timer.SetMaxTime(MAX_TIME_PER_PHASE);
        }

        private void Update()
        {
            HandleGame();
        }

        private void HandleGame()
        {
            if(_isGameInit && _isGameStart && !_isGameOver)
            {
                anims[0].SetInteger("Health", _players[0].Health.Current);
                anims[1].SetInteger("Health", _players[1].Health.Current);

                if(!_isHasWinner)
                {

                    if(_isNextTurn)
                    {
                        _timer.Stop();
                        _timer.StartCountDown();
                        _isInitNextTurn = false;
                        _isInitDelayNextTurn = false;
                        _isNextTurn = false;
                    }

                    if(_timer.IsStarted)
                    {
                        if(!_timer.IsFinished)
                        {
                            _PhaseHandle();

                        }

                    }
                    else
                    {
                        if(_timer.IsFinished)
                        {
                            if(!_isInitNextTurn)
                            {
                                NextTurn();
                                _isInitNextTurn = true;
                            }
                        }
                    }

                }
                else
                {
                    GameOver();
                    _timer.Stop();

                }
            }
        }

        private void _PhaseHandle()
        {
            switch(_currentPhase)
            {
                case Phase.Shuffle:
                    ShufflePhaseHandle();

                    break;

                case Phase.Battle:
                    BattlePhaseHandle();

                    break;

                default:
                    break;
            }
        }

        private void ShufflePhaseHandle()
        {
            if(_currentPlayerIndex == 0)
            {
                if(Input.GetButtonDown("Player1_Y"))
                {
                    NextPhase();
                }

            }
            else if(_currentPlayerIndex == 1)
            {
                if(Input.GetButtonDown("Player2_Y"))
                {
                    NextPhase();
                }

            }
        }

        private void BattlePhaseHandle()
        {
            if(_currentPlayerIndex == 0)
            {
                if(Input.GetButtonDown("Player1_Y"))
                {
                    if(!_isInitNextTurn)
                    {
                        NextTurn();
                        _isInitNextTurn = true;
                    }

                }
                else if(Input.GetButtonDown("Player1_X"))
                {
                    Attack(1);

                }
                else if(Input.GetButtonDown("Player1_B"))
                {
                    Heal(0);

                }

            }
            else if(_currentPlayerIndex == 1)
            {
                if(Input.GetButtonDown("Player2_Y"))
                {
                    if(!_isInitNextTurn)
                    {
                        NextTurn();
                        _isInitNextTurn = true;
                    }

                }
                else if(Input.GetButtonDown("Player2_X"))
                {
                    Attack(0);

                }
                else if(Input.GetButtonDown("Player2_B"))
                {
                    Heal(1);

                }
            }
        }

        private void MoveUsedCard()
        {
            if(_currentSelectedCardCache.Count > 0)
            {
                for(int i = 0; i < _currentSelectedCardCache.Count; i++)
                {
                    _players[_currentPlayerIndex].DisableDeck.Cards.Add(_currentSelectedCardCache[i]);
                    _players[_currentPlayerIndex].FieldDeck.Cards.Remove(_currentSelectedCardCache[i]);
                }

                uiManager.InitHandleSelectingCards(_currentPlayerIndex);
                uiManager.UpdateAvailableButton(_currentPlayerIndex);
            }
        }

        private void Attack(int targetIndex)
        {
            var isAttakAble = IsAttackAble(targetIndex);

            if(isAttakAble)
            {

                uiManager.ClearWarning();
                SetActivateCard(_currentPlayerIndex, false);

                var totalPoint = 0;
                for(int i = 0; i < _currentSelectedCardCache.Count; i++)
                {
                    totalPoint += _currentSelectedCardCache[i];

                }

                MoveUsedCard();
                _currentSelectedCardCache.Clear();

                if(totalPoint > 0)
                {
                    anims[_currentPlayerIndex].Play("Attack");
                    StartCoroutine(nameof(ShowStatusCallBack), totalPoint);
                }

                ReHightlightCard();

                if(uiManager.IsFieldCardsEmpty())
                {
                    if(!_isInitDelayNextTurn)
                    {
                        StartCoroutine(nameof(DelayBeforeNextTurn));
                        _isInitDelayNextTurn = true;
                    }
                }

            }
            else
            {
                uiManager.AlertWarning(2);

            }
        }

        public void Attack()
        {
            if(_currentPlayerIndex == 0)
            {
                Attack(1);
            }
            else if(_currentPlayerIndex == 1)
            {
                Attack(0);
            }
        }

        public void Heal()
        {
            Heal(_currentPlayerIndex);
        }

        private bool IsAttackAble(int targetIndex)
        {
            var totalPoint = 0;
            foreach(int point in _currentSelectedCardCache)
            {
                totalPoint += point;
            }

            if(totalPoint <= 0)
            {
                return false;
            }

            return totalPoint <= _players[targetIndex].Health.Current;
        }

        private void Heal(int targetIndex)
        {
            var isHealable = IsHealable();

            if(isHealable)
            {

                uiManager.ClearWarning();
                SetActivateCard(_currentPlayerIndex, false);

                var totalPoint = 0;

                foreach(int point in _currentSelectedCardCache)
                {
                    totalPoint += point;
                }

                MoveUsedCard();
                _currentSelectedCardCache.Clear();

                _players[targetIndex].Health.Restore(totalPoint);

                if(totalPoint > 0)
                {
                    uiManager.ShowPointStatus(targetIndex, "+", totalPoint);

                }

                ReHightlightCard();

                if(uiManager.IsFieldCardsEmpty())
                {
                    if(!_isInitDelayNextTurn)
                    {
                        StartCoroutine("_DelayBeforeNextTurn");
                        _isInitDelayNextTurn = true;
                    }
                }
            }
        }

        private bool IsHealable()
        {
            var totalPoint = 0;

            if(_currentSelectedCardCache.Count > MAX_HEAL_CARD)
            {
                _isExceedHealCard = true;
                uiManager.AlertWarning(1);
                return false;

            }
            else
            {
                if(_healCardStack < MAX_HEAL_CARD)
                {

                    if(_currentSelectedCardCache.Count <= (MAX_HEAL_CARD - _healCardStack))
                    {

                        foreach(int point in _currentSelectedCardCache)
                        {
                            totalPoint += point;
                        }

                        if((_players[_currentPlayerIndex].Health.Current + totalPoint) <= MAX_PLAYER_HEALTH_PER_GAME)
                        {
                            _healCardStack += _currentSelectedCardCache.Count;
                            _isExceedHealCard = _healCardStack >= MAX_HEAL_CARD;
                            return true;

                        }
                        else
                        {
                            uiManager.AlertWarning(0);
                            return false;

                        }
                    }
                    else
                    {
                        _isExceedHealCard = true;
                        uiManager.AlertWarning(1);
                        return false;

                    }

                }
                else
                {
                    _isExceedHealCard = true;
                    uiManager.AlertWarning(1);
                    return false;

                }
            }
        }

        private void CheckWinner()
        {
            for(int i = 0; i < _players.Length; i++)
            {
                if(_players[i].Health.Current <= 0)
                {

                    if(i == 0)
                    {
                        _players[0].SetWin(false);
                        _players[1].SetWin(true);

                    }
                    else if(i == 1)
                    {
                        _players[0].SetWin(true);
                        _players[1].SetWin(false);

                    }

                    _isHasWinner = true;
                    break;
                }
            }
        }

        private void SetActivateCard(int playerIndex, bool isActivate)
        {
            foreach(Transform cardObj in parentCards[playerIndex].gameObject.transform)
            {
                var view = cardObj.gameObject.GetComponent<FieldCardView>();
                if(view.IsSelected)
                {
                    cardObj.gameObject.SetActive(isActivate);
                }
            }
        }

        private void DrawNewCard(int playerIndex)
        {
            var totalDraw = MAX_FIELD_CARD_PER_GAME - _players[playerIndex].FieldDeck.Cards.Count;

            for(int i = 0; i < totalDraw; i++)
            {
                if(_players[playerIndex].NormalDeck.Cards.Count == 0)
                {
                    if(_players[playerIndex].DisableDeck.Cards.Count == 0)
                    {
                        break;
                    }

                    _players[playerIndex].NormalDeck.Cards.AddRange(_players[playerIndex].DisableDeck.Cards);
                    _players[playerIndex].DisableDeck.Cards.Clear();
                }

                var normalDeck = _players[playerIndex].NormalDeck.Cards;
                var newCardIndex = Random.Range(0, normalDeck.Count);
                var newCard = normalDeck[newCardIndex];

                _players[playerIndex].FieldDeck.Cards.Add(newCard);
                normalDeck.RemoveAt(newCardIndex);
            }

            if(playerIndex == 0)
            {
                _fieldCache_1 = _players[playerIndex].FieldDeck.Cards.ToArray();

            }
            else if(playerIndex == 1)
            {
                _fieldCache_2 = _players[playerIndex].FieldDeck.Cards.ToArray();

            }

            if(_players[playerIndex].DisableDeck.Cards.Count > 0)
            {
                _players[playerIndex].NormalDeck.Cards.AddRange(_players[playerIndex].DisableDeck.Cards);
                _players[playerIndex].DisableDeck.Cards.Clear();
            }
        }

        public void NextTurn()
        {
            _isInitNextTurn = true;
            _timer.Stop();
            uiManager.DisableHandlingSelectingCards();
            uiManager.HideAllSelectDialog();
            StartCoroutine(nameof(NextTurnCallBack));
        }

        private void ChangePlayer()
        {
            _currentSelectedCardCache.Clear();
            _players[_currentPlayerIndex].SelectedDeck.Cards.Clear();

            _players[_currentPlayerIndex].SetTurn(false);
            eventSystem[_currentPlayerIndex].gameObject.SetActive(false);

            _currentPlayerIndex = (_currentPlayerIndex == (_players.Length - 1)) ? 0 : _currentPlayerIndex + 1;

            _players[_currentPlayerIndex].SetTurn(true);
            eventSystem[_currentPlayerIndex].gameObject.SetActive(true);

            eventSystem[_currentPlayerIndex].SetSelectedGameObject(firstSelectedCards[_currentPlayerIndex]);
        }

        private void ReHightlightCard()
        {
            foreach(Transform cardObj in parentCards[_currentPlayerIndex].gameObject.transform)
            {
                if(cardObj.gameObject.activeSelf)
                {
                    eventSystem[_currentPlayerIndex].SetSelectedGameObject(cardObj.gameObject);
                    break;
                }
            }
        }

        private void PlayHurtAnimation(int targetIndex)
        {
            anims[targetIndex].Play("Hurt");
        }

        private IEnumerator ShowStatusCallBack(int totalPoint)
        {
            var targetIndex = _currentPlayerIndex == 0 ? 1 : 0;
            if(totalPoint == _players[targetIndex].Health.Current)
            {
                _players[targetIndex].Health.Remove(totalPoint);
            }

            yield return new WaitForSeconds(0.7f);

            if(_players[targetIndex].Health.Current > 0)
            {
                _players[targetIndex].Health.Remove(totalPoint);
            }

            uiManager.ShowPointStatus(targetIndex, "-", totalPoint);
            anims[targetIndex].Play("Hit");

            CheckWinner();
        }

        private IEnumerator DelayBeforeNextTurn()
        {
            yield return new WaitForSeconds(1.3f);
            if(!_isInitNextTurn)
            {
                NextTurn();
                _isInitNextTurn = true;
            }
        }

        private IEnumerator NextTurnCallBack()
        {
            yield return new WaitForSeconds(0.8f);
            _totalTurn++;
            _healCardStack = 0;
            _isExceedHealCard = false;
            DrawNewCard(_currentPlayerIndex);
            SetActivateCard(_currentPlayerIndex, true);
            ChangePlayer();
            _currentPhase = Phase.Shuffle;
            uiManager.ClearWarning();
            uiManager.AlertCurrentPhase(_currentPhase); // <-- test..
            _isNextTurn = true;
        }
    }
}
