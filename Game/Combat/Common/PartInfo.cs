using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class PartInfo
{
    public int SortingOrder;
    public Transform Transform => BoneFollower.transform;
    public BoneFollower BoneFollower;
}
