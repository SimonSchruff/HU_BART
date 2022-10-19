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
        public bool isDoppleLegend;
        public SecondLegendText seclegendText;
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

    [System.Serializable]
    public struct SecondLegendText
    {
        public string secondLegendText; 
    }


        public Dictionary<string,string> saveDictionary = new Dictionary<string,string>();

    [SerializeField] bool hasLegend = true;
    public int legendTextSize = 35;
 //   public float legendFieldHeight = 80;
    public bool hasLegendStartText = false;
    public string legendStartText = "LegendStartText";
    public int checkboxTextSize = 35;


    public oneQuestion[] oneQuestionDef;
    [Tooltip("Beschriftung der Checkboxes")]
    public string[] legendOfVoteables;
    [SerializeField] GameObject oneCheckboxPrefab;
    [SerializeField] GameObject oneQuestionPrefab;
    [SerializeField] GameObject oneLegendPrefab;
    [SerializeField] GameObject oneLegendEntryPrefab;
    [SerializeField] GameObject oneSliderPrefab;
    [SerializeField] GameObject oneDoppleLegendPrefab;

    [Tooltip("value in between 0-1")] 
    public float questionSpace = .3f;

    public bool allQuestionsAnswered = false;


    public bool textOnEveryEntry = false;

    public bool checkIfAllQuestionsAnswered (){
        OneQuestionElem [] checkedArray = GetComponentsInChildren<OneQuestionElem>();
        foreach (var check in checkedArray){
            if(!check.isQuestionSelected){
                return false;
            }
        }
        allQuestionsAnswered = true;
        GetComponentInParent<ScrollRootScript>().checkIfALLQuestionsAnswered();
        return true;
    }

    void Awake (){
        createSaveDict();
    }

    void createSaveDict (){
        saveDictionary.Clear();             // Add all fields to saveDictionary
        foreach (var ques in oneQuestionDef)
        {
            try
            {
                saveDictionary.Add(ques.saveName,"");
            }
            catch
            {
                Debug.LogError("NOT UNIQUE IDENTIFIERS - CHANGE SAVE NAME IN QUESITON STRUCT");
            }
        }
    }

    public void redrawQuestions() {
        int childCount = transform.childCount;
        Debug.Log(childCount);

        createSaveDict();

        for (int i = 0; i < childCount; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
        if(hasLegend){  // Draw Legend
            GameObject tempLegend = Instantiate(oneLegendPrefab, transform);
            OneQuestionReferenceHelp refLegend = tempLegend.GetComponent<OneQuestionReferenceHelp>();
            foreach (var legend in legendOfVoteables)
            {
                GameObject leg = Instantiate(oneLegendEntryPrefab, refLegend.checkboxesPanel.transform);
                leg.GetComponentInChildren<TMP_Text>().text = legend;
            }
        }

        foreach (var ques in oneQuestionDef)
        {
            if(ques.isSlider){  // When is slider
                GameObject slid = Instantiate(oneSliderPrefab, transform);
                OneSliderReferenceHelp _refs = slid.GetComponent<OneSliderReferenceHelp>();
                _refs.questionTextRef.text = ques.questionText;
                _refs.sliderRef.maxValue = ques.sliderProps.maxValue;
                _refs.sliderRef.value = ques.sliderProps.startValue;
                _refs.sliderRef.minValue = ques.sliderProps.minValue;
                assignSaveName(slid,ques, _refs.sliderRef.value.ToString());

            } else if(ques.isTextInput){ // When is text input field

            } else {    // MultiQuestion answer create check
                GameObject tempObj = Instantiate(ques.isDoppleLegend ? oneDoppleLegendPrefab : oneQuestionPrefab, transform);
                assignSaveName(tempObj,ques);
                OneQuestionReferenceHelp refs = tempObj.GetComponent<OneQuestionReferenceHelp>();
                refs.questionTextRef.text = ques.questionText;  // Assign Text

                int counter = 1; // Start counting with one for saving
                foreach (var legend in legendOfVoteables)
                {
                    GameObject check = Instantiate(oneCheckboxPrefab, refs.checkboxesPanel.transform);
                    check.GetComponent<OneToggleEntry>().index = counter;
                    counter ++;
                }
                if (ques.isDoppleLegend) // Name the second legend text
                {
                    tempObj.GetComponentInChildren<SecondLegendTextIdentifier>().gameObject.GetComponent<TMP_Text>().text = ques.seclegendText.secondLegendText;
                }
            }
        }  
        printSaveDict();
    }

    void assignSaveName (GameObject objToAssignName, oneQuestion questionElem, string value = null){
        OneQuestionElem oneQuesRef;
        if(objToAssignName.TryGetComponent<OneQuestionElem>(out oneQuesRef)){
            oneQuesRef.saveName = questionElem.saveName;
            if(!(value == null)){
                oneQuesRef.assignValue(value);
            }
        } else {
            Debug.Log("Elem did not have an OneQuestionElem");
        }
    }

    public void printSaveDict (){
        foreach (var key in saveDictionary.Keys)
        {
            Debug.Log(key + " key__" + saveDictionary[key] + " value__ length: " + saveDictionary.Count);
        } 
    }

    public void addSaveData(string key, string value){
        saveDictionary[key] = value;

        checkIfAllQuestionsAnswered(); 

        printSaveDict();
    }
}
