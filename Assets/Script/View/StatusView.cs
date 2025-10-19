using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SichuanDynasty.UI
{
    public class StatusView : MonoBehaviour
    {
        [SerializeField]
        private GameObject plus;

        [SerializeField]
        private GameObject minus;

        [SerializeField]
        private Image[] digits;

        [SerializeField]
        private Sprite[] spriteOneToNine;

        [SerializeField]
        private Color colorPlus;

        [SerializeField]
        private Color colorMinus;


        public void ShowStatus(string sign, int totalPoint)
        {
            HideAllDigits();
            HideAllSign();

            var isPlus = (sign == "+");
            var isMinus = (sign == "-");

            if(!isPlus && !isMinus)
            {
                Debug.LogWarning($"Unknown status sign '{sign}'. Expected '+' or '-'", this);
            }

            plus.SetActive(isPlus);
            minus.SetActive(isMinus);

            var digitArry = totalPoint.ToString().ToArray();

            if(digits == null || spriteOneToNine == null)
            {
                Debug.LogError("StatusView is missing digit or sprite assignments.", this);
                return;
            }

            if(digitArry.Length > digits.Length)
            {
                Debug.LogWarning($"StatusView has only {digits.Length} digit slots, but value '{totalPoint}' requires {digitArry.Length}.", this);
                digitArry = digitArry.Take(digits.Length).ToArray();
            }


            for(int i = 0; i < digitArry.Length; i++) {

                var value = Convert.ToInt32(digitArry[i]);
                value -= 48;

                if(value < 0 || value >= spriteOneToNine.Length)
                {
                    Debug.LogWarning($"Digit '{digitArry[i]}' has no configured sprite.", this);
                    continue;
                }

                digits[i].color = isPlus ? colorPlus : colorMinus;
                digits[i].sprite = spriteOneToNine[value];
                digits[i].gameObject.SetActive(true);
            }

            StartCoroutine(nameof(ShowStatusCallBack));
        }

        public void HideAllDigits()
        {
            for (int i = 0; i < digits.Length; i++) {
                digits[i].gameObject.SetActive(false);
            }
        }

        public void HideAllSign()
        {
            plus.SetActive(false);
            minus.SetActive(false);
        }


        private IEnumerator ShowStatusCallBack()
        {
            yield return new WaitForSeconds(0.6f);
            HideAllDigits();
            HideAllSign();
        }
    }
}
