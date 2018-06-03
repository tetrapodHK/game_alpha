using UnityEngine;
using UnityEngine.UI;
using System.Collections;



public class moveScript : MonoBehaviour
{



    float speed = 0f;

    float movePower = 1f;

    bool jumppingFlug = true;

    private int count;
    private float time = 151f;

    public Text counttext;
    public Text timetext;

    public gameOver gameover;

    Rigidbody rb;



    void Start()
    {

        rb = GetComponent<Rigidbody>();

        count = 0;

        SetCountText();

        timetext.text = time.ToString();

    }

    void Update()
    {
        time -= 1f * Time.deltaTime;

        timetext.text = ((int)time).ToString();

        if (time < 0.0f)
        {

            GameOver();

        }
    }


    void FixedUpdate()
    {

        

        if (Input.GetKey("up"))
        {

            Accel(); //アクセル

        }

        if (Input.GetKey("right"))
        {

            Right(); //右移動

        }

        if (Input.GetKey("left"))
        {

            Left(); //左移動

        }

        if (Input.GetKey("down"))
        {

            Stop(); //左移動

        }

        if (Input.GetKeyDown("space"))
        {

            if (jumppingFlug == true)
            {

                Jump();

            }

        }

        //減速

        speed -= 2f;

        //最低速度

        if (speed < 0)
        {

            speed = 0f;

        }

    }



    void Accel()
    {

        if (speed <= (800f + count*50f))
        {
            if (speed <= 5000f)
            {
                speed += 3f;
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, speed);
            }
        }

        

    }



    void Right()
    {

        if (transform.position.x <= 200f)
        {

            Vector3 temp = new Vector3(transform.position.x + movePower, transform.position.y, transform.position.z);

            transform.position = temp;

        }

    }



    public void Left()
    {

        if (transform.position.x >= -200f)
        {

            Vector3 temp = new Vector3(transform.position.x - movePower, transform.position.y, transform.position.z);

            transform.position = temp;

        }

    }

    public void Stop()
    {

        speed -= 2f;

        //最低速度

        if (speed < 0)
        {

            speed = 0f;

        }
    }

    void Jump()
    {

        jumppingFlug = false;

        rb.AddForce(Vector3.up * 3000);

    }

    void OnTriggerEnter(Collider col)
    {

        jumppingFlug = true;

        if (col.gameObject.CompareTag("Cube"))
        {
            col.gameObject.SetActive(false);

            count = count + 1;

            SetCountText();
        }else if (col.gameObject.CompareTag("disCube"))
        {
            col.gameObject.SetActive(false);

            count = count - 3;

            SetCountText();
        }

    }

    void GameOver()

    {

        gameover.SendMessage("Lose");

        Time.timeScale = 0;

        if (Input.GetMouseButtonDown(0))
        {

            Application.LoadLevel("title");

        }

    }

    void SetCountText()
    {
        // Update the text field of our 'countText' variable
        counttext.text = "Count: " + count.ToString();
    }

}