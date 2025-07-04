using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(LineRenderer))]
public class TargetingCard : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private bool isTargeting, mouseReleasedHandled;
    Card currentCard;
    public Vector3 origin;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        mouseReleasedHandled = false;

    }

    void Update()
    {
        if (isTargeting)
        {

            if (!currentCard.noTarget)
            {
                Vector3 cardPosition = transform.position;
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPos.z = 0; // Flatten for 2D
                lineRenderer.SetPosition(0, cardPosition);
                lineRenderer.SetPosition(1, mouseWorldPos);

                //Do a raycast
                RaycastHit2D hit = Physics2D.Raycast(cardPosition, (mouseWorldPos -
                    cardPosition).normalized, Vector2.Distance(cardPosition, mouseWorldPos));
                if (Input.GetMouseButtonUp(0) && hit) // Confirm target
                {
                    //IsCard Targetable?
                    Collider2D target;
                    target = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    if (target.name == "Enemy(Clone)")
                    {
                        // Apply card effect to target
                        hit.collider.GetComponent<InGameActor>().ReturnCard();
                        target.GetComponent<InGameActor>().ReturnTarget();
                        target = null;

                    }

                    StopTargeting();


                }

            }
            else
            {//Move card with mouse
                Collider2D target1;

                target1 = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPos.z = 0; // or whatever your card's normal z-position is

                transform.position = mouseWorldPos;

                if (Input.GetMouseButtonUp(0) && !mouseReleasedHandled)
                {
                    mouseReleasedHandled = true;

                    Collider2D target2;
                    target2 = GetColliderAtMouseOnLayer("Targetable");
                    if (target2.name == "HandTarget")
                    {
                        transform.position = origin;
                    }
                    else
                    {

                        //Somewhere around here 
                        //Is the bug with the cards sticking to the mouse

                        target1.GetComponent<InGameActor>().ReturnCard();
                        GameController.Instance.Action();
                    }

                    StopTargeting();


                }
            }


        }
    }
    private Collider2D GetColliderAtMouseOnLayer(string layerName)
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        LayerMask targetLayer = LayerMask.GetMask(layerName);
        Collider2D[] hits = Physics2D.OverlapPointAll(mouseWorldPos);

        foreach (Collider2D hit in hits)
        {
            if (((1 << hit.gameObject.layer) & targetLayer) != 0)
            {
                return hit;
            }
        }

        return null;
    }

    public void StartTargeting(Card card)
    {
        origin = transform.position;
        origin.z = 0;
        currentCard = card;
        isTargeting = true;
        lineRenderer.enabled = true;

    }

    public void StopTargeting()
    {
        mouseReleasedHandled = false;
        isTargeting = false;
        lineRenderer.enabled = false;
    }
}

