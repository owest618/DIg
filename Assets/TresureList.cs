using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Dig/Create TresureList")]
public class TresureList:ScriptableObject {
    [SerializeField]
    public List<Tresure> DataList;
}
