using LastBastion.Biofeedback;
using LastBastion.Game.Managers;
using UnityEngine;


namespace LastBastion.Game.Player
{
    /// <summary>
    /// Component that represents Player's hand.
    /// </summary>
    public class Hand : MonoBehaviour
    {
        #region Private fields
        [SerializeField] protected BiofeedbackController player;
        private DataState lastState;
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        protected void Start()
        {
            player = GetComponentInParent<BiofeedbackController>();
            lastState = DataState.None;
        }

        // Update is called once per frame
        protected void Update()
        {
            // simulate hand shaking:
            if (player.ArousalCurrentState == DataState.High)
            {
                float x = Random.Range(0.0f, player.ArousalCurrentModifier);
                float y = Random.Range(0.0f, player.ArousalCurrentModifier);
                float z = Random.Range(0.0f, player.ArousalCurrentModifier);
                // update transform rotation:
                transform.localRotation = Quaternion.Euler(x, y, z);

                if (lastState != DataState.High)
                {
                    lastState = DataState.High;
                    // save info about event:
                    if (GameManager.instance.AnalyticsEnabled)
                    {
                        LevelManager.instance.AddGameEvent(Analytics.EventType.Shaking);
                    }
                }
            }
        }
        #endregion
    }
}
