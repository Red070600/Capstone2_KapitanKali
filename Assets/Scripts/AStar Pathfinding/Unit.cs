using UnityEngine;
using System.Collections;

namespace JRLB
{
    public class Unit : MonoBehaviour
    {
        const float minPathUpdateTime = .2f;
        const float pathUpdateMoveThreshold = .5f;

        public float speed = 20;
        public float turnSpeed = 3;
        public float turnDst = 5;
        public float stoppingDst = 10;

        Path path;
        Vector3 targetPosition;
        GameObject fullMapUI;

        void Start()
        {
            fullMapUI = GameObject.Find("FullMapUI");
            StartCoroutine(UpdatePath());
        }

        public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
        {
            if (pathSuccessful)
            {
                path = new Path(waypoints, transform.position, turnDst, stoppingDst);

                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");
            }
        }

        IEnumerator UpdatePath()
        {
            if (Time.timeSinceLevelLoad < .3f)
            {
                yield return new WaitForSeconds(.3f);
            }
            Vector3 targetPosOld = targetPosition;

            while (true)
            {
                yield return new WaitForSeconds(minPathUpdateTime);

                if (fullMapUI.activeSelf) // Check if the full map UI is active
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit))
                        {
                            targetPosition = hit.point;
                            PathRequestManager.RequestPath(new PathRequest(transform.position, targetPosition, OnPathFound));
                        }
                    }
                    else if ((targetPosition - targetPosOld).sqrMagnitude > pathUpdateMoveThreshold * pathUpdateMoveThreshold)
                    {
                        PathRequestManager.RequestPath(new PathRequest(transform.position, targetPosition, OnPathFound));
                        targetPosOld = targetPosition;
                    }
                }
            }
        }

        IEnumerator FollowPath()
        {
            bool followingPath = true;
            int pathIndex = 0;
            transform.LookAt(path.lookPoints[0]);

            float speedPercent = 1;

            while (followingPath)
            {
                Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
                while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
                {
                    if (pathIndex == path.finishLineIndex)
                    {
                        followingPath = false;
                        break;
                    }
                    else
                    {
                        pathIndex++;
                    }
                }

                if (followingPath)
                {
                    if (pathIndex >= path.slowDownIndex && stoppingDst > 0)
                    {
                        speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(pos2D) / stoppingDst);
                        if (speedPercent < 0.01f)
                        {
                            followingPath = false;
                        }
                    }

                    Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                    transform.Translate(Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);
                }

                yield return null;
            }
        }

        public void OnDrawGizmos()
        {
            if (path != null)
            {
                path.DrawWithGizmos();
            }
        }

    }
}