using UnityEngine;
using UnityEngine.Events;

//アニメーションイベント処理クラス
public class AnimationEvent : UnityEvent<string>
{

}

public class AnimationEvents : MonoBehaviour
{
    public AnimationEvent AnimationEvent = new AnimationEvent();

    public void OnAnimationEvent(string evenName)
    {
        AnimationEvent.Invoke(evenName);
    }

}
