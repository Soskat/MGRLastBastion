using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;


/// <summary>
/// Component that manages list behaviour on GameObject.
/// </summary>
public class ListController : MonoBehaviour {
    
    #region Private fields
    [SerializeField] private GameObject contentPanel;
    [SerializeField] private GameObject listItemPrefab;
    [SerializeField] private List<string> connectedBands;
    [SerializeField] private int selectedItem = 0;
    #endregion

    #region Unity methods
    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        Assert.IsNotNull(listItemPrefab);
        Assert.IsNotNull(contentPanel);
    }

    // Use this for initialization
    void Start()
    {
        connectedBands = new List<string>();
    }
    #endregion

    #region Public methods
    /// <summary>
    /// Updates list content with specified items.
    /// </summary>
    /// <param name="newItems">New list content</param>
    public void UpdateList(string[] newItems)
    {
        // remove actual content:
        ClearList();
        if (newItems != null)
        {
            int counter = 0;
            // add new content:
            foreach (var bandName in newItems)
            {
                connectedBands.Add(bandName);
                GameObject item = Instantiate(listItemPrefab) as GameObject;
                item.GetComponentInChildren<Text>().text = bandName;
                int index = counter;
                item.GetComponent<Button>().onClick.AddListener(() => { SelectItem(index); });
                item.transform.SetParent(contentPanel.transform);
                item.transform.localScale = Vector3.one;
                counter++;
            }
            // update selected item:
            selectedItem = 0;
        }
    }

    /// <summary>
    /// Clears list's content.
    /// </summary>
    public void ClearList()
    {
        connectedBands.Clear();
        selectedItem = -1;
        foreach (Transform child in contentPanel.transform) Destroy(child.gameObject);
    }

    /// <summary>
    /// Returns list's selected item.
    /// </summary>
    /// <returns>List's selected item</returns>
    public string GetSelectedItem()
    {
        if (selectedItem >= 0 && selectedItem < connectedBands.Count) return connectedBands[selectedItem];
        else return null;
    }
    #endregion

    #region Private methods
    /// <summary>
    /// Selects list item with specified index.
    /// </summary>
    /// <param name="index">Index of selected item</param>
    private void SelectItem(int index)
    {
        contentPanel.transform.GetChild(selectedItem).GetComponent<Image>().color = Color.white;
        selectedItem = index;
        contentPanel.transform.GetChild(selectedItem).GetComponent<Image>().color = Color.grey;
    }
    #endregion
}
