using LastBastion.Game.Managers;
using UnityEngine;
using UnityEngine.Assertions;


namespace LastBastion.Game.ObjectInteraction
{
    /// <summary>
    /// Component that manages game object visibility to player's interaction raycast test.
    /// </summary>
    public class FocusActivator : MonoBehaviour
    {
        #region Private variables
        [SerializeField] private Door parentObject;
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(parentObject);
        }

        // Use this for initialization
        void Start()
        {
            parentObject.OpenedDoorAction += () => { gameObject.layer = GameManager.instance.InteractiveObjectsLayer; };
            parentObject.ClosedDoorAction += () => { gameObject.layer = 0; };
            parentObject.EndedMovingAction += () => { GetComponent<FocusableObject>().SetOriginalPositionAndRotation(); };
            gameObject.layer = 0;
        }
        #endregion
    }
}
