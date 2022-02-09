// Author: Noah Stolz
// Used to add Content to the logbook with a delay and when a certain amount of content has been collected

using UnityEngine;

public class NoInteractionCollect : MonoBehaviour {

    [SerializeField]
    [Tooltip ("The Object Content used to trigger")]
    private ObjectContent objCont;

    [SerializeField]
    [Tooltip("The Object Content added")]
    private ObjectContent targetCont;

    [SerializeField]
    [Tooltip("How long before the extra Content is added")]
    private float delay;

    [SerializeField]
    [Tooltip("How often the Player needs to interact to trigger the extra content")]
    private int targetNummberOfInteractions;

    private int nummberOfInteractions = 0;

    public void Collected()
    {

        nummberOfInteractions++;

        if(nummberOfInteractions >= targetNummberOfInteractions)
        {

            Invoke("addContent", delay);

        }

    }

    private void addContent()
    {
        
        targetCont.Interact(objCont.player.GetComponent<ArmTool>());

    }

}
