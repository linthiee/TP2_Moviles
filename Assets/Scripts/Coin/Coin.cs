using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
    
    private const float _rotationSpeed = 70.0f;

    private void Update()
    {
        transform.Rotate(Vector3.up * (_rotationSpeed * Time.deltaTime), Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventBus.Raise<OnCoinCollected>();
            gameObject.SetActive(false);
        }
        
    }
}