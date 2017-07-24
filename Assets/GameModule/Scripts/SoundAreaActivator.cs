using LastBastion.Game.Managers;
using UnityEngine;


namespace LastBastion.Game
{
    /// <summary>
    /// Component that manages the IsActive status of assigned area.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class SoundAreaActivator : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private SoundManager watchedArea;
        #endregion


        #region MonoBehaviour methods
        // OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider
        private void OnCollisionExit(Collision collision)
        {
            if (GameManager.instance.ActiveSoundArea != null)
                GameManager.instance.ActiveSoundArea.IsEnabled = false;

            watchedArea.IsEnabled = true;
            GameManager.instance.ActiveSoundArea = watchedArea;
        }
        #endregion
    }
}
