using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTestScript : MonoBehaviour
{
    [SerializeField] GameObject ob;
    [SerializeField] GameObject obAver;
    [SerializeField] GameObject obExp;
    [SerializeField] Vector3 v3;
    AverageFilter Avfilter;
    ExponentialFilter ExpFilter;
    void Start()
    {
        Avfilter = new AverageFilter(4);
        ExpFilter = new ExponentialFilter(4, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = new(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
        Avfilter.FilterIn(v);
        ExpFilter.FilterIn(v);

        ob.transform.position = v;
        obAver.transform.position = Avfilter.FilterOut();
        obExp.transform.position= ExpFilter.FilterOut();
    }
}
