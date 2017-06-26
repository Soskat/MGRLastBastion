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
        [SerializeField] private Player.Player player;
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            player = GetComponentInParent<Player.Player>();
        }

        // Update is called once per frame
        void Update()
        {
            // simulate hand shaking:
            if (player.ArousalState == DataState.High)
            {
                float x = Random.Range(0.0f, player.ArousalModifier);
                float y = Random.Range(0.0f, player.ArousalModifier);
                float z = Random.Range(0.0f, player.ArousalModifier);
                // update transform rotation:
                transform.localRotation = Quaternion.Euler(x, y, z);
            }
        }
        #endregion
    }
}
