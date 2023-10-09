using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveStationEvent
{
    void Begin(PlayerMove player);
    void UpDate(PlayerMove player);
    void End(PlayerMove player);
}
