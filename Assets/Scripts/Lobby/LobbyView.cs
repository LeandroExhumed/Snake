using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Match
{
    public class LobbyView : MonoBehaviour
    {
        public event Action OnIntroOver;

        [SerializeField]
        private Animator intro;
        [SerializeField]
        private TextMeshProUGUI lobbyMessage;

        private void Awake ()
        {
            StartCoroutine(WaitingForIntroOverRoutine());
        }

        public void SetLobbyMessageText (string message)
        {
            lobbyMessage.text = message;
        }

        public void ShowLobbyMessage ()
        {
            lobbyMessage.gameObject.SetActive(true);
        }

        private IEnumerator WaitingForIntroOverRoutine ()
        {
            yield return new WaitUntil(() => intro.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);
            OnIntroOver?.Invoke();
        }
    } 
}