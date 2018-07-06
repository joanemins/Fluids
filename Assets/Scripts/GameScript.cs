using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameScript : MonoBehaviour {
	public Text score1;
	public Text score2;
	public GameObject casilla;
	public GameObject casilla2;
	public GameObject dialogo;




	public AudioClip sound;
	public AudioClip introfx;
	public AudioClip finalfx;


	Camera cam;

	private float tableroAlto;
	private float tableroAncho;
	private float anchoCasilla;
	private float anchoCuadrito;



	private GameObject[,] board;
	private GameObject[,] cuadricula;

	private int color;
	private int rellenoactual;
	private bool canPlay;
	private int puntos;

	private Color32 col;


	private bool musicstart;


	//Nuevos
	private Partida partida;
	private int contadorPartida;

	void Start () {
		//crearTablero ();
		iniciarPartida();
		crearTablero ();
	}

	void iniciarPartida(){
		partida = new Partida();
		partida.crearLaberinto (40);
		contadorPartida = 0;
		color = 5;
		partida.IA (color);
		//partida.IA ();
	}

	void crearTablero(){
		board = new GameObject[partida.getAncho(), partida.getAlto()];
		cuadricula = new GameObject[partida.getAncho2(), partida.getAlto2()];

		cam = Camera.main;
		tableroAlto = 2f * cam.orthographicSize;
		tableroAncho = tableroAlto * cam.aspect;

		anchoCuadrito = (tableroAncho) / (partida.getAncho() + partida.getAnchoCasilla());
		casilla.transform.localScale = new Vector3 (anchoCuadrito, anchoCuadrito, transform.localScale.z);

		anchoCasilla = (tableroAncho) / (partida.getAncho2() + 1);
		casilla2.transform.localScale = new Vector3 (anchoCasilla, anchoCasilla, transform.localScale.z);




		for (int i = 0; i < partida.getAncho2(); i++)
			for (int j = 0; j < partida.getAlto2(); j++) {
				cuadricula [i, j] = (GameObject)(Instantiate (casilla2, new Vector2 (i * anchoCasilla - 2F * anchoCasilla , j * anchoCasilla - 3.5F * anchoCasilla), Quaternion.identity));
				//cuadricula [i, j].GetComponent<SpriteRenderer> ().color = new Color32 (0, 0, 0, 255);
				cuadricula [i, j].GetComponent<CasillaScript> ().x = i;
				cuadricula [i, j].GetComponent<CasillaScript> ().y = j;
			}
		
		for (int i = 0; i < partida.getAncho(); i++)
			for (int j = 0; j < partida.getAlto(); j++) {
				board [i, j] = (GameObject)(Instantiate (casilla, new Vector2 (i * anchoCuadrito - 34.5F * anchoCuadrito, j * anchoCuadrito - 55.5F * anchoCuadrito), Quaternion.identity));
				board [i, j].GetComponent<SpriteRenderer> ().color = new Color32 (0, 0, 0, 255);
			}


		

		pintarTablero ();
	}

	void pintarTablero(){

		for (int i = 0; i < partida.getAncho(); i++)
			for (int j = 0; j < partida.getAlto(); j++) {
				if (partida.tablero[i, j] == 0)
					board [i, j].GetComponent<SpriteRenderer> ().color = new Color32 (200, 200, 200, 255);
				else if (partida.tablero [i, j] == 1 || partida.tablero [i, j] == 10) {
					board [i, j].GetComponent<SpriteRenderer> ().color = new Color32 (50, 50, 50, 255);
				} else if (partida.tablero [i, j] == 5) {
					board [i, j].GetComponent<SpriteRenderer> ().color = new Color32 (30, 144, 255, 255);
					//	board [i, j].GetComponent<SpriteRenderer> ().color = new Color32 (0, 113, 188, 255);
				} else if (partida.tablero [i, j] == 6) {
					board [i, j].GetComponent<SpriteRenderer> ().color = new Color32 (144, 255, 30, 255);
					//	board [i, j].GetComponent<SpriteRenderer> ().color = new Color32 (137, 201, 10, 255);
				} else if (partida.tablero [i, j] == 7) {
					board [i, j].GetComponent<SpriteRenderer> ().color = new Color32 (255, 30, 144, 255);
					//	board [i, j].GetComponent<SpriteRenderer> ().color = new Color32 (193, 10, 201, 255);
				} else if (partida.tablero [i, j] == 8) {
					board [i, j].GetComponent<SpriteRenderer> ().color = new Color32 (255, 140, 0, 255);
					//	board [i, j].GetComponent<SpriteRenderer> ().color = new Color32 (201, 95, 10, 255);
				} else if (partida.tablero [i, j] == -1) {
					board [i, j].GetComponent<SpriteRenderer> ().color = new Color32 (50, 50, 50, 255);
				}
			}
		
		partida.recuentoPuntos ();
		score1.text = (System.Math.Round(partida.getPuntos1(),1)).ToString() + "%";
		score2.text = (System.Math.Round(partida.getPuntos2(),1)).ToString() + "%";
	}

	public void casillaPulsada(int x, int y){
		color = 0;
		if (contadorPartida % 2 == 0) {
			color = 5;
		} else {
			color = 6;
		}
		partida.casillaPulsada (x, y,color);
		pintarTablero ();
		contadorPartida++;
		if(contadorPartida == 6)
			llenarAgua ();
		if (contadorPartida % 2 == 0) {
			color = 5;
		} else {
			color = 6;
		}
		partida.IA (color);
	}

	public void next(){
		iniciarPartida ();
		pintarTablero ();
	}

	public void restart(){
		contadorPartida = 0;
		partida.restart();
		pintarTablero ();
	}

	void llenarAgua(){
		bool llenar = partida.llenaragua();
		pintarTablero ();
		if (llenar){
			Invoke ("llenarAgua", 0.05F);
		}
	}
}