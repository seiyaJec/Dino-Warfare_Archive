using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IEventCheck
{
    //イベントが終了したか確認
    public bool Finished();
}