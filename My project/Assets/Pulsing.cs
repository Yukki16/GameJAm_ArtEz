using System.Collections;
using UnityEngine;

public class Pulsing : MonoBehaviour
{
    SphereCollider sonar;
    [SerializeField] float currentRadius;
    [SerializeField] float targetRadius;
    float lerpValue;
    public bool testBool;
    private void Awake()
    {
        sonar = this.GetComponent<SphereCollider>();
        currentRadius = 0.5f;
    }
    public void pulse()
    {
        testBool = false;
        lerpValue = 0;
        StartCoroutine(pulsed());
    }
    private void Update()
    {
        sonar.radius = Mathf.Lerp(currentRadius, targetRadius, lerpValue);
        if (testBool)
        {
            pulse();
        }
    }
    IEnumerator pulsed()
    {
        for(int i = 0; i < 101; i++)
        {
            lerpValue += 0.01f;
            yield return new WaitForSeconds(0.0025f);
        }
        lerpValue = 0;
        targetRadius = currentRadius;
        currentRadius = sonar.radius;
        for (int i = 0; i < 101; i++)
        {
            lerpValue += 0.01f;
            yield return new WaitForSeconds(0.0025f);
        }
        targetRadius = currentRadius;
        currentRadius = sonar.radius;
        lerpValue = 0;
    }
}
