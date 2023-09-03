//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

////TilemapManager에 customEditor 속성을 추가함
////이렇게 해야지 inspector에서 조절 가능
//[CustomEditor(typeof(TilemapManager))]
//public class TilemapManagerEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        //기본 inspector 필드들 생성 및 커스텀 요소도 추가
//        DrawDefaultInspector();

//        //TilemapManager의 컴포넌트 긁어오기
//        var script = (TilemapManager)target;

//        //inspector창에 Save Map 버튼 추가 및 TilemapManager에 있는 각각 함수 실행
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
