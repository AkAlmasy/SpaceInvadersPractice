using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour {

    WaveConfig waveConfigAsset;
    List<Transform> wayPoints;
    int wayPointIndex = 0;

	// Use this for initialization
	void Start () {
        wayPoints = waveConfigAsset.GetWayPoints();

        transform.position = wayPoints[wayPointIndex].transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Move();
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfigAsset = waveConfig;
    }


    private void Move()
    {
        if (wayPointIndex <= wayPoints.Count - 1)
        {
            var targetPosition = wayPoints[wayPointIndex].transform.position;
            var movementThisFrame = waveConfigAsset.GetMoveSpeed() * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);
            if (transform.position == targetPosition)
            {
                wayPointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
