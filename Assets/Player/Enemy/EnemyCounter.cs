using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    private int _enemyTypes = System.Enum.GetValues(typeof(EnemysType)).Length;
    private int _enemyRemoved = 0;      //�������G�̐��i��{�I�ɑ���������j
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

    //�G�̃J�E���g�𑝂₷
    public void AddCount(EnemysType type)
    {
        ++_enemysNum[(int)type];
    }

    //�G�̃J�E���g�����炷
    public void RemoveCount(EnemysType type)
    {
        --_enemysNum[(int)type];
        ++_enemyRemoved;
    }

    //�G�̑������擾
    public int LengthAll()
    {
        int length = 0;
        foreach(int enemy in _enemysNum)
        {
            length += enemy;
        }
        return length;
    }

    //�|�����G�̐����擾
    public int GetEnemyRemoved()
    {
        return _enemyRemoved;
    }
}
