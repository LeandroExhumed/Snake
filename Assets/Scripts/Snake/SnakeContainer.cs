using UnityEngine;

namespace LeandroExhumed.SnakeGame.Snake
{
    public class SnakeContainer : MonoBehaviour
    {
        private BodyPartController controller;

        public void Install ()
        {
            ResolveMVC();
        }

        private void ResolveMVC ()
        {
            BodyPartModel model = new();
            BodyPartView view = GetComponent<BodyPartView>();
            controller = new BodyPartController(model, view);

            GetComponent<BodyPartFacade>().Constructor(model, controller);
        }
    }
}