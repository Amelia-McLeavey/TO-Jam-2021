using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    public GameObject _movePoint1;
    public GameObject _movePoint2;
    public GameObject _targetMovePoint;
    public Vector2 _delta;

    // tracks whether or not this platform is moving to its targetposition or its initial position
    public bool _targetIsMovePoint1 = true;
    public float _moveSpeed = 1.0f;
    public float _moveSpeedMultplier = 1.0f;

    // sets the platforms position to be its initial position
    void Start()
    {
        transform.position = _movePoint1.transform.position;
    }

    // update the moving platform to translate between its initial poisition and its target position
    void Update()
    {

        if(_targetIsMovePoint1)
        {
            _targetMovePoint = _movePoint2;
            updateMovement(_movePoint2.transform.position);

        } else
        {
            _targetMovePoint = _movePoint1;
            updateMovement(_movePoint1.transform.position);
        }


        if (transform.position == _targetMovePoint.transform.position)
        {
            _targetIsMovePoint1 = !_targetIsMovePoint1;
        }

    }

    void updateMovement(Vector2 targetMove)
    {
        _delta = new Vector2(targetMove.x - transform.position.x, targetMove.y - transform.position.y);
        _delta = Vector2.ClampMagnitude(_delta, (_moveSpeed * _moveSpeedMultplier) * Time.deltaTime);
       
        Vector2 proposedMove = transform.position;
        proposedMove = proposedMove + _delta;

        // ensure that we do not overshoot our target position
        if (Vector3.Distance(transform.position, targetMove) < Vector3.Distance(transform.position, proposedMove))
        {
            transform.position = targetMove;
            _targetIsMovePoint1 = !_targetIsMovePoint1;
            return;
        } else
        {
            transform.position = proposedMove;
        }
    }
}
