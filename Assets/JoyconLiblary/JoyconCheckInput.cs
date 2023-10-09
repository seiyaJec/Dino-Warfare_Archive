using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class JoyconCheckInput : MonoBehaviour
{
    [SerializeField] private GameObject _reconnectMessagePrefab;    //メッセージのプレハブ
    [HideInInspector] private GameObject _ui;                       //UI
    [HideInInspector] private GameObject _reconnectMessage;         //再接続メッセージ

    [HideInInspector]
    private GameObject ReconnectMessage
    {
        get
        {
            if (_reconnectMessage == null)
            {
                if (_ui == null)
                    return null;

                _reconnectMessage = Instantiate(_reconnectMessagePrefab, _ui.transform);
            }
            return _reconnectMessage;
        }
    }

    private static readonly Joycon.Button[] m_buttons =
    Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];
    private List<Joycon> m_joycons;
    public Joycon m_joyconL { get; private set; }
    public Joycon m_joyconR { get; private set; }
    public Joycon.Button? m_pressedButtonL { get; private set; }
    public Joycon.Button? m_pressedButtonR { get; private set; }


    //Singleton
    private static JoyconCheckInput m_Instance;
    public static JoyconCheckInput Instance
    {
        get
        {
            if (m_Instance == null) m_Instance = FindObjectOfType<JoyconCheckInput>();
            return m_Instance;
        }
    }

    private void Awake()
    {
        _ui = GameObject.FindGameObjectWithTag("UI");
        if(ReconnectMessage != null)
        {
            ReconnectMessage.SetActive(false);
        }
    }

    private void Start()
    {
        SetControllers();
    }

    private void Update()
    {
        //ジョイコンのリセット
        if (Input.GetKeyDown(KeyCode.O))
        {
            JoyconManager.Instance.ResetConnection();
        }

        GetInput();
    }

    //ジョイコンの情報を取得
    private void SetControllers()
    {
        m_joycons = JoyconManager.Instance.j;
        if (m_joycons == null || m_joycons.Count <= 0) return;
        m_joyconL = m_joycons.Find(c => c.isLeft);
        m_joyconR = m_joycons.Find(c => !c.isLeft);
    }

    //入力情報を初期化
    private void GetInput()
    {
        m_pressedButtonL = null;
        m_pressedButtonR = null;

        SetControllers();

        //接続チェック
        bool disconnected = false;
        if (m_joycons == null || m_joycons.Count <= 1)
        {
            disconnected = true;
        }
        else
        {
            //接続が落ちたものはリストから削除
            foreach (var joycon in m_joycons)
            {
                if (joycon.state == Joycon.state_.DROPPED)
                {
                    joycon.Detach();
                }
            }

            //左右とも接続されているかチェック（複数接続することを考えて切り分けた）
            if(m_joyconL == null || m_joyconR == null
                || m_joyconL.state <= Joycon.state_.DROPPED || m_joyconR.state <= Joycon.state_.DROPPED)
            {
                disconnected = true;
            }
        }
        if (disconnected == true)
        {
            //再接続メッセージの表示
            if (ReconnectMessage != null)
            {
                _reconnectMessage.SetActive(true);
            }
            return;
        }

        //再接続メッセージの非表示
        if (ReconnectMessage != null && ReconnectMessage.activeSelf == true)
        {
            ReconnectMessage.SetActive(false);
        }


        foreach (var button in m_buttons)
        {
            if (m_joyconL.GetButton(button))
            {
                m_pressedButtonL = button;
            }
            if (m_joyconR.GetButton(button))
            {
                m_pressedButtonR = button;
            }
        }

    }

}
