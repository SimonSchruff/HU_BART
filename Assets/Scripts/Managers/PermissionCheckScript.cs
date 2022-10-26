using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class PermissionCheckScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead)|| !Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite)) // Not allowed to write Storage
        {
            string[] _str = { Permission.ExternalStorageRead, Permission.ExternalStorageWrite };
            Permission.RequestUserPermissions(_str);
        }
    }
}
