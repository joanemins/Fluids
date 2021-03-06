﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Partida{
	public int[,] tablero;
	public int[,] tableroRestart;
	private int[,] laberinto;
	private int[,] laberintoRestart;
	private bool[,] laberintoBool; 
	private int[,] jugadas;
	private int[,] jugadasAux;
	private int[,] jugadasAux2;
	private int[,] rellenos;
	private int alto = 8;
	private int ancho = 5;
	private int tamanyoCasilla = 14;
	private int puntosPosibles = 0;
	private double puntos1;
	private double puntos2;
	private int puntosRecursivos;

	public Partida(){
		this.tablero = new int[ancho * tamanyoCasilla, alto * tamanyoCasilla];
		this.laberinto = new int[ancho, alto];
		this.laberintoBool = new bool[ancho, alto];
		this.tableroRestart = new int[ancho * tamanyoCasilla, alto * tamanyoCasilla];
		this.laberintoRestart = new int[ancho, alto];
		this.jugadas = new int[ancho, alto];
		this.jugadasAux = new int[ancho, alto];
		this.jugadasAux2 = new int[ancho, alto];
		this.rellenos = new int[ancho * tamanyoCasilla, alto * tamanyoCasilla];
	}

	public int getAncho(){
		return ancho * tamanyoCasilla;
	}

	public int getAlto(){
		return alto * tamanyoCasilla;
	}
	public int getAncho2(){
		return ancho;
	}

	public int getAlto2(){
		return alto;
	}
	public int getAnchoCasilla(){
		return tamanyoCasilla;
	}
	public int[,] getTablero(){
		int[,] aux = new int[this.getAncho(), this.getAlto()];
		for(int i = 0; i < this.getAncho(); i++)
			for(int j = 0; j < this.getAlto(); j++)
				aux[i,j] = this.tablero[i, j];
		return aux;
	}

	class Jugada{
		public int fila;
		public int columna;
		public int direccion;
		public double puntuacion1;
		public double puntuacion2;
		public int fila2;
		public int columna2;
	}

	public void crearLaberinto(int numCasillas){
		int i, j, n;
		Jugada[] jugadasPosibles = new Jugada[this.ancho * this.alto];

		int aux; 
		int contJugadas = 0;
		int[] contactos = new int[4];
		int numContactos = 0;

		for(i = 0; i < ancho; i++){
			for (j = 0; j < alto; j++) {
				laberinto [i, j] = -1;
				jugadasPosibles [contJugadas] = new Jugada();
				jugadasPosibles [contJugadas].fila = i;
				jugadasPosibles [contJugadas].columna = j;
				contJugadas++;
			}
		}
		aux = Random.Range (0, contJugadas);
		laberinto [jugadasPosibles [aux].fila, jugadasPosibles [aux].columna] = 1111;

		for (n = 1; n < numCasillas; n++) {
			contJugadas = 0;
			for(i = 0; i < ancho; i++){
				for (j = 0; j < alto; j++) {
					if (laberinto [i, j] == -1) {
						numContactos = 0;
						if (i > 0 && laberinto [i - 1, j] != -1) {
							contactos [numContactos] = 4;
							numContactos++;
						}
						if (j > 0 && laberinto [i, j - 1] != -1) {
							contactos [numContactos] = 3;
							numContactos++;
						}
						if (i < ancho - 1 && laberinto [i + 1, j] != -1) {
							contactos [numContactos] = 2;
							numContactos++;
						}
						if (j < alto - 1 && laberinto [i, j + 1] != -1) {
							contactos [numContactos] = 1;
							numContactos++;
						}
						if (numContactos > 0) {
							aux = Random.Range (0, numContactos);
							jugadasPosibles [contJugadas].fila = i;
							jugadasPosibles [contJugadas].columna = j;
							jugadasPosibles [contJugadas].direccion = contactos[aux];
							contJugadas++;
						}
					}
				}
			}
			aux = Random.Range (0, contJugadas);
			laberinto [jugadasPosibles [aux].fila, jugadasPosibles [aux].columna] = 1111;
			switch (jugadasPosibles [aux].direccion) {
			case 1:
				laberinto [jugadasPosibles [aux].fila, jugadasPosibles [aux].columna] -= 1000;
				laberinto [jugadasPosibles [aux].fila, jugadasPosibles [aux].columna + 1] -= 10;
				break;
			case 2:
				laberinto [jugadasPosibles [aux].fila, jugadasPosibles [aux].columna] -= 100;
				laberinto [jugadasPosibles [aux].fila + 1, jugadasPosibles [aux].columna] -= 1;
				break;
			case 3:
				laberinto [jugadasPosibles [aux].fila, jugadasPosibles [aux].columna] -= 10;
				laberinto [jugadasPosibles [aux].fila, jugadasPosibles [aux].columna - 1] -= 1000;
				break;
			case 4:
				laberinto [jugadasPosibles [aux].fila, jugadasPosibles [aux].columna] -= 1;
				laberinto [jugadasPosibles [aux].fila - 1, jugadasPosibles [aux].columna] -= 100;
				break;
			}

		}

		for(i = 0; i < ancho; i++){
			for (j = 0; j < alto; j++) {
				if (laberinto [i, j] != -1) {
					if (i < ancho - 1 && laberinto [i + 1, j] != -1) {
						aux = Random.Range (0, 5);
						if (aux < 2 && laberinto [i + 1, j] % 1 == 1) {
							laberinto [i, j] -= 100;
							laberinto [i + 1, j] -= 1;
						}
					}
					if (j < alto - 1 && laberinto [i, j + 1] != -1) {
						aux = Random.Range (0, 5);
						if (aux < 2 && (laberinto [i, j + 1] / 10) % 10 == 1) {
							laberinto [i, j] -= 1000;
							laberinto [i, j + 1] -= 10;
						}
					}
				}
			}
		}

		crearCasillas ();
		guardarCopia ();
	}

	void guardarCopia(){
		int i, j;
		for(i = 0; i < ancho * tamanyoCasilla; i++)
			for (j = 0; j < alto * tamanyoCasilla; j++) {
				tableroRestart [i, j] = tablero [i, j];
			}
		for(i = 0; i < ancho; i++)
			for (j = 0; j < alto; j++) {
				laberintoRestart [i, j] = laberinto [i, j];
			}
		
	}

	public void restart(){
		int i, j;
		for(i = 0; i < ancho * tamanyoCasilla; i++)
			for (j = 0; j < alto * tamanyoCasilla; j++) {
				tablero [i, j] = tableroRestart [i, j];
			}
		for(i = 0; i < ancho; i++)
			for (j = 0; j < alto; j++) {
				laberinto [i, j] = laberintoRestart [i, j];
			}
		for(i = 0; i < ancho; i++)
			for (j = 0; j < alto; j++) {
				jugadas [i, j] = 0;
			}

	}

	void crearCasillas(){
		int i, j, f, c, valor;
		for(i = 0; i < ancho; i++){
			for (j = 0; j < alto; j++) {
				if (laberinto [i, j] == -1) {
					for (f = 0; f < tamanyoCasilla; f++) {
						for (c = 0; c < tamanyoCasilla; c++) {
							tablero [i * tamanyoCasilla + f, j * tamanyoCasilla + c] = -1;
						}
					}
				} else {
					valor = laberinto [i, j];
					if (valor % 10 == 1) {
						for (f = 0; f < tamanyoCasilla; f++) {
							tablero [i * tamanyoCasilla, j * tamanyoCasilla + f] = -1;
						}
					} else {
						for (f = 0; f < 1; f++) {
							tablero [i * tamanyoCasilla, j * tamanyoCasilla + f] = -1;
						}
						for (f = tamanyoCasilla - 1; f < tamanyoCasilla; f++) {
							tablero [i * tamanyoCasilla, j * tamanyoCasilla + f] = -1;
						}
					}

					valor = valor / 10;
					if(valor % 10 == 1){
						for (f = 0; f < tamanyoCasilla; f++) {
							tablero [i * tamanyoCasilla + f, j * tamanyoCasilla] = -1;
						}
					}else {
						for (f = 0; f < 1; f++) {
							tablero [i * tamanyoCasilla + f, j * tamanyoCasilla] = -1;
						}
						for (f = tamanyoCasilla - 1; f < tamanyoCasilla; f++) {
							tablero [i * tamanyoCasilla + f, j * tamanyoCasilla] = -1;
						}
					}

					valor = valor / 10;
					if(valor % 10 == 1){
						for (f = 0; f < tamanyoCasilla; f++) {
							tablero [i * tamanyoCasilla + tamanyoCasilla - 1, j * tamanyoCasilla + f] = -1;
						}
					}else {
						for (f = 0; f < 1; f++) {
							tablero [i * tamanyoCasilla + tamanyoCasilla - 1, j * tamanyoCasilla + f] = -1;
						}
						for (f = tamanyoCasilla - 1; f < tamanyoCasilla; f++) {
							tablero [i * tamanyoCasilla + tamanyoCasilla - 1, j * tamanyoCasilla + f] = -1;
						}
					}


					valor = valor/10;
					if(valor % 10 == 1){
						for (f = 0; f < tamanyoCasilla; f++) {
							tablero [i * tamanyoCasilla + f, j * tamanyoCasilla + tamanyoCasilla - 1] = -1;
						}
					}else {
						for (f = 0; f < 1; f++) {
							tablero [i * tamanyoCasilla + f, j * tamanyoCasilla + tamanyoCasilla - 1] = -1;
						}
						for (f = tamanyoCasilla - 1; f < tamanyoCasilla; f++) {
							tablero [i * tamanyoCasilla + f, j * tamanyoCasilla + tamanyoCasilla - 1] = -1;
						}
					}
				}
			}
		}
		int puntos = 0;
		for (i = 0; i < this.ancho * tamanyoCasilla; i++)
			for (j = 0; j < this.alto * tamanyoCasilla; j++) {
				if (tablero [i, j] == 0) {
					puntos++;
				}
			}
		this.puntosPosibles = puntos;

	}

	public void casillaPulsada(int x, int y,int color){
		int val;
		jugadas [x, y] = color;

		if (tablero [tamanyoCasilla/2 + x * tamanyoCasilla, tamanyoCasilla/2 + y * tamanyoCasilla + 0] == 0) {
			tablero [tamanyoCasilla/2 + x * tamanyoCasilla, tamanyoCasilla/2 + y * tamanyoCasilla] = color;
			tablero [tamanyoCasilla/2 -1 + x * tamanyoCasilla, tamanyoCasilla/2 + y * tamanyoCasilla] = color;
			tablero [tamanyoCasilla/2 + x * tamanyoCasilla, tamanyoCasilla/2 - 1 + y * tamanyoCasilla] = color;
			tablero [tamanyoCasilla/2 - 1 + x * tamanyoCasilla, tamanyoCasilla/2 - 1 + y * tamanyoCasilla] = color;



		} else {
			tablero [tamanyoCasilla/2 + x * tamanyoCasilla, tamanyoCasilla/2 + y * tamanyoCasilla] = 0;
			tablero [tamanyoCasilla/2-1 + x * tamanyoCasilla, tamanyoCasilla/2 + y * tamanyoCasilla] = 0;
			tablero [tamanyoCasilla/2 + x * tamanyoCasilla, tamanyoCasilla/2-1 + y * tamanyoCasilla] = 0;
			tablero [tamanyoCasilla/2-1 + x * tamanyoCasilla, tamanyoCasilla/2-1 + y * tamanyoCasilla] = 0;
		}

	}

	public bool llenaragua (){
		bool siono = false;
		int[,] tableroAux= new int[this.ancho * tamanyoCasilla, this.alto * tamanyoCasilla];
		for (int i = 0; i < this.ancho * tamanyoCasilla; i++)
			for (int j = 0; j < this.alto * tamanyoCasilla; j++) {
				tableroAux [i, j] = tablero [i, j];
			}
				
	
		int rotacion;
		bool color1;
		bool color2;
		for (int i = 0; i < this.ancho * tamanyoCasilla; i++)
			for (int j = 0; j < this.alto * tamanyoCasilla; j++) {
				//Debug.Log("dfasdf: " + i + " adfadfafd:  " + j);
				color1 = false;
				color2 = false;
				if ((tablero [i, j] == 0 || tablero [i, j] == 1) && i > 0 && tableroAux [i - 1, j] > 1) {
					if (tableroAux [i - 1, j] == 5) {
						color1 = true;
					} else {
						color2 = true;
					}
				}
				if ((tablero [i, j] == 0 || tablero [i, j] == 1) && i < alto * tamanyoCasilla - 1 && tableroAux [i + 1, j] > 1) {
					if (tableroAux [i + 1, j] == 5) {
						color1 = true;
					} else {
						color2 = true;
					}
				}
				if ((tablero [i, j] == 0 || tablero [i, j] == 1) && j > 0 && tableroAux [i, j - 1] > 1) {
					if (tableroAux [i, j - 1] == 5) {
						color1 = true;
					} else {
						color2 = true;
					}
				}
				if ((tablero [i, j] == 0 || tablero [i, j] == 1) && j < alto * tamanyoCasilla - 1 && tableroAux [i, j + 1] > 1) {
					if (tableroAux [i, j + 1] == 5) {
						color1 = true;
					} else {
						color2 = true;
					}
				}
				if (color1) {
					siono = true;
					if (color2) {
						if (i % 2 == j % 2) {
							tablero [i, j] = 5;
						} else {
							tablero [i, j] = 6;
						}
					} else {
						tablero [i, j] = 5;
					}
				} else if(color2) {
					siono = true;
					tablero [i, j] = 6;
				}


			}

		return siono;

	}
		
	public void recuentoPuntos(){
		int p1 = 0;
		int p2 = 0;
		for (int i = 0; i < this.ancho * tamanyoCasilla; i++)
			for (int j = 0; j < this.alto * tamanyoCasilla; j++) {
				if (tablero [i, j] == 6) {
					p1++;
				}else if(tablero [i, j] == 5){
					p2++;
				}
			}
		
		this.puntos1 = p1 * 100.0F / puntosPosibles;
		this.puntos2 = p2 * 100.0F / puntosPosibles;
	}

	public double getPuntos1(){
		return this.puntos1;
	}
	public double getPuntos2(){
		return this.puntos2;
	}

	public bool jugadaPosible(int x, int y){
		bool siono = true;
		if (tablero [tamanyoCasilla/2  + x * tamanyoCasilla, tamanyoCasilla/2  + y * tamanyoCasilla + 0] != 0) {
			siono = false;
		}
		return siono;
	}

	void setearTablero(int[,] aux){
		int i, j;
		for (i = 0; i < this.ancho * tamanyoCasilla; i++)
			for (j = 0; j < this.alto * tamanyoCasilla; j++) {
				tablero [i, j] = aux [i, j];
			}
	}

	public void IA(int color){
		int[,] jugadasPosibles= new int[ancho, alto];
		int[,] tab = new int[ancho, alto];
		int i, j, f, c;
		int[,] tableroAux = new int[ancho * tamanyoCasilla, alto * tamanyoCasilla];

		int colorContrario = 1 - (color - 5) + 5;
		for (i = 0; i < ancho; i++)
			for (j = 0; j < alto; j++) {
				if(jugadas [i, j] == 0){
					for (f = 0; f < ancho; f++)
						for (c = 0; c < alto; c++) {
							jugadasAux [f, c] = jugadas [f, c];
						}
					jugadasAux [i, j] = color;
					jugadasPosibles [i, j] = puntuacion(jugadasAux, color);
				}

			}
		int max = -1000;
		int a = 0, b = 0;
		for (f = 0; f < ancho; f++)
			for (c = 0; c < alto; c++) {
				if (jugadasPosibles[f,c] > max) {
					max = jugadasPosibles [f, c];
					a = f;
					b = c;
				}
			}
		//Debug.Log (a + " " + b);
		for (f = 0; f < ancho; f++)
			for (c = 0; c < alto; c++) {
				jugadasAux [f, c] = jugadas [f, c];
			}
		Debug.Log (puntuacion(jugadasAux, color));
	}

	/*
	 * 
	//La idea es que añada las casillas expandidas en la iteración a la lista auxiliar, de esta forma podemos diferenciar cuando haya conflictos entre nodos expandidos
	//no expandidos, explorados en la anterior iteración o explorado en esta misma iteración.
	public void expandir(int[,] tablero, Vector2 casillaInicial, int colorPropio, List<Vector3> expandidos, List<Vector3> auxiliar){

	}



	/// <summary>
	/// Puntuacion the specified tablero, colorPropio, colorContrario and fichas.
	/// </summary>
	/// <param name="tablero">Tablero.</param>
	/// <param name="colorPropio">Color propio.</param>
	/// <param name="colorContrario">Color contrario.</param>
	/// <param name="fichas">Casilla donde se han colocado los tokens de los jugadores, incluye (x,y,color)</param>
	public double puntuacion(int[,] tablero, int colorPropio, int colorContrario,Vector3[] fichas){
		List<Vector3> explorados;
		List<Vector3> aux;
		List<Vector3> expandidos;
		foreach(Vector3 casilla in fichas){
			explorados.Add(casilla);
		}
		do{
			aux.Clear();
			foreach(Vector3 v in explorados){
				expandir(tablero, v,colorPropio,expandidos,aux);
				expandidos.Add(v);
				explorados.Remove(v);
			}
			aux.CopyTo(explorados);
			aux.Clear();
		}
		while(expandidos.Capacity + explorados.Capacity <20);
		double resultado = 0.0;
		foreach(Vector3 v in explorados)
			return resultado;
	} 
	*/


	int puntuacion(int[,] tab, int color){
		int i, j, f, c;
		int[,] puntos = new int[ancho, alto];
		int score = 0;
		int colorContrario = 1 - (color - 5) + 5;
		for (i = 0; i < ancho; i++)
			for (j = 0; j < alto; j++) {
				if (laberinto [i, j] != -1 && tab[i,j] == 0) {
					puntos [i, j] = puntosCasilla (tab,i,j,color,colorContrario);
				}
			}
		for (i = 0; i < ancho; i++)
			for (j = 0; j < alto; j++) {
				score += puntos [i, j];
			}
		return score;
	}

	int puntosCasilla(int[,] tab, int x, int y, int color, int colorContrario){
		int puntos = 0, valor;
		int i, j;
		bool[,] tabAux = new bool[ancho, alto];
		bool[,] tabAux1 = new bool[ancho, alto];
		bool siono;
		tabAux1 [x, y] = true;
		bool p1 = false, p2 = false;
		int d1 = 0, d2 = 0, m1, m2;
		do {
			siono = false;

			m1= 100;
			m2 = 100;
			for(i = 0; i < ancho; i++)
				for(j = 0; j < alto; j++){
					tabAux[i,j] = tabAux1[i,j];
				}
			for(i = 0; i < ancho; i++)
				for(j = 0; j < alto; j++){
					if(tabAux[i, j] == false && laberinto[i ,j]!= -1){
						valor = laberinto [i, j];

						if (valor % 10 == 0 && i > 0 && tabAux[i - 1, j]) {
							tabAux1 [i, j] = true;
							if(tab[i,j] == color){
								siono= true;
								p1 = true;
								d1 = Mathf.Abs(i - x) + Mathf.Abs(j-y);
								if(d1<m1){
									m1= d1;
								}else{
									d1 = m1;
								}
							}else if(tab[i,j] == colorContrario){
								siono = true;
								p2 = true;
								d2 = Mathf.Abs(i - x) + Mathf.Abs(j-y);
								if(d2<m2){
									m2= d2;
								}else{
									d2 = m2;
								}
							}
						}
						valor = valor / 10;
						if (valor % 10 == 0 && j > 0 && tabAux[i, j - 1]) {
							tabAux1 [i, j] = true;
							if(tab[i, j] == color){
								siono= true;
								p1 = true;
								d1 = Mathf.Abs(i-x) + Mathf.Abs(j -y);
								if(d1<m1){
									m1= d1;
								}else{
									d1 = m1;
								}
							}else if(tab[i, j] == colorContrario){
								siono = true;
								p2 = true;
								d2 = Mathf.Abs(i-x) + Mathf.Abs(j - y);
								if(d2<m2){
									m2= d2;
								}else{
									d2 = m2;
								}
							}
						}
						valor = valor / 10;
						if (valor % 10 == 0 && i < ancho - 1 && tabAux[i + 1, j]) {
							tabAux1 [i, j] = true;
							if(tab[i,j] == color){
								siono= true;
								p1 = true;
								d1 = Mathf.Abs(i - x) + Mathf.Abs(j-y);
								if(d1<m1){
									m1= d1;
								}else{
									d1 = m1;
								}
							}else if(tab[i,j] == colorContrario){
								siono = true;
								p2 = true;
								d2 = Mathf.Abs(i-x) + Mathf.Abs(j-y);
								if(d2<m2){
									m2= d2;
								}else{
									d2 = m2;
								}
							}
						}
						valor = valor / 10;
						if (valor % 10 == 0 && j < alto - 1 && tabAux[i, j + 1]) {
							tabAux1 [i, j] = true;
							if(tab[i,j] == color){
								siono= true;
								p1 = true;
								d1 = Mathf.Abs(i-x) + Mathf.Abs(j-y);
								if(d1<m1){
									m1= d1;
								}else{
									d1 = m1;
								}
							}else if(tab[i,j] == colorContrario){
								siono = true;
								p2 = true;
								d2 = Mathf.Abs(i-x) + Mathf.Abs(j-y);
								if(d2<m2){
									m2= d2;
								}else{
									d2 = m2;
								}
							}
						}
					}
				}
			if(p1 && p2){
				if(d1 < d2){
					puntos = 1;
				}else if(d2 > d1){
					puntos = -1;
				}else{
					puntos = 0;
				}
			}else if(p1){
				puntos = 1;
			}else if(p2){
				puntos = -1;
			}else{
				puntos = 0;
			}
		} while(!siono);
			
		return puntos;
	}

	void recursiveCount(){
		puntosRecursivos++;
		int valor;
		bool encontrado = false;
		bool[,] laberintoBoolAux = new bool[ancho, alto];
		for (int i = 0; i < ancho; i++)
			for (int j = 0; j < alto; j++) {
				laberintoBoolAux[i,j] = laberintoBool[i,j];
			}

		for (int i = 0; i < ancho; i++)
			for (int j = 0; j < alto; j++) {
				if (laberinto [i, j] != -1 && !laberintoBoolAux [i, j]) {
					valor = laberinto [i, j];
					if (valor % 10 == 0 && laberintoBoolAux[i - 1, j]) {
						laberintoBool [i, j] = true;
						encontrado = true;
					}
					valor = valor / 10;
					if (valor % 10 == 0 && laberintoBoolAux[i, j - 1]) {
						laberintoBool [i, j] = true;
						encontrado = true;
					}
					valor = valor / 10;
					if (valor % 10 == 0 && laberintoBoolAux[i + 1, j]) {
						laberintoBool [i, j] = true;
						encontrado = true;
					}
					valor = valor / 10;
					if (valor % 10 == 0 && laberintoBoolAux[i, j + 1]) {
						laberintoBool [i, j] = true;
						encontrado = true;
					}
				}

			}
		if (encontrado) {
			recursiveCount ();
		}
	}
		
	/*int miniMax(int[,] tab, int min, int max, int color){
		int i, j, f, c, puntos = 0;
		int tableroAux2 = new int[ancho * tamanyoCasilla, alto * tamanyoCasilla];
		int colorcontrario = 5;
		if (color == 5) {
			colorcontrario = 6;
		}
		int tableroAux = new int[ancho * tamanyoCasilla, alto * tamanyoCasilla];
		for (i = 0; i < ancho; i++)
			for (j = 0; j < alto; j++) {
				if(tab [6 + i * 14, 6 + j * 14] == 0){
					for (f = 0; f < ancho * tamanyoCasilla; f++)
						for (c = 0; c < alto * tamanyoCasilla; c++) {
							tableroAux2 [f, c] = tab [f, c];
						}
					puntos = llenaragua2 (tableroAux2, color);
				}
				//if(puntos>)
			}
	}*/



	int llenaragua2 (int[,] tab, int color){
		bool siono;
		bool color1;
		bool color2;
		do {
			siono = false;
			for (int i = 0; i < this.ancho * tamanyoCasilla; i++)
				for (int j = 0; j < this.alto * tamanyoCasilla; j++) {
					rellenos [i, j] = tab[i, j];
				}
			for (int i = 1; i < this.ancho * tamanyoCasilla - 1; i++)
				for (int j = 1; j < this.alto * tamanyoCasilla - 1; j++) {
					//Debug.Log("dfasdf: " + i + " adfadfafd:  " + j);
					color1 = false;
					color2 = false;
					if (tab [i, j] == 0) {
						if (rellenos [i - 1, j] == 5 || rellenos [i + 1, j] == 5 || rellenos [i, j - 1] == 5 || rellenos [i, j + 1] == 5) {
							color1 = true;
						}
						if (rellenos [i - 1, j] == 6 || rellenos [i + 1, j] == 6 || rellenos [i, j - 1] == 6 || rellenos [i, j + 1] == 6) {
							color2 = true;
						}
					}
					if (color1) {
						siono = true;
						if (color2) {
							if (i % 2 == j % 2) {
								tab [i, j] = 5;
							} else {
								tab [i, j] = 6;
							}
						} else {
							tab [i, j] = 5;
						}
					} else if (color2) {
						siono = true;
						tab [i, j] = 6;
					}


				}
		} while(siono);

		int p1 = 0;
		int p2 = 0;
		for (int i = 0; i < this.ancho * tamanyoCasilla; i++)
			for (int j = 0; j < this.alto * tamanyoCasilla; j++) {
				if (tab [i, j] == color) {
					p1++;
				}else if(tab [i, j] > 4){
					p2++;
				}
			}
		return p1 - p2;
	}

	public void jugada(int[,] tab, int x, int y, int color){
		tab [tamanyoCasilla/2  + x * tamanyoCasilla, tamanyoCasilla/2  + y * tamanyoCasilla] = color;
		tab [tamanyoCasilla/2-1  + x * tamanyoCasilla, tamanyoCasilla/2  + y * tamanyoCasilla] = color;
		tab [tamanyoCasilla/2  + x * tamanyoCasilla, tamanyoCasilla/2-1  + y * tamanyoCasilla] = color;
		tab [tamanyoCasilla/2-1  + x * tamanyoCasilla, tamanyoCasilla/2-1  + y * tamanyoCasilla] = color;
	}

	/*public int calcularPuntos(int tab[][],int min,int max,int c,int col,int cont){
		
		int n,a,b,puntos,mejpunt;
		int taux[][]=new int[8][8];
		for(a=0;a<8;a++)
			System.arraycopy(tab[a], 0, taux[a], 0, 8);
		int jug[]=new int[100];
		int movi[][]=new int[8][8];
		if(cont<=0)
			return calcularPuntos2(tab,min,max,c,col,1);
		if(c==col)
			mejpunt=min;
		else
			mejpunt=max;
		n=movimientosPosibles(tab,jug,c);
		ordenarJugadas(n,jug,tab,c);
		for(a=0;a<n;a++){
			calcularMovimientosPieza(taux,movi,jug[a]/1000,jug[a]%1000/100,c);
			moverPieza(taux,movi,jug[a]%100/10,jug[a]%10);
			if(tab[jug[a]%100/10][jug[a]%10]!=0){
				ultx=jug[a]%100/10;
				ulty=jug[a]%10;
			}else{
				ultx=100;
				ulty=100;
			}

			puntos=calcularPuntos(taux,min,max,cambiarColor(c),col,cont-1);
			for(b=0;b<8;b++)
				System.arraycopy(tab[b], 0, taux[b], 0, 8);
			
			iniciarMovimientos(movi);
			if (c==col&&puntos>min){
				if(puntos>=max)
					return max;
				min=puntos;
				mejpunt=puntos;
			}else if(c!=col&&puntos<max){
				if(puntos<=min)
					return min;
				max=puntos;
				mejpunt=puntos;
			}
		}
		return mejpunt;
	}
	public void inteligenciaArtificial(int tab[][],int col){
		
		boolean ko=false;
		int n,a,b,puntos,min=-100001,max=100001,cont=3,fin,ran,m=0,mj=-1002;
		int taux[][]=new int[8][8];
		int jug[]=new int[100];
		int jugfin[]=new int[100];
		int movi[][]=new int[8][8];
		for(a=0;a<8;a++)
			System.arraycopy(tab[a], 0, taux[a], 0, 8);
		n=movimientosPosibles(tab,jug,col);
		ordenarJugadas(n,jug,tab,col);
		jugfin[0]=jug[0];
		for(int i=0;i<n&&!ko;i++){
			calcularMovimientosPieza(taux,movi,jug[i]/1000,jug[i]%1000/100,col);
			moverPieza(taux,movi,jug[i]%100/10,jug[i]%10);
			fin=calcularFinal(taux,col);
			if(fin==1){
				ko=true;
				jugfin[m]=jug[i];
				m++;
			}
			for(b=0;b<8;b++)
				System.arraycopy(tab[b], 0, taux[b], 0, 8);
			
			iniciarMovimientos(movi);
		}
		for(a=0;a<n&&!ko;a++){
			calcularMovimientosPieza(taux,movi,jug[a]/1000,jug[a]%1000/100,col);
			moverPieza(taux,movi,jug[a]%100/10,jug[a]%10);
			puntos=calcularPuntos(taux,min,max,cambiarColor(col),col,cont);
			for(b=0;b<8;b++)
				System.arraycopy(tab[b], 0, taux[b], 0, 8);
			
			iniciarMovimientos(movi);
			if (puntos>mj){
				mj=puntos;
				m=0;
				jugfin[m]=jug[a];
				m++;
				min=puntos;
			}
			if(puntos==100001){
				ko=true;
			}
		}
		ran=(int)(Math.random()*m);
		calcularMovimientosPieza(tab,movi,jugfin[ran]/1000,jugfin[ran]%1000/100,col);
		moverPieza(tab,movi,jugfin[ran]%100/10,jugfin[ran]%10);
		fin=calcularFinal(tab,col);
		if (fin==0){
			iniciarMovimientos(movimientos);
			color=cambiarColor(color);
			nEstado=3;
			repaint();
		}else if(fin==1){
			nEstado=3;
			repaint();
			JOptionPane.showMessageDialog(null, "Jaque Mate!!");
			nEstado=0;
			repaint();
		}else{
			nEstado=3;
			repaint();
			JOptionPane.showMessageDialog(null, "Tablas");
			nEstado=0;
			repaint();
		}
	}



	/*
	 * 
	public void IA(int color){
		int[,] jugadasPosibles= new int[ancho, alto];
		int i, j, f, c;
		int colorcontrario = 5;
		if (color == 5) {
			colorcontrario = 6;
		}
		int cuenta = 0;
		for (i = 0; i < ancho; i++)
			for (j = 0; j < alto; j++) {
				if (laberinto [i, j] != -1 && jugadas [i, j] == 0) {
					for (f = 0; f < ancho; f++)
						for (c = 0; c < alto; c++) {
							jugadasAux [f, c] = jugadas [f, c];
						}
					jugadasAux [i, j] = color;
					recursiveCount2 (color, colorcontrario);
					cuenta = 0;
					for (f = 0; f < ancho; f++)
						for (c = 0; c < alto; c++) {
							if (jugadasAux [f, c] == color) {
								cuenta++;
							}
						}
					jugadasPosibles [i, j] = cuenta;
				}

			}
	}

	void recursiveCount2(int color,int colorContrario){
		bool encontrado1 = false;
		bool encontrado2 = false;
		for (int f = 0; f < ancho; f++)
			for (int c = 0; c < alto; c++) {
				jugadasAux2 [f, c] = jugadasAux [f, c];
			}
		int valor;
		for (int i = 0; i < ancho; i++)
			for (int j = 0; j < alto; j++) {
				if (laberinto [i, j] != -1 && jugadasAux [i, j] == 0) {
					valor = laberinto [i, j];
					if (valor % 10 == 0 && jugadasAux2 [i - 1, j] != 0) {

						if (jugadasAux2 [i - 1, j] == color) {
							encontrado1 = true;
						} else {
							encontrado2 = true;
						}
					}
					valor = valor / 10;
					if (valor % 10 == 0 && jugadasAux2 [i, j - 1] != 0) {
						if (jugadasAux2 [i, j - 1] == color) {
							encontrado1 = true;
						} else {
							encontrado2 = true;
						}
					}

					valor = valor / 10;
					if (valor % 10 == 0 && jugadasAux2 [i + 1, j] != 0) {
						if (jugadasAux2 [i + 1, j] == color) {
							encontrado1 = true;
						} else {
							encontrado2 = true;
						}
					}

					valor = valor / 10;
					if (valor % 10 == 0 && jugadasAux2 [i, j + 1] != 0) {
						if (jugadasAux2 [i, j + 1] == color) {
							encontrado1 = true;
						} else {
							encontrado2 = true;
						}
					}
					if (encontrado1) {
						jugadasAux [i, j] = color;
					} else if (encontrado2) {
						jugadasAux [i, j] = colorContrario;
					}
				}
			}

		if (encontrado1 || encontrado2) {
			recursiveCount2 (color, colorContrario);
		}
	}

	void recursiveCount(){
		puntosRecursivos++;
		int valor;
		bool encontrado = false;
		bool[,] laberintoBoolAux = new bool[ancho, alto];
		for (int i = 0; i < ancho; i++)
			for (int j = 0; j < alto; j++) {
				laberintoBoolAux[i,j] = laberintoBool[i,j];
			}

		for (int i = 0; i < ancho; i++)
			for (int j = 0; j < alto; j++) {
				if (laberinto [i, j] != -1 && !laberintoBoolAux [i, j]) {
					valor = laberinto [i, j];
					if (valor % 10 == 0 && laberintoBoolAux[i - 1, j]) {
						laberintoBool [i, j] = true;
						encontrado = true;
					}
					valor = valor / 10;
					if (valor % 10 == 0 && laberintoBoolAux[i, j - 1]) {
						laberintoBool [i, j] = true;
						encontrado = true;
					}
					valor = valor / 10;
					if (valor % 10 == 0 && laberintoBoolAux[i + 1, j]) {
						laberintoBool [i, j] = true;
						encontrado = true;
					}
					valor = valor / 10;
					if (valor % 10 == 0 && laberintoBoolAux[i, j + 1]) {
						laberintoBool [i, j] = true;
						encontrado = true;
					}
				}

			}
		if (encontrado) {
			recursiveCount ();
		}
	}*/
}
