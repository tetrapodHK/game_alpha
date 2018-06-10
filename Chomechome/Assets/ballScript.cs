using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class ballScript : MonoBehaviour
{



    public GameObject ballPrefab;

    public Sprite[] ballSprites;

    private GameObject firstBall; //最初にドラッグしたボール

    private GameObject lastBall; //最後にドラッグしたボール

    private GameObject midBall;

    private string currentName; //名前判定用のstring変数

    //削除するボールのリスト

    List<GameObject> removableBallList = new List<GameObject>();

    public GameObject exchangeButton;

    public GameObject scoreGUI;

    public bool isPlaying = true;

    private int point = 0;


    void Start()
    {

        StartCoroutine(DropBall(40));


    }

    void Update()
    {

        //画面をクリックし、firstBallがnullの時実行

        if (isPlaying)
        {

            if (Input.GetMouseButtonDown(0) && firstBall == null)
            {

                OnDragStart();

            }
            else if (Input.GetMouseButtonUp(0))
            {

                //クリックを終えた時

                OnDragEnd();

            }
            else if (firstBall != null)
            {

                OnDragging();

            }
        }

    }

    private void OnDragStart()
    {

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);



        if (hit.collider != null)
        {

            GameObject hitObj = hit.collider.gameObject;

            //オブジェクトの名前を前方一致で判定

            string ballName = hitObj.name;

            if (ballName.StartsWith("chome"))
            {

                firstBall = hitObj;

                lastBall = hitObj;

                currentName = hitObj.name;

                //削除対象オブジェクトリストの初期化

                removableBallList = new List<GameObject>();

                //削除対象のオブジェクトを格納

                PushToList(hitObj);

            }

        }

    }

    private void OnDragging()
    {

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null)
        {

            GameObject hitObj = hit.collider.gameObject;
            int obj_cnt = 0;
            int flag = 0;

            //同じ名前のブロックをクリック＆lastBallとは別オブジェクトである時

            if ((hitObj.name == currentName || hitObj.name == "chome5") && lastBall != hitObj && firstBall != hitObj)
            {

                //２つのオブジェクトの距離を取得

                float distance = Vector2.Distance(hitObj.transform.position, lastBall.transform.position);

                if (distance < 1.1f)
                {

                    //削除対象のオブジェクトを格納

                    obj_cnt = removableBallList.Count;

                    for (int i = 0; i < obj_cnt; i++)
                    {
                        if(removableBallList[i] == hitObj)
                        {
                            flag = 1;
                        }

                    }

                    if (flag == 0)
                    {
                        lastBall = hitObj;

                        PushToList(hitObj);

                    }
                    flag = 0;
                }

            }

        }

    }

    private void OnDragEnd()
    {

        int remove_cnt = removableBallList.Count;

        if (remove_cnt >= 3)
        {
            point = 0;

            for (int i = 0; i < remove_cnt; i++)
            {

                Destroy(removableBallList[i]);
                if (i >= 2)
                {
                    point += i-1;
                }
            }

            scoreGUI.SendMessage("AddPoint", point);

            //********** 終了 **********//

            //ボールを新たに生成

            if (remove_cnt >= 7)
            {
                StartCoroutine(DropBall(-1));
                StartCoroutine(DropBall(remove_cnt-1));
            }
            else
            {

                StartCoroutine(DropBall(remove_cnt));

            }
        }
        else
        {

            //色の透明度を100%に戻す

            for (int i = 0; i < remove_cnt; i++)
            {

                ChangeColor(removableBallList[i], 1.0f);

            }

        }

        firstBall = null;

        lastBall = null;

    }


    IEnumerator DropBall(int count)
    {
        if (count == 40)
        {

            StartCoroutine("RestrictPush");

        }

        if (count == -1)
        {
            Vector2 pos = new Vector2(Random.Range(-2.0f, 2.0f), 7f);

            GameObject ball = Instantiate(ballPrefab, pos, Quaternion.AngleAxis(Random.Range(-40, 40), Vector3.forward)) as GameObject;

            ball.name = "chome" + 5;

            SpriteRenderer spriteObj = ball.GetComponent<SpriteRenderer>();

            spriteObj.sprite = ballSprites[5];

            yield return new WaitForSeconds(0.05f);

        }
        else { 


        for (int i = 0; i < count; i++)
        {


            Vector2 pos = new Vector2(Random.Range(-2.0f, 2.0f), 7f);

            GameObject ball = Instantiate(ballPrefab, pos, Quaternion.AngleAxis(Random.Range(-40, 40), Vector3.forward)) as GameObject;

            int spriteId = Random.Range(0, 5);

            ball.name = "chome" + spriteId;

            SpriteRenderer spriteObj = ball.GetComponent<SpriteRenderer>();

            spriteObj.sprite = ballSprites[spriteId];

            yield return new WaitForSeconds(0.05f);

        }
    }
    }

    IEnumerator RestrictPush()
    {

        exchangeButton.GetComponent<Button>().interactable = false;

        yield return new WaitForSeconds(5.0f);

        exchangeButton.GetComponent<Button>().interactable = true;

    }

    void PushToList(GameObject obj)
    {

        removableBallList.Add(obj);

        //色の透明度を50%に落とす

        ChangeColor(obj, 0.5f);

    }

    void ChangeColor(GameObject obj, float transparency)
    {

        //SpriteRendererコンポーネントを取得

        SpriteRenderer ballTexture = obj.GetComponent<SpriteRenderer>();

        //Colorプロパティのうち、透明度のみ変更する

        ballTexture.color = new Color(ballTexture.color.r, ballTexture.color.g, ballTexture.color.b, transparency);

    }

}