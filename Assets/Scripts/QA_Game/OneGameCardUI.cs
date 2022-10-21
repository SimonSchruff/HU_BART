using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class OneGameCardUI : MonoBehaviour
{
    public Image image;
    public TMP_Text text;
    [SerializeField] Sprite robotSprite;
    [SerializeField] Sprite personSprite;
    public GameObject buttonRef;
    bool _isLast;

    public void defineCard (QA_Game_Controller.QA_GameDef cardDef, bool isLast = false)
    {
        _isLast = isLast;
        text.text = cardDef.questionText;
        Debug.Log(cardDef.questionText);
        image.sprite = cardDef.isRobotTalking ? robotSprite : personSprite;
    }
    // Start is called before the first frame update
    void Start()
    {
        buttonRef.gameObject.SetActive(false);
    }

    bool buttonClickOnlyOnce = true;
    public void continueClicked()
    {
        GetComponent<Animator>().SetTrigger("exit");
        if (buttonClickOnlyOnce)
        {
            StartCoroutine(removeSelfCard());
            StartCoroutine(delete());
            buttonClickOnlyOnce = false;
            //Play animation stuff
        }
    }
    IEnumerator removeSelfCard(float delay = .5f)
    {
        yield return new WaitForSeconds(delay); // Change this to animation duration
        if(_isLast)
        {
            LevelManager.lm.nextLevel();
        }
        else
        {
            QA_Game_Controller.gc.nextCard();
        }
    }
    IEnumerator delete (float delay = .5f){
        yield return new WaitForSeconds (delay);
        Destroy(gameObject);
    }
}
