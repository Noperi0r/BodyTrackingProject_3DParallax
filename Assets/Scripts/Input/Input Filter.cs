using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Principal;
using UnityEngine;

public class InputFilter : MonoBehaviour
{
    enum FilterType { ONEEURO, AVERAGE, EXPONENTIAL};
    [SerializeField] FilterType Type;
    [SerializeField] static int windowSize = 4;
    Filter filter;
    void Awake()
    {
        switch(Type)
        {
            case FilterType.ONEEURO:
                filter = new OneEuroFilter(windowSize);
                break;
            case FilterType.AVERAGE:
                filter = new AverageFilter(windowSize);
                break;
            case FilterType.EXPONENTIAL:
                filter = new ExponentialFilter(windowSize);
                break;
        }
    }

/*    private void Update()
    {
        //임시 Vector, 이후 SerializeField로 Handler 연결해서 값 입력받아주면 됨.
        Vector3 vector3 = inputVector;
        filter.FilterIn(vector3);
    }
*/
    public Vector3 GetVector(Vector3 input)
    {
        filter.FilterIn(input);
        return filter.FilterOut();
    }

}
