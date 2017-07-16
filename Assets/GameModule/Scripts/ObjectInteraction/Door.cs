using LastBastion.Game.Managers;
using UnityEngine;
using UnityEngine.Assertions;


namespace LastBastion.Game.ObjectInteraction
{
    /// <summary>
    /// Component that represents door behaviour.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Animator))]
    public class Door : MonoBehaviour, IInteractiveObject
    {
        #region Provate fields
        [SerializeField] DoorType doorType;
        [SerializeField] private bool isClosed = true;
        [SerializeField] private bool isLocked = false;
        [SerializeField] AudioClip doorOpenSound;
        [SerializeField] AudioClip doorCloseSound;
        [SerializeField] AudioClip doorLockedSound;
        private bool isBusy = false;
        private Animator animator;
        private AudioSource audioSource;
        private int openDoorTrigger;
        private int closeDoorTrigger;
        private int tryDoorTrigger;
        #endregion


        #region Public fields & properties
        /// <summary>Is door busy?</summary>
        public bool IsBusy { get { return isBusy; } }
        /// <summary>Is door locked?</summary>
        public bool IsClosed { get { return isClosed; } }
        /// <summary>Is door locked?</summary>
        public bool IsLocked
        {
            get { return isLocked; }
            set { isLocked = value; }
        }
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(doorOpenSound);
            Assert.IsNotNull(doorCloseSound);
            //Assert.IsNotNull(doorLockedSound);
        }

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            openDoorTrigger = Animator.StringToHash("OpenTheDoor");
            closeDoorTrigger = Animator.StringToHash("CloseTheDoor");
            tryDoorTrigger = Animator.StringToHash("TryTheDoor");
        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion


        #region Private methods
        /// <summary>
        /// Opens the door.
        /// </summary>
        private void OpenDoor()
        {
            animator.SetTrigger(openDoorTrigger);
            isClosed = false;
        }

        /// <summary>
        /// Closes the door.
        /// </summary>
        private void CloseDoor()
        {
            animator.SetTrigger(closeDoorTrigger);
            isClosed = true;
        }

        /// <summary>
        /// Tries the lock in the door.
        /// </summary>
        private void LockedDoor()
        {
            animator.SetTrigger(tryDoorTrigger);
        }

        /// <summary>
        /// Plays given sound.
        /// </summary>
        /// <param name="sound">Sound to play</param>
        private void PlaySound(AudioClip sound)
        {
            if (audioSource != null) audioSource.PlayOneShot(sound);
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Implementation of <see cref="IInteractiveObject"/> interface method.
        /// </summary>
        public void Interact()
        {
            if (isLocked) LockedDoor();
            else
            {
                if (isClosed) OpenDoor();
                else CloseDoor();
            }
        }

        /// <summary>
        /// Sets isBusy flag to true.
        /// </summary>
        public void SetBusyOn()
        {
            isBusy = true;
        }

        /// <summary>
        /// Sets isBusy flag to false.
        /// </summary>
        public void SetBusyOff()
        {
            isBusy = false;
        }

        /// <summary>
        /// Plays sound of opening the door.
        /// </summary>
        public void PlayOpenSound()
        {
            PlaySound(doorOpenSound);
        }

        /// <summary>
        /// Plays sound of closing the door.
        /// </summary>
        public void PlayCloseSound()
        {
            PlaySound(doorCloseSound);
        }

        /// <summary>
        /// Plays sound of trying to open locked door.
        /// </summary>
        public void PlayLockedSound()
        {
            if(doorLockedSound != null) PlaySound(doorLockedSound);
        }

        /// <summary>
        /// Plays squeak sound.
        /// </summary>
        public void PlaySqueakSound()
        {
            switch (doorType)
            {
                case DoorType.Wooden:
                    PlaySound(GameManager.instance.Assets.GetWoodenSqueakSound());
                    break;

                case DoorType.Metal:
                    PlaySound(GameManager.instance.Assets.GetMetalSqueakSound());
                    break;
            }
        }
        #endregion
    }
}
