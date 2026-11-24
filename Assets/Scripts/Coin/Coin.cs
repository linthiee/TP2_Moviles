using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
    private GameSessionService Session => ServiceProvider.Instance.GetService<GameSessionService>();
    private bool IsTutorial => Session != null && Session.SelectedMode == GameMode.Tutorial;
    
    private const float _rotationSpeed = 70.0f;

    private void Update()
    {
        transform.Rotate(Vector3.up * (_rotationSpeed * Time.deltaTime), Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!IsTutorial)
                EventBus.Raise<OnCoinCollected>();

            gameObject.SetActive(false);
        }
        
    }
}