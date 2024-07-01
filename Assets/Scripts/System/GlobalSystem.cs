using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSystem : MonoBehaviour
{
	private static GlobalSystem instance;
	private static bool origional = true;
 
	protected virtual void Awake()
	{
		if (origional) {
			instance = this as GlobalSystem;
			origional = false;
			DontDestroyOnLoad(this.gameObject);
		} else {
			Destroy(this.gameObject);
		}
	}
}
