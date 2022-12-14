using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class FragebogenManager : MonoBehaviour
{
    [SerializeField] private int _questionNumber; // Counted up with each button "Continue" Button Click
    [SerializeField] private GameObject ineligableScreen;

    [Serializable]
    public struct Questions
    {
        //public int id;
        //public string name;
        [Tooltip("Parent Question GameObject; Has to be set in inspector;")] public GameObject questionObj;
        [Tooltip("Error Text Obj that is shown when wrong answer is given; Has to be set in inspector;")] public GameObject errorText;
    }

    public static FragebogenManager instance;
    public Questions[] questions;

    /*
    [Serializable]
    public struct ErrorText
    {
        public string parentObjectName;
        public GameObject obj;
    }
    [Tooltip("Set the parent objects name of the Question Obj and the corresponding Error Text Game Object")]
     public ErrorText[] errorTexts;
    */
    
    /*
     * 
     * If all Answers have been given / valid -> Disable current Question and enable next question
     * Save Answer 
     * Count up Question ID
     * Enable Error Text if not all answers have been given / valid 
     * 
     */

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else 
            Destroy(this);

    }
    
    public void NextQuestion() //Called by Continue Button
    {
        DisableAllErrorTexts();
        
        int currentID = _questionNumber;
        bool isAllowedToChange = AllowedToContinue(currentID);

        if (isAllowedToChange)
        {
        //    LevelManager.lm.nextLevel();
            // SaveAnswer(currentID);

            // questions[currentID].questionObj.gameObject.SetActive(false);
            // //questions[currentID +1 ].questionObj.gameObject.SetActive(true);
            // print(questions[currentID].questionObj.gameObject.name); 

            
            // if(questions[currentID].questionObj.gameObject.name ==  "Set Active Self Eff")
            // {
            //     //MySceneManager.Instance.LoadSceneByName("NBackSpiel"); 
            // }
            // else if (currentID != (questions.Length - 1)) // Last question -> Dont activate next UI
            // {
            //     GameObject obj = questions[currentID + 1].questionObj.gameObject;
            //     obj.SetActive(true);
            // }

            // _questionNumber++;
        }
        else
        {
            //Displays Error Text if not all Answers have been answered
            if(questions[currentID].errorText)
                questions[currentID].errorText.gameObject.SetActive(true);
            
            
            /* ==== old version ====
            foreach (ErrorText t in errorTexts)
            {
                if (questions[currentID].questionObj.gameObject.name == t.parentObjectName)
                    t.obj.SetActive(true);
            }
            */
        }
        
    }

    public void SaveData()
    {
        //SQLSaveManager.instance.StartPostCoroutine();
    }

    private void DisableAllErrorTexts()
    {
        foreach (var q in questions)
        {
            if(q.errorText)
                q.errorText.gameObject.SetActive(false);
        }
    }
    
    public bool AllowedToContinue(int currentID)
    {
        if (questions[currentID].questionObj.gameObject.GetComponentInChildren<AnswerSaver>() != null)
        {
            int answerAmount = 0;
            AnswerSaver[] allAnswers = questions[currentID].questionObj.gameObject.GetComponentsInChildren<AnswerSaver>();
            //print(questions[currentID].questionObj.gameObject.name); 

            foreach (AnswerSaver answer in allAnswers)
            {
                answerAmount++; 

                if (answer.questionType == AnswerSaver.QuestionType.toggles || answer.questionType == AnswerSaver.QuestionType.togglesWithFreeInput)
                {
                    if (answer.gameObject.GetComponentInChildren<ToggleGroup>() != null)
                    {
                        ToggleGroup tg = answer.gameObject.GetComponentInChildren<ToggleGroup>(); 
                        if (tg.AnyTogglesOn() == false)
                        {
                            print("False at toogle: " + answer.gameObject.name); 
                            return false;
                        }
                        
                        else if(answer.gameObject.name == "intro1_1" || answer.gameObject.name == "intro1_3" || answer.gameObject.name == "intro2_3")
                        {
                            if (answer.CurrentAnswer == "2") // Answered 2:False
                                return false; 
                        }
                        else if (answer.gameObject.name == "intro1_2" || answer.gameObject.name == "intro2_1" || answer.gameObject.name == "intro2_2")
                        {
                            if (answer.CurrentAnswer == "1") // Answered 1:True
                                return false;
                        }

                    }
                }

                if (answer.questionType == AnswerSaver.QuestionType.freeInputAlphaNum || answer.questionType == AnswerSaver.QuestionType.freeInputNumber || answer.questionType == AnswerSaver.QuestionType.togglesWithFreeInput)
                {
                    if (String.IsNullOrEmpty(answer.CurrentAnswer))
                    {
                        print("False at free input: " + answer.gameObject.name); 
                        return false;
                    }
                    if(answer.gameObject.name == "StartScreen")
                    {
                        if(answer.CurrentAnswer.Length != 4) {
                            return false; 
                        }
                    }
                    
                }

                if(answer.questionType == AnswerSaver.QuestionType.other)
                {
                    if(answer.CurrentAnswer == null || answer.CurrentAnswer == "")
                    {
                        print("False at free input: " + answer.gameObject.name);
                        return false; 
                    }
                }
            } 
            // If code reaches this point, each question has been checked for valid answer
            if (answerAmount == allAnswers.Length)
                return true;
            else
                return false;
            
        }     
        else // IntroductionScreens with no Answer Saver attached; 
        {
            return true;
        }
    }

    void SaveAnswer(int currentID)
    {
        AnswerSaver[] allAnswers = questions[currentID].questionObj.gameObject.GetComponentsInChildren<AnswerSaver>();
        foreach (AnswerSaver answer in allAnswers)
        {
            // If education etc. save number AND free text field 
            answer.SaveAnswer(answer.gameObject.name);
        }
    }

    public void ShowIneligableScreen()
    {
        if(ineligableScreen == null)
        {
            Debug.LogError("Ineligable Screen Refernce, not set!");
            return; 
        }

        DisableAllErrorTexts();

        foreach (Questions q in questions)
            q.questionObj.SetActive(false);

        ineligableScreen.SetActive(true); 


    }


   
}
