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

    
    //判定に使うデータ
    [Serializable]
    struct HideCursorMoveStationNums
    {
        public string name;
        public int start;
        public int end;
    }
    [SerializeField]
    List<HideCursorMoveStationNums> hideCursorMoveStationNums;

    //判定に使うクラス
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

        //色変更フラグの判定
    }

    private bool CanStandHideFlag()
    {
        //隠すフラグの判定

        if (gunMove.IsReloading())
            return true;

        if (UIManager.Instance.IsActiveContinuePanel())
            return true;

        if (UIManager.Instance.IsActiveGameClearPanel())
            return true;
        
        //特定のMoveStation内にいるか判定 PlayerMove内で作る？
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
