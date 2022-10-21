using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class QA_Game_Controller : MonoBehaviour
{
    [System.Serializable]
    public struct QA_GameDef
    {
        public bool isRobotTalking;
        public string questionText;
        public AudioClip audioFileGr_1;
        public AudioClip audioFileGr_2;
        public AudioClip audioFileGr_3;
    }
    [SerializeField] float robotTalkDelay = 2.0f;
    GameObject actualCard;

    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject canvasRef;
    [SerializeField] GameObject randomWheelPrefab;

    public int actualGroup = 1;

    public static QA_Game_Controller gc;

    public QA_GameDef [] roundDefiniton;
    public int actualRound = -1;

    private void Awake()
    {
        if(gc == null)
        {
            gc = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        startRound();

        if(SaveManager.instance.actualGroup == SaveManager.GroupInfo.Group1)
        {
            Debug.Log("assigned 1");
            actualGroup = 1;
        }
        else if (SaveManager.instance.actualGroup == SaveManager.GroupInfo.Group2)
        {
            actualGroup = 2;
        }
        else
        {
            actualGroup = 3;
        }
    }

    void startRound (){
        StartCoroutine(showRandomWheel(0.1f));
        StartCoroutine(initCardAni(2.48f));
        StartCoroutine(letRototTalk(5f));   // needs to be higher than initCardAni
        StartCoroutine(showContinueBut(10));
    }
    public void nextCard (){
        startRound();
    }

    public IEnumerator initCardAni(float delay = 1)
    {
        yield return new WaitForSeconds(delay);
        actualRound++;
        Debug.Log(actualRound + " init card");
        actualCard = Instantiate(cardPrefab, canvasRef.transform);
        bool _tempLast = actualRound == roundDefiniton.Length -1;
        actualCard.GetComponent<OneGameCardUI>().defineCard(roundDefiniton[actualRound], _tempLast);
        Debug.Log(actualCard);
    }

    IEnumerator letRototTalk( float delay = 1.0f)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Robot talk");
        if(roundDefiniton[actualRound].isRobotTalking){
            Debug.Log(actualRound);
            GetComponent<AudioSource>().clip = getActualAudioClip();
            GetComponent<AudioSource>().Play();
        }
    }
        IEnumerator showRandomWheel(float delay = 5.0f)
    {
        yield return new WaitForSeconds(delay);
        Instantiate (randomWheelPrefab, canvasRef.transform);
    }

    IEnumerator showContinueBut(float delay = 5.0f)
    {
        yield return new WaitForSeconds(delay);
        actualCard.GetComponent<OneGameCardUI>().buttonRef.gameObject.SetActive(true);

    }

    AudioClip getActualAudioClip()
    {
        switch (actualGroup)
        {
            case 1:
                return roundDefiniton[actualRound].audioFileGr_1;
            case 2:
                return roundDefiniton[actualRound].audioFileGr_2;
            case 3:
                return roundDefiniton[actualRound].audioFileGr_3;
            default:
                return null;
        }
    }
}
