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

            if(plus != null)
            {
                plus.SetActive(isPlus);
            }

            if(minus != null)
            {
                minus.SetActive(isMinus);
            }

            var digitArray = totalPoint.ToString().ToArray();

            if(digits == null || spriteOneToNine == null)
            {
                Debug.LogError("StatusView is missing digit or sprite assignments.", this);
                return;
            }

            if(digitArray.Length > digits.Length)
            {
                Debug.LogWarning($"StatusView has only {digits.Length} digit slots, but value '{totalPoint}' requires {digitArray.Length}.", this);
                digitArray = digitArray.Take(digits.Length).ToArray();
            }


            for(int i = 0; i < digitArray.Length; i++) {

                var value = digitArray[i] - '0';

                if(value < 0 || value >= spriteOneToNine.Length)
                {
                    Debug.LogWarning($"Digit '{digitArray[i]}' has no configured sprite.", this);
                    continue;
                }

                if(spriteOneToNine[value] == null)
                {
                    Debug.LogWarning($"Sprite for digit '{digitArray[i]}' is not assigned.", this);
                    continue;
                }

                if(digits[i] == null)
                {
                    Debug.LogWarning($"Digit image at index {i} is not assigned.", this);
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
            if(digits == null)
            {
                return;
            }

            for(int i = 0; i < digits.Length; i++)
            {
                if(digits[i] != null)
                {
                    digits[i].gameObject.SetActive(false);
                }
            }
        }

        public void HideAllSign()
        {
            plus.SetActive(false);
            minus.SetActive(false);
            if(plus != null)
            {
                plus.SetActive(false);
            }

            if(minus != null)
            {
                minus.SetActive(false);
            }
        }

        private IEnumerator ShowStatusCallBack()
        {
            yield return new WaitForSeconds(0.6f);
            HideAllDigits();
            HideAllSign();
        }
    }
}
