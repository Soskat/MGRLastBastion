using _Decal;
using LastBastion.Game.Managers;
using UnityEngine;


namespace LastBastion.Game.ObjectInteraction
{
    /// <summary>
    /// Component that represents rune holder.
    /// </summary>
    [RequireComponent(typeof(Highlighter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class RuneHolder : MonoBehaviour, IInteractiveObject
    {
        #region Private fields
        [SerializeField] private bool isEnabled;
        private MeshRenderer meshRenderer;
        private MeshCollider orbCollider;
        private GameObject runeSign;
        #endregion


        #region Public fields & properties
        /// <summary>Is rune sign enabled?</summary>
        public bool IsEnabled { get { return isEnabled; } }
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            isEnabled = false;
            // disable object's mesh renderer and sphere collider:
            meshRenderer = GetComponent<MeshRenderer>();
            orbCollider = GetComponent<MeshCollider>();
            SetOrbEnabledFlag(false);
            // deactivate child sign object:
            runeSign = GetComponentInChildren<Decal>().gameObject;
            runeSign.SetActive(false);
        }
        #endregion


        #region Private methods
        /// <summary>
        /// Sets orb 'enable' property of MeshRenderer and SphereCollider components to given value.
        /// </summary>
        /// <param name="isEnable">New value of 'enable' property</param>
        private void SetOrbEnabledFlag(bool isEnable)
        {
            meshRenderer.enabled = isEnable;
            orbCollider.enabled = isEnable;
        }

        /// <summary>
        /// Activates rune sign decal on the floor.
        /// </summary>
        private void ActivateRuneSignOnTheFloor()
        {
            // disable orb's mesh renderer and collider:
            SetOrbEnabledFlag(false);
            // activate rune sign decal on the floor:
            runeSign.SetActive(true);
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Enables rune holder interaction.
        /// </summary>
        public void EnableInteractivity()
        {
            isEnabled = true;
            // if current plot goal is the last one, activate the orb:
            if (LevelManager.instance.CurrentGoalIsTheOrbGoal) ActivateOrb();
        }

        /// <summary>
        /// Activates the orb.
        /// </summary>
        public void ActivateOrb()
        {
            if (isEnabled) SetOrbEnabledFlag(true);
        }

        /// <summary>
        /// Behaviour on player interaction with object.
        /// </summary>
        public void Interact()
        {
            ActivateRuneSignOnTheFloor();
            LevelManager.instance.ActivatedRune();
        }
        #endregion
    }
}
