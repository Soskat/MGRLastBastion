using LastBastion.Biofeedback;
using UnityEngine;


namespace LastBastion.Game
{
    /// <summary>
    /// Component that represents Player's hand.
    /// </summary>
    public class Hand : MonoBehaviour
    {
        #region Private fields
        [SerializeField] protected Player.Player player;
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        protected void Start()
        {
            player = GetComponentInParent<Player.Player>();
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
            }
        }
        #endregion
    }
}
