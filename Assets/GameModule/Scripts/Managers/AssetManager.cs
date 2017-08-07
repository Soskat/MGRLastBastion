using LastBastion.Game.Plot;
using System.Collections;
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
        #endregion
        [SerializeField] private string plotGoalsPath;
        //[SerializeField] private List<Goal> goals;
        private GoalsSet goalsSet;


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
            metalDoorSqueak.AddRange(Resources.LoadAll<AudioClip>("Audio/metalSqueak"));
            woodenDoorSqueak.AddRange(Resources.LoadAll<AudioClip>("Audio/woodenSqueak"));
            // load plot goals data:
            plotGoalsPath = Application.dataPath + "/GameModule/MainPlot/plot_goals.json";
            //goals = LoadGoalsDataFromFile(plotGoalsPath);
            //if (goals == null)
            //{
            //    // file with plot goals doesn't exist - create new plot goals data:
            //    goals = CreateTestData(5);
            //    Debug.Log(goals.Count); // --- test
            //    SaveGoalsDataToFile(goals, plotGoalsPath);
            //    Debug.Log(goals.Count); // --- test
            //}
            goalsSet = new GoalsSet();
            goalsSet.Goals = CreateTestData(5);

            Debug.Log("TEST_Awake =========================");
            foreach (Goal goal in goalsSet.Goals) Debug.Log(goal.GoalContent);

            SaveGoalsDataToFile(goalsSet.Goals, plotGoalsPath);
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion


        #region Private methods
        /// <summary>
        /// Saves plot goals data to a file with specified file path.
        /// </summary>
        /// <param name="goals">Goals data to save</param>
        /// <param name="goalsFilePath">Path of the file</param>
        private void SaveGoalsDataToFile(List<Goal> goals, string goalsFilePath)
        {
            Debug.Log("TEST_SaveGoalsDataToFile ===========");
            foreach (Goal goal in goals) Debug.Log(goal.GoalContent);

            string dataAsJson = JsonUtility.ToJson(goals, true);

            Debug.Log("JSON:" + dataAsJson);  // --- test

            File.WriteAllText(goalsFilePath, dataAsJson);
            Debug.Log("Saved plot goals data to a file...");
        }

        /// <summary>
        /// Loads plot goals data from a file with specific file path.
        /// </summary>
        /// <param name="goalsFilePath">Path of the file</param>
        /// <returns>Plot goals data</returns>
        private List<Goal> LoadGoalsDataFromFile(string goalsFilePath)
        {
            if (File.Exists(goalsFilePath))
            {
                Debug.Log("Loading plot goals data from a file...");
                string dataAsText = File.ReadAllText(goalsFilePath);
                return JsonUtility.FromJson<List<Goal>>(dataAsText);
            }
            else return null;
        }

        /// <summary>
        /// Creates test set of plot goals.
        /// </summary>
        /// <returns>Test plot goals data</returns>
        private List<Goal> CreateTestData(int goalsCount)
        {
            List<Goal> goals = new List<Goal>();
            for (int i = 0; i < goalsCount; i++) goals.Add(new Goal(i, "Goal #" + i.ToString()));

            Debug.Log("TEST_CreateTestData ================");
            foreach (Goal goal in goals) Debug.Log(goal.GoalContent);

            return goals;
        }
        #endregion


        #region Public methods
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
        #endregion
    }
}
