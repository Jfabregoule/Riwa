using UnityEngine;

public class ShowQAButton : MonoBehaviour
{
    void Start()
    {
#if QA
        gameObject.SetActive(true);
#else
        gameObject.SetActive(false);
#endif
    }
}
