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

    private IEnumerator Start()
    {
        //yield return StartCoroutine(FromLeftToRight());
        //yield return StartCoroutine(FromRightToLeft());
        yield return null;
    }

    public IEnumerator FromLeftToRight()
    {
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

    public IEnumerator FromRightToLeft()
    {
        middlePageAnchor.eulerAngles = new Vector3(middlePageAnchor.eulerAngles.x, middlePageAnchor.eulerAngles.y, minMiddlePageAnchorAngle + epsilonMiddlePageAnchorAngle);
        middlePageAnchor.gameObject.SetActive(true);

        var t = 0f;
        while(t < animationTime)
        {
            middlePageAnchor.eulerAngles = new Vector3(middlePageAnchor.eulerAngles.x, middlePageAnchor.eulerAngles.y, minMiddlePageAnchorAngle + epsilonMiddlePageAnchorAngle + animationCurve.Evaluate(t / animationTime) * (maxMiddlePageAnchorAngle - minMiddlePageAnchorAngle - 2 * epsilonMiddlePageAnchorAngle));
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        middlePageAnchor.gameObject.SetActive(false);
    }
}
