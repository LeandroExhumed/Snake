using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Match
{
    public class MatchView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI winnerMessageText;
        [SerializeField]
        private Vector2 guideOffset;

        [SerializeField]
        private Transform canvas;

        private readonly Dictionary<int, TextMeshProUGUI> guides = new();

        public void SyncGuidePosition (int player, Vector2Int targetPosition)
        {
            if (!guides.ContainsKey(player))
            {
                TextMeshProUGUI guide = Instantiate(GetNewGuide(), canvas);
                guide.text = $"P{player}";
                guides.Add(player, guide);
            }

            Vector3 fixedPosition = new(targetPosition.x + guideOffset.x, targetPosition.y + guideOffset.y, 0);
            guides[player].transform.position = Camera.main.WorldToScreenPoint(fixedPosition);
        }

        public void SetWinnerMessage (string winner)
        {
            winnerMessageText.text = $"{winner} WON!";
        }

        public void SetWinnerMessageActive (bool value)
        {
            winnerMessageText.gameObject.SetActive(value);
        }

        public void RemoveGuide (int player)
        {
            Destroy(guides[player].gameObject);
            guides.Remove(player);
        }

        private TextMeshProUGUI GetNewGuide ()
        {
            return Resources.Load<TextMeshProUGUI>("Guide");
        }
    }
}