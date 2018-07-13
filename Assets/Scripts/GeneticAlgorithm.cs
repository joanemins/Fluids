using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class GeneticAlgorithm : MonoBehaviour {
	FluidIAIndividual[] poblacion;
	double probabilidad_mutacion;
	double probabilidad_combinacion;
	FluidIAIndividual MejorIA;


	public GeneticAlgorithm ()
	{
		Random random = new Random ();
		GenerarPoblacion ();
		for(int j=0;j<30;j++){
			SeleccionarPoblacion(poblacion);
			for(int i=0;i<100;i++){
				if(random.NextDouble() <= probabilidad_combinacion){
					Combinar(poblacion[i],poblacion[random.Next(0,i)]);
				}
				if(random.NextDouble() <= probabilidad_mutacion){
					Mutacion(poblacion[i]);
				}
			}
		}
		MejorIA = SeleccionFinal (poblacion);

		string filename = "ResultadoIA";
		if (!File.Exists (filename)) {
			using (StreamWriter sw = File.CreateText (filename)) {
				sw.WriteLine ("distancia 1 = " + MejorIA.k_distancia1.ToString());
				sw.WriteLine ("distancia 2 = " + MejorIA.k_distancia2.ToString());
				sw.WriteLine ("distancia 3 = " + MejorIA.k_distancia3.ToString());
				sw.WriteLine ("diferencia para ganar = " + MejorIA.diferencia_para_ganar.ToString());
			}
		}

	}
	public void GenerarPoblacion(){
		Random random = new Random ();
		for(int i=0;i<100;i++){
			poblacion [i].k_distancia1 = random.NextDouble();
			poblacion [i].k_distancia2 = random.NextDouble();
			poblacion [i].k_distancia3 = random.NextDouble();
			poblacion [i].diferencia_para_ganar = random.NextDouble ()*100;
		}
	}
	public FluidIAIndividual EvaluarIndividuos(FluidIAIndividual individuo1, FluidIAIndividual individuo2){
		//simulamos partida entre los dos individuos y devolvemos al ganador.

	}

	public FluidIAIndividual SeleccionFinal(FluidIAIndividual[] poblacion){
		FluidIAIndividual ganador = poblacion [0];
		for (int i = 1; i < poblacion.Length (); i++) {
				ganador= EvaluarIndividuos(ganador,poblacion[i]);
		}
	}


	public void SeleccionarPoblacion(FluidIAIndividual[] poblacion){
		FluidIAIndividual[] ganadores;
		Random random = new Random ();
		for(int i=0;i<100;i++){
			ganadores [i] = EvaluarIndividuos (poblacion [random.Next (0, 100)], poblacion [random.Next (0, 100)]);
		}
	}


	public void Mutacion(FluidIAIndividual individuo){
		Random random = new Random ();
		int i= random.Next (0, 3); //elegimos un atributo al azar
		switch (i) {
		case 0:
			individuo.k_distancia1 = random.NextDouble ();
			break;
		case 1:
			individuo.k_distancia2 = random.NextDouble ();
			break;
		case 2:
			individuo.k_distancia3 = random.NextDouble ();
			break;
		case 3:
			individuo.diferencia_para_ganar = random.NextDouble()*100;
			break;
		} 
	}


	public void Combinar(FluidIAIndividual individuo1,FluidIAIndividual individuo2){
		Random random = new Random ();
		int j= random.Next (1, 3); // elegimos punto de corte al azar
		double aux1;
		switch (j) {
		case 1:
			aux1 = individuo1.k_distancia2;
			individuo1.k_distancia2 = individuo2.k_distancia2;
			individuo2.k_distancia2 = aux1;

			aux1 = individuo1.k_distancia3;
			individuo1.k_distancia3 = individuo2.k_distancia3;
			individuo2.k_distancia3 = aux1;

			aux2 = individuo1.diferencia_para_ganar;
			individuo1.diferencia_para_ganar = individuo2.diferencia_para_ganar;
			individuo2.diferencia_para_ganar = aux2;
			break;
		case 2:
			aux1 = individuo1.k_distancia3;
			individuo1.k_distancia3 = individuo2.k_distancia3;
			individuo2.k_distancia3 = aux1;

			aux2 = individuo1.diferencia_para_ganar;
			individuo1.diferencia_para_ganar = individuo2.diferencia_para_ganar;
			individuo2.diferencia_para_ganar = aux2;
			break;
		case 3:
			aux2 = individuo1.diferencia_para_ganar;
			individuo1.diferencia_para_ganar = individuo2.diferencia_para_ganar;
			individuo2.diferencia_para_ganar = aux2;
			break;
		}

	}
}
public class FluidIAIndividual : MonoBehaviour {

	public double k_distancia1,k_distancia2,k_distancia3,diferencia_para_ganar;
	public FluidIAIndividual (double k_distancia1,double k_distancia2,double k_distancia3,double dif)
	{
		this.k_distancia1 = k_distancia1;
		this.k_distancia2 = k_distancia2;
		this.k_distancia3 = k_distancia3;
		this.diferencia_para_ganar = dif;
	}
}