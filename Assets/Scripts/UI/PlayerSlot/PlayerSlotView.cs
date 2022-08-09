using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace LeandroExhumed.SnakeGame.UI.PlayerSlot
{
    public class PlayerSlotView : MonoBehaviour
    {
        public event Action OnConfirmationKeyPressed;

        [SerializeField]
        private Image leftKeyImage;
        [SerializeField]
        private Image rightKeyImage;

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

        [SerializeField]
        private GameObject selectSnakeText;
        [SerializeField]
        private GameObject checkMarkIcon;

        private const float ARROW_HIGHLIGHT_DURATION = 0.5F;

        public void SetInputKeys (char leftKey, char rightKey)
        {
            leftKeyImage.sprite = GetKeySprite(leftKey);
            rightKeyImage.sprite = GetKeySprite(rightKey);
        }

        public void SetKeysIconsActive (bool value)
        {
            leftKeyImage.gameObject.SetActive(value);
            rightKeyImage.gameObject.SetActive(value);
        }

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

        public void SetSelectSnakeTextActive (bool value) => selectSnakeText.SetActive(value);

        public void SetOkIconActive (bool value) => checkMarkIcon.SetActive(value);

        private void Update ()
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                OnConfirmationKeyPressed?.Invoke();
            }
        }

        private Sprite GetSnakeBlockSprite (int blockID)
        {
            return Resources.Load<Sprite>($"Sprites/Blocks/{blockID}");
        }

        private Sprite GetKeySprite (char key)
        {
            return Resources.Load<Sprite>($"Sprites/Keys/{key}");
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