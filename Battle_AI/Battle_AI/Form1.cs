/*
 * 効果音　くらげ工匠　魔王魂
 * BGN 妄奏
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DxLibDLL;

namespace Battle_AI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.ClientSize = new Size(800, 800);
            DX.SetUserWindow(this.Handle); //DxLibの親ウインドウにフォームウィンドウをセット

            DX.DxLib_Init();//DXライブラリ初期化処理
            DX.SetFontSize(30); //フォントサイズ指定
            DX.SetDrawScreen(DX.DX_SCREEN_BACK); //描画先を裏画面に設定
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) //フォームがとじられたら
        {
            DX.DxLib_End(); //DXライブラリ終了処理
        }
        //変数宣言
        //フラグ
        bool souzouA = false; //創造効果Aのフラグ
        bool souzouB1 = false; //創造効果Bのフラグ
        bool souzouB2 = false;
        bool win = false; //勝利
        bool lose = false; //敗北
        bool con = false; //継続するかどうか

        bool hpcomp = false; //自分と相手でHPの比較。自分の方が高ければtrue

        bool FA_V = false;
        bool dame = false;
        bool inf = false;

        int state = 10; //状態。
        //10:普通状態 11:体力7以下状態
        //20:攻撃待機 21:全振り攻撃待機
        int atkptn = 0; //攻撃待機か全振り待機か。1が全振り

        bool Initialise = true; //初期化しているかどうか

        //個性
        int hp = 15; //体力
        int speed = 4; //素早さ
        int charge = 0; //貯めMAX
        int fate = 21; //運

        //入力ステータス
        static string hpS = "0";
        static string speedS = "0";
        static string chargeS = "0";
        static string fateS = "0";
        static string ehpS = "0";

        int ehp = 0; //相手の体力
        int firstehp = 0; //相手の初期の体力
        int count = 0; //ターンカウント
        int pat = 0; //相手の初期体力による型判断

        int atk = 0; //攻撃
        int def = 0; //防御
        int chg = 0; //貯め

        int imgCount = 0;
        int imgCount2 = 0;
        int imgCount3 = 0;
        int imgCount4 = 0;
        int FA_Y = 100;
        int cut1Y = 0; //カット1スライド用
        int cut2X = 0;
        int cut3X = 0;


        //アイドリング画像
        //通常
        static int yun1;
        static int yun2; 
        static int yun3;
        //体力減ってる
        static int yunD1;
        static int yunD2;
        //創造効果B以降通常
        static int yunA1;
        static int yunA2;
        static int yunA3;
        static int yunA4;
        //創造効果B以降フルアタ
        static int yunFA1;
        static int yunFA2;
        //カットイン
        static int yunCut1;
        static int yunCut2;
        static int yunCut3;
        static int yunCut4;

        static int yunA1c;
        static int yunA2c;
        static int yunFA1c;
        static int yunFA2c;

        //ダメージ受けたとき
        static int yunP1;
        static int yunP2;
        static int yunP3;

        static int haikei;
        //オト
        static int krn;
        static int kkn;
        static int haru;
        static int syu;
        static int zun;
        


        public void MainLoop()
        {
            //ループする関数
            //描画、FPS管理等ここで

            DX.ProcessMessage();
            if(Initialise == true){
                //画像のロード（一回だけ初期化）何故か外に書けないから
                yun1 = DX.LoadGraph("画像/ユン1.png");//普通
                yun2 = DX.LoadGraph("画像/ユン2.png");
                yun3 = DX.LoadGraph("画像/ユン3.png");
                yunD1 = DX.LoadGraph("画像/ユンいたみ1.png");//体力少ないとき
                yunD2 = DX.LoadGraph("画像/ユンいたみ2.png");//体力少ないとき
                yunA1 = DX.LoadGraph("画像/ユン攻撃アイド1.png");
                yunA2 = DX.LoadGraph("画像/ユン攻撃アイド2.png");
                yunA3 = DX.LoadGraph("画像/ユン攻撃アイド3.png");
                yunA4 = DX.LoadGraph("画像/ユン攻撃アイド4.png");
                yunFA1 = DX.LoadGraph("画像/ユン攻撃全振りアイド1.png");
                yunFA2 = DX.LoadGraph("画像/ユン攻撃全振りアイド2.png");
                yunCut1 = DX.LoadGraph("画像/ユン全身カット.png");
                yunCut2 = DX.LoadGraph("画像/カット1.png");
                yunCut3 = DX.LoadGraph("画像/カット2.png");
                yunCut4 = DX.LoadGraph("画像/カット3.png");
                yunA1c = DX.LoadGraph("画像/ユン攻撃1.png");
                yunA2c = DX.LoadGraph("画像/ユン攻撃2.png");
                yunFA1c = DX.LoadGraph("画像/ユン攻撃全振り1.png");
                yunFA2c = DX.LoadGraph("画像/ユン攻撃全振り2.png");
                yunP1 = DX.LoadGraph("画像/ユンダメージ1.png");
                yunP2 = DX.LoadGraph("画像/ユンダメージ2.png");
                yunP3 = DX.LoadGraph("画像/ユンダメージ3.png");

                haikei = DX.LoadGraph("画像/背景.png");
                //音楽のロード
                krn = DX.LoadSoundMem("音/キラーン.ogg");
                kkn = DX.LoadSoundMem("音/カキーン.ogg");
                haru = DX.LoadSoundMem("音/春.ogg");
                syu = DX.LoadSoundMem("音/シュー.ogg");
                zun = DX.LoadSoundMem("音/ずん.ogg");

                DX.ChangeVolumeSoundMem(255 * 50 / 100, haru);
                DX.PlaySoundMem(haru, DX.DX_PLAYTYPE_LOOP);
                Initialise = false;
            }

            DX.DrawGraph(0, 0, haikei, DX.TRUE);
            DX.DrawString(50, 500, "体力"+hp, DX.GetColor(255, 255, 64));

            //アイドリングの描画です


            if (souzouA == false && souzouB1 == false && dame == false) //創造効果がオフのとき
            {
                imgCount2 = 0;
                imgCount3 = 0;
                cut1Y = 0;
                cut2X = 0;
                cut3X = 0;

                //状態確認
                switch (state)
                {
                    case 10:
                        imgCount++;
                        if (imgCount > 120)
                        {
                            imgCount = 0;
                        }
                        if (imgCount / 30 == 0)
                        {
                            DX.DrawGraph(170, 80, yun1, DX.TRUE);
                        }
                        else if (imgCount / 60 == 0)
                        {
                            DX.DrawGraph(170, 80, yun2, DX.TRUE);
                        }
                        else if (imgCount / 90 == 0)
                        {
                            DX.DrawGraph(170, 80, yun1, DX.TRUE);

                        }
                        else if (imgCount / 120 == 0)
                        {
                            DX.DrawGraph(170, 80, yun3, DX.TRUE);
                        }

                        break;

                    case 11:
                        label12.Text = string.Format("ユン\n「うう、痛いなあ……\n　このままじゃ負けちゃう」");
                        imgCount++;
                        if (imgCount > 60)
                        {
                            imgCount = 0;
                        }
                        if (imgCount / 30 == 0)
                        {
                            DX.DrawGraph(170, 80, yunD1, DX.TRUE);
                        }
                        else
                        {
                            DX.DrawGraph(170, 80, yunD2, DX.TRUE);
                        }
                        
                        break;

                    case 20:
                        label12.Text = string.Format("ユン\n「気をつけてね！\n　私は負けてなんかあげないんだから！」");
                        imgCount++;
                        if (imgCount > 120)
                        {
                            imgCount = 0;
                        }

                        if (imgCount / 30 == 0)
                        {
                            DX.DrawGraph(170, 80, yunA4, DX.TRUE);//ここだけ変える
                        }
                        else if (imgCount / 60 == 0)
                        {
                            DX.DrawGraph(170, 80, yunA2, DX.TRUE);
                        }
                        else if (imgCount / 90 == 0)
                        {
                            DX.DrawGraph(170, 80, yunA3, DX.TRUE);
                        }
                        else if (imgCount / 120 == 0)
                        {
                            DX.DrawGraph(170, 80, yunA2, DX.TRUE);
                        }
 

                        break;

                    case 21:
                        label12.Text = string.Format("ユン？\n「やはりこうではないとな\n　ただの人間の力じゃ役不足だ」");
                        //上下移動
                        if (FA_V == true)
                        {
                            FA_Y++;
                        }
                        if (FA_V == false)
                        {
                            FA_Y--;
                        }

                        if (FA_Y > 80)
                        {
                            FA_Y = 80;
                            FA_V = false;
                        }
                        if (FA_Y < 0)
                        {
                            FA_Y = 0;
                            FA_V = true;
                        }

                        imgCount++;
                        inf = true;
                        if (imgCount > 60)
                        {
                            imgCount = 0;
                        }
                        if (imgCount / 30 == 0)
                        {
                            DX.DrawGraph(170, FA_Y, yunFA1, DX.TRUE);
                        }
                        else
                        {
                            DX.DrawGraph(170, FA_Y, yunFA2, DX.TRUE);
                        }
                        
                        
                        break;
                }
            }
            

            //体力などの表示
            label8.Text = string.Format("体力:{0}", hp);
            label9.Text = string.Format("素早さ:{0}", speed);
            label10.Text = string.Format("貯めMAX:{0}" ,charge);
            label11.Text = string.Format("運:{0}", fate);

            if (dame == true)
            {
                imgCount3++;
                if (inf == true)//あかいとき
                {
                    if (imgCount3 / 60 == 0)
                    {
                        label12.Text = string.Format("ユン？\n「くッ！」");
                        DX.DrawGraph(170, 80, yunP2, DX.TRUE);
                    }
                    else if (imgCount3 / 90 == 0)
                    {
                        DX.DrawGraph(170, 80, yunP1, DX.TRUE);

                    }
                    else if (imgCount3 / 150 == 0)
                    {
                        dame = false;
                        inf = false;
                        DX.PlaySoundMem(haru, DX.DX_PLAYTYPE_LOOP);
                    }
                }
                else
                {
                    if (imgCount3 / 90 == 0)
                    {
                        label12.Text = string.Format("ユン\n「いたっ！」");
                        DX.DrawGraph(170, 80, yunP3, DX.TRUE);
                    }
                    else if (imgCount3 / 120 == 0)
                    {
                        dame = false;
                    }
                }
                    
            }

            //創造効果
            if (souzouA == true)
            {
                //Console.Write("創造効果A！\n");

                DX.DrawGraph(250, 700-cut1Y*20, yunCut1, DX.TRUE);
                label12.Text = string.Format("ユン\n「負けないよ！」");

                cut1Y++;
                if (cut1Y > 35)
                {
                    DX.DrawGraph(250, 0, yunCut1, DX.TRUE);

                    imgCount2++;

                    if (imgCount2 / 30 == 0)
                    {
                        DX.DrawGraph(250, 0, yunCut1, DX.TRUE);
                    }
                    else if (imgCount2 / 60 == 0)
                    {
                        DX.DrawGraph(250, 0, yunCut1, DX.TRUE);
                    }
                    else if (imgCount2 / 90 == 0)
                    {
                        DX.DrawGraph(250, 0, yunCut1, DX.TRUE);
                    }
                    else if (imgCount2 / 120 == 0)
                    {

                        DX.DrawGraph(250, 0, yunCut1, DX.TRUE);
                    }
                    else if (imgCount2 / 300 == 0)
                    {
                        for (int i = 255; i >= 0; i=i-3)
                        {
                            DX.ClearDrawScreen();
                            DX.DrawGraph(0, 0, haikei, DX.FALSE);
                            DX.SetDrawBright(i, i, i);
                            DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA, i);
                            DX.DrawGraph(250, 0, yunCut1, DX.TRUE);
                            DX.SetDrawBright(255, 255, 255);
                            DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND, 0);
                            DX.ScreenFlip();

                        }
                        souzouA = false;
                    }
  
                }
                
            }

            if (souzouB1 == true)
            {
               //Console.Write("創造効果B！\n");
                
                DX.DrawString(100, 800, "創造効果B発動", DX.GetColor(255, 255, 64));
                if (atkptn == 0)
                {
                    //通常攻撃

                    DX.DrawGraph(500-cut2X * 20, 100, yunCut2, DX.TRUE);

                    cut2X = cut2X + 2;
                    if (cut2X >= 25)
                    {
                        cut2X = 25;
                        DX.DrawGraph(0, 100, yunCut2, DX.TRUE);

                        imgCount2++;
                        
                        if (imgCount2 / 30 == 0)
                        {
                            DX.DrawGraph(0, 100, yunCut2, DX.TRUE);
                            label12.Text = string.Format("ユン\n「いっくよー！」");
                        }
                        else if (imgCount2 / 60 == 0)
                        {
                            DX.DrawGraph(0, 100, yunCut2, DX.TRUE);
                            label12.Text = string.Format("ユン\n「いっくよー！」");
                        }
                        else if (imgCount2 / 90 == 0)
                        {
                            DX.DrawGraph(0, 100, yunCut2, DX.TRUE);
                            DX.DrawGraph(170, 80, yunA1c, DX.TRUE);
                        }
                        else if (imgCount2 / 120 == 0)
                        {
                            DX.DrawGraph(0, 100, yunCut2, DX.TRUE);
                            DX.DrawGraph(170, 80, yunA2c, DX.TRUE);
                            label12.Text = string.Format("ユン\n「気をつけてね！」");
                        }
                        else if (imgCount2 / 300 == 0)
                        {
                            for (int i = 255; i >= 0; i = i - 3)
                            {
                                DX.ClearDrawScreen();
                                DX.DrawGraph(0, 0, haikei, DX.FALSE);
                                DX.DrawGraph(170, 80, yunA2c, DX.TRUE);
                                DX.SetDrawBright(i, i, i);
                                DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA, i);
                                DX.DrawGraph(0, 100, yunCut2, DX.TRUE);
                                DX.DrawGraph(170, 80, yunA2c, DX.TRUE);
                                DX.SetDrawBright(255, 255, 255);
                                DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND, 0);
                                DX.ScreenFlip();
                                label12.Text = string.Format("ユン\n「気をつけてね！\n　私は負けてなんかあげないんだから！」");

                            }
                            souzouB1 = false;

                        }
                    }

                    state = 20;

                }
                else
                {
                    //攻撃全振り
                    DX.StopSoundMem(haru);

                     DX.DrawGraph(500-cut2X * 20, 100, yunCut3, DX.TRUE);

                    cut2X = cut2X + 2;
                    if (cut2X >= 25)
                    {
                        cut2X = 25;
                        DX.DrawGraph(0, 100, yunCut3, DX.TRUE);
                        

                        imgCount2++;

                        if (imgCount2 / 30 == 0)
                        {
                            DX.DrawGraph(0, 100, yunCut3, DX.TRUE);
                            label12.Text = string.Format("ユン\n「…………ッ！」");
                        }
                        else if (imgCount2 / 60 == 0)
                        {
                            DX.DrawGraph(0, 100, yunCut3, DX.TRUE);
                            label12.Text = string.Format("ユン\n「…………ッ！」");
                        }
                        else if (imgCount2 / 90 == 0)
                        {
                            DX.SetDrawBright(255, 0, 0);
                            
                            DX.DrawGraph(0, 100, yunCut3, DX.TRUE);
                            label12.Text = string.Format("ユン？\n「そろそろ終わりだ」");
                        }
                        else if (imgCount2 / 120 == 0)
                        {
                            DX.SetDrawBright(255, 0, 0);
                            DX.DrawGraph(0, 100, yunCut3, DX.TRUE);
                            label12.Text = string.Format("ユン？\n「そろそろ終わりだ」");
                        }
                        else if (imgCount2 / 150 == 0)
                        {
                            DX.SetDrawBright(255, 255, 255);
                            DX.DrawGraph(0, 100, yunCut4, DX.TRUE);
                            label12.Text = string.Format("ユン？\n「そろそろ終わりだ」");

                        }
                        else if(imgCount2 / 180 == 0){
                            DX.PlaySoundMem(syu, DX.DX_PLAYTYPE_BACK);
                            DX.DrawGraph(0, 100, yunCut4, DX.TRUE);
                            DX.DrawGraph(170, 80, yunFA1c, DX.TRUE);
                            label12.Text = string.Format("ユン？\n「地獄の炎、その身に味わうがいい！」");
                        }
                        else if (imgCount2 / 210 == 0)
                        {

                            for (int i = 0; i < 2; i++)
                            {
                                if (i % 2 == 0)
                                {
                                    DX.ClearDrawScreen();
                                    DX.DrawBox(0, 0, 800, 800, DX.GetColor(255, 0, 0), DX.TRUE);
                                    DX.ScreenFlip();
                                }
                                else
                                {
                                    DX.ClearDrawScreen();
                                    DX.DrawGraph(170, 80, yunFA2c, DX.TRUE);
                                    DX.ScreenFlip();
                                }
                            }
                        }
                        else if (imgCount2 / 210 == 0)
                        {
                            DX.DrawGraph(0, 100, yunCut4, DX.TRUE);
                            DX.DrawGraph(170, 80, yunFA2c, DX.TRUE);

                        }
                        else if (imgCount2 / 240 == 0)
                        {
                            for (int i = 255; i >= 0; i = i - 2)
                            {

                                DX.DrawGraph(0, 0, haikei, DX.FALSE);

                            }
                            souzouB1 = false;
                        }

                    }

                    state = 21;
                }
            }

            //ここからAIのターン
            if (con == true)
            {
                //値の初期化
                atk = 0;
                def = 0;

                Console.Write("ステータスを決めます\n\n");
                if(count == 1)
                {
                    Console.Write("1ターン目なので相手のタイプを調べます\n");
                    if (firstehp >= 30)//30以上
                    {
                        Console.Write("--相手は体力が多め。パターン1\n\n");
                        pat = 1;
                    }
                    else if (firstehp >= 20)//20以上
                    {
                        Console.Write("--相手は体力がそこそこ。パターン2\n\n");
                        pat = 2;
                    }
                    else if (firstehp > 10)//10よりは上
                    {
                        Console.Write("--相手は体力がそれなり。パターン3\n\n");
                        pat = 3;
                    }
                    else//10以下
                    {
                        Console.Write("--相手は体力が少ない。パターン4\n\n");
                        pat = 4;
                    }
                }

                Console.Write("相手と自分の体力の現在の値を比較します\n");//現在の値
                if (hpcomp == true)
                {
                    Console.Write("--自分の体力の方が高いです\n\n");
                    switch (pat)
                    {
                        case 1://初期30以上。有利。ターン数は経過しているからためているかも。
                            atk = 7;
                            atkptn = 1;
                            break;

                        case 2://初期20以上
                            atk = 7;
                            atkptn = 1;
                            break;

                        case 3://初期10以上
                            atk = 5;
                            def = 2;
                            atkptn = 0;
                            break;

                        case 4://初期9以下
                            atk = 7;
                            atkptn = 1;
                            break;
                    }
                }
                else
                {
                    Console.Write("--相手の体力の方が高いか同じです\n\n");
                    switch (pat)
                    {
                        case 1://初期30以上。貯めてくるタイプと予想
                            //もし自分の体力が5以下なら攻撃極振り
                            if (hp <= 5 || ehp <= 5)
                            {
                                atk = 7;
                                def = 0;
                                atkptn = 1;
                            }
                            else
                            {
                                atk = 5;
                                def = 2;
                                atkptn = 0;
                                if (count <= 3)
                                {
                                    atk = 4;
                                    def = 3;
                                    atkptn = 0;
                                }
                            }
                            break;

                        case 2://初期20以上
                            //もし自分の体力が5以下なら攻撃極振り
                            if (hp <= 5 || ehp <= 5)
                            {
                                atk = 7;
                                def = 0;
                                atkptn = 1;
                            }
                            else
                            {
                                atk = 5;
                                def = 2;
                                atkptn = 0;
                            }
                            break;

                        case 3://初期10以上。さっさと殺せ
                            //もし自分の体力が5以下なら攻撃極振り
                            if (hp <= 5 || ehp <= 5)
                            {
                                atk = 7;
                                def = 0;
                                atkptn = 1;
                            }
                            else
                            {
                                atk = 5;
                                def = 2;
                                atkptn = 0;
                            }
                            break;

                        case 4://初期9以下。そこでこの状況は一方的に殴られている。
                            //もし自分の体力が5以下なら攻撃極振り
                            if (hp <= 5 || ehp<=5)
                            {
                                atk = 7;
                                def = 0;
                                atkptn = 1;
                            }
                            else
                            {
                                atk = 4;
                                def = 3;
                                atkptn = 0;
                            }
                            break;
                    }
                }



                //もしチャージが3たまっていたら
                /*
                if (chg >= 3)
                {
                    if (hp > ehp)
                    {
                        atk = atk + chg;
                        chg = 0;
                    }
                    else
                    {
                        def = def + chg;
                        chg = 0;
                    }
                }
                 * ためいらない
                 */
                 
                Console.Write("振り分け結果を表示します\n");
                Console.Write("攻撃:{0}", atk);
                Console.Write(" 防御:{0}", def);
                Console.Write(" 溜め:{0}\n\n", chg);

                con = false;
            }
            DX.ScreenFlip(); //裏画面を表画面に反映

        }
        
        //ボタン動作
        //創造効果A
        private void button1_Click(object sender, EventArgs e)
        {
            Console.Write("創造効果Aを発動します\n");
            DX.PlaySoundMem(krn, DX.DX_PLAYTYPE_BACK); // 効果音を再生する
            souzouA = true;
        }
        //創造効果B
        private void button2_Click(object sender, EventArgs e)
        {
            Console.Write("創造効果Bを発動します\n");
            DX.PlaySoundMem(kkn, DX.DX_PLAYTYPE_BACK); 
            souzouB1 = true;
        }

        //ターン進める
        private void button6_Click(object sender, EventArgs e)
        {
            con = true;
            count++;
            Console.Write("\n☆{0}ターン目☆\n\n", count);
        }

        //勝利
        private void button7_Click(object sender, EventArgs e)
        {
            Console.Write("勝利しました\n");
            win = true;
        }
        //敗北
        private void button8_Click(object sender, EventArgs e)
        {
            Console.Write("敗北しました\n");
            lose = true;
        }

        //相手体力の記憶
        private void button3_Click(object sender, EventArgs e)
        {
            //相手の体力
            ehpS = textBox1.Text;
            ehp = int.Parse(ehpS);
            Console.Write("対戦相手の体力:{0}を記憶しました\n",ehp);
            //初期の体力であればfirstehpに記憶する
            if (count == 0)
            {
                firstehp = ehp;
            }

            //自分との比較
            if (hp > ehp)
            {
                //自分の方が高い
                hpcomp = true;
            }
            else
            {
                //相手が高いか同じ
                hpcomp = false;
            }
        }
        //ダメージ
        private void button4_Click(object sender, EventArgs e)
        {
            imgCount3 = 0;
            DX.PlaySoundMem(zun, DX.DX_PLAYTYPE_BACK);
            //受けたダメージ
            hpS = textBox6.Text;
            hp = hp - int.Parse(hpS);
            Console.Write("{0}のダメージを受けました\n",hpS);//受けたダメージだからhpじゃなくてhps
            if (hp < 7)
            {
                state = 11;
            }
            else
            {
                state = 10;
            }
            dame = true;
        }

        //初期値へのボーナスプラス
        private void button5_Click(object sender, EventArgs e)
        {
            hpS = textBox2.Text;
            hp = hp + int.Parse(hpS);
            speedS = textBox3.Text;
            speed = speed + int.Parse(speedS);
            chargeS = textBox4.Text;
            charge = charge + int.Parse(chargeS);
            fateS = textBox5.Text;
            fate = fate + int.Parse(fateS);
            Console.Write("各ステータスにボーナス追加\n");
        }


        //ここから下は消さない
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


    }
}

