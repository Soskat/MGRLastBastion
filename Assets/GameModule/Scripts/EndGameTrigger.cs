using LastBastion.Game.Managers;
using LastBastion.Game.Player;
using UnityEngine;
using UnityEngine.Assertions;


namespace LastBastion.Game
{
    public class EndGameTrigger : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private ParticleSystem glyphParticle;
        [SerializeField] private double delayTime = 2f;
        private bool isInRange;
        private int cooldownTime;
        private int timeToEnd;
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(glyphParticle);
            glyphParticle.gameObject.SetActive(false);
            isInRange = false;
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (isInRange)
            {
                timeToEnd++;
                if (timeToEnd > cooldownTime)
                {
                    // start playing the level outro:
                    LevelManager.instance.IsOutroOn = true;
                    LevelManager.instance.Player.GetComponent<FirstPersonController_Edited>().IsOutroOn = true;
                    glyphParticle.gameObject.SetActive(true);
                }
            }
        }

        // OnTriggerEnter is called when the Collider other enters the trigger
        private void OnTriggerEnter(Collider other)
        {
            isInRange = true;
            cooldownTime = (int)(delayTime / Time.deltaTime);
            timeToEnd = 0;
        }

        // OnTriggerExit is called when the Collider other has stopped touching the trigger
        private void OnTriggerExit(Collider other)
        {
            isInRange = false;
        }
        #endregion
    }
}
