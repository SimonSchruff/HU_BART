using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OneQuestionReferenceHelp : MonoBehaviour
{
    public bool isQuestionSelected = false;

    public GameObject questionTextPanel;
    public GameObject checkboxesPanel;
    public TMP_Text questionTextRef;

    public void setIsQuestionSelected (){
        if(!isQuestionSelected){
            isQuestionSelected = true;
            GetComponentInParent<QuestionCheckMulti>().checkIfAllQuestionsAnswered();   // Update all questions obj
        }
    }
}
