using UnityEngine;
using System.Collections;
using Cubiquity;
public class Abilities : MonoBehaviour 
{
    private TerrainVolume terrainVolume;
    private int Abilite;
    private int range;
	void Start () 
    {
        range = 2;
        terrainVolume = gameObject.GetComponent<TerrainVolume>();
        if (terrainVolume == null)
        {
            Debug.LogError("This 'ClickToCarveTerrainVolume' script should be attached to a game object with a TerrainVolume component");
        }
	}
    void UseAbilite(Vector3 digWhere) 
    {
        Ray ray = new Ray(Camera.main.transform.position, digWhere - Camera.main.transform.position);//Camera.main.ScreenPointToRay(new Vector3(digWhere.x, digWhere.y, 0));
        // Perform the raycasting.
        PickSurfaceResult pickResult;
        bool hit = Picking.PickSurface(terrainVolume, ray, 1000.0f, out pickResult);
        if (hit)
        {
            Vector3 pos = new Vector3(pickResult.volumeSpacePos.x, pickResult.volumeSpacePos.y, pickResult.volumeSpacePos.z);
            switch(Abilite)
            {
                case 1:
                    Debug.Log("1");
                    break;
                case 2:
                    Debug.Log("2");
                    break;
                default:
                    Debug.Log("no abilite");
                    break;
            }
        }
	}
}
