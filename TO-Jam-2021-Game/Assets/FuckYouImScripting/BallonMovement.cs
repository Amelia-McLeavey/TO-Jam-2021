using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallonMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public Transform holder;
    public Rigidbody2D holderRb;
    public float ropeLength;
    public float maxMoveSpeed;
    public float xOffset = 0.1f;
    private LineRenderer lineRenderer;
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();
    private float ropeSegLen = 0.15f;
    private int segmentLength;
    private float lineWidth = 0.05f;
    private float startScale;

    private bool BalloonHasBurst=false;

    private Animator anim;

    public bool burstNow=false;

    public bool floatingAway = false;

    void Start()
    {
        anim = this.GetComponent<Animator>();

        anim.Play(0, -1, Random.Range(0f, 1f));

       segmentLength = Mathf.FloorToInt(ropeLength / ropeSegLen);
        startScale = this.transform.localScale.x;
        rb = this.GetComponent<Rigidbody2D>();
        //holderRb = holder.GetComponent<Rigidbody2D>();
        this.lineRenderer = this.GetComponent<LineRenderer>();
        Vector3 ropeStartPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        for (int i = 0; i < segmentLength; i++)
        {
            this.ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= ropeSegLen;
        }
    }

    void Update()
    {
        this.DrawRope();

        if (!floatingAway)
        {
            if (BalloonHasBurst)
            {
                this.transform.position = holder.position;
            }
            else
            {
                if (burstNow)
                {
                    burstNow = false;
                    burstBalloon();
                }

                if (Vector2.Distance(this.transform.position, holder.position) > ropeLength)
                {
                    //find pos on radius circle
                    //outOfRange();
                    dragBalloons(maxMoveSpeed * 3f);

                    if (Vector2.Distance(this.transform.position, holder.position) > 3 * ropeLength)
                    {
                        this.transform.position = holder.position;
                    }
                }
                else
                {
                    if (Mathf.Abs(holderRb.velocity.x) >= 0.5f)
                    {
                        dragBalloons(maxMoveSpeed);
                    }
                    else if (!BalloonHasBurst)
                    {
                        floatingBalloons();
                    }
                }
            }
 
        }
        else
        {
            rb.gravityScale = -0.6f;
        }


    }

    public void dragBalloons(float speed)
    {
        float dis = Vector2.Distance(holder.position, this.transform.position);

        if (dis >= ropeLength)
        {
            dis = 1f;
        }

        float yPos= Mathf.Lerp(transform.position.y, holder.position.y, 0.1f);


        Vector3 targetPos = new Vector3(holder.position.x, yPos, this.transform.position.z);

        rb.velocity = (targetPos - this.transform.position).normalized * speed * Mathf.Abs(dis);

    }

    public void outOfRange()
    {
        rb.velocity = Vector2.zero;
        Vector3 targetPos = holder.position + ((holder.position - new Vector3(holderRb.velocity.x, holderRb.velocity.y, holder.position.z)).normalized*ropeLength*0.9f);


    }

    public void floatingBalloons()
    {
        Vector3 targetPos = new Vector3(holder.position.x + xOffset, holder.position.y + (ropeLength*0.6f), this.transform.position.z);
        if (Vector2.Distance(targetPos, this.transform.position) > 0.8f)
        {
            rb.velocity = (targetPos - this.transform.position).normalized * maxMoveSpeed/2;
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, 0.1f);            
        }
    }

    public void burstBalloon()
    {
        //play animation of the burst
        BalloonHasBurst = true;
        anim.SetTrigger("Burst");
        rb.gravityScale = 1f;
        this.gameObject.layer = 26;
        StartCoroutine(refillBalloon());
    }

    private IEnumerator refillBalloon()
    {
        yield return new WaitForSeconds(5f);
        BalloonHasBurst = false;
        rb.gravityScale = -0.01f;
        anim.SetTrigger("Refill");
       
        this.gameObject.layer = 24;

    }
    private void FixedUpdate()
    {
        this.Simulate();
    }

    private void Simulate()
    {
        Vector2 forceGravity = new Vector2(0f, -1.5f);

        for (int i = 1; i < this.segmentLength; i++)
        {
            RopeSegment firstSegment = this.ropeSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity;
            firstSegment.posNow += forceGravity * Time.fixedDeltaTime;
            this.ropeSegments[i] = firstSegment;
        }

        for (int i = 0; i < 50; i++)
        {
            this.ApplyConstraint();
        }
    }

    private void ApplyConstraint()
    {

        RopeSegment firstSegment = this.ropeSegments[0];
        firstSegment.posNow = this.transform.position;
        this.ropeSegments[0] = firstSegment;
        if (!floatingAway)
        {
            RopeSegment lastSegment = this.ropeSegments[this.segmentLength - 1];
            lastSegment.posNow = holder.position;

            this.ropeSegments[this.segmentLength - 1] = lastSegment;
        }


        for (int i = 0; i < this.segmentLength - 1; i++)
        {
            RopeSegment firstSeg = this.ropeSegments[i];
            RopeSegment secondSeg = this.ropeSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = Mathf.Abs(dist - this.ropeSegLen);
            Vector2 changeDir = Vector2.zero;

            if (dist > ropeSegLen)
            {
                changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
            }
            else if (dist < ropeSegLen)
            {
                changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
            }

            Vector2 changeAmount = changeDir * error;
            if (i != 0)
            {
                firstSeg.posNow -= changeAmount * 0.5f;
                this.ropeSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * 0.5f;
                this.ropeSegments[i + 1] = secondSeg;
            }
            else
            {
                secondSeg.posNow += changeAmount;
                this.ropeSegments[i + 1] = secondSeg;
            }
        }


    }

    private void DrawRope()
    {
        float lineWidth = this.lineWidth;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        Vector3[] ropePositions = new Vector3[this.segmentLength];
        for (int i = 0; i < this.segmentLength; i++)
        {
            ropePositions[i] = this.ropeSegments[i].posNow;
        }

        lineRenderer.positionCount = ropePositions.Length;
        lineRenderer.SetPositions(ropePositions);
    }

    public struct RopeSegment
    {
        public Vector2 posNow;
        public Vector2 posOld;

        public RopeSegment(Vector2 pos)
        {
            this.posNow = pos;
            this.posOld = pos;
        }
    }

}
