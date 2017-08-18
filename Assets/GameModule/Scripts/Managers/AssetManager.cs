using LastBastion.Game.Plot;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace LastBastion.Game.Managers
{
    /// <summary>
    /// Component that manages common assets resources.
    /// </summary>
    public class AssetManager : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private Color highlightColor;
        [SerializeField] private Color hintColor;
        [SerializeField] private float interactionRange = 2f;
        [SerializeField] private float hintRange = 4f;
        [SerializeField] private List<AudioClip> metalDoorSqueak;
        [SerializeField] private List<AudioClip> woodenDoorSqueak;
        [SerializeField] private List<AudioClip> footstepsGravel;
        [SerializeField] private Goals goals;
        private int footstepGravelIndex;
        #endregion


        #region Public fields
        /// <summary>Highlight color.</summary>
        public Color HighlightColor { get { return highlightColor; } }
        /// <summary>Hint color.</summary>
        public Color HintColor { get { return hintColor; } }
        /// <summary>Interaction range.</summary>
        public float InteractionRange { get { return interactionRange; } }
        /// <summary>Hint range.</summary>
        public float HintRange { get { return hintRange; } }
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            // load sounds:
            metalDoorSqueak = new List<AudioClip>();
            woodenDoorSqueak = new List<AudioClip>();
            footstepsGravel = new List<AudioClip>();
            metalDoorSqueak.AddRange(Resources.LoadAll<AudioClip>("Audio/metalSqueak"));
            woodenDoorSqueak.AddRange(Resources.LoadAll<AudioClip>("Audio/woodenSqueak"));
            footstepsGravel.AddRange(Resources.LoadAll<AudioClip>("Audio/gravel"));
            footstepGravelIndex = 0;
        }
        #endregion


        #region Private methods
        /// <summary>
        /// Loads plot goals data from a file with specific file path.
        /// </summary>
        /// <param name="filePath">Path of the file</param>
        /// <returns>Plot goals data</returns>
        private Goals LoadGoalsDataFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                //Debug.Log("Loading plot goals data from a file...");
                string dataAsText = File.ReadAllText(filePath);
                return JsonUtility.FromJson<Goals>(dataAsText);
            }
            else return null;
        }

        /// <summary>
        /// Saves plot goals data to a file with specified file path.
        /// </summary>
        /// <param name="goals">Goals data to save</param>
        /// <param name="filePath">Path of the file</param>
        private void SaveGoalsDataToFile(Goals goals, string filePath)
        {
            string dataAsJson = JsonUtility.ToJson(goals, true);
            File.WriteAllText(filePath, dataAsJson);
            //Debug.Log("Saved plot goals data to a file...");
        }

        /// <summary>
        /// Creates test set of plot goals.
        /// </summary>
        /// <returns>Test plot goals data</returns>
        private List<Goal> CreateTestData(int goalsCount)
        {
            List<Goal> goals = new List<Goal>();
            for (int i = 0; i < goalsCount; i++) goals.Add(new Goal(i, "Goal #" + i.ToString()));
            return goals;
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Loads plot goals list that match the current level.
        /// </summary>
        /// <returns>List of plot goals</returns>
        public List<Goal> LoadPlotGoals()
        {
            string plotGoalsFilePath;
            // load plot goals for level A:
            if (LevelManager.instance.LevelName == LevelName.LevelA) plotGoalsFilePath = Application.dataPath + "/Resources/TextData/plot_goals_a.json";
            // load plot goals for level B:
            else plotGoalsFilePath = Application.dataPath + "/Resources/TextData/plot_goals_b.json";
            // load plot goals:
            goals = LoadGoalsDataFromFile(plotGoalsFilePath);
            if (goals == null)
            {
                // file with plot goals doesn't exist - create new plot goals data:
                goals = new Goals(CreateTestData(5));
                SaveGoalsDataToFile(goals, plotGoalsFilePath);
            }
            return goals.Set;
        }

        /// <summary>
        /// Returns random metal door squeak sound.
        /// </summary>
        /// <returns>Random squeak sound</returns>
        public AudioClip GetMetalSqueakSound()
        {
            if (metalDoorSqueak.Count > 0)
            {
                int index = Random.Range(0, metalDoorSqueak.Count);
                return metalDoorSqueak[index];
            }
            else return null;
        }

        /// <summary>
        /// Returns random wooden door squeak sound.
        /// </summary>
        /// <returns>Random squeak sound</returns>
        public AudioClip GetWoodenSqueakSound()
        {
            if (woodenDoorSqueak.Count > 0)
            {
                int index = Random.Range(0, woodenDoorSqueak.Count);
                return woodenDoorSqueak[index];
            }
            else return null;
        }

        /// <summary>
        /// Returns next footstep on gravel sound.
        /// </summary>
        /// <returns>Footstep on gravel sound</returns>
        public AudioClip GetFootstepOnGravelSound()
        {
            if (footstepGravelIndex >= footstepsGravel.Count) footstepGravelIndex = 0;
            return footstepsGravel[footstepGravelIndex++];
        }
        #endregion
    }
}
