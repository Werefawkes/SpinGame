using CustomInspector;
using UnityEngine;

[CreateAssetMenu]
public class WeaponSO : ScriptableObject
{

	[HorizontalLine("Stats")]
	public float damage = 1;
	public float knockbackMultiplier = 1;
	public float speed = 10;
	public float firingArc = 5;
	public float duration = 10;
	public float cooldown = 0.5f;
	public float drag = 0;

	[HorizontalLine("Behaviour")]
	[Tooltip("If checked, the projectile will not be destroyed on collisions or when expiring.")]
	public bool isPersistent = false;
	[Tooltip("The maximum number of this projectile that can be in the world at once. Set to -1 to allow an infinite amount.")]
	public int maxCount = -1;
	public ProjectileBehaviour projectileBehaviour = ProjectileBehaviour.Simple;

	[ShowIfIs(nameof(projectileBehaviour), ProjectileBehaviour.Orbit)]
	public float orbitDistance = 3;
	[ShowIfIs(nameof(projectileBehaviour), ProjectileBehaviour.Orbit)]
	public bool orbitMaxOnly = false;

	[Tooltip("The distance at which the flail orbits when swinging it."), ShowIfIs(nameof(projectileBehaviour), ProjectileBehaviour.Flail)]
	public float flailSwingDistance = 2;
	[Tooltip("The max distance the flail can travel when releasing it."), ShowIfIs(nameof(projectileBehaviour), ProjectileBehaviour.Flail)]
	public float flailLaunchDistance = 6;
	[Tooltip("The speed at which the flail can change distances (such as pulling it back)."), ShowIfIs(nameof(projectileBehaviour), ProjectileBehaviour.Flail)]
	public float flailDistanceDelta = 2;
	[Tooltip("The speed at which the flail's drag will change."), ShowIfIs(nameof(projectileBehaviour), ProjectileBehaviour.Flail)]
	public float flailDragDelta = 2;
	[Tooltip("The force with which the flail will be yanked when starting to spin after a launch."), ShowIfIs(nameof(projectileBehaviour), ProjectileBehaviour.Flail)]
	public float flailYankForce = 5;

	[HorizontalLine("Audio")]
	public float volume = 1;
	[AsRange(0, 2)] public Vector2 pitchRange = new(0.75f, 1.25f);
	public AudioClip[] fireSounds;
	
	public enum ProjectileBehaviour
	{
		Simple,
		Orbit,
		Flail
	}

	[HorizontalLine]
	public GameObject prefab;

	public bool IsSimple { get { return projectileBehaviour == ProjectileBehaviour.Simple; } }
	public bool IsOrbiter { get { return projectileBehaviour == ProjectileBehaviour.Orbit; } }
	public bool IsFlail { get { return projectileBehaviour == ProjectileBehaviour.Flail; } }
}