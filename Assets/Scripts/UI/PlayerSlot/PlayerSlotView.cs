using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace LeandroExhumed.SnakeGame.UI.PlayerSlot
{
    public class PlayerSlotView : MonoBehaviour
    {
        public event Action OnConfirmationKeyPressed;

        [SerializeField]
        private GameObject keysGroup;
        [SerializeField]
        private TextMeshProUGUI leftKeyText;
        [SerializeField]
        private TextMeshProUGUI rightKeyText;

        [SerializeField]
        private TextMeshProUGUI messageText;

        [Header("Snake")]
        [SerializeField]
        private GameObject snakePreview;
        [SerializeField]
        private Image[] blockImages;
        [SerializeField]
        private Image leftArrowImage;
        [SerializeField]
        private Image rightArrowImage;
        [SerializeField]
        private Color highlightedArrowColor;        

        private const float ARROW_HIGHLIGHT_DURATION = 0.2F;

        public void SetInputKeys (char leftKey, char rightKey)
        {
            leftKeyText.text = leftKey.ToString().ToUpper();
            rightKeyText.text = rightKey.ToString().ToUpper();
        }

        public void SetKeysIconsActive (bool value) => keysGroup.SetActive(value);

        public void SetSnakePreviewActive (bool value) => snakePreview.SetActive(value);

        public void ShowSnake (int[] blockIDs)
        {
            for (int i = 0; i < blockImages.Length; i++)
            {
                blockImages[i].overrideSprite = GetSnakeBlockSprite(blockIDs[i]);
            }
        }

        public void HighlightLeftArrow ()
        {
            StartCoroutine(ArrowHightlightingRoutine(leftArrowImage));
        }
        
        public void HighlightRightArrow ()
        {
            StartCoroutine(ArrowHightlightingRoutine(rightArrowImage));
        }

        public void SetArrowsActive (bool value)
        {
            leftArrowImage.gameObject.SetActive(value);
            rightArrowImage.gameObject.SetActive(value);
        }

        public void SetSlotMessageActive (bool value) => messageText.gameObject.SetActive(value);

        public void SetSlotMessageText (string text)
        {
            messageText.text = text;
        }

        private void Update ()
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                OnConfirmationKeyPressed?.Invoke();
            }
        }

        private Sprite GetSnakeBlockSprite (int blockID)
        {
            return Resources.Load<Sprite>($"Textures/Blocks/{blockID}");
        }

        private IEnumerator ArrowHightlightingRoutine (Image arrow)
        {
            Color startColor = arrow.color;
            arrow.color = highlightedArrowColor;
            yield return new WaitForSeconds(ARROW_HIGHLIGHT_DURATION);
            arrow.color = startColor;
        }
    } 
}