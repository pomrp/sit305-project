using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//磚塊編輯器  by 陳間時光 v0.7
//BrickEditor by MorningFungame v0.7
public class BrickEditor : EditorWindow
    {
    float gridLength_X = 1, gridLength_Y = 1;
    bool gridLengthEditable=true;

    public Object [ ] OverLapping_A, OverLapping_B;

    string includingTag = "";
    string excludingTag = "";
    string selectingByTag = "";

    string gameObjectName = "";
    int startNumber = 1;

    int MoveOrCopyThenMove_option = 0;
    string [ ] MoveOrCopythenMove_text = new string [ ] { "移動" , "複製" };


    enum alignMode
        { includingTag, excludingTag, All, None }

    alignMode currentAlignMode ( )
        {
        switch ( includeOrExculde_option )
            {
            case 0:
                return alignMode.includingTag;
            case 1:
                return alignMode.excludingTag;
            case 2:
                return alignMode.All;
            default:
                return alignMode.None;
            }

        }

    bool autoAlign = false;
    bool moveMode ( )
        {
        if ( MoveOrCopyThenMove_option == 0 )
            return true;
        else
            return false;
        }

    static string [ ] includeOrExclude_string = new string [ ] { "包含此標籤：" , "排除此標籤：" , "對齊全部" };
    int includeOrExculde_option = 2;//預設對齊所有物件

    Vector2 scroll = Vector2.zero;

    void Awake ( )
        {
        //讀取前次設定
        gridLengthEditable = EditorPrefs.GetBool ( "gridLengthEditable" );
        gridLength_X = EditorPrefs.GetFloat ( "gridLength_X" );
        gridLength_Y = EditorPrefs.GetFloat ( "gridLength_Y" );

        autoAlign = EditorPrefs.GetBool ( "autoAlign" );
        includingTag = EditorPrefs.GetString ( "includingTag" );
        excludingTag = EditorPrefs.GetString ( "excludingTag" );
        selectingByTag = EditorPrefs.GetString ( "selectingByTag" );
        includeOrExculde_option = EditorPrefs.GetInt ( "includeOrExculde_option" , includeOrExculde_option );
        MoveOrCopyThenMove_option = EditorPrefs.GetInt ( "MoveOrCopyThenMove_option" , MoveOrCopyThenMove_option );

        gameObjectName = EditorPrefs.GetString ( "gameObjectName" );
        startNumber = EditorPrefs.GetInt ( "startNumber" , startNumber );
        }

    void OnGUI ( )
        {
        #region test
        int windowWidth = ( int )EditorGUIUtility.currentViewWidth;
        EditorGUILayout.LabelField ( "視窗寬度:" + windowWidth );
        //if ( GUILayout.Button ( "清空EditorPrefs" ) )
        //  EditorPrefs.DeleteAll ( );
        #endregion

        float gridLabelWidth = 50;
        float labelWidth = 60;
        float intWidth = 40;

        #region 儲存設定
        EditorPrefs.SetBool ( "gridLengthEditable" , gridLengthEditable );
        EditorPrefs.SetFloat ( "gridLength_X" , gridLength_X );
        EditorPrefs.SetFloat ( "gridLength_Y" , gridLength_Y );

        EditorPrefs.SetBool ( "autoAlign" , autoAlign );
        EditorPrefs.SetString ( "includingTag" , includingTag );
        EditorPrefs.SetString ( "excludingTag" , excludingTag );
        EditorPrefs.SetString ( "selectingByTag" , selectingByTag );
        EditorPrefs.SetInt ( "includeOrExculde_option" , includeOrExculde_option );
        EditorPrefs.SetInt ( "MoveOrCopyThenMove_option" , MoveOrCopyThenMove_option );

        EditorPrefs.SetString ( "gameObjectName" , gameObjectName );
        EditorPrefs.SetInt ( "startNumber" , startNumber );
        #endregion

        #region 格點的介面
        EditorGUILayout.LabelField ( "=== 格 點 ===" );

        EditorGUILayout.BeginHorizontal ( );
        EditorGUI.BeginDisabledGroup(gridLengthEditable);

        EditorGUILayout.LabelField ( "格點寬度" , GUILayout.Width ( gridLabelWidth ) );
        gridLength_X = EditorGUILayout.FloatField ( gridLength_X , GUILayout.Width ( intWidth ) );

        EditorGUILayout.LabelField ( "格點高度" , GUILayout.Width ( gridLabelWidth ) );
        gridLength_Y = EditorGUILayout.FloatField ( gridLength_Y , GUILayout.Width ( intWidth ) );
        EditorGUI.EndDisabledGroup ( );
        gridLengthEditable = EditorGUILayout.ToggleLeft ( "鎖定格點數據",gridLengthEditable );
        
        EditorGUILayout.EndHorizontal ( );
        #endregion

        EditorGUILayout.Separator ( );

        #region 自動對齊的介面

        EditorGUILayout.LabelField ( "=== 對齊 ===" );

        autoAlign = EditorGUILayout.BeginToggleGroup ( "啟動自動對齊" , autoAlign );
        EditorGUILayout.BeginHorizontal ( );

        includeOrExculde_option = GUILayout.SelectionGrid ( includeOrExculde_option , includeOrExclude_string , 1 , EditorStyles.radioButton , GUILayout.Width ( 120 ) );

        EditorGUILayout.BeginVertical ( );

        EditorGUILayout.Space ( );
        includingTag = EditorGUILayout.TagField ( includingTag );
        excludingTag = EditorGUILayout.TagField ( excludingTag );

        EditorGUILayout.EndHorizontal ( );
        EditorGUILayout.EndVertical ( );
        EditorGUILayout.EndToggleGroup ( );

        if ( GUILayout.Button ( "手動對齊目前選取的物件" ) )
            {
            snapEverything ( );            
            }
        #endregion

        EditorGUILayout.Separator ( );

        #region 跟選擇相關的介面
        EditorGUILayout.LabelField ( "=== 選擇物件  ===" );

        EditorGUILayout.BeginHorizontal ( );
        if ( GUILayout.Button ( "依指定標籤選擇：" ) )
            { Selection.objects = GameObject.FindGameObjectsWithTag ( selectingByTag ); }

        selectingByTag = EditorGUILayout.TagField ( selectingByTag );
        EditorGUILayout.EndHorizontal ( );


        if ( GUILayout.Button ( "選擇目前物件的同標籤物件" ) )
            Selection.objects = findSameTag ( );


        if ( GUILayout.Button ( "選擇目前物件的子物件" ) )
            if ( Selection.objects != null )
                Selection.objects = getChildren ( );

        #endregion

        EditorGUILayout.Separator ( );

        #region 移動或複製相關的介面

        EditorGUILayout.LabelField ( "=== 移動或複製 ===" );
        EditorGUILayout.BeginHorizontal ( );
        if ( GUILayout.Button ( "↖" ) )
            moveOrCopyThenMove ( selectedInSecne ( ) , new Vector3 ( -gridLength_X , gridLength_Y , 0 ) );

        if ( GUILayout.Button ( "↑" ) )
            moveOrCopyThenMove ( selectedInSecne ( ) , new Vector3 ( 0 , gridLength_Y , 0 ) );

        if ( GUILayout.Button ( "↗" ) )
            moveOrCopyThenMove ( selectedInSecne ( ) , new Vector3 ( gridLength_X , gridLength_Y , 0 ) );

        EditorGUILayout.EndHorizontal ( );

        EditorGUILayout.Space ( );

        EditorGUILayout.BeginHorizontal ( );
        if ( GUILayout.Button ( "←" ) )
            moveOrCopyThenMove ( selectedInSecne ( ) , new Vector3 ( -gridLength_X , 0 , 0 ) );

        MoveOrCopyThenMove_option = GUILayout.SelectionGrid ( MoveOrCopyThenMove_option , MoveOrCopythenMove_text , 2 , EditorStyles.radioButton , GUILayout.Width ( 90 ) );

        if ( GUILayout.Button ( "→" ) )
            moveOrCopyThenMove ( selectedInSecne ( ) , new Vector3 ( gridLength_X , 0 , 0 ) );

        EditorGUILayout.EndHorizontal ( );

        EditorGUILayout.Space ( );

        EditorGUILayout.BeginHorizontal ( );
        if ( GUILayout.Button ( "↙" ) )
            moveOrCopyThenMove ( selectedInSecne ( ) , new Vector3 ( -gridLength_X , -gridLength_Y , 0 ) );

        if ( GUILayout.Button ( "↓" ) )
            moveOrCopyThenMove ( selectedInSecne ( ) , new Vector3 ( 0 , -gridLength_Y , 0 ) );

        if ( GUILayout.Button ( "↘" ) )
            moveOrCopyThenMove ( selectedInSecne ( ) , new Vector3 ( gridLength_X , -gridLength_Y , 0 ) );

        EditorGUILayout.EndHorizontal ( );
        #endregion

        EditorGUILayout.Separator ( );

        #region 改檔名的介面
        EditorGUILayout.LabelField ( "=== 批次改名 ===" );
        EditorGUILayout.BeginHorizontal ( );
        EditorGUILayout.LabelField ( "前綴：" , GUILayout.Width ( labelWidth ) );

        gameObjectName = EditorGUILayout.TextField ( gameObjectName );

        EditorGUILayout.LabelField ( "起始編號：" , GUILayout.Width ( labelWidth ) );
        startNumber = EditorGUILayout.IntField ( startNumber , GUILayout.Width ( intWidth ) );

        if ( GUILayout.Button ( "重新命名" ) )
            reName ( );

        EditorGUILayout.EndHorizontal ( );
        #endregion

        EditorGUILayout.Separator ( );

        #region 檢查重疊相關的介面
        EditorGUILayout.LabelField ( "=== 檢查重疊 ===" );
        if ( GUILayout.Button ( "檢查選取的物件有無重疊" ) )
            {
            findPositionOverlapping ( );
            }

        EditorGUILayout.LabelField ( "重疊清單 ( A 與 B 兩兩重疊 )" );

        ScriptableObject brickEditor = this;
        SerializedObject SO_brickEditor = new SerializedObject ( brickEditor );
        SO_brickEditor.ApplyModifiedProperties ( );

        scroll = EditorGUILayout.BeginScrollView ( scroll );

        SerializedProperty SP_OverLapping_A = SO_brickEditor.FindProperty ( "OverLapping_A" );
        EditorGUILayout.PropertyField ( SP_OverLapping_A , true );
        SerializedProperty SP_OverLapping_B = SO_brickEditor.FindProperty ( "OverLapping_B" );
        EditorGUILayout.PropertyField ( SP_OverLapping_B , true );

        EditorGUILayout.EndScrollView ( );
        #endregion
        }

    void Update ( )
        {
        getGridPosition ( );
        }

    [MenuItem ( "陳間時光/磚塊編輯器" )]
    public static void Init ( )
        {
        BrickEditor brickEditor = GetWindow<BrickEditor> ( true , "磚塊編輯器" );
        brickEditor.minSize = new Vector2 ( 300 , 0 );
        }

    GameObject [ ] selectedInSecne ( )
        {
        if ( Selection.gameObjects != null )
            return Selection.gameObjects;
        else
            Debug.Log ( "沒有選取任何東西" );
        return null;
        }

    void getGridPosition ( )
        {
        if ( canSnap ( ) )
            {
            switch ( currentAlignMode ( ) )
                {
                case alignMode.includingTag:
                    snapByInclusion ( includingTag );
                    break;

                case alignMode.excludingTag:
                    snapByExclusion ( excludingTag );
                    break;

                case alignMode.All:
                    snapEverything ( );
                    break;

                default:
                    Debug.Log ( "未知的對齊狀態：" + currentAlignMode ( ) );
                    break;
                }
            }

        }

    void snapByInclusion ( string tag )
        {
        foreach ( var item in selectedInSecne ( ) )
            {
            if ( item.CompareTag ( tag ) )
                item.transform.localPosition = GetSnappedPosition ( item.transform.localPosition );
            }
        }

    void snapByExclusion ( string tag )
        {
        foreach ( var item in selectedInSecne ( ) )
            {
            if ( !item.CompareTag ( tag ) )
                item.transform.localPosition = GetSnappedPosition ( item.transform.localPosition );
            }
        }


    void snapEverything ( )
        {
        foreach ( var item in selectedInSecne ( ) )
            item.transform.localPosition = GetSnappedPosition ( item.transform.localPosition );
        }



    //檢查重疊物件
    void findPositionOverlapping ( )
        {
        List<Object> tempList_A = new List<Object> ( );
        List<Object> tempList_B = new List<Object> ( );
        for ( int A = 0; A <= selectedInSecne ( ).Length - 2; A++ )
            {
            for ( int B = A + 1; B <= selectedInSecne ( ).Length - 1; B++ )
                {
                if ( selectedInSecne ( ) [ A ].transform.position == selectedInSecne ( ) [ B ].transform.position )
                    {
                    tempList_A.Add ( selectedInSecne ( ) [ A ] );
                    tempList_B.Add ( selectedInSecne ( ) [ B ] );
                    }
                }
            }

        if ( tempList_A.Count == 0 && tempList_B.Count == 0 )
            {
            Debug.Log ( "沒有重疊的物件" );
            }
        OverLapping_A = tempList_A.ToArray ( );
        OverLapping_B = tempList_B.ToArray ( );
        Repaint ( );
        }

    GameObject [ ] findSameTag ( )
        {
        List<GameObject> gameObjectList = new List<GameObject> ( );
        List<string> tagList = new List<string> ( );

        foreach ( var g in selectedInSecne ( ) )
            {
            if ( tagIsNotNull ( g ) && !tagList.Contains ( g.tag ) )
                tagList.Add ( g.tag );
            }

        string [ ] tags = tagList.ToArray ( );

        for ( int i = 0; i < tags.Length; i++ )
            gameObjectList.AddRange ( GameObject.FindGameObjectsWithTag ( tags [ i ] ) );

        return gameObjectList.ToArray ( );
        }



    //移動物件 或 複製且搬移物件
    void moveOrCopyThenMove ( GameObject [ ] gameobjects , Vector3 direction )
        {
        if ( moveMode ( ) )
            move ( selectedInSecne ( ) , direction );
        else
            copyAndMove ( selectedInSecne ( ) , direction );
        }

    //移動物件
    void move ( GameObject [ ] gameobjects , Vector3 direction )
        {
        foreach ( var g in gameobjects )
            g.transform.position += direction;
        }

    void move ( GameObject gameobject , Vector3 direction )
        {
        gameobject.transform.localPosition += direction;
        }


    //複製且搬移物件
    void copyAndMove ( GameObject [ ] gameobjects , Vector3 direction )
        {
        List<GameObject> tempList = new List<GameObject> ( );
        GameObject currentGameobject;
        GameObject new_G;

        foreach ( var G in gameobjects )
            {
            if ( G.transform.parent )//有父物件
                {
                //而且是prefab
                if ( PrefabUtility.GetPrefabParent ( G ) )
                    {
                    new_G = getPrefab ( G );
                    new_G.transform.SetParent ( G.transform.parent );

                    }
                //但不是prefab
                else
                    {
                    new_G = Instantiate ( G , G.transform.parent );
                    }

                new_G.transform.localPosition = G.transform.localPosition;
                new_G.transform.localRotation = G.transform.localRotation;
                new_G.transform.localScale = G.transform.localScale;

                move ( new_G , direction );
                currentGameobject = new_G;
                }

            else //沒有父物件的一般物件
                {
                //而且是prefab
                if ( PrefabUtility.GetPrefabParent ( G ) )
                    {
                    new_G = getPrefab ( G );
                    new_G.transform.localPosition = G.transform.localPosition;
                    }
                else//但不是prefab
                    {                    
                    new_G = Instantiate ( G );
                    }

                move ( new_G , direction );
                currentGameobject = new_G;
                }
            currentGameobject.name = G.name;
            if ( !tempList.Contains ( currentGameobject ) )
                {
                tempList.Add ( currentGameobject );
                }
            }

        Selection.objects = tempList.ToArray ( );
        tempList.Clear ( );
        }

    GameObject getPrefab ( GameObject o )
        {
        return ( GameObject )PrefabUtility.InstantiatePrefab ( PrefabUtility.GetPrefabParent ( o ) );
        }


    bool tagIsNotEmpty ( )
        { return includingTag != ""; }

    bool tagIsNotNull ( GameObject gameObject )
        { return gameObject.tag != null; }


    void reName ( )
        {
        if ( selectedInSecne ( ) != null )
            {
            int s = startNumber;
            foreach ( var g in selectedInSecne ( ) )
                {
                g.name = gameObjectName + " (" + s + ")";
                s++;
                }
            }
        else
            Debug.Log ( "沒有選取任何物件" );
        }

    bool canSnap ( )
        { return selectedInSecne ( ) != null && autoAlign == true; }

    Vector3 GetSnappedPosition ( Vector3 position )
        {
        //格點的寬或長不能是0
        if ( gridLength_X == 0 || gridLength_Y == 0 )
            return position;

        Vector3 gridPosition = new Vector3 (
            gridLength_X * Mathf.Round ( position.x / gridLength_X ) ,
            gridLength_Y * Mathf.Round ( position.y / gridLength_Y ) ,
            position.z
            );
        return gridPosition;
        }

    Object [ ] getChildren ( )
        {
        List<Object> tempList = new List<Object> ( );
        foreach ( var Object in selectedInSecne ( ) )
            {
            GameObject gameObject = Object;
            if ( gameObject.transform.childCount > 0 )
                {
                for ( int i = 0; i < gameObject.transform.childCount; i++ )
                    tempList.Add ( gameObject.transform.GetChild ( i ).gameObject );
                }
            }
        return tempList.ToArray ( );
        }
    }