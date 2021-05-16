using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CreditMovement : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    AnimationCurve m_curve;
    [SerializeField]
    float time;


    RectTransform m_transform;
    [SerializeField]
    Vector2 origPosition;


    Text m_text;

    private void Awake()
    {
        m_transform = GetComponent<RectTransform>();
        origPosition = m_transform.anchoredPosition;
        m_text = GetComponent<Text>();

    //    m_text

    }

    private void Update()
    {
        time += Time.deltaTime;

        m_transform.anchoredPosition = Vector2.Lerp(origPosition, -origPosition *3, m_curve.Evaluate(time/2));

        m_text.fontSize =Mathf.RoundToInt( Mathf.Lerp(0, 39, m_curve.Evaluate(time * 5)));
        m_text.color = Color.Lerp(Color.clear, Color.white, m_curve.Evaluate(time * 5));

        if (m_curve.Evaluate(time / 2) >= 1)
        {
            Destroy(this.gameObject);
        }

    }


}
