using UnityEngine;
using System.Collections;

public class Map : DesignPattern.Singleton<Map>
{
	public GroundProperty[] Grounds;
	
	[System.Serializable]
	public class GroundProperty
	{
		public Texture Texture;
		public float SpeedCoeff;
	}
}
