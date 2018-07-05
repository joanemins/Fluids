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

		anchoCuadrito = (tableroAncho) / (partida.getAncho() + 14);
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
		if(contadorPartida == 2)
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


	/*
	public void casillaPulsada(int x, int y){
		int val;
		if (canPlay && x >= 8 && x <= 91 && y >= 37 && y <= 167) {
			if (musicstart == false) {
				//AudioManager.instance.startLoop ();
				AudioManager.instance.PlayFx (sound);
			}
			canPlay = false;
			x -= 8;
			y -= 37;
			x = x / 14;
			y = y / 14;
			if (maze [x * 14 + 9, y * 14 + 38] == 0) {
				for (int i = 0; i < 100; i++)
					for (int j = 0; j < 200; j++)
						mazeaux [i, j] = maze [i, j];

				val = maze [x * 14 + 14, y * 14 + 43];
				for (int i = 0; i < 6; i++)
					for (int j = 0; j < 6; j++) {
						if (maze [x * 14 + 12 + i, y * 14 + 41 + j] == 0) {
							maze [x * 14 + 12+i, y * 14 + 41+j] = val;
							mazeaux [x * 14 + 12+i, y * 14 + 41+j] = -50;
						}
					}
				



				rellenoactual = maze [x * 14 + 14, y * 14 + 43];
				//girarCasilla (x, y);
				pintarTablero ();
				if (val == 0)
					col = new Color32 (200, 200, 200, 255);
				else if (val == 1 || val == 10) {
					col = new Color32 (50, 50, 50, 255);
				} else if (val == 5) {
					col = new Color32 (30, 144, 255, 255);
					//col= new Color32 (0, 113, 188, 255);
				} else if (val == 6) {
					col = new Color32 (144, 255, 30, 255);
					//col = new Color32 (137, 201, 10, 255);
				} else if (val == 7) {
					col = new Color32 (255, 30, 144, 255);
					//col = new Color32 (193, 10, 201, 255);
				} else if (val == 8) {
					col = new Color32 (255, 140, 0, 255);
					//col = new Color32 (201, 95, 10, 255);
				} else if (val == -1) {
					col = new Color32 (50, 50, 50, 255);
				}


				//StartCoroutine(llenaragua());
				llenaragua();
			} else {
				canPlay = true;
			}


		//	Debug.Log (x + " " + y);
		}
			

	}
	void quitarPiezas(){
		bool[,] qpie = new bool[5,8];
		for (int i = 0; i < 5; i++)
			for (int j = 0; j < 8; j++) {
				if (maze [i * 14 + 14, j * 14 + 41]!=0) {
					if (maze [i * 14 + 14, j * 14 + 43] == maze [i * 14, j * 14 + 43] && maze [i * 14 + 14, j * 14 + 41] == maze [i * 14, j * 14 + 41]) {
							qpie [i, j] = true;
						qpie [i-1, j] = true;
					}
					if (maze [i * 14 + 14, j * 14 + 43] == maze [i * 14+ 14, j * 14 + 29] && maze [i * 14 + 14, j * 14 + 41] == maze [i * 14 + 14, j * 14 + 27]) {
							qpie [i, j] = true;
						qpie [i, j-1] = true;
					}
				}
			}
		
		for (int i = 0; i < 5; i++)
			for (int j = 0; j < 8; j++) {
				if (qpie [i, j]) {
					for (int k = 0; k < 14; k++)
						for (int l = 0; l < 14; l++) {
							maze [i * 14 + 8 + k, j * 14 + 37 + l] = -1;
						}
				}
		}
		bool siono = false;
		int contadorpuntos=0;
		for (int i = 0; i < 5; i++)
			for (int j = 0; j < 8; j++)
				if (qpie [i, j]){
					siono = true;
					contadorpuntos += 1;
				}
		puntos += contadorpuntos;

		if (siono) {
			
			for (int i = 0; i < 100; i++)
				for (int j = 0; j < 200; j++) {
					destroy [i, j] = ultimaPintada [i, j];
				}
			pintarTablero ();
			
			Invoke ("destroyPieza", 0.3F);
		} else {
			pintarTablero ();
			calcularFinal ();
			canPlay = true;
		}


	}

	void destroyPieza(){
		for (int i = 0; i < 100; i++)
			for (int j = 0; j < 200; j++) {
				maze [i, j] = destroy [i, j];
			}
		for (int i = 0; i < 100; i++)
			for (int j = 0; j < 200; j++) {
				destroy [i, j] = ultimaPintada [i, j];
			}
		pintarTablero ();
		Invoke ("destroyPieza2", 0.3F);	
	}

	void destroyPieza2(){
		for (int i = 0; i < 100; i++)
			for (int j = 0; j < 200; j++) {
				maze [i, j] = destroy [i, j];
			}
		
		pintarTablero ();
		Invoke ("bajarPiezas", 0.45F);
	}

	void calcularFinal(){
		bool final = true;
		for (int i = 0; i < 6; i++)
			for (int j = 0; j < 9; j++) {
				if (maze [i * 14 + 9, j * 14 + 38] == 0) {
					final = false;
				}
			}
		if (final == true) {
			//AudioManager.instance.stopLoop ();
			//AudioManager.instance.PlayFx (finalfx);
			AudioManager.instance.StopMusic();
			Invoke ("mostrarDialogo", 0.45F);
		}
		
	}

	void mostrarDialogo(){
		finalScore.text = "Score: " + puntos.ToString ();
		if (PlayerPrefs.GetInt ("record", 0) < puntos) {
			PlayerPrefs.SetInt ("record", puntos);
		}
		finalRecord.text = "Record: " + PlayerPrefs.GetInt ("record", 0).ToString ();

		dialogo.SetActive (true);
	}

	void bajarPiezas(){
		bool siono = false;

			for (int j = 0; j < 200; j++)
			for (int i = 0; i < 100 ; i++){
				if (maze [i, j] == -1) {
					for (int k = j; k < 199; k++) {
						if (k == 148 && maze [i, k + 1]==10) {
							if (i == 8) {
								llenarRellenos (0, 8);
								llenarCasilla (0, 8);
							}else if (i == 22) {
								llenarRellenos (1, 8);
								llenarCasilla (1, 8);
							}else if (i == 36) {
								llenarRellenos (2, 8);
								llenarCasilla (2, 8);
							}else if (i == 50) {
								llenarRellenos (3, 8);
								llenarCasilla (3, 8);
							}else if (i == 64) {
								llenarRellenos (4, 8);
								llenarCasilla (4, 8);
							}
								
						}
						maze [i, k] = maze [i, k + 1];
					}
						
					siono = true;
				}
			}
		if (siono) {
			pintarTablero ();
			Invoke ("bajarPiezas", 0.06F);
		} else {
			pintarTablero ();
			quitarPiezas ();
		}
	}

	//IEnumerator llenaragua (){
	void llenaragua (){
		bool siono = false;
		int val = rellenoactual;
		//int[,] mazeaux= new int[100, 200];

		
		bool samecolour = true;



		for (int i = 8; i < 78; i++)
			for (int j = 37; j < 149; j++) {
				if (mazeaux [i, j] !=-50 && mazeaux [i, j] !=1 && mazeaux [i, j] !=10 && i > 0 && maze [i - 1, j] == val && mazeaux [i - 1, j] == -50) {
					board [i , j].GetComponent<SpriteRenderer> ().color = col;

					if (maze [i, j] != val)
						samecolour = false;
					mazeaux [i , j] = -50;


					siono = true;
				}else if (mazeaux [i, j] !=-50 && mazeaux [i, j] !=1 && mazeaux [i, j] !=10 && i <99 && maze [i + 1, j] == val && mazeaux [i + 1, j] == -50) {
					board [i , j].GetComponent<SpriteRenderer> ().color = col;

					if (maze [i, j] != val)
						samecolour = false;
					mazeaux [i , j] = -50;

					siono = true;
				}else if (mazeaux [i, j] !=-50 && mazeaux [i, j] !=1 && mazeaux [i, j] !=10 && j > 0 && maze [i, j-1] == val && mazeaux [i, j-1] == -50) {
					board [i , j].GetComponent<SpriteRenderer> ().color = col;

					if (maze [i, j] != val)
						samecolour = false;
					mazeaux [i, j] = -50;

					siono = true;
				}else if (mazeaux [i, j] !=-50 && mazeaux [i, j] !=1 && mazeaux [i, j] !=10 && j < 199 && maze [i, j+1] == val && mazeaux [i, j+1] == -50) {
					board [i , j].GetComponent<SpriteRenderer> ().color = col;
					if (maze [i, j] != val)
						samecolour = false;
					mazeaux [i , j] = -50;

					siono = true;
				}
			}
		
		if (siono) {
			for (int i = 0; i < 100; i++)
				for (int j = 0; j < 200; j++)
					if(mazeaux[i,j]==-50)
						maze [i , j] = val;
						
			//pintarTablero ();
			if (samecolour) {
				//StartCoroutine(llenaragua());
				llenaragua();
			} else {
				//yield return new WaitForSeconds(0.04F);
				//StartCoroutine(llenaragua());
				Invoke ("llenaragua", 0.038F);
			}

		} else {
			for (int i = 0; i < 100; i++)
				for (int j = 0; j < 200; j++) {
					ultimaPintada[i,j]= maze [i, j];
				}
			quitarPiezas ();
		}
				
	}

	public void restart(){
		musicstart = false;
		dialogo.SetActive (false);
		AudioManager.instance.StopMusic ();

		if (canPlay) {
			
			puntos = 0;
			for (int i = 8; i < 78; i++)
				for (int j = 37; j < 149; j++)
					maze [i, j] = -1;
			pintarTablero ();
			Invoke ("bajarPiezas", 0.5F);
		}
		//AudioManager.instance.stopLoop ();
		//AudioManager.instance.PlayFx (introfx);

	}

	public void share(){
		
		string facebookshare = "https://www.facebook.com/sharer/sharer.php?u=" + System.Uri.EscapeUriString("dsasdfas");
		Application.OpenURL(facebookshare);


	}


	// Update is called once per frame
	void Update () {
	
	}
	*/
}
