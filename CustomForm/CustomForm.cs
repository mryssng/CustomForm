// -------------------------------------------------------------------------------------------------
// Copyright (c) mryssng. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using NativeAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;

// -------------------------------------------------------------------------------------------------
// If Windows.Form is used the below lines have to be added in .csproj file
//  < ItemGroup >
//      < FrameworkReference Include = "Microsoft.WindowsDesktop.App" />
//  </ ItemGroup >
// -------------------------------------------------------------------------------------------------

namespace CustomControl
{
    public class CustomForm : Form
    {
        #region Enum
        /// <summary>
        /// Defines the MouseState
        /// </summary>
        private enum MouseStateType
        {
            /// <summary>
            /// Defines the HOVER
            /// </summary>
            HOVER,

            /// <summary>
            /// Defines the DOWN
            /// </summary>
            DOWN,

            /// <summary>
            /// Defines the OUT
            /// </summary>
            OUT
        }

        /// <summary>
        /// Defines the ResizeDirection
        /// </summary>
        private enum ResizeDirection
        {
            TopLeft,
            TopRight,
            Top,
            BottomLeft,
            BottomRight,
            Bottom,
            Left,
            Right,
            None
        }

        /// <summary>
        /// Defines the ButtonState
        /// </summary>
        private enum ButtonState
        {
            XOver,
            MaxOver,
            MinOver,
            XDown,
            MaxDown,
            MinDown,
            None
        }
        #endregion

        #region BeforeFormClosingEventArgsClass
        /// <summary>
        /// Formのクローズをキャンセルするためのクラス
        /// </summary>
        public class BeforeFormClosingEventArgs : EventArgs
        {
            public bool Cancel;

            public BeforeFormClosingEventArgs()
            {
                Cancel = false;
            }
        }
        #endregion

        #region Propereties
        /// <summary>
        /// FormBorderStyleをプロパティウィンドウで非表示に設定する
        /// </summary>
        [Browsable(false)]                              // プロパティウィンドウ非表示
        [EditorBrowsable(EditorBrowsableState.Never)]   // インテリセンス非表示
        public new FormBorderStyle FormBorderStyle
        {
            get { return base.FormBorderStyle; }
            protected set { base.FormBorderStyle = value; }
        }

        /// <summary>
        /// BackColorをプロパティウィンドウで非表示に設定する
        /// </summary>
        [Browsable(false)]                              // プロパティウィンドウ非表示
        [EditorBrowsable(EditorBrowsableState.Never)]   // インテリセンス非表示
        public new Color BackColor
        {
            get { return base.BackColor; }
            protected set { base.BackColor = value; }
        }

        /// <summary>
        /// ForeColorをプロパティウィンドウで非表示に設定する
        /// </summary>
        [Browsable(false)]                              // プロパティウィンドウ非表示
        [EditorBrowsable(EditorBrowsableState.Never)]   // インテリセンス非表示
        public new Color ForeColor
        {
            get { return base.ForeColor; }
            protected set { base.ForeColor = value; }
        }

        /// <summary>
        /// HelpButtonをプロパティウィンドウで非表示に設定する
        /// </summary>
        [Browsable(false)]                              // プロパティウィンドウ非表示
        [EditorBrowsable(EditorBrowsableState.Never)]   // インテリセンス非表示
        public new bool HelpButton
        {
            get { return base.HelpButton; }
            protected set { base.HelpButton = value; }
        }

        /// <summary>
        /// Iconをプロパティウィンドウで非表示に設定する
        /// </summary>
        [Browsable(false)]                              // プロパティウィンドウ非表示
        [EditorBrowsable(EditorBrowsableState.Never)]   // インテリセンス非表示
        public new Icon Icon
        {
            get { return base.Icon; }
            protected set { base.Icon = value; }
        }

        /// <summary>
        /// ShowIconをプロパティウィンドウで非表示に設定する
        /// </summary>
        [Browsable(false)]                              // プロパティウィンドウ非表示
        [EditorBrowsable(EditorBrowsableState.Never)]   // インテリセンス非表示
        public new bool ShowIcon
        {
            get { return base.ShowIcon; }
            protected set { base.ShowIcon = value; }
        }

        /// <summary>
        /// Sizable
        /// </summary>
        [Category("Layout")]
        public bool Sizable { get; set; } = true;

        /// <summary>
        /// ユーザーエリア
        /// </summary>
        [Browsable(false)]
        public Rectangle UserArea
        {
            get
            {
                return new Rectangle(0, TitleBarHeight, Width, Height - (TitleBarHeight));
            }
        }

        /// <summary>
        /// タイトルバーの境界矩形
        /// </summary>
        [Browsable(false)]
        public Rectangle TitleBarBounds
        {
            get
            {
                return titleBarBounds;
            }
        }

        /// <summary>
        /// タイトルバーの高さ
        /// </summary>
        [Description("タイトルバーの高さを設定します。")]
        [Category("CustomForm")]
        [DefaultValue(typeof(int), "28")]
        [Browsable(true)]
        public int TitleBarHeight
        {
            get; set;
        } = 28;

        /// <summary>
        /// Formの境界線の幅
        /// </summary>
        [Description("Formの境界線の幅を設定します。")]
        [Category("CustomForm")]
        [DefaultValue(typeof(int), "2")]
        [Browsable(true)]
        public int BorderWidth
        {
            get; set;
        } = 2;

        /// <summary>
        /// タイトルバーのボタンの幅
        /// </summary>
        [Description("タイトルバーのボタンの幅を設定します。")]
        [Category("CustomForm")]
        [Browsable(false)]
        public int TitleBarButtonWidth
        {
            get;
            private set;
        }

        /// <summary>
        /// タイトルバーの色
        /// </summary>
        [Description("タイトルバーの色を設定します。")]
        [Category("CustomForm")]
        [DefaultValue(typeof(Color), "DarkSlateGray")]
        [Browsable(true)]
        public Color TitleBarColor
        {
            get;
            set;
        } = Color.DarkSlateGray;

        /// <summary>
        /// Formの枠の色
        /// </summary>
        [Description("Formの枠の色を設定します。")]
        [Category("CustomForm")]
        [DefaultValue(typeof(Color), "DarkSlateGray")]
        [Browsable(true)]
        public Color BorderColor
        {
            get;
            set;
        } = Color.DarkSlateGray;

        /// <summary>
        /// Formの背景の色
        /// </summary>
        [Description("Formの背景の色を設定します。")]
        [Category("CustomForm")]
        [DefaultValue(typeof(Color), "Snow")]
        [Browsable(true)]
        public Color BackDropColor
        {
            get;
            set;
        } = Color.Snow;

        /// <summary>
        /// Formが開く時のフェードイン アニメーションの時間長さ
        /// </summary>
        [Description("Formが開く時のフェードイン アニメーションの時間長さを設定します。(msec)")]
        [Category("CustomForm")]
        [DefaultValue(typeof(int), "2")]
        [Browsable(true)]
        public int FadeinAnimetionTime
        {
            get; set;
        } = FADEIN_ANIMATION_TIME;

        /// <summary>
        /// Formが閉じる時のフェードアウト アニメーションの時間長さ
        /// </summary>
        [Description("Formが閉じる時のフェードアウト アニメーションの時間長さを設定します。(msec)")]
        [Category("CustomForm")]
        [DefaultValue(typeof(int), "2")]
        [Browsable(true)]
        public int FadeoutAnimetionTime
        {
            get; set;
        } = FADEOUT_ANIMATION_TIME;

        /// <summary>
        /// Formがアクティブかどうか
        /// </summary>
        [Browsable(false)]
        public bool IsFormActivated
        {
            get
            {
                return isFormActivated;
            }
        }

        /// <summary>
        /// フォームのコントロールボックスの左側ボタンの境界線
        /// </summary>
        [Browsable(false)]
        public Rectangle LeftButtonBounds
        {
            get
            {
                return leftButtonBounds;
            }
        }

        /// <summary>
        /// フォームのコントロールボックスの真ん中ボタンの境界線
        /// </summary>
        [Browsable(false)]
        public Rectangle CenterButtonBounds
        {
            get
            {
                return centerButtonBounds;
            }
        }

        /// <summary>
        /// フォームのコントロールボックスのクローズボタンの境界線
        /// </summary>
        [Browsable(false)]
        public Rectangle XButtonBounds
        {
            get
            {
                return xButtonBounds;
            }
        }

        /// <summary>
        /// コントロールの作成時に必要な情報を取得売る
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                var par = base.CreateParams;
                // WS_SYSMENU: システムメニューの作成に対するトリガー
                // WS_MINIMIZEBOX: Formを最小化してタスクバーに表示することを許可する
                par.Style = par.Style | (int)WindowStyle.WS_MINIMIZEBOX | (int)WindowStyle.WS_SYSMENU; // WS_MINIMIZEBOX style flagを有効にする

                // ドロップシャドウの描画の可否を判定
                aeroEnabled = CheckAeroEnabled();
                if (!aeroEnabled)
                    par.ClassStyle |= NativeConstants.CS_DROPSHADOW;

                return par;
            }
        }
        #endregion

        #region Fields
        // 画面の解像度(DPI値)を取得
        public static readonly float DPI = NativeMethods.GetDeviceCaps(NativeMethods.GetWindowDC(IntPtr.Zero), NativeConstants.LOGPIXELSX);
        // 高DPI時のスケーリング比
        public static readonly float DPI_SCALE = DPI / 96f;

        // リサイズの方向
        private ResizeDirection resizeDir;
        // ボタンの状態
        private ButtonState buttonState = ButtonState.None;

        // リサイズの方向とコマンドをコマンドDictionaryに格納
        private readonly Dictionary<int, int> ResizingLocationsToCmd = new Dictionary<int, int>
        {
            {(int)HitTestValues.HT_TOP,         (int)ResizingEdge.WMSZ_TOP},
            {(int)HitTestValues.HT_TOPLEFT,     (int)ResizingEdge.WMSZ_TOPLEFT},
            {(int)HitTestValues.HT_TOPRIGHT,    (int)ResizingEdge.WMSZ_TOPRIGHT},
            {(int)HitTestValues.HT_LEFT,        (int)ResizingEdge.WMSZ_LEFT},
            {(int)HitTestValues.HT_RIGHT,       (int)ResizingEdge.WMSZ_RIGHT},
            {(int)HitTestValues.HT_BOTTOM,      (int)ResizingEdge.WMSZ_BOTTOM},
            {(int)HitTestValues.HT_BOTTOMLEFT,  (int)ResizingEdge.WMSZ_BOTTOMLEFT},
            {(int)HitTestValues.HT_BOTTOMRIGHT, (int)ResizingEdge.WMSZ_BOTTOMRIGHT}
        };

        // リサイズが可能なForm境界線からの距離
        private readonly int RESIZE_BORDER_WIDTH = 5;

        // Form表示時のフェードイン・エフェクトの時間(msec)
        private const int FADEIN_ANIMATION_TIME = 250;
        // Formクローズ時のフェードアウト・エフェクトの時間(msec)
        private const int FADEOUT_ANIMATION_TIME = 200;

        // コントロールボックス上にカーソルがある時の色を設定
        private static readonly Color BACKGROUND_HOVER_COLOR = Color.FromArgb(20, 0, 0, 0);
        // コントロールボックス上にカーソルがある時のブラシのインスタンスを生成
        private static readonly Brush BACKGROUND_HOVER_BRUSH = new SolidBrush(BACKGROUND_HOVER_COLOR);
        // コントロールボックスをクリックした時の色を設定
        private static readonly Color BACKGROUND_FOCUS_COLOR = Color.FromArgb(30, 0, 0, 0);
        // コントロールボックスをクリックした時のブラシのインスタンスを生成
        private static readonly Brush BACKGROUND_FOCUS_BRUSH = new SolidBrush(BACKGROUND_FOCUS_COLOR);
        // Formがアクティブでない時のタイトルテキストの色
        private static readonly Color TEXT_DISABLED_COLOR = Color.FromArgb(97, 255, 255, 255); // Alpha 38%

        // タイトルテキストのForm左端からの距離
        private int FORM_PADDING = 14;

        // ディスプレイのDPIを考慮したタイトルバーの高さ
        private int titleBarHeightDPI;

        // ディスプレイのDPIを考慮したForm境界線の線幅
        private int borderWidthDPI;

        // タイトルバーを塗りつぶすブラシ
        private Brush titleBarBrush;

        // タイトルテキストの色
        private Color textColor;

        // リサイズ時の各種カーソル
        private readonly Cursor[] resizeCursors = { Cursors.SizeNESW, Cursors.SizeWE, Cursors.SizeNWSE, Cursors.SizeWE, Cursors.SizeNS };

        // コントロールボックスの左側ボタンの境界線
        private Rectangle leftButtonBounds;
        // コントロールボックスの真ん中ボタンの境界線
        private Rectangle centerButtonBounds;
        // コントロールボックスのクローズボタンの境界線
        private Rectangle xButtonBounds;

        // タイトルバーの境界線
        private Rectangle titleBarBounds;

        // For Drag and move with clicking on Title bar when the form maximized
        private bool isClickingTitleBar = false;
        private Point? diffPoint = null;

        // 最大化・最小化する前のNormalState時のウィンドウサイズ
        private Rectangle oldWindowRect;

        // Formのタイトルのフォント
        private readonly Font titleFont;

        // Fromの枠に影（ドロップシャドウ）をつけることができるかかどうか.(OSの種類によりCheckAeroEnabledにて自動判定)
        private bool aeroEnabled = false;

        // Formがアクティブかどうか
        private bool isFormActivated = false;
        #endregion

        #region Event
        // Formをクローズする前に発生するイベント
        public delegate void BeforeFormClosingEventHandler(object sender, BeforeFormClosingEventArgs e);
        public event BeforeFormClosingEventHandler BeforeFormClosing;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CustomForm()
        {
            InitializeComponent();

            // Formのフォントをタイトルフォントに設定
            titleFont = this.Font;

            // FormBorderStyleをNoneに設定
            FormBorderStyle = FormBorderStyle.None;
            // FormのAutoScaleModeをDipに設定して、高DPI対応にする
            AutoScaleMode = AutoScaleMode.Dpi;

            // ダブルバッファーを有効にする
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);

            // この設定でフォーム上のほかのコントロールの上にカーソルがある時にMouseMove, MouseLDownイベントが発生されるようになる。
            Application.AddMessageFilter(new MouseMessageFilter());
            MouseMessageFilter.MouseMove += OnGlobalMouseMove;
        }
        #endregion

        #region Events
        /// <summary>
        /// FormClosingイベントで任意の処理を実行
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnBeforeFormClosing(BeforeFormClosingEventArgs e)
        {
            if (BeforeFormClosing != null)
            {
                BeforeFormClosing(this, e);
            }
        }
        #endregion

        #region Methods For Window Messages
        /// <summary>
        /// ウインドウプロシージャ
        /// </summary>
        /// <param name="m">メッセージ</param>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            // デザインモードの時はここでこのメソッドを終了する
            if (DesignMode || IsDisposed)
                return;

            switch (m.Msg)
            {
                //case (int)WindowMessages.WM_MOUSEFIRST:
                //    {
                //        Debug.Print("WM_MOUSEFIRST");
                //        break;
                //    }
                //case (int)WindowMessages.WM_MOUSELAST:
                //    {
                //        Debug.Print("WM_MOUSELAST");
                //        break;
                //    }
                case (int)WindowMessages.WM_RBUTTONDOWN: // クライアント領域でマウスの右ボタンを押した時に送られるメッセージ
                    {
                        WmRButtonDown(ref m);
                        break;
                    }
                case (int)WindowMessages.WM_LBUTTONDOWN: // クライアント領域でマウスの左ボタンを押した時に送られるメッセージ
                    {
                        WmLButtonDown(ref m);
                        break;
                    }
                case (int)WindowMessages.WM_NCLBUTTONDOWN: // 非クライアント領域でマウスの左ボタンを押した時に送られるメッセージ
                    {
                        WmNCLButtonDown(ref m);
                        break;
                    }
                //case (int)WindowMessages.WM_LBUTTONUP:
                //    {
                //        WmLButtonUp(ref m);
                //        break;
                //    }
                //case (int)WindowMessages.WM_MOUSEMOVE:
                //    {
                //        WmMouseMove(ref m);
                //        break;
                //    }
                case (int)WindowMessages.WM_NCPAINT: // 非クライアント領域の描画が必要になった時に送られるメッセージ
                    {
                        WmNCPaint(ref m);
                        break;
                    }
                //case (int)WindowMessages.WM_SYSCOMMAND:
                //    {
                //        WmSyscommand(ref m);
                //        break;
                //    }
                //case (int)WindowMessages.WM_WINDOWPOSCHANGING:
                //    {
                //        WmWindowsPosChanging(ref m);
                //        break;
                //    }
                default:
                    break;
            }
        }

        /// <summary>
        /// クライアント領域でマウスの右ボタンを押した時の処理
        /// </summary>
        /// <param name="m">メッセージ</param>
        private void WmRButtonDown(ref Message m)
        {
            // カーソルの座標をクライアント領域の座標に変換
            Point cursorPos = PointToClient(Cursor.Position);

            // 右クリックの座標がタイトルバーの内側の時
            if (titleBarBounds.Contains(cursorPos))
            {
                // システムのデフォルトのメニューを表示する
                var id = NativeMethods.TrackPopupMenuEx(NativeMethods.GetSystemMenu(Handle, false), NativeConstants.TPM_LEFTALIGN | NativeConstants.TPM_RETURNCMD, Cursor.Position.X, Cursor.Position.Y, Handle, IntPtr.Zero);

                // WM_SYSCOMMANDメッセージを送信
                NativeMethods.SendMessage(Handle, (int)WindowMessages.WM_SYSCOMMAND, id, 0);
            }
        }

        /// <summary>
        /// クライアント領域でマウスの左ボタンを押した時の処理
        /// </summary>
        /// <param name="m">メッセージ</param>
        private void WmLButtonDown(ref Message m)
        {
            // カーソルの座標をクライアント領域の座標に変換
            Point cursorPos = PointToClient(Cursor.Position);

            // クリックしながら移動する場合のフラグ
            bool isClickingAndMove = false;

            // 左クリックの座標がタイトルバーの内側の時
            if (titleBarBounds.Contains(cursorPos))
            {
                // 各ボタンが表示/非表示の設定に応じて、クリックしながら移動するかどうかを判定
                if (MinimizeBox && MaximizeBox && !(leftButtonBounds.Contains(cursorPos)
                || centerButtonBounds.Contains(cursorPos)
                || xButtonBounds.Contains(cursorPos)))
                {
                    isClickingAndMove = true;
                }
                else if (((!MinimizeBox && MaximizeBox) || (MinimizeBox && !MaximizeBox))
                && !centerButtonBounds.Contains(cursorPos)
                && !xButtonBounds.Contains(cursorPos))
                {
                    isClickingAndMove = true;
                }
                else if ((!MinimizeBox && !MaximizeBox) && !xButtonBounds.Contains(cursorPos))
                {
                    isClickingAndMove = true;
                }
            }

            // クリックしながら移動の場合
            if (isClickingAndMove)
            {
                if (this.WindowState != FormWindowState.Maximized) // Formが最大化ではない場合
                {
                    // クリックしながらFormを移動する
                    NativeMethods.ReleaseCapture();
                    NativeMethods.SendMessage(Handle, NativeConstants.WM_NCLBUTTONDOWN, (int)HitTestValues.HT_CAPTION, 0);
                }
            }
        }

        /// <summary>
        /// 非クライアント領域でマウスの左ボタンを押した時の処理
        /// </summary>
        /// <param name="m">メッセージ</param>
        private void WmNCLButtonDown(ref Message m)
        {
            // Formがサイズ変更不可の場合、ここでこのメソッドを終了する
            if (!Sizable)
                return;

            byte byteFlag = 0;

            // クリックしているFormの淵を取得
            if (ResizingLocationsToCmd.ContainsKey((int)m.WParam))
                byteFlag = (byte)ResizingLocationsToCmd[(int)m.WParam];

            // クリックしているFormの淵が検出された場合、リサイズを実行する
            if (byteFlag != 0)
                NativeMethods.SendMessage(Handle, (int)WindowMessages.WM_SYSCOMMAND, 0xF000 | byteFlag, (int)m.LParam);
        }

        //private void WmLButtonUp(ref Message m)
        //{
        //}

        //private void WmMouseMove(ref Message m)
        //{
        //}

        /// <summary>
        /// 非クライアント領域(ウインドウのタイトルバーや枠)の描画処理
        /// </summary>
        /// <param name="m">メッセージ</param>
        private void WmNCPaint(ref Message m)
        {
            // ドロップシャドウを描画する
            if (aeroEnabled)
            {
                var v = 2;
                NativeMethods.DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
                MARGINS margins = new MARGINS()
                {
                    bottomHeight = 1,
                    leftWidth = 0,
                    rightWidth = 0,
                    topHeight = 0
                };
                NativeMethods.DwmExtendFrameIntoClientArea(this.Handle, ref margins);
            }
        }

        //private void WmWindowsPosChanging(ref Message m)
        //{
        //}
        #endregion

        #region Methods Override
        /// <summary>
        /// Load イベント
        /// </summary>
        /// <param name="e">EventArgs</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // ディスプレイの解像度を考慮したFormの境界線幅を計算
            borderWidthDPI = (int)(BorderWidth * DPI_SCALE);
            // ディスプレイの解像度を考慮したFormのタイトルバー高さを計算
            titleBarHeightDPI = (int)(TitleBarHeight * DPI_SCALE);
            // ディスプレイの解像度を考慮したFormのタイトルバーのコントロールボックスの幅を計算
            TitleBarButtonWidth = (int)(titleBarHeightDPI * 1.5f);
            // コントロールボックスのボタン領域を計算
            CalcButtonBounds();

            // タイトルバーを塗りつぶすBrushのインスタンスを生成する。
            titleBarBrush = new SolidBrush(TitleBarColor);

            // タイトルバーの文字色を決定する。
            textColor = OptimizedTextColor(TitleBarColor);

            // Formの最小サイズを計算
            if (ControlBox)
            {
                if (MinimizeBox && MaximizeBox)
                {
                    this.MinimumSize = new Size(TitleBarButtonWidth * 3, titleBarHeightDPI);
                }
                else if ((MinimizeBox && !MaximizeBox) || (!MinimizeBox && MaximizeBox))
                {
                    this.MinimumSize = new Size(TitleBarButtonWidth * 2, titleBarHeightDPI);
                }
                else if (!MinimizeBox && !MaximizeBox)
                {
                    this.MinimumSize = new Size(TitleBarButtonWidth, titleBarHeightDPI);
                }
            }
            else
            {
                this.MinimumSize = new Size(TitleBarButtonWidth, titleBarHeightDPI);
            }

            // Formをフェードインしながら表示する
            Opacity = 0;
            Animate(FadeinAnimetionTime, (frame, frequency) =>
            {
                if (!Visible || IsDisposed) return false;
                Opacity = (double)frame / frequency;
                return true;
            });
        }

        /// <summary>
        /// MouseDoubleClick イベント
        /// </summary>
        /// <param name="e">MouseEventArgs</param>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            // コントロールボックスを除くタイトルバーをダブルクリックした場合
            if (titleBarBounds.Contains(e.Location)
                && !leftButtonBounds.Contains(e.Location)
                && !centerButtonBounds.Contains(e.Location)
                && !xButtonBounds.Contains(e.Location))
            {
                // Formが最大化している場合はNormalに変更
                if (this.WindowState == FormWindowState.Maximized)
                {
                    this.FormNormal();
                }
                else // Formが最大化ではない場合は最大化に変更
                {
                    if (Sizable)
                    {
                        this.FormMaximize();
                    }
                }
            }
        }

        /// <summary>
        /// MouseDown イベント
        /// </summary>
        /// <param name="e">MouseEventArgs</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            // デザインモードの場合ここでメソッド終了
            if (DesignMode)
                return;

            base.OnMouseDown(e);

            // Formが最大化している場合
            if (WindowState == FormWindowState.Maximized)
            {
                // コントロールボックスを除くタイトルバーをマウスダウンした場合
                if (e.Button == MouseButtons.Left
                    && titleBarBounds.Contains(e.Location)
                    && !leftButtonBounds.Contains(e.Location)
                    && !centerButtonBounds.Contains(e.Location)
                    && !xButtonBounds.Contains(e.Location))
                {
                    // フラグを更新
                    isClickingTitleBar = true;
                }
                else
                {
                    // コントロールボックスのボタンを更新
                    UpdateButtons(e);
                }
            }
            else
            {
                // コントロールボックスのボタンを更新
                UpdateButtons(e);

                // 左クリックによるマウスダウンかつリサイズ方向が設定されている場合
                if (e.Button == MouseButtons.Left && resizeDir != ResizeDirection.None)
                {
                    // リサイズ
                    ResizeForm(resizeDir);
                }
            }
        }

        /// <summary>
        /// MouseUp イベント
        /// </summary>
        /// <param name="e">MouseEventArgs</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            // デザインモードの場合ここでメソッド終了
            if (DesignMode)
                return;

            base.OnMouseUp(e);

            // 左クリックかつタイトルバーをクリックしている場合
            if (e.Button == MouseButtons.Left && isClickingTitleBar)
            {
                // フラグを更新
                isClickingTitleBar = false;
            }
            else
            {
                // コントロールボックスのボタンを更新
                UpdateButtons(e, true);

                // マウスのキャプチャーを解放する
                NativeMethods.ReleaseCapture();
            }
        }

        /// <summary>
        /// MouseMove イベント
        /// </summary>
        /// <param name="e">MouseEventArgs</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            // デザインモードの場合ここでメソッド終了
            if (DesignMode)
                return;

            base.OnMouseMove(e);

            // タイトルバーをクリックしている時
            if (isClickingTitleBar)
            {
                int x, y;
                Point pt = Cursor.Position; // カーソルの座標を取得

                // Formが最大化している場合
                if (this.WindowState == FormWindowState.Maximized)
                {
                    // Formをノーマルに戻す
                    FormNormal();
                    // Formの移動開始座標を算出
                    diffPoint = new Point(this.Width / 2, RESIZE_BORDER_WIDTH / 2);

                    // 新しい座標を算出
                    x = pt.X - diffPoint.Value.X;
                    y = pt.Y - diffPoint.Value.Y;
                }
                else　// Formが最大化していない場合
                {
                    // 新しい座標を算出
                    x = pt.X - diffPoint.Value.X;
                    y = pt.Y - diffPoint.Value.Y;
                }

                // Formの座標を更新
                this.Location = new Point(x, y);
            }
            else if (Sizable) // Formがサイズ変更可の場合
            {
                // Formが最大化していない場合、Fromのサイズ変更処理
                if (this.WindowState != FormWindowState.Maximized)
                {
                    if (e.Location.X < RESIZE_BORDER_WIDTH && e.Location.Y > Height - RESIZE_BORDER_WIDTH)
                    {
                        resizeDir = ResizeDirection.BottomLeft;
                        Cursor = Cursors.SizeNESW;
                    }
                    else if (e.Location.X < RESIZE_BORDER_WIDTH && e.Location.Y < RESIZE_BORDER_WIDTH)
                    {
                        resizeDir = ResizeDirection.TopLeft;
                        Cursor = Cursors.SizeNWSE;
                    }
                    else if (e.Location.X < RESIZE_BORDER_WIDTH)
                    {
                        resizeDir = ResizeDirection.Left;
                        Cursor = Cursors.SizeWE;
                    }
                    else if (e.Location.X > Width - RESIZE_BORDER_WIDTH && e.Location.Y < RESIZE_BORDER_WIDTH)
                    {
                        resizeDir = ResizeDirection.TopRight;
                        Cursor = Cursors.SizeNESW;
                    }
                    else if (e.Location.X > Width - RESIZE_BORDER_WIDTH && e.Location.Y > Height - RESIZE_BORDER_WIDTH)
                    {
                        resizeDir = ResizeDirection.BottomRight;
                        Cursor = Cursors.SizeNWSE;
                    }
                    else if (e.Location.X > Width - RESIZE_BORDER_WIDTH)
                    {
                        resizeDir = ResizeDirection.Right;
                        Cursor = Cursors.SizeWE;
                    }
                    else if (e.Location.Y > Height - RESIZE_BORDER_WIDTH)
                    {
                        resizeDir = ResizeDirection.Bottom;
                        Cursor = Cursors.SizeNS;
                    }
                    else if (e.Location.Y < RESIZE_BORDER_WIDTH)
                    {
                        resizeDir = ResizeDirection.Top;
                        Cursor = Cursors.SizeNS;
                    }
                    else
                    {
                        resizeDir = ResizeDirection.None;

                        if (resizeCursors.Contains(Cursor))
                        {
                            Cursor = Cursors.Default;
                        }
                    }
                }
                else
                {
                    Cursor = Cursors.Default;
                }
            }

            // コントロールボックスのボタンを更新
            UpdateButtons(e);
        }

        /// <summary>
        /// MouseLeave
        /// </summary>
        /// <param name="e">EventArgs</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            // デザインモードの場合ここでメソッド終了
            if (DesignMode)
                return;

            base.OnMouseLeave(e);

            // buttonStateをNoneに設定
            buttonState = ButtonState.None;

            // 描画更新
            Invalidate();
        }

        /// <summary>
        /// Resizeイベント
        /// </summary>
        /// <param name="e">EventArgs</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // コントロールボックスのボタンの境界を計算
            CalcButtonBounds();
        }

        /// <summary>
        /// Paintイベント
        /// </summary>
        /// <param name="e">PaintEventArgs</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // Graphicsを取得
            var g = e.Graphics;

            //g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // Formの背景をBackDropColorで塗りつぶす
            g.Clear(BackDropColor);

            // タイトルバーを描画
            g.FillRectangle(titleBarBrush, titleBarBounds);

            // Formの境界線を描画
            using (var borderPen = new Pen(BorderColor, borderWidthDPI))
            {
                g.DrawLine(borderPen, new PointF(0, 0), new PointF(0, this.ClientRectangle.Height));
                g.DrawLine(borderPen, new PointF(1, this.ClientRectangle.Height - 1), new PointF(this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1));
                g.DrawLine(borderPen, new PointF(this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 2), new PointF(this.ClientRectangle.Width - 1, 1));
                g.DrawLine(borderPen, new PointF(this.ClientRectangle.Width - 1, 0), new PointF(1, 0));
            }

            // コントロールボックスのどのボタンを描画するかを設定
            bool showMin = MinimizeBox && ControlBox;
            bool showMax = MaximizeBox && ControlBox;
            // コントロールボックスのボタンのマウスホバー時、マウスボタンダウン時の描画Brushを設定
            var hoverBrush = BACKGROUND_HOVER_BRUSH;
            var downBrush = BACKGROUND_FOCUS_BRUSH;

            // 最小化ボタンの表示位置の決定とボタン背景の描画を行う
            if (buttonState == ButtonState.MinOver && showMin)
                g.FillRectangle(hoverBrush, showMax ? leftButtonBounds : centerButtonBounds);
            if (buttonState == ButtonState.MinDown && showMin)
                g.FillRectangle(downBrush, showMax ? leftButtonBounds : centerButtonBounds);

            // 最大化ボタンの表示位置の決定とボタン背景の描画を行う
            if (buttonState == ButtonState.MaxOver && showMax)
                g.FillRectangle(hoverBrush, centerButtonBounds);
            if (buttonState == ButtonState.MaxDown && showMax)
                g.FillRectangle(downBrush, centerButtonBounds);

            // クローズボタンのボタン背景の描画を行う
            if (buttonState == ButtonState.XOver && ControlBox)
                g.FillRectangle(hoverBrush, xButtonBounds);
            if (buttonState == ButtonState.XDown && ControlBox)
                g.FillRectangle(downBrush, xButtonBounds);

            // コントロールボックスのボタン内部の描画
            float offset_x = (leftButtonBounds.Width - leftButtonBounds.Height) / 2f;
            using (var formButtonsPen = new Pen(this.isFormActivated ? this.textColor : TEXT_DISABLED_COLOR, 1f * DPI_SCALE))
            {
                // 最小化ボタン
                if (showMin)
                {
                    int x = showMax ? leftButtonBounds.X : centerButtonBounds.X;
                    int y = showMax ? leftButtonBounds.Y : centerButtonBounds.Y;

                    g.DrawLine(
                        formButtonsPen,
                        x + offset_x + (leftButtonBounds.Height * 0.33f),
                        y + (leftButtonBounds.Height * 0.50f),
                        x + offset_x + (leftButtonBounds.Height * 0.66f),
                        y + (leftButtonBounds.Height * 0.50f));
                }

                // 最大化ボタン
                if (showMax)
                {
                    if (this.WindowState == FormWindowState.Maximized)
                    {
                        float offset = DPI_SCALE;
                        g.DrawRectangle(
                            formButtonsPen,
                            centerButtonBounds.X + offset_x + offset + (int)(centerButtonBounds.Height * 0.33),
                            centerButtonBounds.Y - offset + (int)(centerButtonBounds.Height * 0.33),
                            (int)(centerButtonBounds.Height * 0.35),
                            (int)(centerButtonBounds.Height * 0.35));

                        g.FillRectangle(
                            titleBarBrush,
                            centerButtonBounds.X + offset_x - offset + (int)(centerButtonBounds.Height * 0.33),
                            centerButtonBounds.Y + offset + (int)(centerButtonBounds.Height * 0.33),
                            (int)(centerButtonBounds.Height * 0.35),
                            (int)(centerButtonBounds.Height * 0.35));

                        if (buttonState == ButtonState.MaxOver && showMax)
                        {
                            g.FillRectangle(
                                hoverBrush,
                                centerButtonBounds.X + offset_x - offset + (int)(centerButtonBounds.Height * 0.33),
                                centerButtonBounds.Y + offset + (int)(centerButtonBounds.Height * 0.33),
                                (int)(centerButtonBounds.Height * 0.35),
                                (int)(centerButtonBounds.Height * 0.35));
                        }
                        if (buttonState == ButtonState.MaxDown && showMax)
                        {
                            g.FillRectangle(
                                downBrush,
                                centerButtonBounds.X + offset_x - offset + (int)(centerButtonBounds.Height * 0.33),
                                centerButtonBounds.Y + offset + (int)(centerButtonBounds.Height * 0.33),
                                (int)(centerButtonBounds.Height * 0.35),
                                (int)(centerButtonBounds.Height * 0.35));
                        }

                        g.DrawRectangle(
                            formButtonsPen,
                            centerButtonBounds.X + offset_x - offset + (int)(centerButtonBounds.Height * 0.33),
                            centerButtonBounds.Y + offset + (int)(centerButtonBounds.Height * 0.33),
                            (int)(centerButtonBounds.Height * 0.35),
                            (int)(centerButtonBounds.Height * 0.35));
                    }
                    else
                    {
                        g.DrawRectangle(
                            formButtonsPen,
                            centerButtonBounds.X + offset_x + (int)(centerButtonBounds.Height * 0.33),
                            centerButtonBounds.Y + (int)(centerButtonBounds.Height * 0.33),
                            (int)(centerButtonBounds.Height * 0.35),
                            (int)(centerButtonBounds.Height * 0.35));
                    }
                }

                // クローズボタン
                if (ControlBox)
                {
                    g.DrawLine(
                        formButtonsPen,
                        xButtonBounds.X + offset_x + (int)(xButtonBounds.Height * 0.33),
                        xButtonBounds.Y + (int)(xButtonBounds.Height * 0.33),
                        xButtonBounds.X + offset_x + (int)(xButtonBounds.Height * 0.66),
                        xButtonBounds.Y + (int)(xButtonBounds.Height * 0.66));

                    g.DrawLine(
                        formButtonsPen,
                        xButtonBounds.X + offset_x + (int)(xButtonBounds.Height * 0.66),
                        xButtonBounds.Y + (int)(xButtonBounds.Height * 0.33),
                        xButtonBounds.X + offset_x + (int)(xButtonBounds.Height * 0.33),
                        xButtonBounds.Y + (int)(xButtonBounds.Height * 0.66));
                }
            }

            // Formタイトルのテキスト描画
            // テキスト レンダリングにアンチエイリアスを設定する
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            using (StringFormat sf = new StringFormat(StringFormat.GenericTypographic)) // Formタイトルのテキストの前後の隙間を最小にする
            using (Brush textBrush = new SolidBrush(this.isFormActivated ? this.textColor : TEXT_DISABLED_COLOR))
            {
                sf.Trimming = StringTrimming.EllipsisWord; // テキストが最も近い語にトリムされ、トリムされた行の末尾に省略記号が挿入されるように設定
                sf.Alignment = StringAlignment.Near; // テキストの水平方向位置を左寄せに設定
                sf.LineAlignment = StringAlignment.Center; // テキストの垂直方向位置を中心に設定
                // テキスト描画座標を算出
                RectangleF textLocation = new RectangleF(FORM_PADDING * DPI_SCALE,
                                                         titleBarBounds.Y,
                                                         titleBarBounds.Width - FORM_PADDING * DPI_SCALE - TitleBarButtonWidth * 3,
                                                         titleBarBounds.Height);
                // テキストを描画
                g.DrawString(Text, this.titleFont, textBrush, textLocation, sf);
            }
        }

        /// <summary>
        /// Activatedイベント
        /// </summary>
        /// <param name="e">EventArgs</param>
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            // Formアクティベイト フラグをtrueに設定
            isFormActivated = true;
            // 再描画
            Invalidate();
        }

        /// <summary>
        /// Deactivateイベント
        /// </summary>
        /// <param name="e">EventArgs</param>
        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);

            // Formアクティベイト フラグをfalseに設定
            isFormActivated = false;
            // 再描画
            Invalidate();
        }

        /// <summary>
        /// GlobalMouseMoveイベント
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">MouseEventArgs</param>
        protected void OnGlobalMouseMove(object sender, MouseEventArgs e)
        {
            // デザインモードの場合ここでメソッド終了
            if (IsDisposed)
                return;

            // カーソルの座標をクライアント座標に変換してForm.MouseMoveを処理
            var clientCursorPos = PointToClient(e.Location);
            MouseEventArgs newEvent = new MouseEventArgs(MouseButtons.None, 0, clientCursorPos.X, clientCursorPos.Y, 0);
            OnMouseMove(newEvent);
        }

        /// <summary>
        /// GlobalMouseLDownイベント
        /// </summary>
        /// <param name="sender"><object/param>
        /// <param name="e">MouseEventArgs</param>
        protected void OnGlobalMouseLDown(object sender, MouseEventArgs e)
        {
            // デザインモードの場合ここでメソッド終了
            if (IsDisposed)
                return;

            // カーソルの座標をクライアント座標に変換してForm.OnMouseDownを処理
            var clientCursorPos = PointToClient(e.Location);
            MouseEventArgs newEvent = new MouseEventArgs(MouseButtons.Left, 0, clientCursorPos.X, clientCursorPos.Y, 0);
            OnMouseDown(newEvent);
        }

        /// <summary>
        /// GlobalMouseLUpイベント
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">MouseEventArgs</param>
        protected void OnGlobalMouseLUp(object sender, MouseEventArgs e)
        {
            // デザインモードの場合ここでメソッド終了
            if (IsDisposed)
                return;

            // カーソルの座標をクライアント座標に変換してForm.OnMouseUpを処理
            var clientCursorPos = PointToClient(e.Location);
            MouseEventArgs newEvent = new MouseEventArgs(MouseButtons.Left, 0, clientCursorPos.X, clientCursorPos.Y, 0);
            OnMouseUp(newEvent);
        }
        #endregion

        #region Methods
        /// <summary>
        /// コントロールボックスのボタンの状態を更新
        /// </summary>
        /// <param name="e">MouseEventArgs</param>
        /// <param name="up">MouseUp処理直後の場合はtrueに設定する.デフォルトの値はfalse</param>
        private void UpdateButtons(MouseEventArgs e, bool up = false)
        {
            // デザインモードの場合ここでメソッド終了
            if (DesignMode)
                return;

            // 更新前のButtonStateを保存
            var oldState = buttonState;
            // 最小化ボタンの表示の可否を取得
            bool showMin = MinimizeBox && ControlBox;
            // 最大化ボタンの表示の可否を取得
            bool showMax = MaximizeBox && ControlBox;

            // 左ボタンクリックのマウスアップではない場合
            if (e.Button == MouseButtons.Left && !up)
            {
                if (showMin && !showMax && centerButtonBounds.Contains(e.Location))
                    buttonState = ButtonState.MinDown;
                else if (showMin && showMax && leftButtonBounds.Contains(e.Location))
                    buttonState = ButtonState.MinDown;
                else if (showMax && centerButtonBounds.Contains(e.Location))
                    buttonState = ButtonState.MaxDown;
                else if (ControlBox && xButtonBounds.Contains(e.Location))
                    buttonState = ButtonState.XDown;
                else
                    buttonState = ButtonState.None;
            }
            else // 右ボタンクリックまたはマウスアップの場合
            {
                if (showMin && !showMax && centerButtonBounds.Contains(e.Location))
                {
                    buttonState = ButtonState.MinOver;

                    if (oldState == ButtonState.MinDown && up)
                    {
                        this.FormMinimize();
                    }
                }
                else if (showMin && showMax && leftButtonBounds.Contains(e.Location))
                {
                    buttonState = ButtonState.MinOver;

                    if (oldState == ButtonState.MinDown && up)
                    {
                        this.FormMinimize();
                    }
                }
                else if (MaximizeBox && ControlBox && centerButtonBounds.Contains(e.Location))
                {
                    buttonState = ButtonState.MaxOver;

                    if (oldState == ButtonState.MaxDown && up)
                    {
                        if (this.WindowState != FormWindowState.Maximized)
                        {
                            this.FormMaximize();
                        }
                        else
                        {
                            this.FormNormal();
                        }
                    }
                }
                else if (ControlBox && xButtonBounds.Contains(e.Location))
                {
                    buttonState = ButtonState.XOver;

                    if (oldState == ButtonState.XDown && up)
                    {
                        // DataUpdatedイベントの発生
                        BeforeFormClosingEventArgs evt = new BeforeFormClosingEventArgs();
                        OnBeforeFormClosing(evt);

                        if (!evt.Cancel)
                            FormClosingWithAnimetion();
                    }
                }
                else
                    buttonState = ButtonState.None;
            }

            // 更新の前後でButtonStateの値が異なるとき、再描画する
            if (oldState != buttonState)
                Refresh();
        }

        /// <summary>
        /// Formのリサイズを行う
        /// </summary>
        /// <param name="direction"></param>
        private void ResizeForm(ResizeDirection direction)
        {
            // デザインモードの場合ここでメソッド終了
            if (DesignMode)
                return;

            // Formのリサイズの方向を検出
            var dir = -1;
            switch (direction)
            {
                case ResizeDirection.BottomLeft:
                    dir = (int)HitTestValues.HT_BOTTOMLEFT;
                    break;

                case ResizeDirection.Left:
                    dir = (int)HitTestValues.HT_LEFT;
                    break;

                case ResizeDirection.Right:
                    dir = (int)HitTestValues.HT_RIGHT;
                    break;

                case ResizeDirection.BottomRight:
                    dir = (int)HitTestValues.HT_BOTTOMRIGHT;
                    break;

                case ResizeDirection.Bottom:
                    dir = (int)HitTestValues.HT_BOTTOM;
                    break;

                case ResizeDirection.TopLeft:
                    dir = (int)HitTestValues.HT_TOPLEFT;
                    break;

                case ResizeDirection.Top:
                    dir = (int)HitTestValues.HT_TOP;
                    break;

                case ResizeDirection.TopRight:
                    dir = (int)HitTestValues.HT_TOPRIGHT;
                    break;
            }

            // リサイズの方向を検出できた場合は非クライアント領域の左ボタンダウンメッセージを送信
            NativeMethods.ReleaseCapture();
            if (dir != -1)
            {
                NativeMethods.SendMessage(Handle, NativeConstants.WM_NCLBUTTONDOWN, dir, 0);
            }
        }

        /// <summary>
        /// Formをクローズする際のフェードアウト アニメーションを実行
        /// </summary>
        private void FormClosingWithAnimetion()
        {
            Opacity = 1;
            Animate(FadeoutAnimetionTime, (frame, frequency) =>
            {
                Opacity = (1d - (double)frame / frequency);

                if (Opacity == 0)
                    Close();
                return true;
            });
        }

        /// <summary>
        /// Formを最小化する
        /// </summary>
        private void FormMinimize()
        {
            // 最小化前のFormの位置とサイズを保存
            this.oldWindowRect = this.ClientRectangle;
            // 最小化するためFormBorderStyleをSizableにしなければならないため、最小化前のFormBorderStyleを保存しておく
            FormBorderStyle lastStyle = this.FormBorderStyle;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            // Formを最小化する
            WindowState = FormWindowState.Minimized;
            // FormBorderStyleをもとに戻す
            this.FormBorderStyle = lastStyle;
        }

        /// <summary>
        /// Formを最大化する
        /// </summary>
        private void FormMaximize()
        {
            // 最大化前のFormの位置とサイズを保存
            this.oldWindowRect = this.ClientRectangle;
            // Formが表示されているディスプレイを取得（マルチディスプレイ対応）
            Screen currentScreen = Screen.FromControl(this);
            // WorkingAreaを取得（タスクバーを除くサイズ）
            Rectangle workArea = currentScreen.WorkingArea;
            // Formを最大化する
            this.MaximumSize = workArea.Size;
            // WindowStateをMaximizedに変更
            this.WindowState = FormWindowState.Maximized;
        }

        /// <summary>
        /// Formを最大化または最小化から通常の状態にする
        /// </summary>
        private void FormNormal()
        {
            // 保存したFormの位置とサイズに設定
            this.Location = new Point(this.oldWindowRect.X, this.oldWindowRect.Y);
            this.Size = new Size(this.oldWindowRect.Width, this.oldWindowRect.Height);
            // WindowStateをNormalに変更
            this.WindowState = FormWindowState.Normal;
        }

        /// <summary>
        /// Windows Aeroが使えるかどうか確認
        /// </summary>
        /// <returns></returns>
        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6) // Vista以上であることを確認
            {
                int enabled = 0;
                // Windows Aeroが使えるかどうか確認
                NativeMethods.DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }

        /// <summary>
        /// コントロールボックスのボタンおよびタイトルバーの境界を計算
        /// </summary>
        private void CalcButtonBounds()
        {
            leftButtonBounds = new Rectangle(Width - 3 * TitleBarButtonWidth, 0, TitleBarButtonWidth, titleBarHeightDPI);
            centerButtonBounds = new Rectangle(Width - 2 * TitleBarButtonWidth, 0, TitleBarButtonWidth, titleBarHeightDPI);
            xButtonBounds = new Rectangle(Width - TitleBarButtonWidth, 0, TitleBarButtonWidth, titleBarHeightDPI);
            titleBarBounds = new Rectangle(0, 0, Width, titleBarHeightDPI);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CustomForm
            // 
            this.ClientSize = new Size(284, 261);
            this.Name = "CustomForm";
            this.ResumeLayout(false);

        }
        #endregion

        #region Text Color Optimization
        // 背景色から黒文字か白文字の見やすい方を自動判定 （WCAG 2.0 利用版）
        // https://qiita.com/mainy/items/f5540f33d37d8ce6a46e

        /// <summary>
        /// RGB から相対輝度を算出（0.0 ～ 1.0）
        /// </summary>
        /// <param name="R">Red</param>
        /// <param name="G">Green</param>
        /// <param name="B">Blue</param>
        /// <returns>ni</returns>
        private double RelativeLuminance(byte R, byte G, byte B)
        {
            // RGB の各値を相対輝度算出用に変換
            Func<byte, double> toRgb = (rgb) => {
                double srgb = (double)rgb / 255;
                return srgb <= 0.03928 ? srgb / 12.92 : Math.Pow((srgb + 0.055) / 1.055, 2.4);
            };

            return 0.2126 * toRgb(R) + 0.7152 * toRgb(G) + 0.0722 * toRgb(B);
        }

        /// <summary>
        /// 2つの相対輝度値から、相対輝度比率を算出（0.0 ～ 21.0）
        /// </summary>
        /// <param name="relativeLuminance1">比較する輝度1</param>
        /// <param name="relativeLuminance2">比較する輝度2</param>
        /// <returns></returns>
        private double RelativeLuminanceRatio(double relativeLuminance1, double relativeLuminance2)
        {
            // 相対輝度比率 = (大きい値 + 0.05) / (小さい値 + 0.05)
            return (Math.Max(relativeLuminance1, relativeLuminance2) + 0.05) / (Math.Min(relativeLuminance1, relativeLuminance2) + 0.05);
        }

        /// <summary>
        /// 背景色から白文字か黒文字を判定
        /// </summary>
        /// <param name="backgroundColor">背景色</param>
        /// <returns>最適化された文字色</returns>
        //private Color OptimizedTextColor(byte R, byte G, byte B)
        private Color OptimizedTextColor(Color backgroundColor)
        {
            // 背景色の相対輝度
            double background = RelativeLuminance(backgroundColor.R, backgroundColor.G, backgroundColor.B);

            const double white = 1.0D;  // 白の相対輝度
            const double black = 0.0D;  // 黒の相対輝度

            // 文字色と背景色のコントラスト比を計算
            double whiteContrast = RelativeLuminanceRatio(white, background);   // 文字色：白との比
            double blackContrast = RelativeLuminanceRatio(black, background);   // 文字色：黒との比

            // コントラスト比が大きい文字色を採用
            return whiteContrast < blackContrast ? Color.Black : Color.White;
        }
        #endregion

        #region Animation Related Methods
        /// <summary>
        /// 1 フレームの時間とフレーム数を指定してアニメーション機能を提供します。
        /// </summary>
        /// <param name="interval">1 フレームの時間をミリ秒単位で指定します。</param>
        /// <param name="frequency">
        /// frequency はコールバックが呼ばれる回数から 1 を引いたものです。例えば frequency が 10 の時には 11 回呼ばれます。
        /// </param>
        /// <param name="callback">
        /// bool callback(int frame, int frequency) の形でコールバックを指定します。
        /// frame は 0 から frequency の値まで 1 ずつ増加します。
        /// frequency は引数と同じものです。
        /// </param>
        public void Animate(int interval, int frequency, Func<int, int, bool> callback)
        {
            var timer = new System.Windows.Forms.Timer();
            timer.Interval = interval;
            int frame = 0;
            timer.Tick += (sender, e) =>
            {
                if (callback(frame, frequency) == false || frame >= frequency)
                {
                    timer.Stop();
                }
                frame++;
            };
            timer.Start();
        }

        /// <summary>
        /// 持続時間を指定してアニメーション機能を提供します。
        /// </summary>
        /// <param name="duration">持続時間をミリ秒単位で指定します。</param>
        /// <param name="callback">
        /// bool callback(int frame, int frequency) の形でコールバックを指定します。
        /// frame は 0 から frequency の値まで 1 ずつ増加します。
        /// frequency はコールバックが呼ばれる回数から 1 を引いたものです。例えば frequency が 10 の時には 11 回呼ばれます。
        /// </param>
        public void Animate(int duration, Func<int, int, bool> callback)
        {
            const int interval = 25;
            if (duration < interval) duration = interval;
            Animate(25, duration / interval, callback);
        }
        #endregion
    }


    /// <summary>
    /// メッセージフィルタを登録してフォーム上のほかのコントロールの上にカーソルがある時に
    /// MouseMove, MouseLDownイベントが発生されるようにする
    /// </summary>
    public class MouseMessageFilter : IMessageFilter
    {
        public static event MouseEventHandler MouseMove;
        public static event MouseEventHandler MouseLDown;
        public static event MouseEventHandler MouseLUp;

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == (int)WindowMessages.WM_MOUSEMOVE)
            {
                if (MouseMove != null)
                {
                    int x = Control.MousePosition.X, y = Control.MousePosition.Y;
                    MouseMove(null, new MouseEventArgs(MouseButtons.None, 0, x, y, 0));
                }
            }
            else if (m.Msg == (int)WindowMessages.WM_LBUTTONDOWN)
            {
                if (MouseLDown != null)
                {
                    int x = Control.MousePosition.X, y = Control.MousePosition.Y;
                    MouseLDown(null, new MouseEventArgs(MouseButtons.Left, 1, x, y, 0));
                }
            }
            else if (m.Msg == (int)WindowMessages.WM_LBUTTONUP)
            {
                if (MouseLUp != null)
                {
                    int x = Control.MousePosition.X, y = Control.MousePosition.Y;
                    MouseLUp(null, new MouseEventArgs(MouseButtons.Left, 1, x, y, 0));
                }
            }

            return false;
        }
    }
}
