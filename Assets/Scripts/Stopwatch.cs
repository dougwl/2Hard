using UnityEngine;
using UnityEngine.UI;
using System.Globalization;


public class Stopwatch : MonoBehaviour
{

    [SerializeField] private Text ClockString;
    public static float Clock;
    private GameManager GM;

    // Use this for initialization
    void Start()
    {
        if (GM is null) GM = GameManager.GM;
        Clock = 0.0f;
    }
 
    // Update is called once per frame
    void Update()
    {
        //Check and Set the Clock
        if (GM.Playing)
        {
            Clock += Time.deltaTime;
            ClockString.text = Clock.ToString("F1", CultureInfo.InvariantCulture);
        }
    }
}
