using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerTalkManager : MonoBehaviour
{
    //�C�x���g�`�F�b�N�t����b�f�[�^
    [System.Serializable]
    public class TalkDataWithCheck
    {
        public TalkPanel.TalkData _talkData;
        public EventCheckSender[] _eventCheckSender;
        public List<IEventCheck> _eventCheck;
    }

    [HideInInspector] private List<TalkDataWithCheck> _talkDataWCs = new List<TalkDataWithCheck>();
    [HideInInspector] private List<TalkDataWithCheck> _finishedTalkDataWCs = new List<TalkDataWithCheck>();

    // Update is called once per frame
    void Update()
    {
        if (_talkDataWCs.Count > 0)
        {
            foreach (var talkDataWC in _talkDataWCs)
            {
                foreach (var eventCheck in talkDataWC._eventCheck)
                {
                    if (eventCheck.Finished() == false)
                    {
                        continue;
                    }

                    UIManager.Instance.talkPanel.AddTalk(talkDataWC._talkData);
                    _finishedTalkDataWCs.Add(talkDataWC);
                }
            }
            if(_finishedTalkDataWCs.Count > 0)
            {
                foreach(var finishedTalkDataWC in _finishedTalkDataWCs)
                {
                    _talkDataWCs.Remove(finishedTalkDataWC);
                }
                _finishedTalkDataWCs.Clear();
            }
        }
    }

    //��b�̒ǉ�
    public void AddTalkDataWithCheck(TalkDataWithCheck[] talkDataWtihCheck)
    {
        foreach (var talkDataWC in talkDataWtihCheck)
        {
            //�C�x���g�`�F�b�N�̏�����
            talkDataWC._eventCheck = new List<IEventCheck>();
            if (talkDataWC._eventCheckSender.Length > 0)
            {
                foreach (var eventCheckSender in talkDataWC._eventCheckSender)
                {
                    if (eventCheckSender != null)
                    {
                        talkDataWC._eventCheck.Add(eventCheckSender.GetEventCheck());
                    }
                    else
                    {
                        talkDataWC._eventCheck.Add(new EventNoneCheck());
                    }
                }
            }
            else
            {
                talkDataWC._eventCheck.Add(new EventNoneCheck());
            }

            //�ǉ�
            _talkDataWCs.Add(talkDataWC);
        }
    }
}
