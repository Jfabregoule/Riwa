using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class InteractTest : MonoBehaviour, IMovable, IRotatable
{
    [SerializeField] private float _speed;
    [SerializeField] private float _angle;

    public event IRotatable.RotatableEvent OnRotataFinish;

    public float MoveSpeed { get => _speed; set => value = _speed; }
    public float OffsetRadius { get; set; }

    InteractTest()
    {
        OffsetRadius = 1;
    }

    public void Interactable()
    {
        Debug.Log("GameObject name: " + name);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hold()
    {
        throw new System.NotImplementedException();
    }

    public void Move(Vector3 direction)
    {
        transform.position += direction * MoveSpeed * Time.deltaTime * 10;
    }

    public void Rotate(int sens)
    {
        StartCoroutine(CoroutineRotate(sens));
    }

    private IEnumerator CoroutineRotate(int sens)
    {
        float clock = 0;

        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(0f, _angle * sens, 0f);

        while (clock < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, Mathf.Clamp01(clock * 3));

            clock += Time.deltaTime;

            yield return null;
        }

        OnRotataFinish?.Invoke();
    }
}
