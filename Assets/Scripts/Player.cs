using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private AudioClip _moveClip, _loseClip,TapSOund;

    [SerializeField] private GameplayManager _gm;
    [SerializeField] private GameObject _explosionPrefab;

    private float _currentRotationSpeed; // To store the active rotation speed

    private void Start()
    {
        _currentRotationSpeed = 0f; // Start with no rotation
    }

    // Public methods to be called by buttons
    public void RotateLeft()
    {
        SoundManager.Instance.PlaySound(TapSOund);
        _currentRotationSpeed = Mathf.Abs(_rotateSpeed); // Positive for left rotation
    }

    public void RotateRight()
    {
        SoundManager.Instance.PlaySound(TapSOund);
        _currentRotationSpeed = -Mathf.Abs(_rotateSpeed); // Negative for right rotation
    }

    private void FixedUpdate()
    {
        transform.Rotate(0, 0, _currentRotationSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            Instantiate(_explosionPrefab, transform.GetChild(0).position, Quaternion.identity);
            SoundManager.Instance.PlaySound(_loseClip);
            _gm.GameEnded();
            Destroy(gameObject);
        }
    }
}