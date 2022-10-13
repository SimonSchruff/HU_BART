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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
        }
    }
}