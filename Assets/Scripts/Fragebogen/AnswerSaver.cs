using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class AnswerSaver : MonoBehaviour
{

    [Tooltip("Set Type of Question to save Answer correctly;")] public QuestionType questionType;
    [Tooltip("Current Value of answer that will be saved;")] public string CurrentAnswer;
    
    [Header("Toggles with InputField")]
    [Tooltip("If Question Type is Toggles with Input Field, it needs to be set here;")] public TMP_InputField ToggleIF; 

    // Private Vars
    private Toggle[] _toggles; 
    private TMP_InputField _tmp_inputField;
    private Slider _slider;

    public enum QuestionType {
        toggles, 
        togglesWithFreeInput, 
        freeInputNumber,
        freeInputAlphaNum,
        slider,
        other
    }

    public void checkIfCont(int index = 1)
    {
        SaveManager.instance.checkIfVPContinuee(index);
    }

    void Awake()
    {
        switch(questionType) {
            case QuestionType.toggles:
                _toggles = GetComponentsInChildren<Toggle>();
                break;
            case QuestionType.togglesWithFreeInput:
                _toggles = GetComponentsInChildren<Toggle>();
                _tmp_inputField = GetComponentInChildren<TMP_InputField>(); 
                break;
            case QuestionType.freeInputNumber:
                _tmp_inputField = GetComponentInChildren<TMP_InputField>(); 
                break;
            case QuestionType.slider:
                _slider = GetComponentInChildren<Slider>();
                break;
            case QuestionType.freeInputAlphaNum:
                _tmp_inputField = GetComponentInChildren<TMP_InputField>();
                break;
            case QuestionType.other:
                CurrentAnswer = "null"; 
                break;
        }
    }

    void Update()
    {
        switch(questionType)
        {
            case QuestionType.toggles: 

                foreach(Toggle toggle in _toggles)
                {
                    if(toggle.isOn == true)
                    {
                        CurrentAnswer = toggle.GetComponentInChildren<Text>().text;
                        if(name == "education" && CurrentAnswer == "8" || name == "countryOfResidence" && CurrentAnswer == "9" || name == "employment" && CurrentAnswer == "10" )
                        {
                            ToggleIF.GetComponentInParent<AnswerSaver>().CurrentAnswer = ToggleIF.text; 
                        }
                    }
                }
                break; 
            case QuestionType.togglesWithFreeInput: 

                foreach(Toggle toggle in _toggles)
                {
                    if(toggle.isOn == true)
                    {
                        if(toggle.GetComponentInChildren<InputField>() != null)
                        {
                            _tmp_inputField.interactable = true; 
                            CurrentAnswer = _tmp_inputField.text; 
                        }
                        else
                        {
                            _tmp_inputField.interactable = false;
                            CurrentAnswer = toggle.GetComponentInChildren<Text>().text; 
                        }
                        
                    }
                }

                break;
            case QuestionType.freeInputNumber: 
                CurrentAnswer = _tmp_inputField.text; 
                break;
            case QuestionType.slider:
                CurrentAnswer = _slider.value.ToString(); 
                break;
            case QuestionType.freeInputAlphaNum:
                if (_tmp_inputField != null) {
                    CurrentAnswer = _tmp_inputField.text;
                    return;
                }

                CurrentAnswer = _tmp_inputField.text;
                break;
            case QuestionType.other:
                break;
        }
    }


    public void SaveAnswer(string name)
    {
        //Save to SQL Database with SQL Save Manager        
        if(name == "StartScreen")
        { 
            SaveManager.instance.SaveID(CurrentAnswer);
        }
        else if(name == "intro1_1" || name == "intro1_2" || name == "intro1_3" || name == "intro2_1" || name == "intro2_2" || name == "intro2_3" || name == "declarationConsent") // Do not need to be saved; 
        {
            return;
        }
        else
        {
           //SQLSaveManager.instance.AddAnswerToList(name, currentAnswer);  
        }

    }
}
