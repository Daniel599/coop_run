using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Side
{
    Upper,
    Lower
}
public interface Character
{
    Side Side { get; set; }

    void Pause();
    void Continue();
    void Reset(Vector3 position);
}
