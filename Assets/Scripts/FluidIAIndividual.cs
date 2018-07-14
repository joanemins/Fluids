using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
