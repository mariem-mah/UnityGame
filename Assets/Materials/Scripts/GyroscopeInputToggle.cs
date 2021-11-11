using UnityEngine;
using System.Collections;

public class GyroscopeInputToggle : MonoBehaviour
{
	public GameObject disabled;

	private bool gyroscopeAvailable;

	void Awake()
	{
		gyroscopeAvailable = SystemInfo.supportsGyroscope;
	}

	void Start()
	{
		if (!gyroscopeAvailable)
		{
			Input.gyro.enabled = false;
			this.gameObject.SetActive(false);
		}
		else
		{
			Input.gyro.enabled = true;
		}
	}

	public void Toggle()
	{
		if (Input.gyro.enabled)
		{
			disabled.SetActive(true);
			Input.gyro.enabled = false;
		}
		else
		{
			disabled.SetActive(false);
			Input.gyro.enabled = true;
		}
	}
}
