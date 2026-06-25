#nullable disable

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace Text_Editor
{
    public class AppSettings
    {
        public int WindowX { get; set; } = 100;
        public int WindowY { get; set; } = 100;
        public int WindowWidth { get; set; } = 900;
        public int WindowHeight { get; set; } = 600;
        public bool IsMaximized { get; set; } = false;
        public bool IsDarkTheme { get; set; } = true;
        public bool WordWrap { get; set; } = true;
        public string FontName { get; set; } = "Consolas";
        public float FontSize { get; set; } = 10f;
        public bool ToolbarVisible { get; set; } = true;
        public bool RegisterContextMenu { get; set; } = false;

        private static readonly string SettingsDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "TextEditor");
        private static readonly string SettingsPath = Path.Combine(SettingsDir, "settings.json");

        public static AppSettings Load()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    string json = File.ReadAllText(SettingsPath);
                    return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
                }
            }
            catch { }
            return new AppSettings();
        }

        public void Save()
        {
            try
            {
                Directory.CreateDirectory(SettingsDir);
                string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(SettingsPath, json);
            }
            catch { }
        }
    }

    struct TextState
    {
        public string Text;
        public int CursorPos;
    }

    public class DarkToolStripRenderer : ToolStripProfessionalRenderer
    {
        public DarkToolStripRenderer() : base(new DarkColorTable()) 
        { 
            this.RoundedEdges = false;
        }

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            Rectangle rect = new Rectangle(Point.Empty, e.Item.Size);
            int radius = 5;

            if (e.Item is ToolStripButton btn && btn.Checked)
            {
                using (var brush = new SolidBrush(Color.FromArgb(68, 68, 72)))
                using (var path = RoundedRect(rect, radius))
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(brush, path);
                }
            }
            else if (e.Item.Selected || e.Item.Pressed)
            {
                Color bgColor = e.Item.Pressed 
                    ? Color.FromArgb(70, 70, 74) 
                    : Color.FromArgb(65, 65, 69);
                using (var brush = new SolidBrush(bgColor))
                using (var path = RoundedRect(rect, radius))
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(brush, path);
                }
            }
        }

        public static System.Drawing.Drawing2D.GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            using (var pen = new Pen(Color.FromArgb(100, 100, 100), 1))
            {
                e.Graphics.DrawLine(pen, 0, e.ToolStrip.Height - 1, e.ToolStrip.Width, e.ToolStrip.Height - 1);
            }
        }
        
        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            int margin = 10;
            using (var pen = new Pen(Color.FromArgb(100, 100, 100), 1))
            {
                e.Graphics.DrawLine(pen, e.Item.Width / 2, margin, e.Item.Width / 2, e.Item.Height - margin);
            }
        }
    }

    public class DarkColorTable : ProfessionalColorTable
    {
        public override Color ToolStripGradientBegin => Color.FromArgb(45, 45, 48);
        public override Color ToolStripGradientMiddle => Color.FromArgb(45, 45, 48);
        public override Color ToolStripGradientEnd => Color.FromArgb(45, 45, 48);
        public override Color ToolStripBorder => Color.Transparent;
        public override Color MenuItemSelected => Color.FromArgb(65, 65, 69);
        public override Color MenuItemBorder => Color.Transparent;
        public override Color ButtonSelectedHighlight => Color.FromArgb(65, 65, 69);
        public override Color ButtonSelectedHighlightBorder => Color.Transparent;
        public override Color ButtonPressedHighlight => Color.FromArgb(70, 70, 74);
        public override Color ButtonPressedHighlightBorder => Color.Transparent;
        public override Color ButtonCheckedHighlight => Color.FromArgb(68, 68, 72);
        public override Color ButtonCheckedHighlightBorder => Color.Transparent;
    }

    public class LightToolStripRenderer : ToolStripProfessionalRenderer
    {
        public LightToolStripRenderer() : base(new LightColorTable()) 
        { 
            this.RoundedEdges = false;
        }

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            Rectangle rect = new Rectangle(Point.Empty, e.Item.Size);
            int radius = 5;

            if (e.Item is ToolStripButton btn && btn.Checked)
            {
                using (var brush = new SolidBrush(Color.FromArgb(200, 200, 200)))
                using (var path = DarkToolStripRenderer.RoundedRect(rect, radius))
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(brush, path);
                }
            }
            else if (e.Item.Selected || e.Item.Pressed)
            {
                Color bgColor = e.Item.Pressed 
                    ? Color.FromArgb(200, 200, 200) 
                    : Color.FromArgb(220, 220, 220);
                using (var brush = new SolidBrush(bgColor))
                using (var path = DarkToolStripRenderer.RoundedRect(rect, radius))
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(brush, path);
                }
            }
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            using (var pen = new Pen(Color.FromArgb(200, 200, 200), 1))
            {
                e.Graphics.DrawLine(pen, 0, e.ToolStrip.Height - 1, e.ToolStrip.Width, e.ToolStrip.Height - 1);
            }
        }
        
        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            int margin = 10;
            using (var pen = new Pen(Color.FromArgb(200, 200, 200), 1))
            {
                e.Graphics.DrawLine(pen, e.Item.Width / 2, margin, e.Item.Width / 2, e.Item.Height - margin);
            }
        }
    }

    public class LightColorTable : ProfessionalColorTable
    {
        public override Color ToolStripGradientBegin => Color.FromArgb(245, 245, 245);
        public override Color ToolStripGradientMiddle => Color.FromArgb(245, 245, 245);
        public override Color ToolStripGradientEnd => Color.FromArgb(245, 245, 245);
        public override Color ToolStripBorder => Color.Transparent;
        public override Color MenuItemSelected => Color.FromArgb(220, 220, 220);
        public override Color MenuItemBorder => Color.Transparent;
        public override Color ButtonSelectedHighlight => Color.FromArgb(220, 220, 220);
        public override Color ButtonSelectedHighlightBorder => Color.Transparent;
        public override Color ButtonPressedHighlight => Color.FromArgb(200, 200, 200);
        public override Color ButtonPressedHighlightBorder => Color.Transparent;
        public override Color ButtonCheckedHighlight => Color.FromArgb(200, 200, 200);
        public override Color ButtonCheckedHighlightBorder => Color.Transparent;
    }

    /// <summary>
    /// RichTextBox dengan seleksi teks karakter-per-karakter yang benar.
    ///
    /// RichTextBox bawaan WinForms memiliki dua masalah snap-select:
    ///   1. Auto Word Select: saat drag dalam satu kata, langsung snap ke seluruh kata.
    ///   2. Cross-space snap: saat drag melewati spasi antar kata, snap ke kedua kata
    ///      di kiri dan kanan spasi sekaligus.
    ///
    /// Fix: matikan ECO_AUTOWORDSELECTION via EM_SETOPTIONS, lalu override WndProc
    /// untuk intercept WM_LBUTTONDOWN dan WM_MOUSEMOVE. Pada mouse-drag, kita hitung
    /// sendiri char index dari posisi mouse dan set selection secara manual,
    /// sehingga RichEdit internal word-boundary logic sama sekali tidak jalan.
    /// </summary>
    public class NoAutoWordSelectRichTextBox : RichTextBox
    {
        // Win32 constants
        private const int ECO_AUTOWORDSELECTION = 0x00000001;
        private const int EM_SETOPTIONS         = 0x044D;
        private const int ECOOP_XOR             = 0x0002;

        private const int WM_LBUTTONDOWN        = 0x0201;
        private const int WM_LBUTTONDBLCLK      = 0x0203;
        private const int WM_MOUSEMOVE          = 0x0200;
        private const int WM_LBUTTONUP          = 0x0202;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        // State untuk manual drag-select
        private bool _manualDrag = false;
        private int  _dragAnchor = -1;

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            SendMessage(this.Handle,
                        EM_SETOPTIONS,
                        (IntPtr)ECOOP_XOR,
                        (IntPtr)ECO_AUTOWORDSELECTION);
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_LBUTTONDOWN:
                {
                    int charIdx = CharIndexFromLParam(m.LParam);
                    if (charIdx >= 0)
                    {
                        _dragAnchor = charIdx;
                        _manualDrag = true;

                        if (!this.Focused) this.Focus();

                        this.SelectionStart  = charIdx;
                        this.SelectionLength = 0;
                        this.Capture = true;

                        // Jangan teruskan ke base agar RichEdit tidak jalankan
                        // word-boundary logic-nya sendiri
                        m.Result = IntPtr.Zero;
                        return;
                    }
                    break;
                }

                case WM_MOUSEMOVE:
                {
                    if (_manualDrag && _dragAnchor >= 0 &&
                        (m.WParam.ToInt32() & 0x0001) != 0) // MK_LBUTTON
                    {
                        int charIdx = CharIndexFromLParam(m.LParam);
                        if (charIdx >= 0)
                        {
                            int start  = Math.Min(_dragAnchor, charIdx);
                            int length = Math.Abs(charIdx - _dragAnchor);
                            this.SelectionStart  = start;
                            this.SelectionLength = length;
                        }
                        m.Result = IntPtr.Zero;
                        return;
                    }
                    break;
                }

                case WM_LBUTTONUP:
                {
                    if (_manualDrag)
                    {
                        _manualDrag  = false;
                        _dragAnchor  = -1;
                        this.Capture = false;
                        m.Result     = IntPtr.Zero;
                        return;
                    }
                    break;
                }

                case WM_LBUTTONDBLCLK:
                {
                    // Double-click: biarkan RichEdit select satu kata (behavior normal)
                    _manualDrag = false;
                    _dragAnchor = -1;
                    break;
                }
            }

            base.WndProc(ref m);
        }

        /// <summary>
        /// Konversi posisi mouse (client coords dari lParam) ke char index yang tepat.
        ///
        /// Masalah: GetCharIndexFromPosition selalu clamp ke karakter yang ADA,
        /// tidak pernah mengembalikan TextLength (posisi setelah karakter terakhir).
        /// Akibatnya klik di sebelah kanan karakter terakhir selalu jatuh di karakter
        /// terakhir, bukan di posisi end-of-text.
        ///
        /// Fix: setelah dapat char index, ambil posisi pixel karakter tersebut dan
        /// karakter berikutnya. Jika posisi X mouse berada di sebelah kanan titik
        /// tengah karakter itu, increment index-nya.
        /// </summary>
        private int CharIndexFromLParam(IntPtr lParam)
        {
            int   val    = lParam.ToInt32();
            short mouseX = (short)(val & 0xFFFF);
            short mouseY = (short)((val >> 16) & 0xFFFF);
            var   pt     = new Point(mouseX, mouseY);

            int idx = this.GetCharIndexFromPosition(pt);

            // Dapatkan posisi pixel karakter idx
            Point charPos = this.GetPositionFromCharIndex(idx);

            // Hitung midpoint X karakter: bandingkan dengan posisi char berikutnya
            int charMidX;
            if (idx + 1 <= this.TextLength)
            {
                Point nextPos = this.GetPositionFromCharIndex(
                    Math.Min(idx + 1, this.TextLength));
                // Hanya increment jika char berikutnya masih di baris yang sama
                if (nextPos.Y == charPos.Y)
                    charMidX = (charPos.X + nextPos.X) / 2;
                else
                    charMidX = int.MaxValue; // akhir baris, jangan increment
            }
            else
            {
                charMidX = int.MaxValue; // sudah di char terakhir, jangan increment
            }

            if (mouseX > charMidX)
                idx = Math.Min(idx + 1, this.TextLength);

            return Math.Max(0, Math.Min(idx, this.TextLength));
        }
    }

    public class MainForm : Form
    {
        private NoAutoWordSelectRichTextBox _editBox;
        private ToolStrip   _toolbar;
        private StatusStrip _statusBar;
        private ToolStripStatusLabel _statusPart0;
        private ToolStripStatusLabel _statusPart1;
        private ToolStripStatusLabel _statusPart2;
        private ToolStripButton      _wordWrapBtn;
        private System.Windows.Forms.Timer _statusTimer;

        private string _currentFile  = string.Empty;
        private bool   _modified     = false;
        private bool   _wordWrap     = true;

        private bool   _isDarkTheme  = true;
        private bool   _ignoreChange = false;
        private AppSettings _settings;

        private readonly Stack<TextState> _undoStack = new Stack<TextState>();
        private readonly Stack<TextState> _redoStack = new Stack<TextState>();
        private const int MaxUndo = 1000;

        public MainForm(string[] args)
        {
            _settings = AppSettings.Load();
            _isDarkTheme = _settings.IsDarkTheme;
            _wordWrap = _settings.WordWrap;

            InitializeUI();
            ApplySettings();
            UpdateTitle();
            UpdateStatusBar();

            if (args.Length > 0 && File.Exists(args[0]))
            {
                LoadFile(args[0]);
            }

            if (_settings.RegisterContextMenu)
                RegisterContextMenuHandler(true);
        }

        private void InitializeUI()
        {
            this.Text            = "Untitled - Text Editor";
            this.MinimumSize     = new Size(400, 300);

var iconStream = typeof(MainForm).Assembly.GetManifestResourceStream("icon.ico");
if (iconStream != null) this.Icon = new Icon(iconStream);
else this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            _toolbar = new ToolStrip { 
                GripStyle = ToolStripGripStyle.Hidden, 
                AutoSize = false,
                Height = 45
            };
            _toolbar.Items.Add(MakeTButton("New",   OnNew));
            _toolbar.Items.Add(MakeTButton("Open",  OnOpen));
            _toolbar.Items.Add(MakeTButton("Save",  OnSave));
            _toolbar.Items.Add(MakeTButton("Cut",   OnCut));
            _toolbar.Items.Add(MakeTButton("Copy",  OnCopy));
            _toolbar.Items.Add(MakeTButton("Paste", OnPaste));
            _toolbar.Items.Add(MakeTButton("Undo",  OnUndo));
            _toolbar.Items.Add(MakeTButton("Redo",  OnRedo));
            _toolbar.Items.Add(MakeTButton("Find",   OnFind));
            _toolbar.Items.Add(MakeTButton("Go To",  OnGotoLine));
            _toolbar.Items.Add(new ToolStripSeparator());
            _wordWrapBtn = new ToolStripButton("Word Wrap") { 
                CheckOnClick = true, 
                Checked = _wordWrap,
                AutoSize = false,
                Width = 75,
                Height = 35,
                Margin = new Padding(2, 5, 2, 5)
            };
            _wordWrapBtn.Click += (s, e) => ToggleWordWrap();
            _toolbar.Items.Add(_wordWrapBtn);
            
            var settingsBtn = MakeTButton("Settings", OnSettings);
            _toolbar.Items.Add(settingsBtn);

            this.Controls.Add(_toolbar);

            _editBox = new NoAutoWordSelectRichTextBox
            {
                Dock          = DockStyle.Fill,
                AcceptsTab    = true,
                ScrollBars    = RichTextBoxScrollBars.Both,
                WordWrap      = _wordWrap,
                Font          = GetDefaultFont(),
                HideSelection = false,
                DetectUrls    = false,
                BorderStyle   = BorderStyle.None,
            };
            _editBox.TextChanged   += OnTextChanged;
            _editBox.SelectionChanged += (s, e) => UpdateStatusBar();
            _editBox.DragEnter += (s, e) => { e.Effect = e.Data.GetDataPresent(DataFormats.Text) ? DragDropEffects.Copy : DragDropEffects.None; };
            _editBox.DragDrop  += (s, e) => { if (e.Data.GetDataPresent(DataFormats.Text)) _editBox.SelectedText = e.Data.GetData(DataFormats.Text).ToString(); };

            var ctxMenu = new ContextMenuStrip();
            var undoItem    = new ToolStripMenuItem("Undo")    { ShortcutKeys = Keys.Control | Keys.Z };
            var cutItem     = new ToolStripMenuItem("Cut")     { ShortcutKeys = Keys.Control | Keys.X };
            var copyItem    = new ToolStripMenuItem("Copy")    { ShortcutKeys = Keys.Control | Keys.C };
            var pasteItem   = new ToolStripMenuItem("Paste")   { ShortcutKeys = Keys.Control | Keys.V };
            var deleteItem  = new ToolStripMenuItem("Delete")  { ShortcutKeys = Keys.Delete };
            var selAllItem  = new ToolStripMenuItem("Select All") { ShortcutKeys = Keys.Control | Keys.A };

            undoItem.Click   += (s, e) => OnUndo(s, e);
            cutItem.Click    += (s, e) => _editBox.Cut();
            copyItem.Click   += (s, e) => _editBox.Copy();
            pasteItem.Click  += (s, e) => _editBox.Paste();
            deleteItem.Click += (s, e) => { if (_editBox.SelectionLength > 0) _editBox.SelectedText = string.Empty; };
            selAllItem.Click += (s, e) => _editBox.SelectAll();

            undoItem.Click += (s, e) =>
            {
                undoItem.Enabled   = _undoStack.Count > 0;
                cutItem.Enabled    = _editBox.SelectionLength > 0;
                copyItem.Enabled   = _editBox.SelectionLength > 0;
                deleteItem.Enabled = _editBox.SelectionLength > 0;
            };
            cutItem.Click += (s, e) =>
            {
                cutItem.Enabled    = _editBox.SelectionLength > 0;
                copyItem.Enabled   = _editBox.SelectionLength > 0;
                deleteItem.Enabled = _editBox.SelectionLength > 0;
            };

            ctxMenu.Items.AddRange(new ToolStripItem[] {
                undoItem,
                new ToolStripSeparator(),
                cutItem, copyItem, pasteItem, deleteItem,
                new ToolStripSeparator(),
                selAllItem
            });

            ctxMenu.Opening += (s, e) =>
            {
                undoItem.Enabled   = _undoStack.Count > 0;
                cutItem.Enabled    = _editBox.SelectionLength > 0;
                copyItem.Enabled   = _editBox.SelectionLength > 0;
                deleteItem.Enabled = _editBox.SelectionLength > 0;
            };

            _editBox.ContextMenuStrip = ctxMenu;

            var editorPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(5, 3, 5, 3) };
            editorPanel.Controls.Add(_editBox);
            this.Controls.Add(editorPanel);

            _statusBar   = new StatusStrip();
            _statusPart0 = new ToolStripStatusLabel("Line 1, Col 1") { AutoSize = true };
            _statusPart1 = new ToolStripStatusLabel("0 lines, 0 chars") { AutoSize = true };
            _statusPart2 = new ToolStripStatusLabel("UTF-8") { AutoSize = true };
            var _statusPart3 = new ToolStripStatusLabel("Ctrl+T: Toggle Toolbar") { AutoSize = true, Spring = true, TextAlign = ContentAlignment.MiddleRight };
            _statusBar.Items.AddRange(new ToolStripItem[] { _statusPart0, _statusPart1, _statusPart2, _statusPart3 });
            this.Controls.Add(_statusBar);

            _statusTimer          = new System.Windows.Forms.Timer { Interval = 500 };
            _statusTimer.Tick    += (s, e) => UpdateStatusBar();
            _statusTimer.Start();

            this.KeyPreview = true;
            this.KeyDown   += OnKeyDown;

            editorPanel.BringToFront();
            
            ApplyTheme();
        }

        private void ApplySettings()
        {
            var screen = Screen.FromPoint(new Point(_settings.WindowX, _settings.WindowY));
            int x = Math.Max(screen.WorkingArea.Left, Math.Min(_settings.WindowX, screen.WorkingArea.Right - 200));
            int y = Math.Max(screen.WorkingArea.Top, Math.Min(_settings.WindowY, screen.WorkingArea.Bottom - 200));
            int w = Math.Max(400, Math.Min(_settings.WindowWidth, screen.WorkingArea.Width));
            int h = Math.Max(300, Math.Min(_settings.WindowHeight, screen.WorkingArea.Height));

            this.Location = new Point(x, y);
            this.Size = new Size(w, h);

            if (_settings.IsMaximized)
                this.WindowState = FormWindowState.Maximized;

            try { _editBox.Font = new Font(_settings.FontName, _settings.FontSize); }
            catch { _editBox.Font = GetDefaultFont(); }

            _wordWrap = _settings.WordWrap;
            _editBox.WordWrap = _wordWrap;
            _wordWrapBtn.Checked = _wordWrap;
            _editBox.ScrollBars = _wordWrap
                ? RichTextBoxScrollBars.Vertical
                : RichTextBoxScrollBars.Both;

            ApplyTheme();

            _toolbar.Visible = _settings.ToolbarVisible;
        }

        private void SaveSettings()
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                _settings.WindowX = this.Location.X;
                _settings.WindowY = this.Location.Y;
                _settings.WindowWidth = this.Size.Width;
                _settings.WindowHeight = this.Size.Height;
            }
            else
            {
                var bounds = this.RestoreBounds;
                _settings.WindowX = bounds.X;
                _settings.WindowY = bounds.Y;
                _settings.WindowWidth = bounds.Width;
                _settings.WindowHeight = bounds.Height;
            }
            _settings.IsMaximized = this.WindowState == FormWindowState.Maximized;
            _settings.IsDarkTheme = _isDarkTheme;
            _settings.WordWrap = _wordWrap;
            _settings.FontName = _editBox.Font.Name;
            _settings.FontSize = _editBox.Font.Size;
            _settings.ToolbarVisible = _toolbar.Visible;
            _settings.Save();
        }

        public void ApplyTheme()
        {
            if (_isDarkTheme)
            {
                this.BackColor       = Color.FromArgb(30, 30, 30);
                this.ForeColor       = Color.White;
                _toolbar.BackColor   = Color.FromArgb(45, 45, 48);
                _toolbar.ForeColor   = Color.White;
                _toolbar.Renderer    = new DarkToolStripRenderer();
                _editBox.BackColor   = Color.FromArgb(30, 30, 30);
                _editBox.ForeColor   = Color.White;
                _statusBar.BackColor = Color.FromArgb(45, 45, 48);
                _statusBar.ForeColor = Color.White;
            }
            else
            {
                this.BackColor       = SystemColors.Control;
                this.ForeColor       = SystemColors.ControlText;
                _toolbar.BackColor   = Color.FromArgb(245, 245, 245);
                _toolbar.ForeColor   = SystemColors.ControlText;
                _toolbar.Renderer    = new LightToolStripRenderer();
                _editBox.BackColor   = SystemColors.Window;
                _editBox.ForeColor   = SystemColors.WindowText;
                _statusBar.BackColor = Color.FromArgb(245, 245, 245);
                _statusBar.ForeColor = SystemColors.ControlText;
            }
        }

        private static ToolStripButton MakeTButton(string text, EventHandler handler)
        {
            var btn = new ToolStripButton(text);
            btn.Click += handler;
            btn.AutoSize = false;
            btn.Width = 75;
            btn.Height = 35;
            btn.Margin = new Padding(2, 5, 2, 5);
            return btn;
        }

        private static Font GetDefaultFont()
        {
            try { return new Font("Consolas", 10f, FontStyle.Regular); }
            catch { return new Font("Lucida Console", 10f, FontStyle.Regular); }
        }

        private void UpdateTitle()
        {
            string name = string.IsNullOrEmpty(_currentFile)
                ? "Untitled"
                : Path.GetFileName(_currentFile);
            this.Text = (_modified ? name + " *" : name) + " - Text Editor";
        }

        private void UpdateStatusBar()
        {
            if (_statusBar == null) return;

            int selStart = _editBox.SelectionStart;

            int line = _editBox.GetLineFromCharIndex(selStart);
            int lineStart = _editBox.GetFirstCharIndexFromLine(line);
            int col = selStart - lineStart;

            int totalLines = _editBox.Lines.Length;
            int totalChars = _editBox.TextLength;

            _statusPart0.Text = $"Line {line + 1}, Col {col + 1}";
            _statusPart1.Text = $"{totalLines} lines, {totalChars} chars";
            _statusPart2.Text = "UTF-8";
        }

        private void RecordUndoState()
        {
            if (_ignoreChange) return;

            var state = new TextState { Text = _editBox.Text, CursorPos = _editBox.SelectionStart };

            if (_undoStack.Count > 0 && _undoStack.Peek().Text == state.Text) return;

            _undoStack.Push(state);
            if (_undoStack.Count > MaxUndo)
            {
                var temp = new List<TextState>(_undoStack);
                temp.RemoveAt(temp.Count - 1);
                _undoStack.Clear();
                for (int i = temp.Count - 1; i >= 0; i--) _undoStack.Push(temp[i]);
            }
            _redoStack.Clear();
        }

        private void RestoreText(TextState state)
        {
            _ignoreChange = true;
            _editBox.Text            = state.Text;
            int pos = Math.Min(state.CursorPos, _editBox.TextLength);
            _editBox.SelectionStart  = pos;
            _editBox.SelectionLength = 0;
            _editBox.ScrollToCaret();
            _ignoreChange = false;
        }

        private void OnUndo(object sender, EventArgs e)
        {
            if (_undoStack.Count == 0) return;

            string current = _editBox.Text;
            while (_undoStack.Count > 0 && _undoStack.Peek().Text == current)
                _undoStack.Pop();

            if (_undoStack.Count == 0) return;

            _redoStack.Push(new TextState { Text = current, CursorPos = _editBox.SelectionStart });

            RestoreText(_undoStack.Pop());
        }

        private void OnRedo(object sender, EventArgs e)
        {
            if (_redoStack.Count == 0) return;

            string current = _editBox.Text;
            while (_redoStack.Count > 0 && _redoStack.Peek().Text == current)
                _redoStack.Pop();

            if (_redoStack.Count == 0) return;

            _undoStack.Push(new TextState { Text = current, CursorPos = _editBox.SelectionStart });

            RestoreText(_redoStack.Pop());
        }

        private void ClearUndoStack()
        {
            _undoStack.Clear();
            _redoStack.Clear();
        }

        private static string ReadFileContent(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);
            if (bytes.Length == 0) return string.Empty;

            if (bytes.Length >= 2 && bytes[0] == 0xFF && bytes[1] == 0xFE)
                return Encoding.Unicode.GetString(bytes, 2, bytes.Length - 2);

            int offset = 0;
            if (bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
                offset = 3;

            try
            {
                return new UTF8Encoding(false, true).GetString(bytes, offset, bytes.Length - offset);
            }
            catch
            {
                return Encoding.Default.GetString(bytes, offset, bytes.Length - offset);
            }
        }

        private static void WriteFileContent(string path, string content)
        {
            File.WriteAllText(path, content, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
        }

        private void LoadFile(string path)
        {
            _ignoreChange = true;
            _editBox.Text = ReadFileContent(path);
            _ignoreChange = false;
            _currentFile  = path;
            _modified     = false;
            ClearUndoStack();
            _editBox.SelectionStart = 0;
            _editBox.ScrollToCaret();
            UpdateTitle();
            UpdateStatusBar();
        }

        private bool ConfirmSave()
        {
            if (!_modified) return true;

            var result = MessageBox.Show(
                "Ada perubahan yang belum disimpan.\nSimpan perubahan?",
                "Konfirmasi",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)  return DoSave(false);
            if (result == DialogResult.No)   return true;
            return false;
        }

        private bool DoSave(bool saveAs)
        {
            if (string.IsNullOrEmpty(_currentFile) || saveAs)
            {
                using var dlg = new SaveFileDialog
                {
                    Filter      = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                    DefaultExt  = "txt",
                    Title       = "Save As",
                    FileName    = _currentFile,
                };
                if (dlg.ShowDialog(this) != DialogResult.OK) return false;
                _currentFile = dlg.FileName;
            }

            try
            {
                WriteFileContent(_currentFile, _editBox.Text);
                _modified = false;
                UpdateTitle();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal menyimpan file!\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void OnNew(object sender, EventArgs e)
        {
            if (!ConfirmSave()) return;
            _ignoreChange     = true;
            _editBox.Text     = string.Empty;
            _ignoreChange     = false;
            _currentFile      = string.Empty;
            _modified         = false;
            ClearUndoStack();
            UpdateTitle();
            UpdateStatusBar();
        }

        private void OnOpen(object sender, EventArgs e)
        {
            if (!ConfirmSave()) return;

            using var dlg = new OpenFileDialog
            {
                Filter      = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                Title       = "Open File",
                CheckFileExists = true,
            };
            if (dlg.ShowDialog(this) != DialogResult.OK) return;
            LoadFile(dlg.FileName);
        }

        private void OnSave(object sender, EventArgs e)   => DoSave(false);
        private void OnCut(object sender, EventArgs e)       => _editBox.Cut();
        private void OnCopy(object sender, EventArgs e)      => _editBox.Copy();
        private void OnPaste(object sender, EventArgs e)     => _editBox.Paste();
        private void OnSelectAll(object sender, EventArgs e) => _editBox.SelectAll();

        private void OnSettings(object sender, EventArgs e)
        {
            using var dlg = new SettingsDialog(_isDarkTheme, _editBox.Font, _toolbar.Visible, _settings.RegisterContextMenu);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                _editBox.Font = dlg.SelectedFont;
                if (_isDarkTheme != dlg.IsDarkTheme)
                {
                    _isDarkTheme = dlg.IsDarkTheme;
                    ApplyTheme();
                }
                _toolbar.Visible = dlg.ToolbarVisible;
                if (_settings.RegisterContextMenu != dlg.RegisterContextMenu)
                {
                    _settings.RegisterContextMenu = dlg.RegisterContextMenu;
                    RegisterContextMenuHandler(dlg.RegisterContextMenu);
                }
                SaveSettings();
            }
        }

        private void ToggleToolbar()
        {
            _toolbar.Visible = !_toolbar.Visible;
            SaveSettings();
        }

        private static void RegisterContextMenuHandler(bool register)
        {
            try
            {
                string exePath = Application.ExecutablePath;
                string iconPath = exePath + ",0";
                string keyPath = @"*\shell\TextEditor";

                if (register)
                {
                    using var key = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(keyPath);
                    key.SetValue("", "Open with Text Editor");
                    key.SetValue("Icon", iconPath);

                    using var cmdKey = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey($@"{keyPath}\command");
                    cmdKey.SetValue("", $"\"{exePath}\" \"%1\"");
                }
                else
                {
                    Microsoft.Win32.Registry.ClassesRoot.DeleteSubKeyTree(keyPath, false);
                }
            }
            catch { }
        }

        private void ToggleWordWrap()
        {
            _wordWrap            = !_wordWrap;
            _editBox.WordWrap    = _wordWrap;
            _wordWrapBtn.Checked = _wordWrap;
            _editBox.ScrollBars  = _wordWrap
                ? RichTextBoxScrollBars.Vertical
                : RichTextBoxScrollBars.Both;
        }

        private void OnFind(object sender, EventArgs e)
        {
            string initial = _editBox.SelectionLength > 0 ? _editBox.SelectedText : string.Empty;
            using var dlg = new FindDialog(initial);
            dlg.ApplyTheme(_isDarkTheme);
            dlg.FindNext += (query) =>
            {
                if (string.IsNullOrEmpty(query)) return;

                string text   = _editBox.Text;
                string lower  = text.ToLowerInvariant();
                string search = query.ToLowerInvariant();

                int start = _editBox.SelectionStart + _editBox.SelectionLength;

                int found = lower.IndexOf(search, start, StringComparison.OrdinalIgnoreCase);

                if (found < 0 && start > 0)
                    found = lower.IndexOf(search, 0, start, StringComparison.OrdinalIgnoreCase);

                if (found >= 0)
                {
                    _editBox.SelectionStart  = found;
                    _editBox.SelectionLength = query.Length;
                    _editBox.ScrollToCaret();
                }
                else
                {
                    MessageBox.Show(dlg, "Text tidak ditemukan.", "Find", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };
            dlg.ShowDialog(this);
        }

        private void OnGotoLine(object sender, EventArgs e)
        {
            using var dlg = new GotoLineDialog();
            dlg.ApplyTheme(_isDarkTheme);
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            int line = dlg.LineNumber - 1;
            if (line < 0 || line >= _editBox.Lines.Length) return;

            int idx = _editBox.GetFirstCharIndexFromLine(line);
            if (idx >= 0)
            {
                _editBox.SelectionStart  = idx;
                _editBox.SelectionLength = 0;
                _editBox.ScrollToCaret();
            }
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            RecordUndoState();
            if (!_modified)
            {
                _modified = true;
                UpdateTitle();
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.N: OnNew(sender, e);      e.SuppressKeyPress = true; break;
                    case Keys.O: OnOpen(sender, e);     e.SuppressKeyPress = true; break;
                    case Keys.S: OnSave(sender, e);     e.SuppressKeyPress = true; break;
                    case Keys.Z: OnUndo(sender, e);     e.SuppressKeyPress = true; break;
                    case Keys.Y: OnRedo(sender, e);     e.SuppressKeyPress = true; break;
                    case Keys.X: OnCut(sender, e);      e.SuppressKeyPress = true; break;
                    case Keys.C: OnCopy(sender, e);     e.SuppressKeyPress = true; break;
                    case Keys.V: OnPaste(sender, e);    e.SuppressKeyPress = true; break;
                    case Keys.A: OnSelectAll(sender, e);e.SuppressKeyPress = true; break;
                    case Keys.F: OnFind(sender, e);     e.SuppressKeyPress = true; break;
                    case Keys.G: OnGotoLine(sender, e); e.SuppressKeyPress = true; break;
                    case Keys.T: ToggleToolbar();       e.SuppressKeyPress = true; break;
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!ConfirmSave()) e.Cancel = true;
            else SaveSettings();
            base.OnFormClosing(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) { _statusTimer?.Dispose(); }
            base.Dispose(disposing);
        }
    }

    public class SettingsDialog : Form
    {
        public bool IsDarkTheme { get; private set; }
        public Font SelectedFont { get; private set; }
        public bool ToolbarVisible { get; private set; }
        public bool RegisterContextMenu { get; private set; }

        public SettingsDialog(bool currentDarkTheme, Font currentFont, bool toolbarVisible, bool registerContextMenu)
        {
            IsDarkTheme = currentDarkTheme;
            SelectedFont = currentFont;
            ToolbarVisible = toolbarVisible;
            RegisterContextMenu = registerContextMenu;

            this.Text            = "Settings";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.MinimizeBox     = false;
            this.ShowInTaskbar   = false;
            this.StartPosition   = FormStartPosition.CenterParent;
            this.Size            = new Size(380, 300);

            var lblTheme = new Label { Text = "Theme:", Left = 20, Top = 25, Width = 80 };
            var cbTheme = new ComboBox { Left = 120, Top = 22, Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cbTheme.Items.Add("Light");
            cbTheme.Items.Add("Dark");
            cbTheme.SelectedIndex = IsDarkTheme ? 1 : 0;

            var lblFont = new Label { Text = "Font:", Left = 20, Top = 65, Width = 80 };
            var lblFontName = new Label { Text = $"{SelectedFont.Name}, {SelectedFont.Size}pt", Left = 120, Top = 65, Width = 120 };
            var btnFont = new Button { Text = "Change...", Left = 260, Top = 60, Width = 80 };
            btnFont.Click += (s, e) =>
            {
                using var fd = new FontDialog { Font = SelectedFont, ShowEffects = true };
                if (fd.ShowDialog(this) == DialogResult.OK)
                {
                    SelectedFont = fd.Font;
                    lblFontName.Text = $"{SelectedFont.Name}, {SelectedFont.Size}pt";
                }
            };

            var cbToolbar = new CheckBox
            {
                Text = "Show Toolbar",
                Left = 20, Top = 105, Width = 200,
                Checked = ToolbarVisible,
            };

            var cbContextMenu = new CheckBox
            {
                Text = "\"Open with Text Editor\" in right-click menu",
                Left = 20, Top = 135, Width = 320,
                Checked = RegisterContextMenu,
            };

            var btnOk = new Button { Text = "OK", Left = 160, Top = 210, Width = 80, DialogResult = DialogResult.OK };
            var btnCancel = new Button { Text = "Cancel", Left = 260, Top = 210, Width = 80, DialogResult = DialogResult.Cancel };

            btnOk.Click += (s, e) =>
            {
                IsDarkTheme = cbTheme.SelectedIndex == 1;
                ToolbarVisible = cbToolbar.Checked;
                RegisterContextMenu = cbContextMenu.Checked;
            };

            this.AcceptButton = btnOk;
            this.CancelButton = btnCancel;
            this.Controls.AddRange(new Control[] { lblTheme, cbTheme, lblFont, lblFontName, btnFont, cbToolbar, cbContextMenu, btnOk, btnCancel });

            ApplyThemeToDialog(this, IsDarkTheme);
        }

        public static void ApplyThemeToDialog(Form form, bool isDark)
        {
            if (isDark)
            {
                form.BackColor = Color.FromArgb(30, 30, 30);
                form.ForeColor = Color.White;
                foreach (Control c in form.Controls)
                {
                    if (c is TextBox || c is ComboBox)
                    {
                        c.BackColor = Color.FromArgb(45, 45, 48);
                        c.ForeColor = Color.White;
                    }
                    else if (c is CheckBox cb)
                    {
                        cb.ForeColor = Color.White;
                    }
                    else if (c is Button b)
                    {
                        b.BackColor = Color.FromArgb(60, 60, 65);
                        b.ForeColor = Color.White;
                        b.FlatStyle = FlatStyle.Flat;
                        b.FlatAppearance.BorderSize = 0;
                    }
                }
            }
            else
            {
                form.BackColor = SystemColors.Control;
                form.ForeColor = SystemColors.ControlText;
                foreach (Control c in form.Controls)
                {
                    if (c is TextBox || c is ComboBox)
                    {
                        c.BackColor = SystemColors.Window;
                        c.ForeColor = SystemColors.WindowText;
                    }
                    else if (c is CheckBox cb)
                    {
                        cb.ForeColor = SystemColors.ControlText;
                    }
                    else if (c is Button b)
                    {
                        b.BackColor = SystemColors.Control;
                        b.ForeColor = SystemColors.ControlText;
                        b.FlatStyle = FlatStyle.Standard;
                    }
                }
            }
        }
    }

    public class FindDialog : Form
    {
        public event Action<string> FindNext;

        private TextBox _inputBox;

        public FindDialog(string initial = "")
        {
            this.Text            = "Find";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.MinimizeBox     = false;
            this.ShowInTaskbar   = false;
            this.StartPosition   = FormStartPosition.CenterParent;
            this.Size            = new Size(400, 140);

            var label = new Label { Text = "Find what:", Left = 20, Top = 25, Width = 70 };

            _inputBox = new TextBox
            {
                Left  = 95, Top = 23, Width = 265,
                Text  = initial,
                MaxLength = 255,
            };

            var btnFind = new Button
            {
                Text         = "Find Next",
                Left         = 195, Top = 65, Width = 80,
                DialogResult = DialogResult.None,
            };
            btnFind.Click += (s, e) => FindNext?.Invoke(_inputBox.Text);

            var btnCancel = new Button
            {
                Text         = "Cancel",
                Left         = 280, Top = 65, Width = 80,
                DialogResult = DialogResult.Cancel,
            };

            this.AcceptButton = btnFind;
            this.CancelButton = btnCancel;
            this.Controls.AddRange(new Control[] { label, _inputBox, btnFind, btnCancel });
            _inputBox.Select();
            
            // Note: Since this is created from MainForm, we apply theme after initialization.
        }
        
        public void ApplyTheme(bool isDark)
        {
            SettingsDialog.ApplyThemeToDialog(this, isDark);
        }
    }

    public class GotoLineDialog : Form
    {
        public int LineNumber { get; private set; } = 1;

        private TextBox _inputBox;

        public GotoLineDialog()
        {
            this.Text            = "Go To Line";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.MinimizeBox     = false;
            this.ShowInTaskbar   = false;
            this.StartPosition   = FormStartPosition.CenterParent;
            this.Size            = new Size(330, 150);

            var label = new Label { Text = "Line number:", Left = 20, Top = 25, Width = 85 };

            _inputBox = new TextBox
            {
                Left     = 115, Top = 23, Width = 175,
                MaxLength = 10,
            };
            _inputBox.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    e.Handled = true;
            };

            var btnGo = new Button
            {
                Text         = "Go",
                Left         = 125, Top = 70, Width = 80,
                DialogResult = DialogResult.OK,
            };
            btnGo.Click += (s, e) =>
            {
                if (int.TryParse(_inputBox.Text, out int n) && n >= 1)
                    LineNumber = n;
                else
                    this.DialogResult = DialogResult.None;
            };

            this.AcceptButton = btnGo;
            this.Controls.AddRange(new Control[] { label, _inputBox, btnGo });
            _inputBox.Select();
        }

        public void ApplyTheme(bool isDark)
        {
            SettingsDialog.ApplyThemeToDialog(this, isDark);
        }
    }

    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(args));
        }
    }
}
