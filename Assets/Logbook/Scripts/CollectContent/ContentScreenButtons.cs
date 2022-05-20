// Author: Noah Stolz
// Used to switch between entries
// Should be attached to the entryButton prefab

using UnityEngine;
using UnityEngine.UI;

public class ContentScreenButtons : MonoBehaviour {

    [SerializeField]
    [Tooltip("The panel this button activates")]
    private GameObject panel;

    // The currently displayed panel
    public static  GameObject currentPanel;

    /// <summary>Set the panel referenced by this component to active and disable the currently active panel</summary>
    public void SwitchEntry() {

        if (currentPanel != null)
        {

            currentPanel.SetActive(false);

        } else
        {

            DisableSiblings();

        }  

        currentPanel = panel;
        panel.SetActive(true);
        
    }

    /// <summary>Used to switch to an entry when it has been collected</summary>
    public static void SwitchEntry(GameObject panel) {

        if (currentPanel != null) {

            currentPanel.SetActive(false);

        }

        currentPanel = panel;
        panel.SetActive(true);

    }

    void DisableSiblings()
    {

        foreach(Transform sibling in panel.transform.parent)
        {

            if(sibling.GetSiblingIndex() != panel.transform.GetSiblingIndex())
            {

                sibling.gameObject.SetActive(false);

            }

        }

    }
}
