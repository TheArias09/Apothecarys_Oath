using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrimoirePageAnimation : MonoBehaviour
{
    [SerializeField] Transform middlePageAnchor;
    [SerializeField] float animationTime = 1f;
    [SerializeField] AnimationCurve animationCurve;
    [SerializeField] float epsilonMiddlePageAnchorAngle = 0.01f;
    [SerializeField] float maxMiddlePageAnchorAngle = 180f;
    [SerializeField] float minMiddlePageAnchorAngle = 0f;

    private void Awake()
    {
        StartCoroutine(FromLeftToRight());
    }

    public IEnumerator FromLeftToRight()
    {
        yield return new WaitForSeconds(2f);

        middlePageAnchor.eulerAngles = new Vector3(middlePageAnchor.eulerAngles.x, middlePageAnchor.eulerAngles.y, maxMiddlePageAnchorAngle - epsilonMiddlePageAnchorAngle);
        middlePageAnchor.gameObject.SetActive(true);

        var t = 0f;
        while(t < animationTime)
        {
            middlePageAnchor.eulerAngles = new Vector3(middlePageAnchor.eulerAngles.x, middlePageAnchor.eulerAngles.y, maxMiddlePageAnchorAngle - epsilonMiddlePageAnchorAngle - animationCurve.Evaluate(t / animationTime) * maxMiddlePageAnchorAngle - minMiddlePageAnchorAngle - 2 * epsilonMiddlePageAnchorAngle);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        middlePageAnchor.gameObject.SetActive(false);
    }
}
