using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace LastBastion.Game.Managers
{
    /// <summary>
    /// Component that manages Level scene behaviour.
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private string sceneName;
        [SerializeField] private int collectedRunes = 0;
        [SerializeField] private int maxRunesAmount = 0;

        // test:
        [SerializeField] private Text runesText;
        #endregion


        #region Public fields & properties
        /// <summary>Fixed max amount of the runes that player can find in this level.</summary>
        public int MaxRunesAmount { get { return maxRunesAmount; } }
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            GameManager.instance.LevelManager = this;

            // test - update GUI:
            runesText.text = maxRunesAmount.ToString();
        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion


        #region Public methods
        /// <summary>
        /// Actions after finding a rune.
        /// </summary>
        public void FoundRune()
        {
            // update runes count:
            maxRunesAmount++;
            // test - update GUI:
            runesText.text = maxRunesAmount.ToString();
        }
        #endregion
    }
}
