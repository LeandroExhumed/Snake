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
        private TextMeshProUGUI joinMatchMessage;

        private const string JOIN_MATCH_MESSAGE_TEXT = "Hold any letter or number for {0} seconds to join the match.";

        private void Awake ()
        {
            StartCoroutine(WaitingForIntroOverRoutine());
        }

        public void SetJoinMatchMessageText (int holdDuration)
        {
            joinMatchMessage.text = string.Format(JOIN_MATCH_MESSAGE_TEXT, holdDuration);
        }

        public void SetJoinMatchMessageActive (bool value) => joinMatchMessage.gameObject.SetActive(value);

        private IEnumerator WaitingForIntroOverRoutine ()
        {
            yield return new WaitUntil(() => intro.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);
            OnIntroOver?.Invoke();
        }
    } 
}