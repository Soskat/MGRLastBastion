using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class ListController : MonoBehaviour {

    public GameObject contentPanel;
    public GameObject listItemPrefab;
    private List<string> connectedBands;
    //public List<string> connectedBands;

    [SerializeField] private int selectedItem = 0;


    private void Awake()
    {
        // check if everything is initialized:
        Assert.IsNotNull(listItemPrefab);
        Assert.IsNotNull(contentPanel);
    }
    
    void Start () {
        connectedBands = new List<string>();
	}
	
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
        foreach (Transform child in contentPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }

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

    /// <summary>
    /// Returns list's selected item.
    /// </summary>
    /// <returns>List's selected item</returns>
    public string GetSelectedItem()
    {
        if (selectedItem >= 0 && selectedItem < connectedBands.Count)
            return connectedBands[selectedItem];
        else
            return null;
    }
}
