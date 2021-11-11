using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Windows.Speech;
using System;

public class PlayerInput : MonoBehaviour
{
	public LayerMask targetLayerMask;
	private Camera cam;
	//public Camera cam;
	private Rect inputRect;
	
	private KeywordRecognizer KeywordRecognizer;
	private Dictionary<string, Action> actions = new Dictionary<string, Action>();

	


	/*
	void Start()
	{
	
		actions.Add("fire", Fire);
		KeywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
		KeywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
		KeywordRecognizer.Start();

	}
	*/

	
	private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
	{
		Debug.Log(speech.text);
		actions[speech.text].Invoke();

	}
	
	private void Fire()
	{
		//transform.Translate(0, 5, 0);
		
			Shoot();
		
	}
	
	void Start()
	{
		
		actions.Add("kill", Fire);
		KeywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
		KeywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
		KeywordRecognizer.Start();

		
		/////
		inputRect = new Rect(Screen.width / 2, 0, Screen.width, Screen.height * 0.75f);
		cam = GetComponentInChildren<Camera>();
		Debug.Log("Game Started");
	}


	void Update()
	{
		Debug.Log("Game updated");
		KeyboardInput();
	}

	/*
	void Update()
	{
		if (GameManager.instance.gameState == GameState.Running)
		{
			Debug.Log("Game updated");
			TouchInput();
#if UNITY_EDITOR
			KeyboardInput ();
#endif
		}
	}
*/

	/*  
	   private void TouchInput()
	   {
		   Debug.Log("TouchInput111");
		   if (Input.touchCount > 0)
		   {
		   Debug.Log("TouchInput");
		   foreach (Touch touch in Input.touches)
			   {
				   Debug.Log("TouchInput222");
				   if (touch.phase != TouchPhase.Began)
					   continue;

				   if (!inputRect.Contains(touch.position))
					   continue;

				   Debug.Log("shooting");
				   Shoot();
			   }
		   }
	   }
	*/

	private void KeyboardInput()
	{
		Debug.Log("KeyboardInput");
		if (Input.GetButtonDown("Fire1"))
		{
			Debug.Log("KeyboardInput11");
			Shoot();
		}
	}
	/*
#if UNITY_EDITOR
	private void KeyboardInput ()
	{
		if (Input.GetButtonDown ("Fire1")) {
			Shoot ();
		}
	}
#endif
*/
	private void Shoot()
	{
		// Check for target
		RaycastHit hit;
		Debug.Log("shooted");
		//
		if (cam != null)
		{
			Debug.Log("Hello");
			if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 100f, targetLayerMask))
			{
				Debug.Log("Hellooooo22");
				Target target = hit.collider.GetComponentInParent<Target>();
				target.Hit(hit);
				Debug.Log("finish");
			}

		}

	}

}
