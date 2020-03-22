using System.Collections;
using UnityEngine;
using LuaFramework; 

public class AppStart : MonoBehaviour {
       
    IEnumerator Start()
    { 
        AppConst.UpdateMode = false;
        AppConst.LuaBundleMode = false; 

        AppFacade.GetApp(FaceType.vm1).StartUp(); 
        yield return null;
    } 
     
     
}
