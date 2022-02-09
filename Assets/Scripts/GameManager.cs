using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour {

    public static GameManager instance = null;

    public Settings settings;
    public InputSettings input;

    public string catrionaName;
    public string robertName;

    public GameObject catriona;
    public GameObject robert;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        catrionaName = catrionaName.ToLower();
        robertName = robertName.ToLower();
    }
}