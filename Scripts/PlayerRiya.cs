

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerRiya : MonoBehaviour
{
    private bool hasSwiped = false;
    public bool jump = false;
    public bool slide = false;
    public Animator anim;
    private DiamondScoreAnim motor;
    public int laneNum = 2;
    public float horizVel = 0;
    public int zVel = 6;
    public float coinsCollected = 0, score = 0, modifierScore = 0;
    public Text coinText, scoreText, modifierText;
    private float speed;
    private float speedIncreaseLastTick;
    private float speedIncreaseTime = 2.9f;
    private float speedIncreaseAmount = 0.3f;
    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;
    private float originalSpeed = 7.0f;
    public Animator deathMenuAnim;
    private bool isalreadyDead = false;
    public Animator gameCanvas;
    public Text deadScoreText, deadCoinText;
    private int maxHealth = 4;
    private int currentHealth;
    public HealthBar healthbar;
    public bool boost = false;
    public Rigidbody rbody;
    public CapsuleCollider myCollider;
    public bool isRunning = false;
    private void Awake()
    {
        modifierText.text = modifierScore.ToString("0.0");
        scoreText.text = score.ToString("0");
    }
    void Start()
    {
        motor = GameObject.FindGameObjectWithTag("ScoreBoard").GetComponent<DiamondScoreAnim>();
        speed = originalSpeed;
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
        gameCanvas.SetTrigger("Show");
        rbody = GetComponent<Rigidbody>();
        myCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {  
        if(isalreadyDead)
        {
            return;
        }
        if(currentHealth == 0) 
        {
            Death();
            return;
        }      
        if (Time.time - speedIncreaseLastTick > speedIncreaseTime)
        {
            speedIncreaseLastTick = Time.time;
            speed += speedIncreaseAmount;
            Debug.Log(speed);
            modifierScore = speed - originalSpeed;
            modifierText.text = modifierScore.ToString("0.0");
        }
        GetComponent<Rigidbody>().velocity = new Vector3 (horizVel, 0, speed);
        

        //Mobile Input
        //Replace the aread between ---- with the lapControl function to get drag
        score += (Time.deltaTime * modifierScore);
        scoreText.text = score.ToString("0");
        // -----------------------------------------------
        if(Input.GetMouseButtonDown(0))
        {
            //save began touch 2d point
            firstPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
        }
        if(Input.GetMouseButtonUp(0))
        {
                //save ended touch 2d point
            secondPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
        
                //create vector from the two points
            currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
            
            //normalize the 2d vector
            currentSwipe.Normalize();
    
            //swipe upwards
            if(currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
            {
                jump = true;
            } else {
                jump = false;
            }
            if(jump == true)
            {
                anim.SetBool("isJump", jump);
                GetComponent<Rigidbody>().velocity = new Vector3 (horizVel, 5.5f, speed + 4f);
                StartCoroutine(stopJump());
            } else if(jump == false) {
                anim.SetBool("isJump", jump);
            }
                
            //swipe down
            if(currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
            {
                slide = true;
            } else {
                slide = false;
            }

            if(slide == true)
            {
                anim.SetBool("isSlide", slide);
                transform.Translate(0, 0, 0.2f);
                StartCoroutine(stopSlide());
            } else if(slide == false) {
                anim.SetBool("isSlide", slide);
            }
                
            //swipe left
            if(currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f && (laneNum > 1) && (laneNum <= 3) && !hasSwiped)
            {
                hasSwiped = true;
                horizVel = -4;
                StartCoroutine(stopLaneChange());
                laneNum -= 1;                
            }
            //swipe right
            if(currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f && (laneNum >= 1) && (laneNum < 3) && !hasSwiped)
            {
                hasSwiped = true;
                horizVel = 4;
                StartCoroutine(stopLaneChange());
                laneNum += 1;
            }
        }
        //------------------------------------------------
        if(boost == true) 
        {
            rbody.isKinematic = true;
            myCollider.enabled = false;  
            GetComponent<Rigidbody>().velocity = new Vector3(horizVel, 0, speed + 25f);
        } else {
            myCollider.enabled = true;
            rbody.isKinematic = false;
        }

        coinText.text = coinsCollected.ToString();
        
    }
    
    IEnumerator stopLaneChange()
    {
        yield return new WaitForSeconds(.25f);
        horizVel = 0;
        Debug.Log(laneNum);
        hasSwiped = false;
    }

    IEnumerator stopJump()
    {
        GetComponent<Rigidbody>() .velocity = new Vector3 (horizVel, 5.5f, speed);
        yield return new WaitForSeconds(.5f);
        GetComponent<Rigidbody>() .velocity = new Vector3 (horizVel, -5.5f, speed + 4.5f);
        // yield return new WaitForSeconds(.5f);
        GetComponent<Rigidbody>() .velocity = new Vector3 (horizVel, 0, speed);
        anim.SetBool("isJump", false);
    }

    IEnumerator stopSlide()
    {
        yield return new WaitForSeconds(.5f);
        GetComponent<Rigidbody>() .velocity = new Vector3 (horizVel, 0, speed);
        anim.SetBool("isSlide", false);
    }

    IEnumerator boostController()
    {
        boost = true;  
        // GetComponent<Collider>().isTrigger = false;
        yield return new WaitForSeconds(3);
        boost = false;
    }

    private void Death()
    {
            anim.SetTrigger("isDeath");
            isalreadyDead = true;
            deadScoreText.text = score.ToString("0");
            deadCoinText.text = coinsCollected.ToString("0");
            deathMenuAnim.SetTrigger("Dead");
            gameCanvas.SetTrigger("Hide");
            return;
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthbar.SetHealth(currentHealth);
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "coin")
        {
            motor.CollectAnim();
            GetComponent<AudioSource>().Play();
            Destroy(other.gameObject);
            coinsCollected += 1f;
            score += 2;
            scoreText.text = score.ToString("0");
        }

        if(other.gameObject.tag == "boost")
        {
            boost = true;
            Destroy(other.gameObject);
            StartCoroutine(boostController());
        }
        if(other.gameObject.tag == "lethal")
        {
            TakeDamage(1);
            Debug.Log("Hit");
        }
    }


    void MobControl()
    {
        if(Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);
            if(t.phase == TouchPhase.Began)
            {
                //save began touch 2d point
                firstPressPos = new Vector2(t.position.x,t.position.y);
            }
            if(t.phase == TouchPhase.Ended)
            {
                //save ended touch 2d point
                secondPressPos = new Vector2(t.position.x,t.position.y);
                            
                //create vector from the two points
                currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
                
                //normalize the 2d vector
                currentSwipe.Normalize();
    
                //swipe upwards
                if(currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    jump = true;
                } else {
                    jump = false;
                }
                anim.SetBool("isJump", jump);

                if(jump == true)
                {
                    GetComponent<Rigidbody>().velocity = new Vector3 (horizVel, 3.5f, zVel);
                    StartCoroutine(stopJump());
                }
                    
                //swipe down
                if(currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    slide = true;
                } else {
                    slide = false;
                }

                if(slide == true)
                {
                    anim.SetBool("isSlide", slide);
                    transform.Translate(0, 0, 0.1f);
                    StartCoroutine(stopSlide());
                } else if(slide == false) {
                    anim.SetBool("isSlide", slide);
                }
                
                //swipe left
                if(currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f && (laneNum > 1) && (laneNum <= 3) && !hasSwiped)
                {
                    hasSwiped = true;
                    horizVel = -4;
                    StartCoroutine(stopLaneChange());
                    laneNum -= 1;
                }
                //swipe right
                if(currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f && (laneNum >= 1) && (laneNum < 3) && !hasSwiped)
                {
                    hasSwiped = true;
                    horizVel = 4;
                    StartCoroutine(stopLaneChange());
                    laneNum += 1;
                }
            }
        }
    }
}