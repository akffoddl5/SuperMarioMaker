using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Obj", menuName = "ScriptableObject/ScriptObj")]

//��ũ��Ʈ ������Ʈȭ
public class ScriptableMapInfo : ScriptableObject
{
    public int levelIndex;
    public int backgroundNum;
    public float timerCount;
    public int playerLifePoint;
    public Vector3 playerStartPos;
    public int mapScaleNum;

    public List<CreateObjectInfo> createObjectInfoList = new List<CreateObjectInfo>();

    [Serializable]
    public class CreateObjectInfo
    {
        public string objectName;
        public Vector3 createPos;
        public int dirInfo; //(�� : 0, ���� : 1, �� : 2, �Ʒ� : 3)
        public Vector3 pipeLinkPos;
        public List<int> brickListInfo;

        public CreateObjectInfo() { }

        public CreateObjectInfo(string _objectName, Vector3 _createPos,
            Vector3 _pipeLinkPos, int _dirInfo = 0, List<int> _brickListInfo = null)
        {
            objectName = _objectName;
            createPos = _createPos;
            pipeLinkPos = _pipeLinkPos;
            dirInfo = _dirInfo;
            brickListInfo = _brickListInfo;
        }
    }
}