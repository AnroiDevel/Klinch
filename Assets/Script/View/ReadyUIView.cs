using UnityEngine;
using UnityEngine.UI;

namespace SichuanDynasty.UI
{
    public class ReadyUIView : MonoBehaviour
    {
        [SerializeField]
        private GameController gameController;

        [SerializeField]
        private Image[] imgReadyList;

        [SerializeField]
        private Sprite[] preReadySprites;

        [SerializeField]
        private Sprite[] postReadySprites;

        [SerializeField]
        private GameObject nextUI;


        private bool _isProcess = true;
        private bool[] _readyList;


        private void Awake()
        {
            _readyList = new bool[GameController.MAX_PLAYER_SUPPORT];
        }

        private void OnEnable()
        {
            _isProcess = true;

            if(_readyList == null || _readyList.Length != GameController.MAX_PLAYER_SUPPORT)
            {
                _readyList = new bool[GameController.MAX_PLAYER_SUPPORT];
            }

            for(int i = 0; i < _readyList.Length; i++)
            {
                _readyList[i] = false;

                if(imgReadyList != null && i < imgReadyList.Length &&
                   preReadySprites != null && i < preReadySprites.Length &&
                   imgReadyList[i] != null && preReadySprites[i] != null)
                {
                    imgReadyList[i].sprite = preReadySprites[i];
                }
            }
        }


        private void Update()
        {
            if(gameController && _isProcess)
            {
                if(gameController.IsGameInit)
                {

                    if(Input.GetButtonDown("StartPlayer1"))
                    {
                        ToggleReady(0);

                    }
                    else if(Input.GetButtonDown("StartPlayer2"))
                    {
                        ToggleReady(1);

                    }

                    CheckIsAllReady();
                }
            }
        }

        public void ToggleReady(int playerIndex)
        {
            if(_readyList == null || playerIndex < 0 || playerIndex >= _readyList.Length)
            {
                Debug.LogWarning($"ReadyUIView received invalid player index {playerIndex}.", this);
                return;
            }

            _readyList[playerIndex] = !_readyList[playerIndex];
            var isReady = _readyList[playerIndex];

            var sprites = isReady ? postReadySprites : preReadySprites;

            if(imgReadyList == null || playerIndex >= imgReadyList.Length || imgReadyList[playerIndex] == null)
            {
                Debug.LogWarning("ReadyUIView is missing Image references for ready indicators.", this);
                return;
            }

            if(sprites == null || playerIndex >= sprites.Length || sprites[playerIndex] == null)
            {
                Debug.LogWarning("ReadyUIView is missing sprite assignments for ready indicators.", this);
                return;
            }

            imgReadyList[playerIndex].sprite = sprites[playerIndex];
        }

        private void CheckIsAllReady()
        {
            var isReady = true;
            foreach(bool result in _readyList)
            {
                if(result == false)
                {
                    isReady = false;
                    break;
                }
            }
            if(isReady)
            {
                ChangeToNextUI();
            }
        }

        private void ChangeToNextUI()
        {
            gameObject.SetActive(false);
            nextUI.SetActive(true);
            _isProcess = false;
        }
    }
}
