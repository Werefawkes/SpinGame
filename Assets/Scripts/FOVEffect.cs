using UnityEngine;
using CustomInspector;
using Mirror;
using ReadOnlyAttribute = CustomInspector.ReadOnlyAttribute;

public class FOVEffect : MonoBehaviour
{
	public float lerpSpeed = 0.5f;
	public float targetViewDistance;
	float currentViewDistance;

	[Range(0, 360)]
	public float targetFOV = 360;
	float currentFOV;

	[LineGraph]
	public LineGraph cameraLerpPerViewDistance;

	public int rayCount = 8;
	[ReadOnly]
	public Vector2 origin;
	[ReadOnly]
	public float lookAngle = 0;

	public LayerMask layerMask;

	Mesh mesh;


	void Start()
	{
		mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
	}

	void LateUpdate()
	{
		if (GameManager.localPlayer)
		{
			PlayerController lp = GameManager.localPlayer;
			origin = lp.transform.position;
			lookAngle = Vector2.SignedAngle(Vector2.right, lp.AimDirection);

			if (lp.isSecondaryActionHeld)
			{
				targetFOV = lp.shooter.weapon.aimFOV;
				targetViewDistance = lp.stats.sightDistance * lp.shooter.weapon.aimDistanceMultiplier;
			}
			else
			{
				targetFOV = lp.stats.fieldOfView;
				targetViewDistance = lp.stats.sightDistance;
			}

			lp.cameraLerpStep = cameraLerpPerViewDistance.GetYValue(currentViewDistance);
		}

		currentViewDistance = Mathf.Lerp(currentViewDistance, targetViewDistance, lerpSpeed * Time.deltaTime);
		currentFOV = Mathf.Lerp(currentFOV, targetFOV, lerpSpeed * Time.deltaTime);

		Vector3[] vertices = new Vector3[rayCount + 2];
		Vector2[] uv = new Vector2[vertices.Length];
		int[] triangles = new int[rayCount * 3];

		float angle = lookAngle + (currentFOV / 2);
		float angleStep = currentFOV / rayCount;

		vertices[0] = origin;

		int vertexIndex = 1;
		int triangleIndex = 0;

		for (int i = 0; i <= rayCount; i++)
		{
			Vector2 vector = new(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
			RaycastHit2D hit = Physics2D.Raycast(origin, vector, currentViewDistance, layerMask);

			Vector3 vertex;
			if (hit.collider == null)
			{
				vertex = origin + vector * currentViewDistance;
			}
			else
			{
				vertex = hit.point;
			}

			vertices[vertexIndex] = vertex;

			if (i > 0)
			{
				triangles[triangleIndex] = 0;
				triangles[triangleIndex + 1] = vertexIndex - 1;
				triangles[triangleIndex + 2] = vertexIndex;

				triangleIndex += 3;
			}

			vertexIndex++;

			angle -= angleStep;
		}

		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;

		mesh.RecalculateBounds();
	}
}
