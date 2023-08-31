using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��ũ��Ʈ ������Ʈȭ
public class ScriptableMapInfo : ScriptableObject
{
    public int backgroundNum;
    public float timerCount;
    public int playerLifePoint;
    public Vector3 playerStartPos;
    public int mapScaleNum;
    public class CreateObjectInfo
    {
        public string objectName;
        public Vector3 createPos;
        public Vector3 pipeLinkPos;
        public int dirInfo; //(�� : 0, ���� : 1, �� : 2, �Ʒ� : 3)
        public List<int> brickListInfo;

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
    public List<CreateObjectInfo> createObjectInfoList;

}