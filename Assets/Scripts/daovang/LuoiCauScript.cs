using UnityEngine;
using System.Collections;
using UnityEngine.Android;

public class LuoiCauScript : MonoBehaviour
{
    public float speed;
    public float speedMin;
    public float speedBegin;
    public Vector2 velocity;
    public float maxX;
    public float minX;
    public float minY;
    public float maxY;
    public Transform target;
    Vector3 prePosition;
    private Rigidbody2D rib;
    public int type;
    public Collider2D collider;
    public bool isUpSpeed;
    public float timeUpSpeed;
    public bool ab = false;
    DayCauScript daycau;
    GamePlayScript gps;


    public AudioSource audioSource;
    public AudioClip audioClip;
    //AudioClip audioClip;
    //public AudioClip audioClip;

    public bool isAttached = false;

    protected Vector3 originPosition;
    // Use this for initialization
    void Start()
    {
        originPosition = transform.position;
        isUpSpeed = false;
        prePosition = transform.localPosition;
        rib = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        gps = FindObjectOfType<GamePlayScript>();
        daycau = FindObjectOfType<DayCauScript>();
        audioSource.GetComponent<AudioSource>();
        //		this.StartCoroutine("TimeUpSpeed");
    }

    public void ReverseMovement(float goldSpeed)
    {
        speed = speed - goldSpeed;
        if (isAttached == false)
        {
            isAttached = true;
            velocity = -velocity;
        }
    }
    public IEnumerator TimeUpSpeed()
    {
        while (true)
        {
            if (isUpSpeed)
            {
                timeUpSpeed = timeUpSpeed - 1;
                if (timeUpSpeed <= 0)
                    isUpSpeed = false;
            }
            yield return new WaitForSeconds(1);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (speed <= 0 && daycau.typeAction == TypeAction.KeoCau)
        {
            gps.endGame();
        }
        if (gameObject.transform.position.y >= (2.21f - 0.56f) && collider.enabled == true)
        {
            speed = 0;
            velocity = Vector3.zero;
            gameObject.transform.position = originPosition;
            isAttached = false;
            GameObject.Find("dayCau").GetComponent<DayCauScript>().typeAction = TypeAction.Nghi;
        }



        //checkKeoCauXong ();
        //		if(CGameManager.Instance.gameState == EnumStateGame.Play) 

        checkTouchScene();

        checkMoveOutCameraView();


        var cameraPos = Camera.main.transform.position;
        cameraPos.y = transform.position.y;
        Camera.main.transform.position = cameraPos;

        if (Input.GetKey(KeyCode.A))
        {
            //var _currentPosition = gameObject.transform.localPosition;
            //_currentPosition += (Vector3)Vector2.left * speed * Time.deltaTime;
            //gameObject.transform.localPosition = _currentPosition;
            float maxX = Camera.main.orthographicSize;

            Vector3 resultPosition = transform.localPosition + (Vector3)(Vector2.left * speed * Time.deltaTime);
            resultPosition.x = Mathf.Clamp(resultPosition.x, -maxX, maxX);
            gameObject.transform.localPosition = resultPosition;
        }

        if (Input.GetKey(KeyCode.D))
        {
            float maxX = Camera.main.orthographicSize;

            Vector3 resultPosition = transform.localPosition + (Vector3)(Vector2.right * speed * Time.deltaTime);
            resultPosition.x = Mathf.Clamp(resultPosition.x, -maxX, maxX);
            gameObject.transform.localPosition = resultPosition;

        }

    }

    void FixedUpdate()
    {
        //		if(CGameManager.Instance.gameState == EnumStateGame.Play) 
        
            var _daycau = GameObject.Find("dayCau").GetComponent<DayCauScript>();

            if (_daycau.typeAction == TypeAction.ThaCau ||
                GameObject.Find("dayCau").GetComponent<DayCauScript>().typeAction == TypeAction.KeoCau)
            {
                rib.velocity = velocity * speed;
            }
            else if (_daycau.typeAction == TypeAction.Nghi)
                rib.velocity = Vector3.zero;
        
    }


    //	void OnTriggerEnter2D(Collider2D other) {
    //		//		Debug.Log("enter");
    //		if(other.gameObject.name.CompareTo("dau") == 0) {
    //			GameObject fish = other.gameObject.transform.parent.gameObject;
    //			fish.GetComponent<CFishScript>().fishAction = EnumFishAction.CanCau;
    //			if(!isUpSpeed) {
    //				if(speed > fish.GetComponent<CFishScript>().reduceSpeed) {
    //					speed = speed - fish.GetComponent<CFishScript>().reduceSpeed;
    //					if(speed < speedMin)
    //						speed = speedMin;
    //				}
    //			}
    //
    //			if(GameObject.Find("dayCau").GetComponent<DayCauScript>().typeAction == TypeAction.ThaCau) {
    //				GameObject.Find("dayCau").GetComponent<DayCauScript>().typeAction = TypeAction.KeoCau;
    //				velocity = -velocity;
    //			}
    //		}
    //
    //	}

    void OnTriggerExit2D(Collider2D other)
    {
        //		Debug.Log("exit");
        //		if(other.gameObject.name == "luoiCau") {
        //			isBorder = false;
        //		}
    }
    bool checkPositionOutBound()
    {
        if (gameObject.transform.position.y <= -31f)
        {
            return false;
        }
        return true;
    }

    void checkTouchScene()
    {
        bool istouch = Input.GetMouseButtonDown(0);
        if (istouch && GameObject.Find("dayCau").GetComponent<DayCauScript>().typeAction == TypeAction.Nghi)
        {
            if(audioClip && audioSource)
            {
                audioSource.PlayOneShot(audioClip);

            }
            speed = speedBegin;
            collider.enabled = false;
            GameObject.Find("dayCau").GetComponent<DayCauScript>().typeAction = TypeAction.ThaCau;
            velocity = new Vector2(transform.position.x - target.position.x,
                transform.position.y - target.position.y);
            velocity.Normalize();
        }
    }

    //kiem tra khi luoi cau ra ngoai tam nhin cua camera
    void checkMoveOutCameraView()
    {
        if (GameObject.Find("dayCau").GetComponent<DayCauScript>().typeAction == TypeAction.ThaCau)
        {
            if (!checkPositionOutBound())
            {
                GameObject.Find("dayCau").GetComponent<DayCauScript>().typeAction = TypeAction.KeoCau;
                velocity = -velocity;
                collider.enabled = true;
                isAttached = true;
            }
        }
    }

    //kiem tra khi luoi ca keo len mat nuoc
    //void checkKeoCauXong ()
    //{
    //	if (transform.localPosition.y > maxY && GameObject.Find ("dayCau").GetComponent<DayCauScript> ().typeAction == TypeAction.KeoCau) {
    //		Debug.Log ("keo cau xong");
    //		rib.velocity = Vector2.zero;
    //		GameObject.Find ("dayCau").GetComponent<DayCauScript> ().typeAction = TypeAction.Nghi;
    //		transform.localPosition = prePosition;
    //	}
    //}
}
