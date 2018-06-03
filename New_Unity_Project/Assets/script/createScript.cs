using UnityEngine;
using UnityEngine.UI;
using System.Collections;



public class createScript : MonoBehaviour
{



    public GameObject road1;

    public GameObject road2;

    public GameObject cube;

    public GameObject discube;

    int border = 1000;

    int makecube = 0;



    void Update()
    {

        if (transform.position.z > border)
        {

            CreateMap();

        }

    }



    void CreateMap()
    {

        if (road1.transform.position.z < border)
        {

            border += 2000;

            Vector3 temp = new Vector3(0, 0.05f, border);

            road1.transform.position = temp;

            Vector3 tempa = new Vector3(Random.Range(-60, 60), 15, border + Random.Range(-500, 500));

            makecube = Random.Range(-1, 5);

            if (makecube > 0)
            {
                cube.gameObject.SetActive(true);
                discube.gameObject.SetActive(false);
                cube.transform.position = tempa;
            }
            else
            {
                cube.gameObject.SetActive(false);
                discube.gameObject.SetActive(true);
                discube.transform.position = tempa;
            } 
           

        }
        else if (road2.transform.position.z < border)
        {

            border += 2000;

            Vector3 temp = new Vector3(0, 0.05f, border);

            road2.transform.position = temp;

        }

    }

}