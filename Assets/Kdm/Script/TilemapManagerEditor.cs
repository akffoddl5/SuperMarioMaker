//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

////TilemapManager�� customEditor �Ӽ��� �߰���
////�̷��� �ؾ��� inspector���� ���� ����
//[CustomEditor(typeof(TilemapManager))]
//public class TilemapManagerEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        //�⺻ inspector �ʵ�� ���� �� Ŀ���� ��ҵ� �߰�
//        DrawDefaultInspector();

//        //TilemapManager�� ������Ʈ �ܾ����
//        var script = (TilemapManager)target;

//        //inspectorâ�� Save Map ��ư �߰� �� TilemapManager�� �ִ� ���� �Լ� ����
//        if(GUILayout.Button("Save Map"))
//        {
//            script.SaveMap();
//        }

//        if (GUILayout.Button("Clear Map"))
//        {
//            script.ClearMap();
//        }

//        if (GUILayout.Button("Load Map"))
//        {
//            script.LoadMap();
//        }
//    }
//}
