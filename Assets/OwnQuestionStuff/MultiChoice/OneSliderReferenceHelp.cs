using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OneSliderReferenceHelp : MonoBehaviour
{
    public bool isQuestionSelected = false;

    public GameObject questionTextPanel;
    public GameObject sliderPanel;
    public Slider sliderRef;
    public TMP_Text questionTextRef;

    public void setIsQuestionSelected (){
        if(!isQuestionSelected){
            isQuestionSelected = true;
            GetComponentInParent<QuestionCheckMulti>().checkIfAllQuestionsAnswered();   // Update all questions obj
        }
    }
}
