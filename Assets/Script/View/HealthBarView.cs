using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SichuanDynasty.UI
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField]
        private int playerIndex;

        [SerializeField]
        private GameController gameController;

        [SerializeField]
        private Text txtHealths;


        private void Update()
        {
            if (gameController) {
                if (gameController.IsGameInit && gameController.IsGameStart && !gameController.IsGameOver) {
                    txtHealths.text = gameController.Players[playerIndex].Health.Current.ToString();
                }
            }
        }
    }
}
