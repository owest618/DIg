using UnityEngine;
using System.Collections;

public class StageGenerater : MonoBehaviour {
    [SerializeField]
    GameObject[] terraTile;

    [SerializeField]
    GameObject[] objTile ;
    [SerializeField]
    Object mapDirectory;
    [SerializeField]
    GameObject tileParent;

    string mapname = "test";
    // Use this for initialization
    void Start () {
        generate(1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void generate(int stagenumber)
    {
        GameObject tile;
        Terra[,] terramap = CSVDataReader.DataToTerra(CSVDataReader.readCSVData("/Map/" + mapname + stagenumber + "_terra.csv"));
        MovingObject[,] objmap = CSVDataReader.DataToObj(CSVDataReader.readCSVData("/Map/" + mapname + stagenumber + "_obj.csv"));
        Debug.Log(terramap[0, 0]);
        for (int i=0;i<terramap.GetLength(0);i++)
        {
            for (int j = 0; j < terramap.GetLength(1); j++)
            {
                if (terraTile[(int)terramap[i, j]] != null)
                {
                    tile = (GameObject)Instantiate(terraTile[(int)terramap[i, j]], new Vector3(j, -i, 0), Quaternion.identity);
                    tile.transform.parent = tileParent.transform;
                }
            }
        }
        for (int i = 0; i < objmap.GetLength(0); i++)
        {
            for (int j = 0; j < objmap.GetLength(1); j++)
            {
                if (terraTile[(int)objmap[i, j]] != null)
                {
                    tile = (GameObject)Instantiate(objTile[(int)objmap[i, j]], new Vector3(j, -i, 0), Quaternion.identity);
                    tile.transform.parent = tileParent.transform;
                }
            }
        }
    }
}
