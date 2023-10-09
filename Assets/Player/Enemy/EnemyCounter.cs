using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    private int _enemyTypes = System.Enum.GetValues(typeof(EnemysType)).Length;
    private int _enemyRemoved = 0;      //消えた敵の数（基本的に増え続ける）
    public enum EnemysType
    {
        Velociraptor = 0,
        Triceratops,
        Tyrannosaurus,
        Pteranodon,

    }
    [HideInInspector] public int[] _enemysNum { get; private set; }
    private void Awake()
    {
        _enemysNum = new int[_enemyTypes];
    }

    //敵のカウントを増やす
    public void AddCount(EnemysType type)
    {
        ++_enemysNum[(int)type];
    }

    //敵のカウントを減らす
    public void RemoveCount(EnemysType type)
    {
        --_enemysNum[(int)type];
        ++_enemyRemoved;
    }

    //敵の総数を取得
    public int LengthAll()
    {
        int length = 0;
        foreach(int enemy in _enemysNum)
        {
            length += enemy;
        }
        return length;
    }

    //倒した敵の数を取得
    public int GetEnemyRemoved()
    {
        return _enemyRemoved;
    }
}
