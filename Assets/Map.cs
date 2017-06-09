using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map  {
    const int mapSize=50;
    class cell
    {
        public int x, y;
        public int dartID;
        public int tresureID;
        public Sprite wallSprite;
        public GameObject dartObject;
        public tresureInstance instance;
        public cell(int x,int y)
        {
            this.x = x;
            this.y = y;
            dartID = 0;
            tresureID = 0;
        }
    }

    class tresureInstance
    {
        public Tresure data;
        public List<cell> cellList;
        public tresureInstance(Tresure t)
        {
            data = t;
            cellList = new List<cell>();
        }
    }

    cell[,] tresureMap;
    GameObject dartparent;

    public void generate()
    {
        tresureMap = new cell[mapSize, mapSize];
        for(int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                tresureMap[i, j] = new cell(j,i);
            }
        }
        dartObjectGenerate();
        setTileSprite();
        generateTresure();
    }

    public void Dig(int x, int y)
    {
        tresureMap[y, x].dartObject.GetComponent<SpriteRenderer>().sprite = tresureMap[y, x].wallSprite;
    }

    private void dartObjectGenerate()
    {
        GameObject dartprehub = Resources.Load<GameObject>("Tilecip/TilePrehub");
        if (dartparent == null)
            dartparent = new GameObject("DartParent");
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                tresureMap[i, j].dartObject = GameObject.Instantiate(dartprehub, new Vector3(j, -i, 0f),Quaternion.identity);
                tresureMap[i, j].dartObject.transform.parent = dartparent.transform;
            }
        }
    }

    private void generateTresure()
    {
        TresureList list = Resources.Load<TresureList>("Tresure/TresureList");
        for (int i = 0; i < list.DataList.Count; i++)
            list.DataList[i].ID = i;
        setTresureTile(0, 1, list.DataList[0]);
    }

    private void setTresureTile(int x,int y,Tresure tresure)
    {
        tresureInstance ins = new tresureInstance(tresure);
        for (int i = 0; i < tresure.h*tresure.w; i++)
        {
            tresureMap[i / tresure.w+y, i % tresure.w+x].wallSprite = tresure.Slice()[i];
            tresureMap[i / tresure.w + y, i % tresure.w + x].instance = ins;
            tresureMap[i / tresure.w + y, i % tresure.w + x].tresureID = tresure.ID;
            ins.cellList.Add(tresureMap[i / tresure.w + y, i % tresure.w + x]);
        }
    }

    private void setTileSprite()
    {
        Sprite darttile = Resources.Load<Sprite>("Tilecip/dart");
        Sprite walltile = Resources.Load<Sprite>("Tilecip/wall");
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                tresureMap[i, j].dartObject.GetComponent<SpriteRenderer>().sprite=darttile;
                tresureMap[i, j].wallSprite = walltile;
            }
        }
    }
}
