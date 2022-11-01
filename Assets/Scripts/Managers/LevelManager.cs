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
        public void nextLevel (bool saveGame = false) {
            Debug.Log(SceneManager.sceneCountInBuildSettings + "scenecount   " + SceneManager.GetActiveScene().buildIndex + "bi");
            if(SceneManager.sceneCountInBuildSettings == SceneManager.GetActiveScene().buildIndex + 1 || saveGame)
            {
                Debug.Log("SAVE");
                SaveManager.instance.WriteCSVFile();
         //       SceneManager.LoadScene(0);

      //          try
        //        {
          //          openChromeBrowser();
            //    }
              //  catch
      //          {
        //            Debug.Log("Propably NOT Android");
          //      }

            }
            if (SceneManager.sceneCountInBuildSettings != SceneManager.GetActiveScene().buildIndex + 1)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
            }
        }

        public void openChromeBrowser()
        {
            Application.Quit();
        //    Application.OpenURL("com.android.chrome");
            /*
            bool fail = false;
            string bundleId = "com.android.chrome"; // your target bundle id

            AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");

            AndroidJavaObject launchIntent = null;
            try
            {
                launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage",bundleId);
            }
            catch
            {
                fail = true;
            }

            if (fail)
            { //open app in store
                Application.OpenURL("https://google.com");
            }
            else //open the app
            {
                ca.Call("startActivity", launchIntent);
            }

            up.Dispose();
            ca.Dispose();
            packageManager.Dispose();
            launchIntent.Dispose();
            */
        }
    }
}

