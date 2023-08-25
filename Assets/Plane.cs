using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public Camera main_camera;
    Vector3 pos;
    float x_scale;
    float total_length = 0; //should be 20 at end
    float z_pos;
    public GameObject plane;
    public GameObject cube; 
    List<GameObject> planes_list = new List<GameObject>();

    float count = 0;

    float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        z_pos = plane.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        bool blocks = false;

        float end_pos = 10;

        float max_length = GetComponent<Renderer>().bounds.size.x;
       // Debug.Log("plane len: " + plane.GetComponent<Renderer>().bounds.size.x);
        
        if(timer >= 0.5f)
        {
            z_pos = z_pos - 2;

            while (total_length < max_length)
            {
                x_scale = Random.Range(0.2f, 0.5f);
                plane.transform.localScale = new Vector3(x_scale, 1f, 0.2f);

                float x_pos = end_pos - (x_scale * 5);
      
                pos = new Vector3(x_pos, plane.transform.position.y , z_pos);

                float x_length = plane.GetComponent<Renderer>().bounds.size.x;

                if (total_length + x_length < 20)
                {
                    planes_list.Add(Instantiate(plane, pos, transform.rotation) as GameObject);
                    count++;
                    blocks = true;
                }
                else
                {
                    //Debug.Log("in else because > 20: "+total_length + x_length);

                    x_length = 20 - total_length;
                    x_scale = x_length/10;
                    plane.transform.localScale = new Vector3(x_scale, 1f, 0.2f);
                    x_pos = end_pos - (x_scale * 5);
                    pos = new Vector3(x_pos, 0f, z_pos);

                    planes_list.Add(Instantiate(plane, pos, transform.rotation) as GameObject);
                    count++;
                }

                total_length += x_length;

                end_pos = x_pos - (x_scale * 5);
            }

            //choose random block to delete
            int hole_num = (int) Random.Range(1, planes_list.Count+1); //inclusive, exclusive
            int block_num = (int) Random.Range(1, planes_list.Count + 1);

            Destroy(planes_list[hole_num-1].gameObject);

            float y;
            if (block_num != hole_num)
            {
                float x = planes_list[block_num - 1].transform.position.x;

                if (plane.transform.position.y == 0)
                    y = 0.25f;
                else
                    y = 4.45f;

                Vector3 block_pos = new Vector3(x, y, z_pos);
                Instantiate(cube, block_pos, transform.rotation);
                count++;
            }
            

            planes_list.Clear();

            total_length = 0;
            timer = 0; 
        }

        //delete passed planes
        if(blocks)
        {
            GameObject p = GameObject.Find("Plane(Clone)");

            if (!(p.GetComponent<Renderer>().isVisible))
            {
                //Debug.Log("in visible thing: "+p.transform.position);
                Destroy(p);
            }
        }
        
    }
}
