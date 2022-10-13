using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class QuestionCheckMulti : MonoBehaviour
{
    [System.Serializable]
    public struct oneQuestion
    {
        public string questionText;
        public string saveName;
        public string value;
        public bool isSlider;
        public sliderDef sliderProps;
        public bool isTextInput;
    }
    [System.Serializable]
    public struct sliderDef {
        public int startValue;
        public int minValue;
        public int maxValue;
     //   public bool onlyWholeNums;

    }

    public oneQuestion[] oneQuestionDef;
    [Tooltip("Alle Fragen")]
    public string[] allQuestions;
    [Tooltip("Beschriftung der Checkboxes")]
    public string[] legendOfVoteables;
    [SerializeField] GameObject oneCheckboxPrefab;
    [SerializeField] GameObject oneQuestionPrefab;
    [SerializeField] GameObject oneLegendPrefab;
    [SerializeField] GameObject oneLegendEntryPrefab;

    [Tooltip("value in between 0-1")] 
    public float questionSpace = .3f;

    public bool allQuestionsAnswered = false;


    public bool textOnEveryEntry = false;

    public bool checkIfAllQuestionsAnswered (){
        OneQuestionReferenceHelp [] checkedArray = GetComponentsInChildren<OneQuestionReferenceHelp>();
        foreach (var check in checkedArray){
            if(!check.isQuestionSelected){
                return false;
            }
        }
        allQuestionsAnswered = true;
        return true;
    }

    public void redrawQuestions() {
        int childCount = transform.childCount;
        Debug.Log(childCount);
        for (int i = 0; i < childCount; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        GameObject tempLegend = Instantiate(oneLegendPrefab, transform);
        OneQuestionReferenceHelp refLegend = tempLegend.GetComponent<OneQuestionReferenceHelp>();
        foreach (var legend in legendOfVoteables)
        {
            GameObject leg = Instantiate(oneLegendEntryPrefab, refLegend.checkboxesPanel.transform);
            leg.GetComponentInChildren<TMP_Text>().text = legend;
        }

        foreach (var ques in oneQuestionDef)
        {
            GameObject tempObj = Instantiate(oneQuestionPrefab, transform);
            OneQuestionReferenceHelp refs = tempObj.GetComponent<OneQuestionReferenceHelp>();
            refs.questionTextRef.text = ques.questionText;  // Assign Text

            if(ques.isSlider){  // When is slider

            } else if(ques.isTextInput){ // When is text input field

            } else {    // MultiQuestion answer create check

                foreach (var legend in legendOfVoteables)
                {
                    Instantiate(oneCheckboxPrefab, refs.checkboxesPanel.transform);
                }
            }
        }   
    }
}
