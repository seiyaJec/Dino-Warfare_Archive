/*this code is written in UTF-8*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotater : MonoBehaviour
{
    [Header("パラメータ")]
    public Vector3 rotValue;                //回転
    public int duration;                        //フレーム単位の間隔
    public float FOV;                           //ズームイン,アウト
    [Header("デバッグ")]
    public bool did;                              //回転処理を終えた

    /// <summary>
    /// 回転の条件が揃っているかどうか確認
    /// </summary>
    /// <returns></returns> <summary>
    /// 処理してよいならtrue
    /// </summary>
    /// <returns></returns>
    public bool CheckCond()
    {
        return true;
    }
}
