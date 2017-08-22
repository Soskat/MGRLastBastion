using LastBastion.Game.Managers;
using LastBastion.Game.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;


namespace LastBastion.Game
{
    /// <summary>
    /// Component that triggers start of the level outro.
    /// </summary>
    public class EndGameTrigger : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private ParticleSystem glyphParticle;
        [SerializeField] private float endGameDelay = 38f;
        [SerializeField] private float outroStartDelay = 2f;
        private bool isInRange;
        private bool wasActivated;
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
            wasActivated = false;
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!wasActivated && isInRange)
            {
                timeToEnd++;
                if (timeToEnd > cooldownTime)
                {
                    // start playing the level outro:
                    LevelManager.instance.IsOutroOn = true;
                    LevelManager.instance.Player.GetComponent<FirstPersonController_Edited>().IsOutroOn = true;
                    glyphParticle.gameObject.SetActive(true);
                    StartCoroutine(EndGameCounter());
                    wasActivated = true;
                }
            }
        }

        // OnTriggerEnter is called when the Collider other enters the trigger
        private void OnTriggerEnter(Collider other)
        {
            isInRange = true;
            cooldownTime = (int)(outroStartDelay / Time.deltaTime);
            timeToEnd = 0;
        }

        // OnTriggerExit is called when the Collider other has stopped touching the trigger
        private void OnTriggerExit(Collider other)
        {
            isInRange = false;
        }
        #endregion


        #region Private methods
        /// <summary>
        /// Counts time to end the level.
        /// </summary>
        /// <returns></returns>
        private IEnumerator EndGameCounter()
        {
            yield return new WaitForSeconds(endGameDelay);
            // play the riser sound (which triggers camera fading out):
            LevelManager.instance.Player.GetComponent<PlayerAudioManager>().PlayRiserSound();
            yield return new WaitForSeconds(LevelManager.instance.Player.GetComponent<PlayerAudioManager>().RiserSoundLenght);
            // inform LevelManager, that level has ended:
            LevelManager.instance.EndLevel();
        }
        #endregion
    }
}
