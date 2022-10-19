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
        StartCoroutine(initAnimation());

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
    public IEnumerator initAnimation()
    {
        actualRound++;
        actualCard = Instantiate(cardPrefab, canvasRef.transform);
        bool _tempLast = actualRound == roundDefiniton.Length -1;
        actualCard.GetComponent<OneGameCardUI>().defineCard(roundDefiniton[actualRound], _tempLast);
        Debug.Log(actualCard);

        yield return new WaitForSeconds(2.0f);
        if (roundDefiniton[actualRound].isRobotTalking) // Start rotot talk 
        {
            StartCoroutine(letRototTalk());
        }
        StartCoroutine(showContinueBut());
    }

    IEnumerator letRototTalk()
    {
        yield return new WaitForSeconds(robotTalkDelay);
        GetComponent<AudioSource>().clip = getActualAudioClip();
        GetComponent<AudioSource>().Play();
    }

    IEnumerator showContinueBut()
    {
        yield return new WaitForSeconds(5);
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
