// Author: Noah Stolz
// Used to change the material of an Object
// Should be attached to the Object whose material you want to change

using UnityEngine;
using UnityEngine.Networking;

public class ChangeMaterial : NetworkBehaviour {

    [SerializeField]
    [Tooltip("The Materials that you want to change to")]
    private Material[] materials = new Material[1];

    // The Material of this Object
    private MeshRenderer meshRend;

    void Start()
    {

        meshRend = GetComponent<MeshRenderer>();

    }

    /// <summary>Change to the desired material within the array</summary>
    /// <param name="index">The index of the material</param>
    public void ChangeToMaterial(int index) {

        //meshRend.material = materials[index];
        CmdChangeMaterial(index);

    }

    [Command]
    void CmdChangeMaterial(int index)
    {

        RpcChangeMaterial(index);

    }

    [ClientRpc]
    void RpcChangeMaterial(int index)
    {

        meshRend.material = materials[index];

    }
}
