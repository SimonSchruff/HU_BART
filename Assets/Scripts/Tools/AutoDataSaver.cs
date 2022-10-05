using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;


namespace Tools
{
    public class AutoDataSaver : MonoBehaviour
    {
        
        public enum SaveDataType {
            csv, 
            sql
        }
        [Tooltip("Specify if data should be saved locally as csv file, or online;")] public SaveDataType CurrentSaveDataType; 
        
        void Start()
        {
            switch (CurrentSaveDataType) {
                case SaveDataType.csv:
                    SaveManager.instance.WriteCSVFile();
                    break;
                case SaveDataType.sql: 
                    SaveManager.instance.StartPostCoroutine();
                    break;
            }
        }
        
    }
}
