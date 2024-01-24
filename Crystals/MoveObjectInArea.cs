using UnityEngine;

public class MoveObjectInArea : MonoBehaviour
{
    public Rigidbody target;
    public Vector3 areaSize = new Vector3(10, 10, 10);
    public float moveSpeed = 1.0f;

    private Vector3 targetPosition;
    private GameObject cubepoint = null;

    private void Start()
    {
        targetPosition = GenerateRandomPoint();
    }

    private void FixedUpdate()
    {
        MoveTowardsTarget();
        CheckIfReachedTarget();
    }

    private void MoveTowardsTarget()
    {
        target.velocity = (targetPosition - target.position).normalized * moveSpeed;
    }

    private void CheckIfReachedTarget()
    {
        if (Vector3.Distance(target.position, targetPosition) < 0.1f)
        {
            targetPosition = GenerateRandomPoint();
        }
    }

    private Vector3 GenerateRandomPoint()
    {
        Vector3 randomPoint = new Vector3(
            Random.Range(-areaSize.x / 2, areaSize.x / 2),
            Random.Range(-areaSize.y / 2, areaSize.y / 2),
            Random.Range(-areaSize.z / 2, areaSize.z / 2)
        );

        //Test para ver los puntos
        if (cubepoint != null)
            Destroy(cubepoint);

        cubepoint = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), randomPoint, Quaternion.identity);
        cubepoint.GetComponent<BoxCollider>().enabled = false;      


        return randomPoint;
    }
}