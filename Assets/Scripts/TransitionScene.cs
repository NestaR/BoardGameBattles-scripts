using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionScene : MonoBehaviour
{
    public GameObject transitionObjectIn, transitionObjectOut;
    RectTransform transitionTransformIn, transitionTransformOut;
    public float speed;
    public bool animationFinished;
    Vector2 startPosIn, startPosOut;
    // Start is called before the first frame update
    void Start()
    {//Controls an animation that plays entering and leaving a scene
        transitionTransformIn = transitionObjectIn.GetComponent<RectTransform>();
        transitionTransformOut = transitionObjectOut.GetComponent<RectTransform>();
        startPosIn = transitionTransformIn.anchoredPosition;
        startPosOut = transitionTransformOut.anchoredPosition;
    }
    void Awake()
    {
        animationFinished = false;
    }
    // Update is called once per frame
    void Update()
    {
        speed = 3333f * Time.deltaTime;
        if(transitionObjectIn.activeSelf == true && animationFinished == false)
        {//Activate if transitioning away from a scene
            if (transitionTransformIn.anchoredPosition.x >= -5971)
            {
                transitionTransformIn.anchoredPosition = new Vector2(transitionTransformIn.anchoredPosition.x - speed, transitionTransformIn.anchoredPosition.y);
            }
            else
            {
                animationFinished = true;
                //animateOutScene();      
            }
        }
        else if(transitionObjectOut.activeSelf == true && animationFinished == false)
        {//Activate if transitioning into a scene
            if (transitionTransformOut.anchoredPosition.x <= 1921)
            {
                transitionTransformOut.anchoredPosition = new Vector2(transitionTransformOut.anchoredPosition.x + speed, transitionTransformOut.anchoredPosition.y);             
            }
            else
            {
                animationFinished = true;
            }
        }
    }
    public void animateInScene()
    {
        animationFinished = false;
        transitionObjectIn.SetActive(true);
        transitionObjectOut.SetActive(false);
    }
    public void animateOutScene()
    {
        transitionObjectOut.SetActive(true);
        transitionObjectIn.SetActive(false);
    }
    public void resetPositions()
    {
        transitionTransformIn.anchoredPosition = startPosIn;
        transitionTransformOut.anchoredPosition = startPosOut;
        transitionObjectOut.SetActive(false);
        transitionObjectIn.SetActive(false);
        animationFinished = false;
    }
}
