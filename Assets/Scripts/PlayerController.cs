using System;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public event Action onRealDig;
    public event Action onFakeDig;
    public event Action<DigSpot> onDigUp;
    public event Action<int> onNutChange;

    [SerializeField]
    public Rigidbody attachedRigidbody = default;
    [SerializeField]
    public bool isLocal = false;
    [SerializeField]
    public PlayerData data = new PlayerData();

    [Header("Remote Settings")]
    [SerializeField, Range(0, 1)]
    float interpolationDuration = 0;
    Vector3 positionVelocity;
    float rotationVelocity;

    public Vector3 position {
        get => attachedRigidbody.position;
        set => attachedRigidbody.position = value;
    }
    public int nutCount {
        get => data.nuts;
        set {
            if (data.nuts != value) {
                data.nuts = value;
                onNutChange?.Invoke(value);
            }
        }
    }
    void Awake() {
        OnValidate();
    }
    void OnValidate() {
        if (!attachedRigidbody) {
            attachedRigidbody = GetComponentInParent<Rigidbody>();
        }
    }

    void FixedUpdate() {
        if (isLocal) {
            UpdateStorage();
        } else {
            UpdateState();
        }
    }

    void UpdateState() {
        // position
        var currentPosition = transform.position;
        var targetPosition = data.position;
        currentPosition = Vector3.SmoothDamp(currentPosition, targetPosition, ref positionVelocity, interpolationDuration);
        transform.position = currentPosition;

        // rotation
        float currentAngle = transform.rotation.eulerAngles.y;
        float targetAngle = data.angle;
        currentAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref rotationVelocity, interpolationDuration);
        transform.rotation = Quaternion.Euler(0, currentAngle, 0);
    }
    void UpdateStorage() {
        data.position = attachedRigidbody.position;
        data.angle = attachedRigidbody.rotation.eulerAngles.y;
        data.speed = new Vector2(attachedRigidbody.velocity.x, attachedRigidbody.velocity.z).magnitude;
    }

    public void RealDig() {
        nutCount--;
        onRealDig?.Invoke();
    }
    public void FakeDig() {
        onFakeDig?.Invoke();
    }
    public void DigUp(DigSpot spot) {
        if (spot.data.isReal) {
            nutCount++;
        }
        onDigUp?.Invoke(spot);
    }
}
