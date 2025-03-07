using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Encyclopedia
{
    public int id;
    public bool caught;
}

[Serializable]
public class EncyclopediaCollection
{
    public List<Encyclopedia> encyclopedia = new List<Encyclopedia>();
}