using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
	[Serializable]
	public struct Stats
	{
		public float MaxHealth;
		public float Speed;
		public DamageType Strength;
		public DamageType Defense;
	}

	[Serializable]
	public struct DamageType
	{
		public PhysicalDamage Physical;
		public MagicalDamage Magical;

		public DamageType IncreaseByStrength(DamageType strength)
		{
			DamageType dt = new();
			dt.Physical = Physical.IncreaseByStrength(strength.Physical);
			dt.Magical = Magical.IncreaseByStrength(strength.Magical);

			return dt;
		}
	}
	[Serializable]
	public struct PhysicalDamage
	{
		public float Total { get { return Slash + Blunt + Pierce; } }
		public float Slash;
		public float Blunt;
		public float Pierce;

		public PhysicalDamage IncreaseByStrength(PhysicalDamage Strength)
		{
			PhysicalDamage pd = new();
			pd.Slash = Slash * Strength.Slash * 0.2f;
			pd.Blunt = Blunt * Strength.Blunt * 0.2f;
			pd.Pierce = Pierce * Strength.Pierce * 0.2f;

			return pd;
		}
	}
	[Serializable]
	public struct MagicalDamage
	{
		public float Total { get { return Fire + Frost + Lightning; } }
		public float Fire;
		public float Frost;
		public float Lightning;

		public MagicalDamage IncreaseByStrength(MagicalDamage strength)
		{
			MagicalDamage md = new();
			md.Fire = Fire * strength.Fire * 0.2f;
			md.Frost = Frost * strength.Frost * 0.2f;
			md.Lightning = Lightning * strength.Lightning * 0.2f;

			return md;
		}

		public MagicalDamage ReduceByDefense(MagicalDamage defense)
		{
			MagicalDamage md = new();
			md.Fire = Mathf.Max(Fire / defense.Fire, Fire - defense.Fire + 1);

			return md;
		}
	}

	public void TakeDamage(float amount, Vector2 knockback, Vector2 hitPosition);
}