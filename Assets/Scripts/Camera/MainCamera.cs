using Unity.VisualScripting;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
    private Vector3 _targetPosition;

    private float _offsetY;
    private float _moveSpeed;

    public void OnEnable()
    {
        EventBus.Subscribe<OnArchPassed>(OnArchPassed);
    }
    public void OnDisable()
    {
        EventBus.Unsubscribe<OnArchPassed>(OnArchPassed);
    }

    private void Start()
    {
        if (EventBus != null)
            Debug.Log("EventBus is not null");
        
        _targetPosition = transform.position;
        _offsetY = 0.5f;
        _moveSpeed = 7.0f;
    }

   private void LateUpdate()
   {
       transform.position = Vector3.Lerp(transform.position, _targetPosition, _moveSpeed * Time.deltaTime);
   }
   
   private void OnArchPassed(in OnArchPassed eventData)
   {
       Debug.Log("Assigning new camera Y position");
       _targetPosition.y = eventData.ArchPosY + _offsetY;
   }
}
