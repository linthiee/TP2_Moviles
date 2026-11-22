using System.Collections;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
    
    private Vector3 _currentPosition;
    private Vector3 _targetPosition;
    private Vector3 _initialPosition;
    
    private float _moveSpeed;
    private float _lerpProgress;
    private Coroutine _moveCoroutine;

    private float _archSpacing;
    
    private float _cameraOffset;

    public void OnEnable()
    {
        EventBus.Subscribe<OnArchPassed>(OnArchPassed);
        EventBus.Subscribe<OnPlayerRespawn>(OnPlayerRespawn);
    }

    public void OnDisable()
    {
        EventBus.Unsubscribe<OnArchPassed>(OnArchPassed);
        EventBus.Unsubscribe<OnPlayerRespawn>(OnPlayerRespawn);
    }

    private void Start()
    {
        if (EventBus != null)
            Debug.Log("EventBus is not null");

        _cameraOffset = transform.position.y;
        _targetPosition = transform.position;
        
        _initialPosition =  transform.position;
        
        _archSpacing = 0.55f;
        _moveSpeed = 5.0f;
        _lerpProgress = 0.0f;
    }

    private IEnumerator MoveCameraCoroutine()
    {
        while (_lerpProgress < 1.0f)
        {
            _lerpProgress += Time.deltaTime * _moveSpeed;
            transform.position = Vector3.Lerp(_currentPosition, _targetPosition, _lerpProgress);
            yield return null;
        }

        transform.position = _targetPosition;
        _moveCoroutine = null;
    }

    private void OnArchPassed(in OnArchPassed eventData)
    {
        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);

        _lerpProgress = 0.0f;
        _currentPosition = transform.position;
        
        _targetPosition.y = eventData.ArchPosY - _archSpacing + _cameraOffset;

        _moveCoroutine = StartCoroutine(MoveCameraCoroutine());
    }
    
    private void OnPlayerRespawn(in OnPlayerRespawn eventData)
    {
        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
            _moveCoroutine = null;
        }

        _lerpProgress = 0.0f;
        
        transform.position = _initialPosition;
        _targetPosition = _initialPosition;
    }
}