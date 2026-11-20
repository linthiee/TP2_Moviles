using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private const float _rotationSpeed = 70.0f;

    private void Update()
    {
        transform.Rotate(Vector3.up * (_rotationSpeed * Time.deltaTime), Space.Self);
    }
}