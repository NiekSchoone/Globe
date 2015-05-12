using UnityEngine;
using System.Collections;

using Cubiquity;

public class Dig : MonoBehaviour {

    private TerrainVolume terrainVolume;
    public Transform cube;
    // Bit of a hack - we want to detect mouse clicks rather than the mouse simply being down,
    // but we can't use OnMouseDown because the voxel terrain doesn't have a collider (the
    // individual pieces do, but not the parent). So we define a click as the mouse being down
    // but not being down on the previous frame. We'll fix this better in the future...

    // Use this for initialization
    void Start()
    {
        // We'll store a reference to the colored cubes volume so we can interact with it later.
        terrainVolume = gameObject.GetComponent<TerrainVolume>();
        if (terrainVolume == null)
        {
            Debug.LogError("This 'ClickToCarveTerrainVolume' script should be attached to a game object with a TerrainVolume component");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Bail out if we're not attached to a terrain.
        if (terrainVolume == null)
        {
            return;
        }

        // If the mouse btton is down and it was not down last frame
        // then we consider this a click, and do our destruction.
        if(Input.GetMouseButton(0))
        {
            placeblock(Input.mousePosition.x,Input.mousePosition.y,Input.mousePosition.z);
        }
    }
    public void DigFunction(Vector3 digWhere) 
    {
        Ray ray = new Ray(Camera.main.transform.position, digWhere - Camera.main.transform.position);//Camera.main.ScreenPointToRay(new Vector3(digWhere.x, digWhere.y, 0));
        // Perform the raycasting.
        PickSurfaceResult pickResult;
        bool hit = Picking.PickSurface(terrainVolume, ray, 1000.0f, out pickResult);
        // If we hit a solid voxel then create an explosion at this point.
        if (hit)
        {
            
            int range = 2;
            DestroyVoxels((int)pickResult.volumeSpacePos.x, (int)pickResult.volumeSpacePos.y, (int)pickResult.volumeSpacePos.z, range);
        }
    }
    public void Build(Vector3 buildWhere) 
    {
        Ray ray = new Ray(Camera.main.transform.position, buildWhere - Camera.main.transform.position);//Camera.main.ScreenPointToRay(new Vector3(buildWhere.x, buildWhere.y, 0));

        // Perform the raycasting.
        PickSurfaceResult pickResult;
        bool hit = Picking.PickSurface(terrainVolume, ray, 1000.0f, out pickResult);

        // If we hit a solid voxel then create an explosion at this point.
        if (hit)
        {
            
            int range = 2;
            AddVoxels((int)pickResult.volumeSpacePos.x, (int)pickResult.volumeSpacePos.y, (int)pickResult.volumeSpacePos.z, range);
        }
    }
    void AddVoxels(int xPos, int yPos, int zPos, int range = 2)
    {
        
        // Initialise outside the loop, but we'll use it later.
        int rangeSquared = range * range;

        // Iterage over every voxel in a cubic region defined by the received position (the center) and
        // the range. It is quite possible that this will be hundreds or even thousands of voxels.
        for (int z = zPos - range; z < zPos + range; z++)
        {
            for (int y = yPos - range; y < yPos + range; y++)
            {
                for (int x = xPos - range; x < xPos + range; x++)
                {
                    // Compute the distance from the current voxel to the center of our explosion.
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

                    // Working with squared distances avoids costly square root operations.
                    int distSquared = xDistance * xDistance + yDistance * yDistance + zDistance * zDistance;

                    // We're iterating over a cubic region, but we want our explosion to be spherical. Therefore 
                    // we only further consider voxels which are within the required range of our explosion center. 
                    // The corners of the cubic region we are iterating over will fail the following test.
                    if (distSquared > rangeSquared)
                    {
                        terrainVolume.data.SetVoxel(x, y, z, materialSet);
                    }
                }
            }
        }
    }
    void DestroyVoxels(int xPos, int yPos, int zPos, int range = 2)
    {
        // Initialise outside the loop, but we'll use it later.
        int rangeSquared = range * range;
        MaterialSet emptyMaterialSet = new MaterialSet();

        // Iterage over every voxel in a cubic region defined by the received position (the center) and
        // the range. It is quite possible that this will be hundreds or even thousands of voxels.
        for (int z = zPos - range; z < zPos + range; z++)
        {
            for (int y = yPos - range; y < yPos + range; y++)
            {
                for (int x = xPos - range; x < xPos + range; x++)
                {
                    // Compute the distance from the current voxel to the center of our explosion.
                    int xDistance = x - xPos;
                    int yDistance = y - yPos;
                    int zDistance = z - zPos;

                    // Working with squared distances avoids costly square root operations.
                    int distSquared = xDistance * xDistance + yDistance * yDistance + zDistance * zDistance;

                    // We're iterating over a cubic region, but we want our explosion to be spherical. Therefore 
                    // we only further consider voxels which are within the required range of our explosion center. 
                    // The corners of the cubic region we are iterating over will fail the following test.
                    if (distSquared < rangeSquared)
                    {
                        terrainVolume.data.SetVoxel(x, y, z, emptyMaterialSet);
                    }
                }
            }
        }

        range += 2;

        TerrainVolumeEditor.BlurTerrainVolume(terrainVolume, new Region(xPos - range, yPos - range, zPos - range, xPos + range, yPos + range, zPos + range));
        //TerrainVolumeEditor.BlurTerrainVolume(terrainVolume, new Region(xPos - range, yPos - range, zPos - range, xPos + range, yPos + range, zPos + range));
        //TerrainVolumeEditor.BlurTerrainVolume(terrainVolume, new Region(xPos - range, yPos - range, zPos - range, xPos + range, yPos + range, zPos + range));
    }
    void placeblock(float xPos, float yPos, float zPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(xPos, yPos, 0));

        // Perform the raycasting.
        PickSurfaceResult pickResult;
        bool hit = Picking.PickSurface(terrainVolume, ray, 1000.0f, out pickResult);

        if (hit)
        {
            Vector3 pos = new Vector3((int)pickResult.worldSpacePos.x, (int)pickResult.worldSpacePos.y, (int)pickResult.worldSpacePos.z);

            Instantiate(cube, pos, transform.rotation);
        }
    }
}
