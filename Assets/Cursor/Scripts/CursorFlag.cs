using System;
using UnityEngine;

public class CursorFlag
{
    [Flags]
    public enum Flag
    {
        Hide        = 1 << 0,
        LockonEnemy = 1 << 1,
        //必要に応じて追加 1 << n
    }

    private Flag _flag;

    public CursorFlag()
    {
        _flag = 0;
    }

    public Flag GetFlag()
    {
        return _flag;
    }

    //フラグが1つ以上立っている
    public bool HasSomethingFlag()
    {
        int nothingFlag = 0;
        return (_flag.GetHashCode() | nothingFlag) != nothingFlag;
    }
    public bool IsNothingFlag()
    {
        return _flag.GetHashCode() == 0;
    }

    public void DebugOutput()
    {
        if (_flag.HasFlag(Flag.Hide))
            Debug.Log("隠す");

        if (_flag.HasFlag(Flag.LockonEnemy))
            Debug.Log("色変更");

        if (IsNothingFlag())
            Debug.Log("何もなし");

        if (HasSomethingFlag())
            Debug.Log("なんかある");
    }

    public void AddFlag(in CursorFlag.Flag flag)
    {
         _flag |= flag;
    }
    public void SubFlag(in CursorFlag.Flag flag)
    {
        _flag &= ~flag;
    }
}
