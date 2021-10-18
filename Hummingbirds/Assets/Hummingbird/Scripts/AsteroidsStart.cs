using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsStart : MonoBehaviour
{
    public float globalScaleMultiplier = 1f;

    public bool scaleUniformly;

    public float uniformScaleMin = .1f;
    public float uniformScaleMax = 1f;

    public float xScaleMin = 1f;
    public float xScaleMax = 2f;
    public float yScaleMin = 1f;
    public float yScaleMax = 2f;
    public float zScaleMin = 1f;
    public float zScaleMax = 2f;

    public float moveUpMin = .1f;
    public float moveUpMax = 1f;
    public float moveDownMin = .1f;
    public float moveDownMax = 1f;

    // Start is called before the first frame update
    void Start()
    {
        RandomizeObjectTransforms();
    }

    void RandomizeObjectTransforms()
    {
        Vector3 randomizedScale = Vector3.one;
        if (scaleUniformly)
        {
            float uniformScale = Random.Range(uniformScaleMin, uniformScaleMax);
            randomizedScale = new Vector3(uniformScale, uniformScale, uniformScale);
        }
        else
        {
            randomizedScale = new Vector3(Random.Range(xScaleMin, xScaleMax), Random.Range(yScaleMin, yScaleMin), Random.Range(zScaleMin, zScaleMax));
        }

        transform.localScale = randomizedScale * globalScaleMultiplier;
        transform.localRotation = Random.rotation;
        transform.Translate(Vector3.up * Random.Range(moveUpMin, moveUpMax));
        transform.Translate(Vector3.down * Random.Range(moveDownMin, moveDownMax));
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
