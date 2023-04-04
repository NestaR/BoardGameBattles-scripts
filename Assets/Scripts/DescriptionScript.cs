using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DescriptionScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public string nameShow, tipToShow;
    private float timeToWait = 0.5f;

    public void OnPointerEnter(PointerEventData pointerEventData)
    {//Display a tooltip when the user hovers over the button
        StopAllCoroutines();
        StartCoroutine(StartTimer());
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {//Removes the tooltip when the mouse leaves the button
        StopAllCoroutines();
        Tooltip.OnMouseLoseFocus();
    }
    private void ShowMessage()
    {
        Tooltip.OnMouseHover(nameShow, tipToShow);
    }
    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(timeToWait);
        ShowMessage();
    }
}
