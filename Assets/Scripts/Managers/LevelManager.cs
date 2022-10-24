using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;



namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager lm;

        // Private Vars
        private int _balloonAmount; 

        
        void Awake() 
        {
            if (lm == null)
                lm = this;
            else
                Destroy(this);
            
            DontDestroyOnLoad(gameObject);
        }
        public void nextLevel () {
            Debug.Log(SceneManager.sceneCountInBuildSettings + "scenecount   " + SceneManager.GetActiveScene().buildIndex + "bi");
            if(SceneManager.sceneCountInBuildSettings == SceneManager.GetActiveScene().buildIndex + 1)
            {
                Debug.Log("SAVE");
                SaveManager.instance.WriteCSVFile();
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
            }

        }
    }
}