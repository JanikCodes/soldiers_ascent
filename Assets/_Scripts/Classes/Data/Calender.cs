using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that is used to generate the ScriptableObject <see cref="CalenderSO"/> at runtime from.
/// </summary>
[Serializable]
public class CalenderData : BaseData
{
    public string StartDateUnix;
}