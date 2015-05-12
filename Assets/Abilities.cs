using UnityEngine;
using System.Collections;
using Cubiquity;
public class Abilities : MonoBehaviour 
{
    private TerrainVolume terrainVolume;
    private int abilite = 3;
    private int range;
    [SerializeField]
    private GameObject[] items;
    public Transform cube;
	void Start () 
    {
        range = 2;
        terrainVolume = gameObject.GetComponent<TerrainVolume>();
        if (terrainVolume == null)
        {
            Debug.LogError("This 'ClickToCarveTerrainVolume' script should be attached to a game object with a TerrainVolume component");
        }
	}
    void Update ()
    {
        if(Input.GetMouseButton(0))
        {
            Debug.Log("hi");
            UseAbilite(cube.position);
        }
    }
    void UseAbilite(Vector3 digWhere) 
    {
        Ray ray = new Ray(Camera.main.transform.position, digWhere - Camera.main.transform.position);//Camera.main.ScreenPointToRay(new Vector3(digWhere.x, digWhere.y, 0));
        // Perform the raycasting.
        PickSurfaceResult pickResult;
        bool hit = Picking.PickSurface(terrainVolume, ray, 1000.0f, out pickResult);

        int xPos = (int)pickResult.volumeSpacePos.x;
        int yPos = (int)pickResult.volumeSpacePos.y;
        int zPos = (int)pickResult.volumeSpacePos.z;

        int rangeSquared = range * range;
        if (hit)
        {
            switch(abilite)
            {
                //build
                case 1:
                    

                    for (int z = zPos - range; z < zPos + range; z++)
                    {
                        for (int y = yPos - range; y < yPos + range; y++)
                        {
                            for (int x = xPos - range; x < xPos + range; x++)
                            {
                                int xDistance = x + xPos;
                                int yDistance = y + yPos;
                                int zDistance = z + zPos;

                                float noiseScale = 32.0f;
                                float invNoiseScale = 1.0f / noiseScale;

                                float sampleX = (float)xPos * invNoiseScale;
                                float sampleY = (float)yPos * invNoiseScale;
                                float sampleZ = (float)zPos * invNoiseScale;
                                MaterialSet materialSet = new MaterialSet();
                                materialSet.weights[0] = (byte)255;


                                int distSquared = xDistance * xDistance + yDistance * yDistance + zDistance * zDistance;

                                if (distSquared > rangeSquared)
                                {
                                    terrainVolume.data.SetVoxel(x, y, z, materialSet);
                                }
                            }
                        }
                    }
                    break;
                case 2:
                    MaterialSet emptyMaterialSet = new MaterialSet();

                    for (int z = zPos - range; z < zPos + range; z++)
                    {
                        for (int y = yPos - range; y < yPos + range; y++)
                        {
                            for (int x = xPos - range; x < xPos + range; x++)
                            {
                                int xDistance = x - xPos;
                                int yDistance = y - yPos;
                                int zDistance = z - zPos;
                                int distSquared = xDistance * xDistance + yDistance * yDistance + zDistance * zDistance;

                                if (distSquared < rangeSquared)
                                {
                                    terrainVolume.data.SetVoxel(x, y, z, emptyMaterialSet);
                                }
                            }
                        }
                    }

                    range += 2;

                    TerrainVolumeEditor.BlurTerrainVolume(terrainVolume, new Region(xPos - range, yPos - range, zPos - range, xPos + range, yPos + range, zPos + range));
                    range -= 2;
                    break;
                case 3:
                    Vector3 pos = new Vector3(xPos,yPos,zPos);
                    Vector3 heading = (pos - transform.position);
                    heading = heading.normalized;
                    Quaternion rot = Quaternion.LookRotation(heading);
                    Instantiate(items[0], pos, rot);
                    break;
                default:
                    Debug.Log("no abilite");
                    break;
            }
        }
	}
}
