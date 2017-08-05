using LastBastion.Game.Managers;
using UnityEngine;


namespace LastBastion.Game.ObjectInteraction
{
    public class Rune : MonoBehaviour, IInteractiveObject
    {
        #region Public methods
        public void Interact()
        {
            // informa that new rune was found:
            GameManager.instance.LevelManager.FoundRune();
            // destroy this game object:
            Destroy(gameObject);
        }
        #endregion
    }
}
