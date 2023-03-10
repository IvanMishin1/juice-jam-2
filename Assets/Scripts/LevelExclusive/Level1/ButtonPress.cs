using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    public float debugnumber;
    bool pressed = false;
    bool goingdown = true;
    bool goingup = false;
    public float secondstowait;
    public GameObject BrokenBottleCollector;
    public Animator BrokeBottleCollectorAnimator;
    public static bool isthereabrokenbottle;
    public AudioSource falsegrab;
    public AudioSource falsefill;
    public static bool isthereabottle;
    Animator FillerAnimator;
    public GameObject Spill;
    public GameObject BottleFill;
    bool doit = true;

    void Start()
    {
        FillerAnimator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if(PauseManager.playing){
        if(Input.GetMouseButtonDown(0))
        {
            pressed = true;
        }
        if(pressed && goingdown)
        {
            transform.Translate(0, -1 * debugnumber, 0);
            if(transform.position.y <= 1.88f)
            {
                goingdown = false;
                StartCoroutine(WaitBeforeGoingUp());
            }
        }
        if(pressed && goingup)
        {
            if(isthereabottle)
            {
                StartCoroutine(doAnimation());
                transform.Translate(0, 1 * debugnumber, 0);
                if(transform.position.y >= 3.7f)
                {   
                    goingup = false;
                    goingdown = true;
                    pressed = false;
                    isthereabottle = false;
                }
            }
            else
            {
                if(doit)
                {
                    Instantiate(Spill, new Vector3(1.35f, -3.416f, 0), Quaternion.identity);
                    doit = false;
                }
                falsefill.Play();
                transform.Translate(0, 1 * debugnumber, 0);
                if(transform.position.y >= 3.7f)
                {   
                    goingup = false;
                    goingdown = true;
                    pressed = false;
                    isthereabottle = false;
                    doit = true;
                }
            }
        }
        if(Input.GetMouseButtonDown(1))
        {
            StartCoroutine(CollectBrokenGlass());
        }
        }
    }

    IEnumerator WaitBeforeGoingUp()
    {
        BottleFill.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        BottleFill.SetActive(false);
        yield return new WaitForSeconds(secondstowait);
        goingup = true;
    }

    IEnumerator CollectBrokenGlass()
    {
        BrokenBottleCollector.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        BrokenBottleCollector.SetActive(false);
        if(isthereabrokenbottle)
        {
            BrokeBottleCollectorAnimator.SetBool("isgrabbing", true);
            yield return new WaitForSeconds(0.5f);
            BrokeBottleCollectorAnimator.SetBool("isgrabbing", false);
        }
        else
        {
            falsegrab.Play();
            BrokeBottleCollectorAnimator.SetBool("falsegrab", true);
            yield return new WaitForSeconds(0.3f);
            BrokeBottleCollectorAnimator.SetBool("falsegrab", false);
        }
        isthereabrokenbottle = false;
    }

    IEnumerator doAnimation()
    {
        FillerAnimator.SetBool("filled", true);
        yield return new WaitForSeconds(0.4f);
        FillerAnimator.SetBool("filled", false);
    }
}
