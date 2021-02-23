using UnityEngine;
using TMPro;
public class Timer : MonoBehaviour
{
    private float totalTime = 0;
    private float timeInMin = 0;
    private int timeInSec = 0;
    public TextMeshProUGUI timerTextSec;
    public TextMeshProUGUI timerTextMin;
    private GameObject player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null&&timerTextMin != null)
        {
            totalTime += Time.deltaTime;
            timerTextMin.text = timeInMin.ToString();
            timerTextSec.text = timeInSec.ToString();
        }
        
        
        timeInMin = (int)totalTime / 60;
        timeInSec = (int)totalTime % 60;
       
    }
}
