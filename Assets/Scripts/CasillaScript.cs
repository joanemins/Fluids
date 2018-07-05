using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CasillaScript : MonoBehaviour {
	public int x;
	public int y;

	GameObject juego;
	GameScript script;



	// Use this for initialization
	void Start () 
	{
	}

	void Awake(){

		juego=GameObject.Find("ControladorJuego");
		script = juego.GetComponent<GameScript>();

	}


	void OnMouseDown() {
		script.casillaPulsada (x, y);
	}



}
