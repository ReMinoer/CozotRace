using UnityEngine;

public class TerrainTexture : MonoBehaviour
{
    public int SurfaceIndex;
    private Terrain _terrain;
    private TerrainData _terrainData;
    private Vector3 _terrainPos;

    public int GetMainTexture(Vector3 worldPos)
    {
        if (_terrain == null)
            return -1;

        float[] mix = GetTextureMix(worldPos);

        float maxMix = 0;
        int maxIndex = 0;

        for (int n = 0; n < mix.Length; n++)
            if (mix[n] > maxMix)
            {
                maxIndex = n;
                maxMix = mix[n];
            }

        return maxIndex;
    }

    private void Start()
    {
        _terrain = Terrain.activeTerrain;
        if (_terrain != null)
        {
            _terrainData = _terrain.terrainData;
            _terrainPos = _terrain.transform.position;
        }
    }

    private void Update()
    {
        SurfaceIndex = GetMainTexture(transform.position);
    }

    /*
	void OnGUI() {
		GUI.Box (new Rect (100, 100, 200, 25), "index : " + surfaceIndex.ToString () + ", name : " + terrainData.splatPrototypes [surfaceIndex].texture.name);
	}
    */

    private float[] GetTextureMix(Vector3 worldPos)
    {
        if (_terrain == null)
            return new float[0];

        int mapX = (int)(((worldPos.x - _terrainPos.x) / _terrainData.size.x) * _terrainData.alphamapWidth);
        int mapZ = (int)(((worldPos.z - _terrainPos.z) / _terrainData.size.z) * _terrainData.alphamapHeight);

        float[,,] splatmapData = _terrainData.GetAlphamaps(mapX, mapZ, 1, 1);
        var cellMix = new float[splatmapData.GetUpperBound(2) + 1];

        for (int n = 0; n < cellMix.Length; n++)
            cellMix[n] = splatmapData[0, 0, n];

        return cellMix;
    }
}