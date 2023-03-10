using LastBastion.Game.Managers;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;


namespace LastBastion.Game.Plot
{
    /// <summary>
    /// Component that triggers start of the level outro.
    /// </summary>
    public class EndGameTrigger : MonoBehaviour
    {
        #region Private fields
        /// <summary>Glyph particle system, that is played on game end.</summary>
        [SerializeField] private ParticleSystem glyphParticle;
        /// <summary><see cref="PlotGoal"/> component that triggers this game object.</summary>
        [SerializeField] private PlotGoal triggeringGoal;
        /// <summary>Delay for loading new scene.</summary>
        [SerializeField] private float endGameDelay = 38f;
        /// <summary>Is end game trigger in player's range?</summary>
        private bool isInRange;
        /// <summary>Has end game trigger been activated?</summary>
        private bool wasActivated;
        /// <summary>Is end game trigger enabled?</summary>
        private bool isEnabled;
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(glyphParticle);
            Assert.IsNotNull(triggeringGoal);
            glyphParticle.gameObject.SetActive(false);
            isInRange = false;
            wasActivated = false;
            isEnabled = false;
            triggeringGoal.Triggered += () => { isEnabled = true; };
        }

        // Update is called once per frame
        void Update()
        {
            if (isEnabled && isInRange)
            {
                if (!wasActivated && Input.GetKeyDown(KeyCode.T))
                {
                    // start playing the level outro:
                    LevelManager.instance.OutroHasStarted();
                    glyphParticle.gameObject.SetActive(true);
                    StartCoroutine(EndGameCounter());
                    wasActivated = true;
                    // turn on end game panel visibility:
                    LevelManager.instance.SetEndGamePanelActivityStateTo(false);
                }
            }
        }
        
        // OnTriggerEnter is called when the Collider other enters the trigger
        private void OnTriggerEnter(Collider other)
        {
            if (isEnabled)
            {
                isInRange = true;
                LevelManager.instance.SetEndGamePanelActivityStateTo(isInRange);
            }
        }

        // OnTriggerExit is called when the Collider other has stopped touching the trigger
        private void OnTriggerExit(Collider other)
        {
            if (isEnabled)
            {
                isInRange = false;
                LevelManager.instance.SetEndGamePanelActivityStateTo(isInRange);
            }
        }
        #endregion


        #region Private methods
        /// <summary>
        /// Counts time to the end of the level.
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