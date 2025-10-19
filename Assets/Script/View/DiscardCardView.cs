using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SichuanDynasty.UI
{
    public class DiscardCardView : MonoBehaviour
    {
        [SerializeField]
        private int playerIndex;

        [SerializeField]
        private GameController gameController;

        [SerializeField]
        private Text txtCardNum;


        private void Update()
        {
            if (gameController) {

                if (gameController.IsGameInit && gameController.IsGameStart && !gameController.IsGameOver) {

                }
            }
        }
    }
}
