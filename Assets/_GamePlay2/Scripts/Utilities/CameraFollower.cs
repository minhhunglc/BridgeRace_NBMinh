using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform _target;
    [SerializeField] private Vector3 _offset;


    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _target.position + _offset, Time.deltaTime * 4);
    }
}
