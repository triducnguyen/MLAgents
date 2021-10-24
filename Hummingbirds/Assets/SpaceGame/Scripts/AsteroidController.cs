using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public MeshRenderer asteroidRenderer;

    [Range(0.9f, 1.5f)]
    public float xScaleRange = 1;
    [Range(0.9f, 1.5f)]
    public float yScaleRange = 1;
    [Range(0.9f, 1.5f)]
    public float zScaleRange = 1;

    [Range(-.3f, .3f)]
    public float xTorqueRange = 0f;
    [Range(-.3f, .3f)]
    public float yTorqueRange = 0f;
    [Range(-.3f, .3f)]
    public float zTorqueRange = 0f;

    public Vector3 angularVelocity;
    public Rigidbody rigidbody;

    public bool spedUp = false;

    // Start is called before the first frame update
    void Start()
    {
        xTorqueRange = Random.Range(-0.3f, 0.3f);
        yTorqueRange = Random.Range(-0.3f, 0.3f);
        zTorqueRange = Random.Range(-0.3f, 0.3f);

        RandomizeObjectTransforms();
        angularVelocity = new Vector3(xTorqueRange, yTorqueRange, zTorqueRange);
        float h;
        float s;
        float v;
        Color.RGBToHSV(asteroidRenderer.materials[0].GetColor("AsteroidColor"), out h, out s, out v);
        Color newColor = Color.HSVToRGB(h, s, Mathf.Clamp(v*Random.Range(0.5f,1.5f),0.1f, 0.9f));
        asteroidRenderer.materials[0].SetColor("AsteroidColor", newColor);
    }

    void RandomizeObjectTransforms()
    {
        transform.localScale = new Vector3(transform.localScale.x * xScaleRange, transform.localScale.y * yScaleRange, transform.localScale.z * zScaleRange);
    }
    // Update is called once per frame
    void Update()
    {
        if (!spedUp)
        {
            rigidbody.angularVelocity = Vector3.Lerp(rigidbody.angularVelocity, angularVelocity, .1f);
            if (rigidbody.angularVelocity.x > angularVelocity.x && rigidbody.angularVelocity.y > angularVelocity.y && rigidbody.angularVelocity.z > angularVelocity.z)
            {
                spedUp = true;
            }
        }
    }
}
