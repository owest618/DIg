using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public static class CSVDataReader
{
    
    //CSVデータ読み込み関数
    //引数：データパス
    public static string[,] readCSVData(string path)
    {
        //返り値の２次元配列
        string[,] returnData;

        //ストリームリーダーsrに読み込む
        //※Application.dataPathはプロジェクトデータのAssetフォルダまでのアクセスパスのこと,
        StreamReader sr = new StreamReader(Application.dataPath + path);
        //ストリームリーダーをstringに変換
        string strStream = sr.ReadToEnd();

        //StringSplitOptionを設定(要はカンマとカンマに何もなかったら格納しないことにする)
        System.StringSplitOptions option = StringSplitOptions.RemoveEmptyEntries;

        //行に分ける
        string[] lines = strStream.Split(new char[] { '\r', '\n' }, option);

        //カンマ分けの準備(区分けする文字を設定する)
        char[] spliter = new char[1] { ',' };

        //行数設定
        int heightLength = lines.Length;
        //列数設定
        int widthLength = lines[0].Split(spliter, option).Length;

        //返り値の2次元配列の要素数を設定
        returnData = new string[heightLength, widthLength];

        //カンマ分けをしてデータを完全分割
        for (int i = 0; i < heightLength; i++)
        {
            //カンマ分け
            string[] readStrData = lines[i].Split(spliter, option);
            for (int j = 0; j < widthLength; j++)
            {
                //型変換
                returnData[i, j] = readStrData[j];
            }
        }
        //返り値
        return returnData;
    }

    public static Terra[,] DataToTerra(string[,] data) {
        Terra[,] returnterra= new Terra[data.GetLength(0), data.GetLength(1)];
        for(int i=0;i< data.GetLength(0); i++)
        {
            for(int j = 0; j < data.GetLength(1); j++)
            {
                returnterra[i, j] = (Terra)Enum.Parse(typeof(Terra), data[i, j]);
            }
        }
        return returnterra;
    }

    public static MovingObject[,] DataToObj(string[,] data)
    {
        MovingObject[,] returnobj = new MovingObject[data.GetLength(0), data.GetLength(1)];
        for (int i = 0; i < data.GetLength(0); i++)
        {
            for (int j = 0; j < data.GetLength(1); j++)
            {
                returnobj[i, j] = (MovingObject)Enum.Parse(typeof(MovingObject), data[i, j]);
            }
        }
        return returnobj;
    }

    //確認表示用の関数
    //引数：2次元配列データ,行数,列数
    //private void WriteMapDatas(int[,] arrays, int hgt, int wid)
    //{
    //    for (int i = 0; i < hgt; i++)
    //    {

    //        for (int j = 0; j < wid; j++)
    //        {
    //            //行番号-列番号:データ値 と表示される
    //            Debug.Log(i + "-" + j + ":" + arrays[i, j]);
    //        }
    //    }
    //}

    //void Start()
    //{
    //    //データパスを設定
    //    //このデータパスは、Assetフォルダ以下の位置を書くので/で階層を区切り、CSVデータ名まで書かないと読み込んでくれない
    //    string path = "/csvData/MapData0.csv";
    //    //データを読み込む(引数：データパス)
    //    this.stageMapDatas = readCSVData(path);

    //    WriteMapDatas(this.stageMapDatas, this.height, this.width);
    //}

    //void UpDate()
    //{

    //}
}