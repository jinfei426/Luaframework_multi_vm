
using UnityEngine;
using System;
using System.Collections.Generic;
using LuaInterface;
using LuaFramework;
using UnityEditor;

using BindType = ToLuaMenu.BindType;
using UnityEngine.UI;
using UnityEngine.Events;



public static class CustomSettings
{
    public static string FrameworkPath = AppConst.FrameworkRoot;
    public static string saveDir = FrameworkPath + "/ToLua/Source/Generate/";   
    public static string toluaBaseType = FrameworkPath + "/ToLua/BaseType/";
	public static string toluaLuaDir = FrameworkPath + "/ToLua/Lua";

    //导出时强制做为静态类的类型(注意customTypeList 还要添加这个类型才能导出)
    //unity 有些类作为sealed class, 其实完全等价于静态类
    public static List<Type> staticClassTypes = new List<Type>
    {        
        typeof(UnityEngine.Application),
        typeof(UnityEngine.Time),
        typeof(UnityEngine.Screen),
        typeof(UnityEngine.SleepTimeout),
        typeof(UnityEngine.Input),
        typeof(UnityEngine.Resources),
        typeof(UnityEngine.Physics),
        typeof(UnityEngine.RenderSettings),
        typeof(UnityEngine.QualitySettings),
        typeof(UnityEngine.GL),
        typeof(UnityEngine.Graphics),
    };

    //附加导出委托类型(在导出委托时, customTypeList 中牵扯的委托类型都会导出， 无需写在这里)
    public static DelegateType[] customDelegateList = 
    {     
        _DT(typeof(Action)),                
        _DT(typeof(UnityEngine.Events.UnityAction)),
        _DT(typeof(System.Predicate<int>)),
        _DT(typeof(System.Action<int>)),
        _DT(typeof(System.Comparison<int>)),
        _DT(typeof(System.Func<int, int>)),
    };

    //在这里添加你要导出注册到lua的类型列表
    public static BindType[] customTypeList =
    {                    
                        
        _GT(typeof(Debugger)).SetNameSpace(null), 
                                         
        _GT(typeof(Component)),
        _GT(typeof(Transform)),
        _GT(typeof(Material)),
        _GT(typeof(SpriteRenderer)),
        _GT(typeof(Light)),
        _GT(typeof(Rigidbody)),
        _GT(typeof(Camera)),
        _GT(typeof(AudioSource)),
        //_GT(typeof(LineRenderer)) 
          

        _GT(typeof(Behaviour)),
        _GT(typeof(MonoBehaviour)),
        _GT(typeof(GameObject)),
        _GT(typeof(TrackedReference)),
        _GT(typeof(Application)),
        _GT(typeof(Physics)),
        _GT(typeof(Collider)),
        _GT(typeof(Time)),
        _GT(typeof(Texture)),
        _GT(typeof(Texture2D)),
        _GT(typeof(Shader)),
        _GT(typeof(Renderer)),
        _GT(typeof(WWW)),
        _GT(typeof(Screen)),
        _GT(typeof(CameraClearFlags)),
        _GT(typeof(AudioClip)),
        _GT(typeof(AssetBundle)),
        _GT(typeof(ParticleSystem)),
        _GT(typeof(AsyncOperation)).SetBaseType(typeof(System.Object)),
        _GT(typeof(LightType)),
        _GT(typeof(SleepTimeout)),
#if UNITY_5_3_OR_NEWER && !UNITY_5_6_OR_NEWER
        _GT(typeof(UnityEngine.Experimental.Director.DirectorPlayer)),
#endif
        _GT(typeof(Animator)),
        _GT(typeof(AnimatorStateInfo)),
        _GT(typeof(AnimationCurve)),
        _GT(typeof(Input)),  
        _GT(typeof(KeyCode)),
        _GT(typeof(SkinnedMeshRenderer)),
        _GT(typeof(Space)),


        _GT(typeof(MeshRenderer)),
#if !UNITY_5_4_OR_NEWER
        _GT(typeof(ParticleEmitter)),
        _GT(typeof(ParticleRenderer)),
        _GT(typeof(ParticleAnimator)), 
#endif 

         //_GT(typeof(Button)),
         _GT(typeof(Button.ButtonClickedEvent)),
         _GT(typeof(UnityEventBase)),
         _GT(typeof(UnityEvent)),
         _GT(typeof(DelegateFactory)),
         _GT(typeof(Debug)),



        _GT(typeof(BoxCollider)),
        _GT(typeof(MeshCollider)),
        _GT(typeof(SphereCollider)),
        _GT(typeof(CharacterController)),
        _GT(typeof(CapsuleCollider)),
         

        _GT(typeof(Animation)),
        _GT(typeof(AnimationClip)).SetBaseType(typeof(UnityEngine.Object)),
        _GT(typeof(AnimationState)),
        _GT(typeof(AnimationBlendMode)),
        _GT(typeof(QueueMode)),
        _GT(typeof(PlayMode)),
        _GT(typeof(WrapMode)),

        _GT(typeof(QualitySettings)),
        _GT(typeof(RenderSettings)),
        _GT(typeof(BlendWeights)),
        _GT(typeof(RenderTexture)),
        _GT(typeof(Resources)),
        _GT(typeof(LuaProfiler)),
          
        //for LuaFramework
        _GT(typeof(RectTransform)),
        _GT(typeof(Text)),
        _GT(typeof(Image)),
        _GT(typeof(RawImage)),
        _GT(typeof(Sprite)),  
        _GT(typeof(Util)),
        _GT(typeof(AppConst)),
        _GT(typeof(LuaHelper)),
        _GT(typeof(LuaBehaviour)),   
         
        _GT(typeof(LuaManager)),
        _GT(typeof(PanelManager)),
        _GT(typeof(TimerManager)), 
        _GT(typeof(ResourceManager)),

        _GT(typeof(EventManager)),

        //UI
        _GT(typeof(UnityEngine.UI.Button)),
        _GT(typeof(UnityEngine.UI.InputField)),
        _GT(typeof(UnityEngine.UI.Toggle)),
        _GT(typeof(UnityEngine.UI.Toggle.ToggleEvent)),
        _GT(typeof(UnityEngine.UI.ScrollRect)),
        _GT(typeof(UnityEngine.UI.ScrollRect.ScrollRectEvent)),
        _GT(typeof(UnityEngine.UI.Scrollbar)),
        _GT(typeof(UnityEngine.UI.Slider)),
        _GT(typeof(UnityEngine.UI.Slider.SliderEvent)),
        _GT(typeof(UnityEngine.UI.CanvasScaler)),
        _GT(typeof(UnityEngine.Canvas)),
        _GT(typeof(UnityEngine.CanvasGroup)),
        _GT(typeof(UnityEngine.UI.GridLayoutGroup)),
        _GT(typeof(UnityEngine.UI.HorizontalLayoutGroup)),
        _GT(typeof(UnityEngine.UI.Dropdown)),
        _GT(typeof(UnityEngine.UI.Dropdown.DropdownEvent)),
        _GT(typeof(UnityEngine.UI.Dropdown.OptionData)),
        _GT(typeof(UnityEngine.UI.Dropdown.OptionDataList)),

        _GT(typeof(System.Collections.Hashtable)), 
        _GT(typeof(UnityEngine.Rect)),
        _GT(typeof(GlobalVar)), 
    };

    public static List<Type> dynamicList = new List<Type>()
    {
        typeof(MeshRenderer),
#if !UNITY_5_4_OR_NEWER
        typeof(ParticleEmitter),
        typeof(ParticleRenderer),
        typeof(ParticleAnimator),
#endif

        typeof(BoxCollider),
        typeof(MeshCollider),
        typeof(SphereCollider),
        typeof(CharacterController),
        typeof(CapsuleCollider),

        typeof(Animation),
        typeof(AnimationClip),
        typeof(AnimationState),

        typeof(BlendWeights),
        typeof(RenderTexture),
        typeof(Rigidbody),
    };

    //重载函数，相同参数个数，相同位置out参数匹配出问题时, 需要强制匹配解决
    //使用方法参见例子14
    public static List<Type> outList = new List<Type>()
    {
        
    };
        
    //ngui优化，下面的类没有派生类，可以作为sealed class
    public static List<Type> sealedList = new List<Type>()
    {
        /*typeof(Transform),
        typeof(UIRoot),
        typeof(UICamera),
        typeof(UIViewport),
        typeof(UIPanel),
        typeof(UILabel),
        typeof(UIAnchor),
        typeof(UIAtlas),
        typeof(UIFont),
        typeof(UITexture),
        typeof(UISprite),
        typeof(UIGrid),
        typeof(UITable),
        typeof(UIWrapGrid),
        typeof(UIInput),
        typeof(UIScrollView),
        typeof(UIEventListener),
        typeof(UIScrollBar),
        typeof(UICenterOnChild),
        typeof(UIScrollView),        
        typeof(UIButton),
        typeof(UITextList),
        typeof(UIPlayTween),
        typeof(UIDragScrollView),
        typeof(UISpriteAnimation),
        typeof(UIWrapContent),
        typeof(TweenWidth),
        typeof(TweenAlpha),
        typeof(TweenColor),
        typeof(TweenRotation),
        typeof(TweenPosition),
        typeof(TweenScale),
        typeof(TweenHeight),
        typeof(TypewriterEffect),
        typeof(UIToggle),
        typeof(Localization),*/
    };

    public static BindType _GT(Type t)
    {
        return new BindType(t);
    }

    public static DelegateType _DT(Type t)
    {
        return new DelegateType(t);
    }    


    [MenuItem("Lua/Attach Profiler", false, 151)]
    static void AttachProfiler()
    {
        if (!Application.isPlaying)
        {
            EditorUtility.DisplayDialog("警告", "请在运行时执行此功能", "确定");
            return;
        }

        LuaClient.Instance.AttachProfiler();
    }

    [MenuItem("Lua/Detach Profiler", false, 152)]
    static void DetachProfiler()
    {
        if (!Application.isPlaying)
        {            
            return;
        }

        LuaClient.Instance.DetachProfiler();
    }
      
     
}