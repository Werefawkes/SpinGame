using UnityEngine;
using CustomInspector;

public class FOVEffect : MonoBehaviour
{
	[Header("Game")]
	public float viewDistance;

	[Header("Technical")]
	public int rayCount = 8;
	[Range(0, 360)]
	public float fov = 360;

	public Vector2 origin;

	public LayerMask layerMask;

	Mesh mesh;

	[HorizontalLine("Rendering")]
	public ComputeShader shader;
	public RenderTexture renderTexture;

	void Start()
	{
		mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
		shader.SetTexture(shader.FindKernel("CSMain"), "Result", renderTexture);
	}

	void LateUpdate()
	{
		Vector3[] vertices = new Vector3[rayCount + 2];
		Vector2[] uv = new Vector2[vertices.Length];
		int[] triangles = new int[rayCount * 3];

		float angle = 0;
		float angleIncrease = fov / rayCount;

		vertices[0] = origin;
		//int mask = LayerMask.GetMask(layerMask);

		int vertexIndex = 1;
		int triangleIndex = 0;
		for (int i = 0; i <= rayCount; i++)
		{
			Vector2 vector = new(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

			RaycastHit2D hit = Physics2D.Raycast(origin, vector, viewDistance, layerMask);

			Vector3 vertex;
			if (hit.collider == null)
			{
				vertex = origin + vector * viewDistance;
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

			angle -= angleIncrease;
		}

		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;

		mesh.RecalculateBounds();
	}
}
