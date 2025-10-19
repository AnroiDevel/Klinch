using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SichuanDynasty.UI
{
    public class PhaseView : MonoBehaviour
    {
        [SerializeField]
        private GameController gameController;

        [SerializeField]
        private Image imgPhase;

        [SerializeField]
        private Sprite[] imgAllPhases;


        private void Update()
        {
            if(gameController)
            {
                if(gameController.IsGameInit && gameController.IsGameStart && !gameController.IsGameOver)
                {
                    imgPhase.sprite = imgAllPhases[(int)gameController.CurrentPhase];
                }
            }
        }
    }
}
