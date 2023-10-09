using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class JoyconCheckInput : MonoBehaviour
{
    [SerializeField] private GameObject _reconnectMessagePrefab;    //���b�Z�[�W�̃v���n�u
    [HideInInspector] private GameObject _ui;                       //UI
    [HideInInspector] private GameObject _reconnectMessage;         //�Đڑ����b�Z�[�W

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
        //�W���C�R���̃��Z�b�g
        if (Input.GetKeyDown(KeyCode.O))
        {
            JoyconManager.Instance.ResetConnection();
        }

        GetInput();
    }

    //�W���C�R���̏����擾
    private void SetControllers()
    {
        m_joycons = JoyconManager.Instance.j;
        if (m_joycons == null || m_joycons.Count <= 0) return;
        m_joyconL = m_joycons.Find(c => c.isLeft);
        m_joyconR = m_joycons.Find(c => !c.isLeft);
    }

    //���͏���������
    private void GetInput()
    {
        m_pressedButtonL = null;
        m_pressedButtonR = null;

        SetControllers();

        //�ڑ��`�F�b�N
        bool disconnected = false;
        if (m_joycons == null || m_joycons.Count <= 1)
        {
            disconnected = true;
        }
        else
        {
            //�ڑ������������̂̓��X�g����폜
            foreach (var joycon in m_joycons)
            {
                if (joycon.state == Joycon.state_.DROPPED)
                {
                    joycon.Detach();
                }
            }

            //���E�Ƃ��ڑ�����Ă��邩�`�F�b�N�i�����ڑ����邱�Ƃ��l���Đ؂蕪�����j
            if(m_joyconL == null || m_joyconR == null
                || m_joyconL.state <= Joycon.state_.DROPPED || m_joyconR.state <= Joycon.state_.DROPPED)
            {
                disconnected = true;
            }
        }
        if (disconnected == true)
        {
            //�Đڑ����b�Z�[�W�̕\��
            if (ReconnectMessage != null)
            {
                _reconnectMessage.SetActive(true);
            }
            return;
        }

        //�Đڑ����b�Z�[�W�̔�\��
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
