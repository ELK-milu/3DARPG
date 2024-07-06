using UnityEngine;

public class StaminabarLiquid : Liquid
{

	public override void GetMeshAndRend()
	{
		
	}

	public override void Update()
	{
		UpdatePos(Time.deltaTime);
	}

	public void FixedUpdate()
	{
		var faceTrans = Camera.main.transform.eulerAngles;
		// 始终朝向摄像机
		transform.eulerAngles = new Vector3(faceTrans.x, faceTrans.y, faceTrans.z);
	}

	override protected void UpdatePos (float deltaTime)
	{
		if (CompensateShapeAmount > 0)
		{
			pos = new Vector3(0, (fillAmount - (comp.y * CompensateShapeAmount)), 0);
		}
		else
		{
			pos = new Vector3(0, fillAmount, 0);
		}
		rend.sharedMaterial.SetVector("_FillAmount", pos);
	}
}
