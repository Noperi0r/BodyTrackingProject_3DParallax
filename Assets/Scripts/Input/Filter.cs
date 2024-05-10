using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Filter
{
    public abstract Vector3 FilterOut();
    public abstract void FilterIn(Vector3 input);
}
