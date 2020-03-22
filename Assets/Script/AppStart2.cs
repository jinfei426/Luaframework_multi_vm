using System.Collections;
using UnityEngine;
using LuaFramework; 

public class AppStart2 : MonoBehaviour {
       
    IEnumerator Start()
    { 
        AppConst.UpdateMode = false;
        AppConst.LuaBundleMode = false; 

        AppFacade.GetApp(FaceType.vm2).StartUp(); 
        yield return null;
    } 
     
     
}
