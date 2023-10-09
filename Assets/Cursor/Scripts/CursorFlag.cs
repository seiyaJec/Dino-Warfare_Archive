using System;
using UnityEngine;

public class CursorFlag
{
    [Flags]
    public enum Flag
    {
        Hide        = 1 << 0,
        LockonEnemy = 1 << 1,
        //�K�v�ɉ����Ēǉ� 1 << n
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

    //�t���O��1�ȏ㗧���Ă���
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
            Debug.Log("�B��");

        if (_flag.HasFlag(Flag.LockonEnemy))
            Debug.Log("�F�ύX");

        if (IsNothingFlag())
            Debug.Log("�����Ȃ�");

        if (HasSomethingFlag())
            Debug.Log("�Ȃ񂩂���");
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
