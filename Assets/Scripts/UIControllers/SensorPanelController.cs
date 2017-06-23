using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;


/// <summary>
/// Component that manages SensorPanel UI logic.
/// </summary>
public class SensorPanelController : MonoBehaviour {

    #region Private fields
    [SerializeField] private Text pairedBandLabel;
    [SerializeField] private Text hrReadingLabel;
    [SerializeField] private Text averageHrLabel;
    [SerializeField] private Text gsrReadingLabel;
    [SerializeField] private Text averageGsrLabel;
    #endregion


    #region MonoBehaviour methods
    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        Assert.IsNotNull(pairedBandLabel);
        Assert.IsNotNull(hrReadingLabel);
        Assert.IsNotNull(averageHrLabel);
        Assert.IsNotNull(gsrReadingLabel);
        Assert.IsNotNull(averageGsrLabel);
    }

    // Use this for initialization
    private void Start()
    {
        ResetLabels();
    }
    #endregion


    #region Public methods
    /// <summary>
    /// Resets labels values.
    /// </summary>
    public void ResetLabels()
    {
        pairedBandLabel.text = "-";
        hrReadingLabel.text = "-";
        averageHrLabel.text = "-";
        gsrReadingLabel.text = "-";
        averageGsrLabel.text = "-";
    }
    
    /// <summary>
    /// Updates paired Band label with new text.
    /// </summary>
    /// <param name="newLabel">New label text</param>
    public void UpdateBandLabel(string newLabel)
    {
        pairedBandLabel.text = newLabel;
    }

    /// <summary>
    /// Updates values of average readings labels.
    /// </summary>
    /// <param name="hr">New average HR value</param>
    /// <param name="gsr">New average GSR value</param>
    public void UpdateAverageReadings(int hr, int gsr)
    {
        averageHrLabel.text = hr.ToString();
        averageGsrLabel.text = gsr.ToString();
    }

    /// <summary>
    /// Updates values of current readings labels.
    /// </summary>
    /// <param name="hr">New current HR value</param>
    /// <param name="gsr">New current GSR value</param>
    public void UpdateCurrentReadings(int hr, int gsr)
    {
        hrReadingLabel.text = hr.ToString();
        gsrReadingLabel.text = gsr.ToString();
    }
    #endregion
}
