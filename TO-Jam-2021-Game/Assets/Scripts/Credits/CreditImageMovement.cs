using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditImageMovement : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    AnimationCurve m_curve;
    [SerializeField]
    float time;


    RectTransform m_transform;
    [SerializeField]
    Vector2 origPosition;


    Image m_Image;

    private void Awake()
    {
        m_transform = GetComponent<RectTransform>();
        origPosition = m_transform.anchoredPosition;
        m_Image = GetComponent<Image>();

        m_Image.preserveAspect = true;
        //    m_text

    }

    private void Update()
    {
        time += Time.deltaTime;

        m_transform.anchoredPosition = Vector2.Lerp( new Vector2 (origPosition.x,origPosition.y), new Vector2(origPosition.x, -origPosition.y*3), m_curve.Evaluate(time / 2));

        m_Image.color = Color.Lerp(Color.clear, Color.white, m_curve.Evaluate(time * 3));

        if (m_curve.Evaluate(time / 2) >= 1)
        {
            Destroy(this.gameObject);
        }

    }
}
