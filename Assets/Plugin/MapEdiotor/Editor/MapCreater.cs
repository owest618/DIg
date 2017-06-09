using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Linq;



/// <summary>
/// Map creater 
/// </summary>
public class MapCreater : EditorWindow {
    
	// 画像ディレクトリ
	private Object imgDirectory;
	// 出力先ディレクトリ(nullだとAssets下に出力します)
	private Object outputDirectory;
	// マップエディタのマスの数
	private int mapSize = 10;
	// グリッドの大きさ、小さいほど細かくなる
	private float gridSize = 50.0f;
    public TextAsset LoadTerra;
    public TextAsset LoadObj;
    // 出力ファイル名
    private string outputFileName;
	// 選択した画像パス
	private Terra selectedTerra=Terra.NONE;
    private MovingObject selectedObj = MovingObject.NONE;
    // サブウィンドウ
    private MapCreaterSubWindow subWindow;
    public Texture2D[] terratex = new Texture2D[System.Enum.GetValues(typeof(Terra)).Length];
    public Texture2D[] objtex = new Texture2D[System.Enum.GetValues(typeof(MovingObject)).Length];


    [UnityEditor.MenuItem("Window/MapCreater")]
	static void ShowTestMainWindow(){
		EditorWindow.GetWindow (typeof (MapCreater));
	}
	
    void OnFocus()
    {
        for(int i = 0; i < terratex.Length; i++)
        {
            if (MapCreaterConfig.setterras[i] != null)
            {
                Rect texrect = MapCreaterConfig.setterras[i].textureRect;
                Texture2D tex = new Texture2D((int)texrect.width, (int)texrect.height);
                Texture2D oritex = MapCreaterConfig.setterras[i].texture;
                Color[] co = oritex.GetPixels((int)texrect.x, (int)texrect.y, (int)texrect.width, (int)texrect.height);
                tex.SetPixels(co);
                tex.Apply();
                terratex[i] = tex;
            }
            else
            {
                terratex[i] = null;
            }
        }
        for (int i = 0; i < objtex.Length; i++)
        {
            if (MapCreaterConfig.setobjects[i] != null)
            {
                Rect texrect = MapCreaterConfig.setobjects[i].textureRect;
                Texture2D tex = new Texture2D((int)texrect.width, (int)texrect.height);
                Texture2D oritex = MapCreaterConfig.setobjects[i].texture;
                Color[] co = oritex.GetPixels((int)texrect.x, (int)texrect.y, (int)texrect.width, (int)texrect.height);
                tex.SetPixels(co);
                tex.Apply();
                objtex[i] = tex;
            }
            else
            {
                objtex[i] = null;
            }
        }
    }

    void OnGUI(){
		// GUI

		GUILayout.BeginHorizontal();
		GUILayout.Label("map size : ", GUILayout.Width(110));
		mapSize = EditorGUILayout.IntField(mapSize);
		GUILayout.EndHorizontal();
		EditorGUILayout.Space();

		GUILayout.BeginHorizontal();
		GUILayout.Label("grid size : ", GUILayout.Width(110));
		gridSize = EditorGUILayout.FloatField(gridSize);
		GUILayout.EndHorizontal();
		EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.Label("LoadTerra : ", GUILayout.Width(110));
        LoadTerra = (TextAsset)EditorGUILayout.ObjectField(LoadTerra, typeof(TextAsset),true);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.Label("LoadObj : ", GUILayout.Width(110));
        LoadObj = (TextAsset)EditorGUILayout.ObjectField(LoadObj, typeof(TextAsset), true);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
		GUILayout.Label("Save Directory : ", GUILayout.Width(110));
		outputDirectory = EditorGUILayout.ObjectField(outputDirectory, typeof(UnityEngine.Object), true);
		GUILayout.EndHorizontal();
		EditorGUILayout.Space();
		
		GUILayout.BeginHorizontal();
		GUILayout.Label("Save filename : ", GUILayout.Width(110));
		outputFileName = (string)EditorGUILayout.TextField(outputFileName);
		GUILayout.EndHorizontal();
		EditorGUILayout.Space();

		DrawImageParts();

        if (subWindow != null)
            DrawSelectedImage();

		DrawMapWindowButton();
	}

	// 画像一覧をボタン選択出来る形にして出力
	private void DrawImageParts(){
			float x = 0.0f;
			float y = 00.0f;
			float w = 50.0f;
			float h = 50.0f;
			float maxW = 300.0f;

        if (subWindow != null)
        {
            EditorGUILayout.BeginVertical();
            if (subWindow.selected == 0)
            {
                for (int i = 0; i < System.Enum.GetValues(typeof(Terra)).Length; i++)
                {
                    if (x > maxW)
                    {
                        x = 0.0f;
                        y += h;
                        EditorGUILayout.EndHorizontal();
                    }
                    if (x == 0.0f)
                    {
                        EditorGUILayout.BeginHorizontal();
                    }
                    GUILayout.FlexibleSpace();
                    if (terratex[i] != null)
                    {
                        Texture2D tex = Texture2D.Instantiate(terratex[i]);
                        TextureScale.Bilinear(tex, (int)w - 10, (int)h - 10);
                        if (GUILayout.Button(tex, GUILayout.MaxWidth(w), GUILayout.MaxHeight(h), GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false)))
                        {
                            selectedTerra = (Terra)i;
                        }
                    }
                    else
                    {
                        if (GUILayout.Button("", GUILayout.MaxWidth(w), GUILayout.MaxHeight(h), GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false)))
                        {
                            selectedTerra = (Terra)i;
                        }
                    }
                    GUILayout.FlexibleSpace();
                    x += w;
                }
            }
            else if (subWindow.selected == 1)
            {
                for (int i = 0; i < System.Enum.GetValues(typeof(MovingObject)).Length; i++)
                {
                    if (x > maxW)
                    {
                        x = 0.0f;
                        y += h;
                        EditorGUILayout.EndHorizontal();
                    }
                    if (x == 0.0f)
                    {
                        EditorGUILayout.BeginHorizontal();
                    }
                    GUILayout.FlexibleSpace();
                    if (objtex[i] != null)
                    {
                        Texture2D tex = Texture2D.Instantiate(objtex[i]);
                        TextureScale.Bilinear(tex, (int)w - 10, (int)h - 10);
                        if (GUILayout.Button(tex, GUILayout.MaxWidth(w), GUILayout.MaxHeight(h), GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false)))
                        {
                            selectedObj = (MovingObject)i;
                        }
                    }
                    else
                    {
                        if (GUILayout.Button("", GUILayout.MaxWidth(w), GUILayout.MaxHeight(h), GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false)))
                        {
                            selectedObj = (MovingObject)i;
                        }
                    }
                    GUILayout.FlexibleSpace();
                    x += w;
                }
            }
            EditorGUILayout.EndVertical();
        }
	}

	// 選択した画像データを表示
	private void DrawSelectedImage(){
        EditorGUILayout.BeginVertical();
        if (subWindow != null)
        {
            if (subWindow.selected == 0)
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("select : " + selectedTerra);
                if (terratex[(int)selectedTerra] != null)
                {
                    Texture2D tex = Texture2D.Instantiate(terratex[(int)selectedTerra]);
                    TextureScale.Bilinear(tex, (int)64, (int)64);
                    GUILayout.Box(tex);
                }
            }
            else if (subWindow.selected == 1)
            {

                GUILayout.FlexibleSpace();
                GUILayout.Label("select : " + selectedObj);
                if (objtex[(int)selectedObj] != null)
                {
                    Texture2D tex = Texture2D.Instantiate(objtex[(int)selectedObj]);
                    TextureScale.Bilinear(tex, (int)64, (int)64);
                    GUILayout.Box(tex);
                }
            }
        }
        EditorGUILayout.EndVertical();
	}

	// マップウィンドウを開くボタンを生成
	private void DrawMapWindowButton(){
		EditorGUILayout.BeginVertical();
		GUILayout.FlexibleSpace();
		if(GUILayout.Button("open map editor")){
			if(subWindow == null){
				subWindow = MapCreaterSubWindow.WillAppear(this);
			}else{
				subWindow.Focus();
			}
		}
        if (GUILayout.Button("open map config"))
        {
            MapCreaterConfig.ShowTestMainWindow();
        }
        EditorGUILayout.EndVertical();
	}

    void OnDestroy()
    {
        if(subWindow!=null)
            subWindow.Close();
    }

    public Terra SelectedTerra{
		get{ return selectedTerra; }
	}

    public MovingObject SelectedObj
    {
        get { return selectedObj; }
    }

    public int MapSize{
		get{ return mapSize; }
	}

	public float GridSize{
		get{ return gridSize; }
	}
    

	// 出力先パスを生成
	public string OutputFilePath(){
        if (outputFileName == null)
            return null;
		string resultPath = "";
		if(outputDirectory != null){
			resultPath = AssetDatabase.GetAssetPath(outputDirectory);
		}else{
			resultPath = Application.dataPath;
		}
		return resultPath + "/" + outputFileName;
	}
}

/// <summary>
/// Map creater sub window.
/// </summary>
public class MapCreaterSubWindow : EditorWindow
{
	// マップウィンドウのサイズ
	const float WINDOW_W = 750.0f;
	const float WINDOW_H = 750.0f;
	// マップのグリッド数
	private int mapSize = 0;
	// グリッドサイズ。親から値をもらう
	private float gridSize = 0.0f;
	// マップデータ
	private Terra[,] terramap;
    private MovingObject[,] objmap;
    // グリッドの四角
    private Rect[,] gridRect;
	// 親ウィンドウの参照を持つ
	private MapCreater parent;
    public int selected;

	// サブウィンドウを開く
	public static MapCreaterSubWindow WillAppear(MapCreater _parent){
		MapCreaterSubWindow window = (MapCreaterSubWindow)EditorWindow.GetWindow(typeof(MapCreaterSubWindow) , false);
		window.Show();
		window.minSize = new Vector2(WINDOW_W ,WINDOW_H);
		window.SetParent (_parent);
		window.init ();
		return window;
	}
	
	private void SetParent(MapCreater _parent){
		parent = _parent;
	}

	// サブウィンドウの初期化
	public void init(){
		mapSize = parent.MapSize;
		gridSize = parent.GridSize;

        // マップデータを初期化
        terramap = new Terra[mapSize, mapSize];
        objmap = new MovingObject[mapSize, mapSize];
        for (int i = 0; i < mapSize; i++) {
			for (int j = 0; j < mapSize; j++) {
				terramap[i,j] = Terra.NONE;
                objmap[i, j] = MovingObject.NONE;
            }
		}
		// グリッドデータを生成
		gridRect = CreateGrid(mapSize);
        if (parent.LoadTerra != null)
        {
            string path = AssetDatabase.GetAssetPath(parent.LoadTerra);
            path=path.Remove(0, 6);
            terramap = CSVDataReader.DataToTerra(CSVDataReader.readCSVData(path));
        }
        if (parent.LoadObj != null)
        {
            string path = AssetDatabase.GetAssetPath(parent.LoadObj);
            path = path.Remove(0, 6);
            objmap = CSVDataReader.DataToObj(CSVDataReader.readCSVData(path));
        }
    }

	void OnGUI(){
        EditorGUI.BeginChangeCheck();
        selected = GUILayout.Toolbar(selected, new string[] { "terra", "object"});
        if (EditorGUI.EndChangeCheck())
        {
            parent.Focus();
        }
        // グリッド線を描画する
        for (int yy = 0 ; yy < mapSize ; yy++){
			for(int xx = 0 ; xx < mapSize ; xx++){
				DrawGridLine(gridRect[yy,xx]);
			}
		}

		// クリックされた位置を探して、その場所に画像データを入れる
		Event e = Event.current;
		if(e.type == EventType.MouseDown){
			Vector2 pos = Event.current.mousePosition;
			int xx;
			// x位置を先に計算して、計算回数を減らす
			for(xx = 0 ; xx < mapSize ; xx++){
				Rect r = gridRect[0 ,xx];
				if(r.x <= pos.x && pos.x <= r.x + r.width){
					break;
				}
			}
            if (xx == mapSize)
                xx--;
			// 後はy位置だけ探す
			for(int yy = 0 ; yy < mapSize ; yy++){
				if(gridRect[yy,xx].Contains(pos)){
                    if (selected == 0)
                        terramap[yy, xx] = parent.SelectedTerra;
                    else if (selected == 1)
                        objmap[yy, xx] = parent.SelectedObj;
                    Repaint();
					break;
				}
			}
		}

		// 選択した画像を描画する
		for(int yy = 0 ; yy < mapSize ; yy++){
			for(int xx = 0 ; xx < mapSize ; xx++){
				if(parent.terratex[(int)terramap[yy, xx]]!=null)
                {
                    GUI.DrawTexture(gridRect[yy, xx], parent.terratex[(int)terramap[yy, xx]]);
                }
			}
		}
        for (int yy = 0; yy < mapSize; yy++)
        {
            for (int xx = 0; xx < mapSize; xx++)
            {
                if (parent.objtex[(int)objmap[yy, xx]] != null)
                {
                    GUI.DrawTexture(gridRect[yy, xx], parent.objtex[(int)objmap[yy, xx]]);
                }
            }
        }

        // 出力ボタン
        Rect rect = new Rect (0, WINDOW_H - 50 , 300 , 50);
		GUILayout.BeginArea(rect);
		if(GUILayout.Button("output file" , GUILayout.MinWidth(300) , GUILayout.MinHeight(50))){
			OutputFile();
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndArea();

	}

	// グリッドデータを生成
	private Rect[,] CreateGrid(int div){
		int sizeW = div;
		int sizeH = div;

		float x = 0.0f;
		float y = 25.0f;
		float w = gridSize;
		float h = gridSize;

		Rect[,] resultRects = new Rect[sizeH ,sizeW];

		for(int yy = 0 ; yy < sizeH ; yy++){
			x = 0.0f;
			for(int xx = 0 ; xx < sizeW ; xx++){
				Rect r = new Rect(new Vector2(x , y) , new Vector2(w , h));
				resultRects[yy , xx] = r;
				x += w;
			}
			y += h;
		}

		return resultRects;
	}

	// グリッド線を描画
	private void DrawGridLine(Rect r){
		// grid
		Handles.color = new Color(1f, 1f, 1f, 0.5f);

		// upper line
		Handles.DrawLine(
			new Vector2(r.position.x 			, r.position.y) , 
			new Vector2(r.position.x + r.size.x , r.position.y));

		// bottom line
		Handles.DrawLine(
			new Vector2(r.position.x 			, r.position.y + r.size.y) , 
			new Vector2(r.position.x + r.size.x , r.position.y + r.size.y));

		// left line
		Handles.DrawLine(
			new Vector2(r.position.x , r.position.y) , 
			new Vector2(r.position.x , r.position.y + r.size.y));

		// right line
		Handles.DrawLine(
			new Vector2(r.position.x + r.size.x , r.position.y) , 
			new Vector2(r.position.x + r.size.x , r.position.y + r.size.y));
	}

	// ファイルで出力
	private void OutputFile(){
		string path = parent.OutputFilePath();
        if (path == null)
        {
		EditorUtility.DisplayDialog("MapCreater" , "保存名を入力してください" , "ok");
            return;
        }
		FileInfo fileInfo = new FileInfo(path+"_terra.csv");
        fileInfo.Delete();
		StreamWriter sw = fileInfo.AppendText();
		sw.WriteLine(GetMapTerraFormat());
		sw.Flush();
		sw.Close();
        fileInfo = new FileInfo(path + "_obj.csv");
        fileInfo.Delete();
        sw = fileInfo.AppendText();
        sw.WriteLine(GetMapObjFormat());
        sw.Flush();
        sw.Close();

        // 完了ポップアップ
        EditorUtility.DisplayDialog("MapCreater" , "output file success\n" + path , "ok");
	}

	// 出力するマップデータ整形
	private string GetMapTerraFormat(){
		string result = "";
		for (int i = 0; i < mapSize; i++) {
			for(int j = 0 ; j < mapSize ; j++){
				result += OutputDataFormat(terramap[i,j].ToString());
				if(j < mapSize - 1){
					result += ",";
				}
			}
			result += "\n";
		}
		return result;
	}

    private string GetMapObjFormat()
    {
        string result = "";
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                result += OutputDataFormat(objmap[i, j].ToString());
                if (j < mapSize - 1)
                {
                    result += ",";
                }
            }
            result += "\n";
        }
        return result;
    }

    private string OutputDataFormat(string data){
		if(data != null && data.Length > 0){
			string[] tmps = data.Split('/');
			string fileName = tmps[tmps.Length - 1];
			return fileName.Split('.')[0];
		}else{
			return "";
		}
	}
}