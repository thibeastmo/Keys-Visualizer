using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;
using JsonObjectConverter;
using File = Google.Apis.Drive.v3.Data.File;
using Timer = System.Windows.Forms.Timer;

namespace Keys_Visualizer
{
    public partial class Form1 : Form
    {
        //top values
        public static string cmbModeText = string.Empty;

        //Numeriek keyboard kan 4 tegelijk (1-3) + (4-6) + (7-9) + 0
        //keyboard kan +10 toetsen tegelijk indrukken
        public static string jsonFile = @"./layouts.json";
        public static string splitter = @"<!>";
        public static Color defaultButtonForeColor = Color.Black;
        public static Color defaultButtonColor = Color.Red;
        public static Color defaultButtonColorOnActive = Color.White;
        public static Tuple<bool, KeyboardButton> lastEditedKeyboardButton;
        public static string APPLICATION_NAME = "Keys Visualizer";
        public static long version = 12;
        List<Panel> brawlKeys = new List<Panel>();
        bool isRunning = false;
        private static short MAX_COLUMNS = 5;
        private static short margin = 5;
        private Rectangle r;
        private Graphics g;
        private Timer timer;
        private ThreadStart ts;
        private Thread t;
        private GoogleDriveHelper.GoogleDriveHelper gdh;
        bool topInitHasRan = false;
        private List<string> charList = new List<string>();
        string buttonString = string.Empty;
        Keys[] allKeys = (Keys[])Enum.GetValues(typeof(Keys));
        private DateTime justActivated;
        bool mousISDown;
        private Point? beginLocation;
        bool hoveringOverPnlMain = false;
        bool singleClick = true;
        bool alreadyInPnlMain = true;
        public static bool alreadyEditing = false;
        Panel hoveringPanel;
        Tuple<List<int>, List<int>> positions;
        List<MagnetSide> magnets;
        private int difference = 0;

        [Obsolete]
        public Form1()
        {
            InitializeComponent();
            if (!System.IO.Directory.GetCurrentDirectory().EndsWith("Debug")) {
                //MessageBox.Show("Voor checken van updates\nVersie: " + version);
                Tuple<bool, File> updates = new Tuple<bool, File>(false, null);
                try
                {
                    updates = checkForUpdates();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error tijdens checken of er updates zijn:\n" + ex.Message + "\n\nStacktrace:\n" + ex.StackTrace);
                }
                //MessageBox.Show("Na checken van updates\nVersie: " + version);
                if (updates.Item1)
                {
                    bool askIfUserWantsToUpdate = true;
                    bool updateIt = false;
                    if (askIfUserWantsToUpdate && MessageBox.Show("New version found, want to download it?", "Update", MessageBoxButtons.YesNo) == DialogResult.Yes || !askIfUserWantsToUpdate)
                    {
                        updateIt = true;
                    }
                    //er is een update
                    if (updateIt)
                    {
                        try
                        {
                            gdh.updateApplication();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error tijdens installeren van update:\n" + ex.Message + "\n\nStacktrace:\n" + ex.StackTrace);
                        }
                    }
                }
            }
            this.Text = Form1.APPLICATION_NAME + " V" + Form1.version;
            voorgrondInit();
            knoppenGlobaalInit();
            //nakijkenInit();
            topInit();
            initializeR();
            timer = new Timer();
            timer.Tick += timer_Tick;
            g = pnlMain.CreateGraphics();
        }

        private void topInit()
        {
            //ScrollBar hScrollBar1 = new HScrollBar();
            //hScrollBar1.Dock = DockStyle.Bottom;
            //hScrollBar1.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            //hScrollBar1.Scroll += (sender, e) => { pnlSearchResults.HorizontalScroll.Value = hScrollBar1.Value; };
            //pnlSearchResults.Controls.Add(hScrollBar1);
            //scrollBarHeight = (short)hScrollBar1.Height;
            pnlTop.MaximumSize = new Size(int.MaxValue, pnlTop.Height);
            topInitHasRan = true;
            fillCmbMode();
            cmbMode.SelectedIndex = 2;
            cmbLayouts.Enabled = false;
            pnlSearchResults.HorizontalScroll.Visible = false;
            pnlSearchResults.Size = new Size(pnlSearchResults.Width, pnlTop.Height + 15);
        }

        private void fillCmbMode()
        {
            cmbMode.Items.Clear();
            bool firstTime = true;
            foreach (Modes item in Enum.GetValues(typeof(Modes)))
            {
                if (firstTime)
                {
                    firstTime = false;
                    cmbMode.SelectedText = item.ToString();
                }
                cmbMode.Items.Add(item.ToString());
            }
        }

        private void showPressedKeys()
        {
            if (cmbModeText.ToLower().Equals("inputs"))
            {
                InputsCombo inputsCombo = new InputsCombo();
                inputsCombo.runInput();
            }
            else
            {
                string firstMode = ((Modes)Enum.GetValues(typeof(Modes)).GetValue(0)).ToString();
                List<string> previousCharlist = new List<string>();
                while (isRunning)
                {
                    Thread.Sleep(10);
                    string cmbModeString = cmbModeText;
                    if (cmbModeString.Equals(firstMode))
                    {
                        //layout mode
                        foreach (Panel brawlKey in brawlKeys)
                        {
                            Color bgColor;
                            if (charList.Contains(brawlKey.Name))
                            {
                                if (brawlKey.Controls.Count > 2)
                                {
                                    bgColor = brawlKey.Controls[2].BackColor;
                                }
                                else
                                {
                                    bgColor = defaultButtonColorOnActive;
                                }
                            }
                            else
                            {
                                if (brawlKey.Controls.Count > 2)
                                {
                                    bgColor = brawlKey.Controls[1].BackColor;
                                }
                                else
                                {
                                    bgColor = defaultButtonColor;
                                }
                            }
                            foreach (Panel pnl in pnlMain.Controls)
                            {
                                if (pnl.Equals(brawlKey))
                                {
                                    this.BeginInvoke(new Action(() => {
                                        pnl.BackColor = bgColor;
                                    }));
                                }
                            }
                        }
                    }
                    else
                    {
                        //all buttons mode
                        List<string> tempCharList = new List<string>(charList);
                        List<string> removeButtonsList = previousCharlist.Except(tempCharList).ToList();
                        List<string> addButtonsList = tempCharList.Except(previousCharlist).ToList();
                        List<string> movebuttonsList = previousCharlist.Except(removeButtonsList).ToList();

                        //move buttons
                        if (movebuttonsList.Count > 0 && movebuttonsList != tempCharList)
                        {
                            List<Panel> panelsToBeMoved = new List<Panel>();
                            for (int i = 0; i < pnlMain.Controls.Count; i++)
                            {
                                if (movebuttonsList.Contains(pnlMain.Controls[i].Name))
                                {
                                    if (!panelsToBeMoved.Contains((Panel)pnlMain.Controls[i]))
                                    {
                                        panelsToBeMoved.Add((Panel)pnlMain.Controls[i]);
                                    }
                                    if (panelsToBeMoved.Count.Equals(removeButtonsList.Count))
                                    {
                                        break;
                                    }
                                }
                            }
                            int j = 0;
                            foreach (Panel pnl in panelsToBeMoved)
                            {
                                this.BeginInvoke(new Action(() =>
                                {
                                    //short temprows = (short)(j % MAX_COLUMNS);
                                    //temprows = (short)((j - temprows) / MAX_COLUMNS);
                                    //int gebruikteSizesInDezeRij = (j * size) - (temprows * MAX_COLUMNS * size);
                                    //int gebruikteMarginsInDezeRij = (j * margin) - (j * MAX_COLUMNS * margin) + margin;
                                    //pnl.Location = new Point(gebruikteSizesInDezeRij + gebruikteMarginsInDezeRij, temprows * size + (temprows + 1) * margin);
                                    pnl.Location = calculateLocation(j);
                                    j++;
                                }));
                            }
                        }

                        //remove buttons
                        List<Panel> panelsToBeRemoved = new List<Panel>();
                        for (int i = 0; i < pnlMain.Controls.Count; i++)
                        {
                            if (removeButtonsList.Contains(pnlMain.Controls[i].Name))
                            {
                                if (!panelsToBeRemoved.Contains((Panel)pnlMain.Controls[i]))
                                {
                                    panelsToBeRemoved.Add((Panel)pnlMain.Controls[i]);
                                }
                                if (panelsToBeRemoved.Count.Equals(removeButtonsList.Count))
                                {
                                    break;
                                }
                            }
                        }
                        foreach (Panel panel in panelsToBeRemoved)
                        {
                            this.BeginInvoke(new Action(() =>
                            {
                                pnlMain.Controls.Remove(panel);
                            }));
                        }

                        //add buttons
                        int amountOfControls = pnlMain.Controls.Count;
                        foreach (string key in addButtonsList)
                        {
                            Panel tempPanel = generateButton(key);
                            tempPanel.Location = calculateLocation(amountOfControls);
                            this.BeginInvoke(new Action(() =>
                            {
                                pnlMain.Controls.Add(tempPanel);
                            }));
                            amountOfControls = amountOfControls + 1;
                        }
                        previousCharlist = tempCharList;

                    }

                }
            }
        }

        private Point calculateLocation(int amountOfControls)
        {
            short rows = (short)(amountOfControls % MAX_COLUMNS);
            rows = (short)((amountOfControls - rows) / MAX_COLUMNS);
            int gebruikteSizesInDezeRij = (amountOfControls * KeyboardButton.DEFAULT_SIZE) - (rows * MAX_COLUMNS * KeyboardButton.DEFAULT_SIZE);
            int gebruikteMarginsInDezeRij = (amountOfControls * margin) - (rows * MAX_COLUMNS * margin) + margin;
            return new Point(gebruikteSizesInDezeRij + gebruikteMarginsInDezeRij, rows * KeyboardButton.DEFAULT_SIZE + (rows + 1) * margin);
        }

        #region altijd op voorgrond
        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private const UInt32 SWP_NOSIZE = 0x0001;
        private const UInt32 SWP_NOMOVE = 0x0002;
        private const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        private void voorgrondInit()
        {
            this.BackColor = Color.Lime;
            TransparencyKey = this.BackColor;
            Form1.SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
        }
        #endregion

        #region knoppen globaal gebruiken
        private IKeyboardMouseEvents m_GlobalHook;

        public void Subscribe()
        {
            m_GlobalHook = Hook.GlobalEvents();
            m_GlobalHook.KeyDown += M_GlobalHook_KeyDown;
            m_GlobalHook.KeyUp += M_GlobalHook_KeyUp;
        }

        private void M_GlobalHook_KeyDown(object sender, KeyEventArgs e)
        {
            if (isRunning)
            {
                //keyDown(sender, e);
                if (!charList.Contains(e.KeyCode.ToString()))
                {
                    charList.Add(e.KeyCode.ToString());
                }
            }
        }

        private void M_GlobalHook_KeyUp(object sender, KeyEventArgs e)
        {
            if (isRunning)
            {
                //keyUp(sender, e);
                charList.Remove(e.KeyCode.ToString());
            }
        }
        private void knoppenGlobaalInit()
        {
            Subscribe();
        }
        #endregion

        #region toetsen nakijken
        //Tuple<int, int> range = new Tuple<int, int>(61, 86);

        //private void nakijkenInit()
        //{

        //}
        //private void keyDown(object sender, KeyEventArgs e)
        //{
        //    for (int i = range.Item1; i <= range.Item2; i++)
        //    {
        //        bool keyIsDown = IsKeyDown(allKeys[i]);
        //        if (!charList.Contains(allKeys[i]) && keyIsDown)
        //        {
        //            pnlMain.Controls.Add(generateButton(allKeys[i]));
        //            charList.Add(allKeys[i]);
        //        }
        //    }
        //}
        //private void keyUp(object sender, KeyEventArgs e)
        //{
        //    for (int i = range.Item1; i <= range.Item2; i++)
        //    {
        //        if (charList.Contains(allKeys[i]) && !IsKeyDown(allKeys[i]))
        //        {
        //            for (int j = 0; j < pnlMain.Controls.Count; j++)
        //            {
        //                if (pnlMain.Controls[j].Name.Equals("pnl" + allKeys[i].ToString()))
        //                {
        //                    pnlMain.Controls.Remove(pnlMain.Controls[j]);
        //                }
        //            }
        //            charList.Remove(allKeys[i]);
        //        }
        //    }
        //}

        //[DllImport("user32.dll")]
        //public extern static Int16 GetKeyState(Int16 nVirtKey);
        //public static bool IsKeyDown(Keys key)
        //{
        //    return (GetKeyState(Convert.ToInt16(key)) & 0X80) == 0X80;
        //}
        #endregion

        private Panel generateButton(string keyCode)
        {
            return generateButton(new KeyboardButton(keyCode));
        }

        private Panel generateButton(KeyboardButton keyboardButton)
        {
            Panel pnlButton = new Panel();
            pnlButton.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            Label lblButton = new Label();
            lblButton.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            lblButton.Dock = DockStyle.Fill;
            lblButton.Text = keyboardButton.text;
            lblButton.AutoSize = true;
            buttonString = keyboardButton.name;
            pnlButton.Controls.Add(lblButton);
            if (keyboardButton.location.HasValue)
            {
                pnlButton.Location = keyboardButton.location.Value;
            }
            if (keyboardButton.size.HasValue)
            {
                pnlButton.Size = keyboardButton.size.Value;
                pnlButton.MinimumSize = keyboardButton.size.Value;
                pnlButton.MaximumSize = keyboardButton.size.Value;
            }
            else
            {
                pnlButton.Size = new Size(KeyboardButton.DEFAULT_SIZE, KeyboardButton.DEFAULT_SIZE - margin);
            }
            if (keyboardButton.backColor.HasValue)
            {
                pnlButton.BackColor = keyboardButton.backColor.Value;
            }
            else
            {
                pnlButton.BackColor = Color.Red;
            }
            pnlButton.Name = keyboardButton.name;
            pnlButton.BorderStyle = BorderStyle.FixedSingle;
            if (keyboardButton.fontColor.HasValue)
            {
                lblButton.ForeColor = keyboardButton.fontColor.Value;
            }
            else
            {
                lblButton.ForeColor = defaultButtonForeColor;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("backColor" + splitter);
            if (keyboardButton.backColor.HasValue)
            {
                sb.AppendLine(keyboardButton.backColor.Value.ToString());
            }
            else
            {
                sb.AppendLine(defaultButtonColor.ToString());
            }
            sb.Append("backColorOnActive" + splitter);
            if (keyboardButton.backColorOnActive.HasValue)
            {
                sb.AppendLine(keyboardButton.backColorOnActive.Value.ToString());
            }
            else
            {
                sb.AppendLine(defaultButtonColorOnActive.ToString());
            }
            pnlButton.MouseDown += PnlButton_MouseDown;
            pnlButton.MouseUp += PnlButton_MouseUp;
            pnlButton.DoubleClick += PnlButton_DoubleClick;
            pnlButton.Click += PnlButton_Click;
            pnlButton.AccessibleDescription = sb.ToString();
            return pnlButton;
        }

        #region hardcoded layouts
        //private void createBrawlLayout()
        //{
        //    Panel s = generateButton("S");
        //    Panel d = generateButton("D");
        //    Panel f = generateButton("F");
        //    Panel e = generateButton("E");
        //    Panel a = generateButton("A");
        //    Panel space = generateButton("Space");
        //    Panel NUM_4 = generateButton("NumPad4");
        //    Panel NUM_5 = generateButton("NumPad5");
        //    Panel NUM_6 = generateButton("NumPad6");

        //    space.Size = new Size((space.Width + margin) * 3 - margin, space.Height);

        //    a.Location = new Point(pnlMain.Location.X + margin, pnlMain.Height - a.Height - margin);
        //    s.Location = new Point(a.Location.X + a.Width + margin, a.Location.Y);
        //    d.Location = new Point(s.Location.X + s.Width + margin, s.Location.Y);
        //    f.Location = new Point(d.Location.X + d.Width + margin, d.Location.Y);
        //    e.Location = new Point(d.Location.X, d.Location.Y - d.Height - margin);
        //    space.Location = new Point(f.Location.X + f.Width * 2 + margin * 2, f.Location.Y);
        //    NUM_6.Location = new Point(space.Location.X + space.Width - NUM_6.Width, e.Location.Y);
        //    NUM_5.Location = new Point(NUM_6.Location.X - margin - NUM_5.Width, e.Location.Y);
        //    NUM_4.Location = new Point(NUM_5.Location.X - margin - NUM_4.Width, e.Location.Y);

        //    pnlMain.Controls.Add(a);
        //    pnlMain.Controls.Add(s);
        //    pnlMain.Controls.Add(d);
        //    pnlMain.Controls.Add(f);
        //    pnlMain.Controls.Add(e);
        //    pnlMain.Controls.Add(space);
        //    pnlMain.Controls.Add(NUM_6);
        //    pnlMain.Controls.Add(NUM_5);
        //    pnlMain.Controls.Add(NUM_4);

        //    foreach (Panel pnlBu in pnlMain.Controls)
        //    {
        //        brawlKeys.Add(pnlBu);
        //    }
        //}
        //private void createRomBrawlLayout()
        //{
        //    Panel End = generateButton("End");
        //    Panel PageDown = generateButton("PageDown");
        //    Panel Clear = generateButton("Clear");
        //    Panel Space = generateButton("Space");
        //    Panel Down = generateButton("Down");
        //    Panel A = generateButton("A");
        //    Panel Z = generateButton("Z");
        //    Panel R = generateButton("R");
        //    Panel E = generateButton("E");

        //    Space.Size = new Size((Space.Width + margin) * 3 - margin, Space.Height);

        //    A.Location = new Point(pnlMain.Location.X + margin, pnlMain.Height - A.Height - margin);
        //    Z.Location = new Point(A.Location.X + A.Width + margin, A.Location.Y);
        //    E.Location = new Point(Z.Location.X + Z.Width + margin, Z.Location.Y);
        //    R.Location = new Point(E.Location.X + E.Width + margin, E.Location.Y);

        //    Space.Location = new Point(R.Location.X + R.Width * 2 + margin * 2, R.Location.Y);
        //    End.Location = new Point(Space.Location.X, Space.Location.Y - End.Height - margin);
        //    Down.Location = new Point(End.Location.X + End.Width + margin, End.Location.Y);
        //    PageDown.Location = new Point(Down.Location.X + Down.Width + margin, Down.Location.Y);
        //    Clear.Location = new Point(Down.Location.X, PageDown.Location.Y - margin - Clear.Height);


        //    pnlMain.Controls.Add(End);
        //    pnlMain.Controls.Add(PageDown);
        //    pnlMain.Controls.Add(Clear);
        //    pnlMain.Controls.Add(Space);
        //    pnlMain.Controls.Add(Down);
        //    pnlMain.Controls.Add(A);
        //    pnlMain.Controls.Add(Z);
        //    pnlMain.Controls.Add(R);
        //    pnlMain.Controls.Add(E);
        //    foreach (Panel pnlBu in pnlMain.Controls)
        //    {
        //        brawlKeys.Add(pnlBu);
        //    }
        //}
        #endregion

        public Tuple<bool, string> addLayoutToLayouts(string json, string layoutName)
        {
            List<Layout> layouts = getAllLayouts();
            //check if name already exists
            if (layouts.Count > 0)
            {
                foreach (Layout layout in layouts)
                {
                    if (layout.name.Equals(layoutName))
                    {
                        return new Tuple<bool, string>(false, "This layout name already exists!");
                    }
                }
            }
            //go on cz name does not exist
            layouts.Add(new Layout(new Json(json, layoutName)));
            System.IO.File.WriteAllText(Form1.jsonFile, generateJsonFromLayoutsList(layouts));
            return new Tuple<bool, string>(true, "The layout has successfully been added!");
        }

        private void fillCmblayouts()
        {
            List<Layout> layouts = getAllLayouts();
            foreach (Layout layout in layouts)
            {
                if (!layout.name.Equals(string.Empty))
                {
                    cmbLayouts.Items.Add(layout.name);
                }
            }
        }

        private List<Layout> getAllLayouts()
        {
            Json mainJson = new Json((System.IO.File.Exists(Form1.jsonFile) ? System.IO.File.ReadAllText(Form1.jsonFile) : string.Empty), string.Empty);
            List<Layout> layouts = new List<Layout>();
            if (mainJson.jsonArray != null && mainJson.jsonArray.Count > 0)
            {
                foreach (Tuple<string, Json> jsonArrayItem in mainJson.jsonArray)
                {
                    Layout tempLayout = new Layout(jsonArrayItem.Item2);
                    layouts.Add(tempLayout);
                }
            }
            return layouts;
        }

        private void cmbLayouts_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Layout> layouts = getAllLayouts();
            foreach (Layout layout in layouts)
            {
                if (layout.name.Equals((sender as ComboBox).Text))
                {
                    loadLayout(layout);
                    break;
                }
            }
        }

        private void loadLayout(Layout layout)
        {
            txtLayoutName.Text = layout.name;
            pnlMain.Controls.Clear();
            if (layout.keyboardButtons != null && layout.keyboardButtons.Count > 0)
            {
                foreach (KeyboardButton kb in layout.keyboardButtons)
                {
                    pnlMain.Controls.Add(generateButton(kb));
                }
                foreach (Panel pnlBu in pnlMain.Controls)
                {
                    brawlKeys.Add(pnlBu);
                }
            }
        }

        private async Task scrollSearchResults()
        {
            await Task.Run(() =>
            {
                while (mousISDown)
                {
                    int lastDifference = difference;
                    difference = beginLocation.Value.X - Cursor.Position.X;
                    this.BeginInvoke(new Action(() =>
                    {
                        int lastScrollPosition = pnlSearchResults.AutoScrollPosition.X;
                        pnlSearchResults.AutoScrollPosition = new Point(difference, pnlSearchResults.AutoScrollPosition.Y);
                        if (pnlSearchResults.AutoScrollPosition.X == lastScrollPosition)
                        {
                            difference = lastDifference;
                        }
                    }));
                    Thread.Sleep(10);
                }
                int maxSize = (pnlSearchResults.Controls.Count * KeyboardButton.DEFAULT_SIZE) + (pnlSearchResults.Controls.Count * margin) + margin;
                if (difference < 0)
                {
                    difference = 0;
                }
                else if (difference > maxSize)
                {
                    difference = maxSize;
                }
            });
        }

        private void placeHoveringPanelInPnlMain()
        {
            Panel panelToAdd;
            if (!alreadyInPnlMain)
            {
                panelToAdd = generateButton(hoveringPanel.Controls[0].Text);
                panelToAdd.Location = hoveringPanel.Location;
            }
            else
            {
                panelToAdd = hoveringPanel;
            }
            pnlMain.Controls.Add(panelToAdd);
            hoveringPanel = null;
            initializeBrawlkeys();
            pnlMain.BackColor = Color.Transparent;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            updateHoveringPanelPosition(false);
        }
        private Tuple<List<int>, List<int>> getAllPositionsOfControls()
        {
            List<int> xs = new List<int>();
            List<int> ys = new List<int>();
            foreach (Panel pnl in pnlMain.Controls)
            {
                if (!pnl.Name.Equals(hoveringPanel.Name))
                {
                    xs.Add(pnl.Location.X);
                    xs.Add(pnl.Location.X + pnl.Width);
                    ys.Add(pnl.Location.Y);
                    ys.Add(pnl.Location.Y + pnl.Height);
                }
            }
            return new Tuple<List<int>, List<int>>(new List<int>(xs), new List<int>(ys));
        }

        private void initializeBrawlkeys()
        {
            brawlKeys.Clear();
            foreach (Panel pnl in pnlMain.Controls)
            {
                brawlKeys.Add(pnl);
            }
        }

        private void initializeR()
        {
            r = pnlMain.RectangleToScreen(pnlMain.ClientRectangle);
        }

        private KeyboardButton getKeyboardButtonFromPanel(Panel panel)
        {
            return new KeyboardButton(panel.Name, panel.Controls[0].Text, panel.Location, panel.Size, panel.Controls[0].ForeColor, KeyboardButton.getColorByString(getValueFromDescription("backColor", (Control)panel)), KeyboardButton.getColorByString(getValueFromDescription("backColorOnActive", (Control)panel)));
        }

        private string generateJsonFromLayoutsList(List<Layout> layouts)
        {
            StringBuilder sb = new StringBuilder();
            bool firstTime = true;
            sb.AppendLine("[");
            foreach (Layout layout in layouts)
            {
                if (firstTime)
                {
                    firstTime = false;
                }
                else
                {
                    sb.AppendLine(",");
                }
                sb.Append(layout.generateJson(1));
            }
            sb.AppendLine("\n]");
            return sb.ToString();
        }

        private void updateHoveringPanelPosition()
        {
            updateHoveringPanelPosition(true);
        }
        private void updateHoveringPanelPosition(bool invoke)
        {
            if (hoveringPanel != null)
            {
                g.Clear(pnlMain.BackColor);
                if (positions == null)
                {
                    positions = getAllPositionsOfControls();
                }
                if (r.Contains(MousePosition))
                {
                    hoveringOverPnlMain = true;
                    int x = MousePosition.X - r.X - hoveringPanel.Width / 2;
                    int y = MousePosition.Y - r.Y - hoveringPanel.Height / 2;
                    if (pnlMain.Controls.Count > 0)
                    {
                        //magnet to sides
                        //doe the magnet counts for the sides
                        int usableMargin = margin * 2;
                        int otherSideX = x + hoveringPanel.Width;
                        int otherSideY = y + hoveringPanel.Height;
                        //check if magnet to use or not
                        magnets = new List<MagnetSide>();
                        foreach (int tempX in positions.Item1)
                        {
                            if (x - tempX >= -usableMargin && x - tempX <= usableMargin)
                            {
                                x = tempX;
                                magnets.Add(MagnetSide.LEFT);
                            }
                            if (otherSideX - tempX >= -usableMargin && otherSideX - tempX <= usableMargin)
                            {
                                x = tempX - hoveringPanel.Width;
                                magnets.Add(MagnetSide.RIGHT);
                            }
                        }
                        foreach (int tempY in positions.Item2)
                        {
                            if (y - tempY >= -usableMargin && y - tempY <= usableMargin)
                            {
                                y = tempY;
                                magnets.Add(MagnetSide.TOP);
                            }
                            if (otherSideY - tempY >= -usableMargin && otherSideY - tempY <= usableMargin)
                            {
                                y = tempY - hoveringPanel.Height;
                                magnets.Add(MagnetSide.BOTTOM);
                            }
                        }
                    }
                    if (invoke)
                    {
                        this.BeginInvoke(new Action(() =>
                        {
                            hoveringPanel.Location = new Point(x, y);
                        }));
                    }
                    else
                    {
                        hoveringPanel.Location = new Point(x, y);
                    }
                    if (magnets != null && magnets.Count > 0)
                    {
                        Pen blackpen = new Pen(Color.Blue, 1);
                        foreach (MagnetSide magnet in magnets)
                        {
                            switch (magnet)
                            {
                                //x1 y1 x2 y2
                                case MagnetSide.TOP: g.DrawLine(blackpen, 0, hoveringPanel.Location.Y, pnlMain.Width, hoveringPanel.Location.Y); break;
                                case MagnetSide.BOTTOM: g.DrawLine(blackpen, 0, hoveringPanel.Location.Y + hoveringPanel.Height, pnlMain.Width, hoveringPanel.Location.Y + hoveringPanel.Height); break;
                                case MagnetSide.LEFT: g.DrawLine(blackpen, hoveringPanel.Location.X, 0, hoveringPanel.Location.X, pnlMain.Height); break;
                                case MagnetSide.RIGHT: g.DrawLine(blackpen, hoveringPanel.Location.X + hoveringPanel.Width, 0, hoveringPanel.Location.X + hoveringPanel.Width, pnlMain.Height); break;
                            }
                        }
                        //g.Dispose();
                    }
                }
                else
                {
                    hoveringOverPnlMain = false;
                }
            }
            else
            {
                positions = null;
            }
        }

        private async Task updateHoveringPanelPositionAsync()
        {
            await Task.Run(() =>
            {
                Graphics g = pnlMain.CreateGraphics();
                if (hoveringPanel != null)
                {
                    g.Clear(pnlMain.BackColor);
                    if (positions == null)
                    {
                        positions = getAllPositionsOfControls();
                    }
                    if (r.Contains(MousePosition))
                    {
                        hoveringOverPnlMain = true;
                        int x = MousePosition.X - r.X - hoveringPanel.Width / 2;
                        int y = MousePosition.Y - r.Y - hoveringPanel.Height / 2;
                        if (pnlMain.Controls.Count > 0)
                        {
                            //magnet to sides
                            //doe the magnet counts for the sides
                            int usableMargin = margin * 2;
                            int otherSideX = x + hoveringPanel.Width;
                            int otherSideY = y + hoveringPanel.Height;
                            //check if magnet to use or not
                            magnets = new List<MagnetSide>();
                            foreach (int tempX in positions.Item1)
                            {
                                if (x - tempX >= -usableMargin && x - tempX <= usableMargin)
                                {
                                    x = tempX;
                                    magnets.Add(MagnetSide.LEFT);
                                }
                                if (otherSideX - tempX >= -usableMargin && otherSideX - tempX <= usableMargin)
                                {
                                    x = tempX - hoveringPanel.Width;
                                    magnets.Add(MagnetSide.RIGHT);
                                }
                            }
                            foreach (int tempY in positions.Item2)
                            {
                                if (y - tempY >= -usableMargin && y - tempY <= usableMargin)
                                {
                                    y = tempY;
                                    magnets.Add(MagnetSide.TOP);
                                }
                                if (otherSideY - tempY >= -usableMargin && otherSideY - tempY <= usableMargin)
                                {
                                    y = tempY - hoveringPanel.Height;
                                    magnets.Add(MagnetSide.BOTTOM);
                                }
                            }
                        }
                        this.BeginInvoke(new Action(() =>
                        {
                            hoveringPanel.Location = new Point(x, y);
                        }));
                        if (magnets != null && magnets.Count > 0)
                        {
                            Pen blackpen = new Pen(Color.Blue, 1);
                            foreach (MagnetSide magnet in magnets)
                            {
                                switch (magnet)
                                {
                                    //x1 y1 x2 y2
                                    case MagnetSide.TOP: g.DrawLine(blackpen, 0, hoveringPanel.Location.Y, pnlMain.Width, hoveringPanel.Location.Y); break;
                                    case MagnetSide.BOTTOM: g.DrawLine(blackpen, 0, hoveringPanel.Location.Y + hoveringPanel.Height, pnlMain.Width, hoveringPanel.Location.Y + hoveringPanel.Height); break;
                                    case MagnetSide.LEFT: g.DrawLine(blackpen, hoveringPanel.Location.X, 0, hoveringPanel.Location.X, pnlMain.Height); break;
                                    case MagnetSide.RIGHT: g.DrawLine(blackpen, hoveringPanel.Location.X + hoveringPanel.Width, 0, hoveringPanel.Location.X + hoveringPanel.Width, pnlMain.Height); break;
                                }
                            }
                            //g.Dispose();
                        }
                    }
                    else
                    {
                        hoveringOverPnlMain = false;
                    }
                }
                else
                {
                    positions = null;
                }
            });
        }

        [Obsolete]
        private Tuple<bool, Google.Apis.Drive.v3.Data.File> checkForUpdates()
        {
            if (MessageBox.Show("Do you want to check for updates?", "Update", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                gdh = new GoogleDriveHelper.GoogleDriveHelper(Form1.APPLICATION_NAME, Form1.version + 4);
                return gdh.isFileLatestVersion();
            }
            return new Tuple<bool, File>(false, null);
        }
        private Size calculatePnlMainSize()
        {
            return new Size(pnlMain.Width, this.Height - pnlTop.Height);
        }

        #region events
        #region Form
        private void Form1_Resize(object sender, EventArgs e)
        {
            pnlMain.Size = calculatePnlMainSize();
            initializeR();
            g = pnlMain.CreateGraphics();
        }

        private void Form1_Move(object sender, EventArgs e)
        {
            initializeR();
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
        private void Form1_Activated(object sender, EventArgs e)
        {
            justActivated = DateTime.Now;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            if (topInitHasRan)
            {
                pnlTop.Height = pnlTop.MaximumSize.Height;
            }
            pnlMain.Size = calculatePnlMainSize();
        }
        private void Form1_Deactivate(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            pnlTop.Height = 0;
            pnlMain.Size = calculatePnlMainSize();
        }

        #endregion
        #region pnlMain
        private void pnlMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (hoveringPanel != null)
            {
                placeHoveringPanelInPnlMain();
            }
        }
        #endregion
        #region pnlSearchResults
        private void pnlSearchResults_Click(object sender, EventArgs e)
        {
            if (hoveringPanel != null)
            {
                hoveringPanel = null;
            }
        }
        private async void pnlSearchResults_MouseDown(object sender, MouseEventArgs e)
        {
            if (!mousISDown)
            {
                beginLocation = new Point(Cursor.Position.X + difference, Cursor.Position.Y);
            }
            mousISDown = true;
            await scrollSearchResults();
        }

        private void pnlSearchResults_MouseUp(object sender, MouseEventArgs e)
        {
            mousISDown = false;
            beginLocation = null;
        }
        #endregion
        #region pnlButton
        private void PnlButton_DoubleClick(object sender, EventArgs e)
        {
            singleClick = false;
            pnlMain.BackColor = Color.Gray;
            timer.Start();
            if ((sender as Panel).Parent.Name.Equals("pnlSearchResults"))
            {
                hoveringPanel = generateButton((sender as Panel).Name);
                pnlMain.Controls.Add(hoveringPanel);
            }
            else
            {
                hoveringPanel = (Panel)sender;
            }
        }
        private async void PnlButton_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                if (hoveringPanel != null && hoveringOverPnlMain && (sender as Panel).Name.Equals(hoveringPanel.Name))
                {
                    this.BeginInvoke(new Action(() => {
                        placeHoveringPanelInPnlMain();
                    }));
                    foreach (Panel pnl in pnlSearchResults.Controls)
                    {
                        if (pnl.Name.Equals(hoveringPanel.Name))
                        {
                            this.BeginInvoke(new Action(() =>
                            {
                                pnlSearchResults.Controls.Remove(pnl);
                            }));
                            break;
                        }
                    }
                }
                else if ((sender as Panel).Parent != null && (sender as Panel).Parent.Name.Equals(pnlMain.Name) && this.FormBorderStyle.Equals(FormBorderStyle.Sizable) && (DateTime.Now - justActivated).TotalMilliseconds > 50)
                {
                    singleClick = true;
                    Thread.Sleep(400);
                    if (singleClick)
                    {
                        bool alreadyEditing = false;
                        alreadyInPnlMain = true;
                        FormCollection fc = Application.OpenForms;
                        ButtonSettingsForm bsf = new ButtonSettingsForm(getKeyboardButtonFromPanel((sender as Panel)));
                        foreach (Form frm in fc)
                        {
                            //iterate through
                            if (frm.Name == bsf.Name)
                            {
                                alreadyEditing = true;
                                break;
                            }
                        }
                        if (!alreadyEditing)
                        {
                            bsf.ShowDialog();
                            foreach (Panel pnl in pnlMain.Controls)
                            {
                                if (pnl.Equals(sender))
                                {
                                    this.BeginInvoke(new Action(() => {
                                        pnlMain.Controls.Remove(pnl);
                                        if (!Form1.lastEditedKeyboardButton.Item1)
                                        {
                                            pnlMain.Controls.Add(generateButton(Form1.lastEditedKeyboardButton.Item2));
                                        }
                                    }));
                                    break;
                                }
                            }
                        }
                    }
                }
            });
            initializeBrawlkeys();
        }
        private void PnlButton_MouseDown(object sender, MouseEventArgs e)
        {
            pnlSearchResults_MouseDown(sender, e);
        }

        private void PnlButton_MouseUp(object sender, MouseEventArgs e)
        {
            mousISDown = false;
            beginLocation = null;
        }
        #endregion
        #region btnStart

        private void btnStart_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Text.ToLower().Equals("start"))
            {
                isRunning = true;
                btn.Text = "Stop";
                btn.BackColor = Color.Red;
                ts = new ThreadStart(showPressedKeys);
                t = new Thread(ts);
                t.Start();
            }
            else
            {
                timer.Stop();
                isRunning = false;
                btn.Text = "Start";
                btn.BackColor = Color.Green;
                t.Abort();
            }
        }
        #endregion
        #region btnDelete
        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<Layout> layouts = getAllLayouts();
            bool found = false;
            for (int i = 0; i < layouts.Count; i++)
            {
                if (layouts[i].name.ToLower().Equals(txtLayoutName.Text.ToLower()))
                {
                    found = true;
                    layouts.RemoveAt(i);
                    break;
                }
            }
            System.IO.File.WriteAllText(Form1.jsonFile, generateJsonFromLayoutsList(layouts));
            if (found)
            {
                MessageBox.Show("Layout has been deleted!");
            }
            else
            {
                MessageBox.Show("Layout was not found and couldn't be deleted!");
            }
        }
        #endregion
        #region txtSearch
        private async void txtSearch_TextChanged(object sender, EventArgs e)
        {
            await Task.Run(() => {
                this.BeginInvoke(new Action(() =>
                {
                    pnlSearchResults.Controls.Clear();
                    //addpnlOverlaySearchResults();
                }));
                foreach (Keys key in allKeys)
                {
                    if (key.ToString().ToLower().StartsWith((sender as TextBox).Text.ToLower()))
                    {
                        bool foundInMain = false;
                        foreach (Panel pnl in pnlMain.Controls)
                        {
                            if (pnl.Name.Equals(key.ToString()))
                            {
                                foundInMain = true;
                                break;
                            }
                        }
                        if (!foundInMain)
                        {
                            Panel pnlButton = generateButton(key.ToString());
                            pnlButton.Size = new Size(pnlButton.Width - margin * 2, pnlButton.Height - margin * 2);
                            pnlButton.Location = new Point((KeyboardButton.DEFAULT_SIZE + (int)(margin / 2)) * pnlSearchResults.Controls.Count, margin);
                            this.BeginInvoke(new Action(() =>
                            {
                                pnlSearchResults.Controls.Add(pnlButton);
                            }));
                        }
                    }
                }
                this.BeginInvoke(new Action(() =>
                {
                    pnlSearchResults.AutoScrollPosition = new Point(0, 0);
                }));
            });
        }
        #endregion
        #region cmbMode
        private void cmbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbModeText = (sender as ComboBox).Text;
            if ((sender as ComboBox).Text.ToLower().Equals("layout"))
            {
                cmbLayouts.Items.Clear();
                fillCmblayouts();
                cmbLayouts.Enabled = true;
                initializeBrawlkeys();
                txtSearch.Enabled = true;
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
                txtLayoutName.Enabled = true;
                pnlSearchResults.Enabled = true;
            }
            else
            {
                cmbLayouts.Enabled = false;
                pnlMain.Controls.Clear();
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
                txtSearch.Enabled = false;
                txtLayoutName.Enabled = false;
                pnlSearchResults.Controls.Clear();
                pnlSearchResults.Enabled = false;
            }
        }
        #endregion
        #region btnSave
        private void btnSave_Click(object sender, EventArgs e)
        {
            List<KeyboardButton> kbs = new List<KeyboardButton>();
            foreach (Panel button in pnlMain.Controls)
            {
                kbs.Add(new KeyboardButton(button.Name, (button.Controls[0] as Label).Text, button.Location, button.Size, (button.Controls[0] as Label).ForeColor, button.BackColor, KeyboardButton.getColorByString(getValueFromDescription("backColor", button))));
            }
            Layout layout = new Layout(txtLayoutName.Text, kbs);
            Tuple<bool, string> result = addLayoutToLayouts(layout.generateJson(), txtLayoutName.Text);
            MessageBox.Show(result.Item2);
        }
        #endregion
        #endregion

        private string getValueFromDescription(string key, Control control)
        {
            string[] splitted = control.AccessibleDescription.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string line in splitted)
            {
                string[] keyValue = line.Split(new string[] { Form1.splitter }, StringSplitOptions.None);
                if (keyValue[0].Equals(key))
                {
                    return keyValue[1];
                }
            }
            return string.Empty;
        }

    }
}