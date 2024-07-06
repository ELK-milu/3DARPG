using System;

namespace Architecture.StateSystem
{
	/// <summary>
	/// 只存储一些属性数值
	/// </summary>
	[Serializable]
	public struct Attribution
	{
		public int MaxHealth;
		public int MaxMana;
		public float MaxStamina;
		public float StaminaRecoverSpeed;
		
		public int Physical;
		public int Magical;
		public int Attack;
		public int Intellect;
		public int Defence;
		public int Agile;
		public int Luck;
		
		public float CurrentStamina;
		public float CurrentHealth;
		public float CurrentMana;
	}
}
