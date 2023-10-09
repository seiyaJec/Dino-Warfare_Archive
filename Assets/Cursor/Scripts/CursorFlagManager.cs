using System;
using System.Collections.Generic;
using UnityEngine;

public class CursorFlagManager : MonoBehaviour
{
    private CursorFlag flag;
    public CursorFlag.Flag GetFlag()
    {
        return flag.GetFlag();
    }

    
    //����Ɏg���f�[�^
    [Serializable]
    struct HideCursorMoveStationNums
    {
        public string name;
        public int start;
        public int end;
    }
    [SerializeField]
    List<HideCursorMoveStationNums> hideCursorMoveStationNums;

    //����Ɏg���N���X
    PlayerMove playerMove;
    GunMove    gunMove;

    // Start is called before the first frame update
    void Start()
    {
        flag = new CursorFlag();

        GameObject player = GameObject.FindWithTag("Player");
        playerMove = player.GetComponent<PlayerMove>();
        gunMove = player.GetComponentInChildren<GunMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CanStandHideFlag())
        {
            flag.AddFlag(CursorFlag.Flag.Hide);
        }
        else
        {
            flag.SubFlag(CursorFlag.Flag.Hide);
        }

        //�F�ύX�t���O�̔���
    }

    private bool CanStandHideFlag()
    {
        //�B���t���O�̔���

        if (gunMove.IsReloading())
            return true;

        if (UIManager.Instance.IsActiveContinuePanel())
            return true;

        if (UIManager.Instance.IsActiveGameClearPanel())
            return true;
        
        //�����MoveStation���ɂ��邩���� PlayerMove���ō��H
        int nowStation = playerMove._nextStation - 1;
        foreach (HideCursorMoveStationNums nums in hideCursorMoveStationNums)
        {
            if (nowStation >= nums.start &&
                nowStation < nums.end)
                return true;
        }

        return false;
    }

    private void CanStandLockonEnemyFlag()
    {

    }
}
