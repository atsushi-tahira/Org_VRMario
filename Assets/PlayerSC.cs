using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSC : MonoBehaviour {

    #region 変数宣言
    Rigidbody pRb;
    public int jumpPower = 1000;
    public Vector3 targetPos;
    float moveSPD = 0.2f;
    public float lerp = 5f;
    public bool isMove = false;
    public bool isRange = false;
    public bool isGround = true;
    float dbgTime;
    GameObject cam;
    Vector3 camForward;
    public GameObject playerPrefab;
    public Text stickVector;
    string vectorText;
    #endregion

    private void Awake()
    {
        GetComponent<PlayerSC>().enabled = true;
    }
    // Use this for initialization
    void Start () {
        pRb = GetComponent<Rigidbody>();
        cam = GameObject.Find("OVRCameraRig");
	}
	
	// Update is called once per frame
	void Update () {

        camForward = new Vector3(0f, cam.transform.forward.y, 90f);
        transform.forward = camForward;

        float dx = Input.GetAxis("Horizontal");
        float dy = Input.GetAxis("Vertical");

        //float stickx = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x;
        //float sticky = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).y;

        float stickx = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick, OVRInput.Controller.LTouch).x;
        float sticky = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick, OVRInput.Controller.LTouch).y;


        float rstickx = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x;
        float rsticky = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y;


        //vectorText = stickL.ToString();
        //stickVector.text = "stickx = " + stickx.ToString() + "\nsticky = " + sticky.ToString();
        stickVector.text = "rstickx = " + rstickx.ToString() + "\nrsticky = " + rsticky.ToString();

        //targetへの移動中は、入力による移動を制限
        if (!isMove)
        {
            //transform.Translate(dx * moveSPD, 0.0f, dy * moveSPD);
			transform.Translate(stickx * moveSPD, 0.0f, sticky * moveSPD);
        }

        //OculusではAボタンでジャンプ、範囲内ならグラップル
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            pRb.AddForce(Vector3.up * jumpPower);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !isMove && isRange)
        {
            isMove = true;
            pRb.useGravity = false;
            dbgTime = 0f;
        }



        //if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
        //{
        //    pRb.AddForce(Vector3.right * jumpPower);
        //    vectorText = "RightIndex now";
        //}

        //if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger))
        //{
        //    pRb.AddForce(Vector3.left * jumpPower);
        //    vectorText = "LeftIndex now";
        //}



        if (OVRInput.GetDown(OVRInput.Button.One) && isGround)
        {
            vectorText = "RawButton A now";
            pRb.AddForce(Vector3.up * jumpPower);

        }
        else if (OVRInput.GetDown(OVRInput.Button.One) && !isMove && isRange)
        {
            isMove = true;
            pRb.useGravity = false;
            dbgTime = 0f;
        }


    }

    private void FixedUpdate()
    {
        if (isMove)
        {
            //targetの位置までLerpで緩やかに移動する
            dbgTime += Time.deltaTime;
            transform.position = Vector3.Lerp(this.transform.position, targetPos, Time.deltaTime * lerp);
            Debug.Log("isMove,targetPos = " + targetPos);
            if (dbgTime > 0.8f)
            {
                isMove = false;
                pRb.useGravity = true;
            }
        }
    }



    private void OnCollisionEnter(Collision collision)
    {

        //ターゲットに接したら移動状態を解除して、重力をOnにする
        if (collision.gameObject.tag == "target") {
            isMove = false;
            pRb.useGravity = true;
        }

        //着陸判定
        if (collision.gameObject.tag == "ground")
        {
            isGround = true;
        }

        //オブジェクト衝突判定
        if (collision.gameObject.tag == "Obstacle")
        {
            Destroy(this.gameObject);
            Instantiate(playerPrefab, new Vector3(0, 0, 10), Quaternion.identity);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //離陸判定
        if (collision.gameObject.tag == "ground")
        {
            isGround = false;
        }
    }
}
