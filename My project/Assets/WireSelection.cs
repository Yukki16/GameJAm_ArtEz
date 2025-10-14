using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireSelection : MonoBehaviour
{
    [SerializeField] List<GameObject> wireStarts;
    [SerializeField] List<GameObject> wireEnds;
    [SerializeField] List<int> noises;

    private void Awake()
    {
        for (int i = 1; i < 7; i++)
        {
            wireStarts.Add(this.transform.Find($"Wire-ends.00{i}").gameObject);
        }
        for(int i = 7; i < 10; i++)
        {
            wireEnds.Add(this.transform.Find($"Wire-ends.00{i}").gameObject);
        }
        for (int i = 10; i < 13; i++)
        {
            wireEnds.Add(this.transform.Find($"Wire-ends.0{i}").gameObject);
        }
    }

    void Start()
    {
        setSounds();
    }

    public void setSounds()
    {
        noises.Clear();
        foreach(GameObject obj in wireStarts)
        {
            int noiseNumber = Random.Range(1, 8);
            obj.GetComponent<WireChoice>().noiseNumber = noiseNumber;
            noises.Add(noiseNumber);
        }
        foreach (GameObject obj in wireEnds)
        {
            int randomNumber = Random.Range(0, noises.Count - 1);
            obj.GetComponent<WireChoice>().noiseNumber = noises[randomNumber];
            noises.RemoveAt(randomNumber);
        }
    }
}
