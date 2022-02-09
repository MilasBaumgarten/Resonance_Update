// Author: Noah Stolz
// Used to set the starting position of the flammes inside the hacking minigame
using UnityEngine;

public class HackingFlame : MonoBehaviour {

    [HideInInspector]
    public bool move = true;

    [HideInInspector]
    public float speed = 10f; // How fast the flammes move

    RectTransform playerRecTrans;

    private Vector2 moveDirection;

    private RectTransform recTrans;

    private float yMin = 0f;
    private float xMin = 0f;

    private float xMax = 3200f;

    private float yMax = -1300f;

	// Use this for initialization
	void Start () {

        recTrans = GetComponent<RectTransform>();

        //playerRecTrans = GameObject.Find("HackingPlayer").GetComponent < RectTransform>() ;
        if (move)
        {

            MoveInit();

        }else {

            StaticInit();

        }
        
       
	}
	
	// Update is called once per frame
	void Update () {

        recTrans.anchoredPosition += moveDirection * speed;

        if (recTrans.anchoredPosition.x < xMin)
            MoveInit();
        if (recTrans.anchoredPosition.x > xMax)
            MoveInit();
        if (recTrans.anchoredPosition.y > yMin)
            MoveInit();
        if (recTrans.anchoredPosition.y < yMax)
            MoveInit();

    }

    /// <summary>Decides randomly where the flame should spawn and move toward </summary>
    void MoveInit()
    {

        int random = Random.Range(0, 4);

        switch (random)
        {
            case 0:

                recTrans.anchoredPosition = new Vector2(xMin, Random.Range(yMax, yMin));

                moveDirection = new Vector2(xMax, Random.Range(-recTrans.anchoredPosition.y, yMax - recTrans.anchoredPosition.y)).normalized;

                //moveDirection = (playerRecTrans.anchoredPosition - recTrans.anchoredPosition).normalized;

                break;

            case 1:

                recTrans.anchoredPosition = new Vector2(xMax, Random.Range(yMax, yMin));

                moveDirection = new Vector2(-xMax, Random.Range(-recTrans.anchoredPosition.y, yMax - recTrans.anchoredPosition.y)).normalized;

                //moveDirection = playerRecTrans.anchoredPosition - recTrans.anchoredPosition;

                break;

            case 2:

                recTrans.anchoredPosition = new Vector2(Random.Range(xMin, xMax), yMin);

                moveDirection = new Vector2(Random.Range(-recTrans.anchoredPosition.x, xMax - recTrans.anchoredPosition.x), yMax).normalized;

                //moveDirection = playerRecTrans.anchoredPosition - recTrans.anchoredPosition;

                break;

            case 3:

                recTrans.anchoredPosition = new Vector2(Random.Range(xMin, xMax), yMax);

                moveDirection = new Vector2(Random.Range(-recTrans.anchoredPosition.x, xMax - recTrans.anchoredPosition.x), -yMax).normalized;

                //moveDirection = playerRecTrans.anchoredPosition - recTrans.anchoredPosition;

                break;

            default:

                Debug.LogWarning("Unexpected value");
                break;
        }
    }

    void StaticInit()
    {

        recTrans.anchoredPosition = new Vector2(Random.Range(xMin, xMax), Random.Range(yMin, yMax));

    }
}
