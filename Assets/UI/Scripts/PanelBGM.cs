using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBGM : MonoBehaviour
{
    [HideInInspector] private float _mainBGMStopTime;
    [HideInInspector] private AudioClip _mainBGM;

    [SerializeField] private AudioClip _panelBGM;
    [Header("メインからパネルへの移行")]
    [SerializeField] private float _mainFadeOutTime;
    [SerializeField] private float _panelFadeInTime;
    [Header("パネルからメインへの移行")]
    [SerializeField] private float _panelFadeOutTime;
    [SerializeField] private float _mainFadeInTime;

    private void OnEnable()
    {
        _mainBGMStopTime = SoundManager.Instance.GetPlayTime(SoundManager.Sound.BGM);
        _mainBGM = SoundManager.Instance.GetAudioClip(SoundManager.Sound.BGM);
        SoundManager.Instance.Swap(_panelBGM, _mainFadeOutTime, _panelFadeInTime, SoundManager.Sound.BGM);
    }

    private void OnDisable()
    {
        SoundManager.Instance.Swap(_mainBGM, _panelFadeOutTime, _mainFadeInTime, SoundManager.Sound.BGM, _mainBGMStopTime);
    }
}
