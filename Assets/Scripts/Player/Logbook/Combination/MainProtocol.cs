using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainProtocol : MonoBehaviour {

    public string[] protocolNames = new string[0];

    [SerializeField]
    private GameObject protocolPrefab;

    [SerializeField]
    private GameObject protocolParent;

    private static GameObject protocolParentStatic, protocolPrefabStatic;

    private static List<GameObject> protocols = new List<GameObject>();

    private static string[] protocolNamesStatic;

    private static int index = 0;

    void Awake()
    {

        protocolParentStatic = protocolParent;
        protocolPrefabStatic = protocolPrefab;
        protocolNamesStatic = protocolNames;

    }

    public static void addProtocolEntry()
    {
        if(index < protocolNamesStatic.Length){

            GameObject protocol = Instantiate(protocolPrefabStatic, protocolParentStatic.transform);

            protocol.transform.GetChild(0).GetComponent<Text>().text = protocolNamesStatic[index];

            protocols.Add(protocol);

        }

        index++;

    }

    public static void resetMainProtocol()
    {

        for(int i = 0; i < protocols.Count; i++)
        {

            Destroy(protocols[i]);

        }

        index = 0;
        protocols.Clear();
    }
}
