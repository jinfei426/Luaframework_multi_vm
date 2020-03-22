using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaFramework;

public class LoadWorker : MonoBehaviour {

    public enum Step
    {
        eNone,
        eUpdateFirstFile,   //下载文件目录
        eCalLoadSize,       //计算要下载的文件的总大小
        eUpdate,            //下载
        eFinish,            //完成
        eClose,             //立即关闭
    }

    public Step step;
    public string describeText; 

    public virtual IEnumerator StartWork(string dataPath, string baseUrl, ThreadManager threadMgr)
    {
        ShowUI();
        yield return null;
    }

    protected void ShowUI()
    { 
    }

    public virtual float GetProgress()
    {
        return 1;
    }
	
	//描述
    public virtual string GetDescribe()
    {
        return describeText;
    }

}
