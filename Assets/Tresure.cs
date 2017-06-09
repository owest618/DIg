using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dig/Create Tresure")]
public class Tresure:ScriptableObject
{
    [SerializeField]
    Texture2D TresureTex;
    Sprite[] Slicedsprite;
    public int ID;
    [HideInInspector]
    public int w, h;
    [HideInInspector]
    public int[] shape;

    private void OnEnable()
    {
        Slicedsprite = null;
    }

    public Sprite[] Slice()
    {
        if (Slicedsprite==null)
        {
            Slicedsprite = new Sprite[w * h];
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    Vector2 slisesiz = new Vector2(TresureTex.width / w, TresureTex.height / h);
                    Sprite s = Sprite.Create(TresureTex, new Rect(slisesiz.x * j, TresureTex.height- slisesiz.y * (i+1), slisesiz.x, slisesiz.y), new Vector2(0.5f, 0.5f), slisesiz.x);
                    Slicedsprite[i * w + j] = s;
                }
            }
        }
        return Slicedsprite;
    }
}