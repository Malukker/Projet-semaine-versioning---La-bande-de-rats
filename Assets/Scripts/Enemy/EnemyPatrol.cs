using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Needed Pathfinding Components - not for balancing")]
    [SerializeField] private AIPath _aiPath;
    [SerializeField] private AIDestinationSetter _destinationSetter;
    [SerializeField] private Transform[] _points;
    [SerializeField] private LayerMask _layers;

    private int _currentPatrolObjective;

    [Header("Balancing Parameters")]
    [SerializeField] private float _idlingTime;
    [SerializeField, Range (1, 180)] private int _maxSightAngle;
    [SerializeField] private float _detectionRadius;
    [SerializeField] private float _detectionTime;
    private float _detectionTimeProgress = 0;
    [SerializeField] private float _patrolSpeed, _chaseSpeed;
    [SerializeField] private float _aggressionTime;
    private float _aggressionTimeProgress = 0;
    [SerializeField] private float _idlingTimeOnHat; 

    private Vector3[] _sightConeAngles;
    private RaycastHit2D _raycastHit;
    private enum EnemyState
    {
        patrolling,
        idling,
        wary,
        chasing,
        fetching,
    };
    private EnemyState _enemyState;

    private void Awake()
    {
        HatProjectile.HitWall += HatDetected;
    }

    void Start()
    {
        _sightConeAngles = new Vector3[_maxSightAngle];
    }

    void Update()
    {
        UpdateAngles();
        UpdatePathfinding();
    }

    void UpdateAngles()
    {
        for (int i = 0; i < _sightConeAngles.Length; i++)
        {
            _sightConeAngles[i] = new Vector3
                (
                transform.position.x + _detectionRadius * Mathf.Cos(i * Mathf.Deg2Rad
                + transform.rotation.eulerAngles.z * Mathf.Deg2Rad
                + (90 - _maxSightAngle/2) * Mathf.Deg2Rad)
                ,
                transform.position.y + _detectionRadius * Mathf.Sin(i * Mathf.Deg2Rad
                + transform.rotation.eulerAngles.z * Mathf.Deg2Rad
                + (90 - _maxSightAngle / 2) * Mathf.Deg2Rad)
                ,
                0
                );
        }
    }

    void CheckForPlayerInSight()
    {
        for (int i = 0; i < _sightConeAngles.Length; i++)
        {
            _raycastHit = Physics2D.Raycast
                (
                origin: transform.position,
                direction: _sightConeAngles[i],
                distance: _detectionRadius,
                layerMask: _layers
                );
            if (_raycastHit.collider != null && _raycastHit.collider.gameObject.layer == 7)
            {
                _enemyState = EnemyState.wary;
            }
        }
    }

    void UpdatePathfinding()
    {
        switch (_enemyState)
        {
            case EnemyState.patrolling:
                Patrol();
                CheckForPlayerInSight();
                _detectionTimeProgress = 0;
                break;
            case EnemyState.wary:
                DetectingMode();
                break;
            case EnemyState.chasing:
                Chase();
                break;
            case EnemyState.fetching:
                FetchingHat();
                break;
            default:
                break;

        }
    }

    void Patrol()
    {
        _destinationSetter.target = _points[_currentPatrolObjective];
        _aiPath.maxSpeed = _patrolSpeed;

        if (Vector2.Distance(_points[_currentPatrolObjective].position, transform.position) < .25f)
        {
            ChangePatrolObjective();
            StartCoroutine(IdleOnPatrolEnd());
            _enemyState = EnemyState.idling;
        }
    }

    void ChangePatrolObjective()
    {
        if (_currentPatrolObjective < _points.Length - 1) { _currentPatrolObjective++; }
        else { _currentPatrolObjective = 0; }
    }

    IEnumerator IdleOnPatrolEnd()
    {
        yield return new WaitForSeconds(_idlingTime);
        _enemyState = EnemyState.patrolling;
    }

    void DetectingMode()
    {
        _aiPath.enabled = false;

        _detectionTimeProgress += Time.deltaTime;
        if (_detectionTimeProgress >= _detectionTime)
        {
            for (int i = 0; i < _sightConeAngles.Length; i++)
            {
                _raycastHit = Physics2D.Raycast
                (
                origin: transform.position,
                direction: _sightConeAngles[i],
                distance: _detectionRadius,
                layerMask: _layers
                );

                if (_raycastHit.collider != null && _raycastHit.collider.gameObject.layer == 7)
                {
                    _aiPath.enabled = true;
                    _enemyState = EnemyState.chasing;
                    return;
                }
            }
            _aiPath.enabled = true;
            _enemyState = EnemyState.patrolling;
        }
    }

    void Chase()
    {
        _destinationSetter.target = GameObject.FindGameObjectWithTag("Player").transform;
        _aiPath.maxSpeed = _chaseSpeed;
        if (Vector2.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) > _detectionRadius)
        {
            _aggressionTimeProgress += Time.deltaTime;
        }
        else
        {
            _aggressionTimeProgress = 0;
        }
        if(_aggressionTimeProgress >= _aggressionTime)
        {
            _enemyState = EnemyState.wary;
        }
    }

    void HatDetected(HatProjectile hat)
    {
        if(Vector2.Distance(hat.gameObject.transform.position, transform.position) > _detectionRadius)
        {
            _destinationSetter.target = hat.gameObject.transform;
            _enemyState = EnemyState.fetching;
        }
    }

    void FetchingHat()
    {
        if (Vector2.Distance(transform.position, _destinationSetter.target.gameObject.transform.position) < 0.5f)
        {
            StartCoroutine(IdleOnHat());
            _enemyState = EnemyState.idling;
        }
    }

    IEnumerator IdleOnHat()
    {
        yield return new WaitForSeconds(_idlingTimeOnHat);
        _enemyState = EnemyState.patrolling;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (_sightConeAngles != null)
        {
            foreach (Vector3 vector in _sightConeAngles)
            {
                Gizmos.DrawLine(transform.position, vector);
            }
        }

        foreach(Transform point in _points)
        {
            Gizmos.DrawSphere(point.position, .5f);
        }
    }
}
