using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SichuanDynasty.UI
{
    public class TimerView : MonoBehaviour
    {
        [SerializeField]
        private GameController gameController;

        [SerializeField]
        private Text txtTimer;


        private void Update()
        {
            if (gameController && txtTimer) {
                if (gameController.IsGameInit && gameController.IsGameStart) {
                    txtTimer.text = gameController.GetComponent<Timer>().TimeLeft.ToString();
                }
            }
        }
    }
}
