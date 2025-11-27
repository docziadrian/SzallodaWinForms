using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;

namespace SzallasFoglalo
{
    

    public class FoForm : MaterialForm
    {
        private MaterialSkinManager materialSkinManager;
        private Panel menuPanel;
        private Panel contentPanel;
        private List<Szallas> szallasok;
        private List<Foglalas> foglalasok;

        public FoForm()
        {
            BetoltAdatok();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue800, Primary.Blue900, Primary.Blue500, Accent.LightBlue200, TextShade.WHITE);

            this.Text = "Szállás Foglaló Rendszer";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;

            menuPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 250,
                BackColor = Color.FromArgb(55, 71, 79)
            };

            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            this.Controls.Add(contentPanel);
            this.Controls.Add(menuPanel);

            KeszitMenuGombok();
            MutatSzallasok();
        }

        private void KeszitMenuGombok()
        {
            int y = 20;
            int magassag = 50;
            int tavolsag = 10;

            var btnSzallasok = new MaterialRaisedButton
            {
                Text = "SZÁLLÁSOK",
                Location = new Point(10, y),
                Size = new Size(230, magassag),
                
            };
            btnSzallasok.Click += (s, e) => MutatSzallasok();
            menuPanel.Controls.Add(btnSzallasok);
            y += magassag + tavolsag;

            var btnUjSzallas = new MaterialRaisedButton
            {
                Text = "ÚJ SZÁLLÁS",
                Location = new Point(10, y),
                Size = new Size(230, magassag)
            };
            btnUjSzallas.Click += (s, e) => MutatUjSzallas();
            menuPanel.Controls.Add(btnUjSzallas);
            y += magassag + tavolsag;

            var btnFoglalasok = new MaterialRaisedButton
            {
                Text = "FOGLALÁSOK",
                Location = new Point(10, y),
                Size = new Size(230, magassag)
            };
            btnFoglalasok.Click += (s, e) => MutatFoglalasok();
            menuPanel.Controls.Add(btnFoglalasok);
            y += magassag + tavolsag;

            var btnUjFoglalas = new MaterialRaisedButton
            {
                Text = "ÚJ FOGLALÁS",
                Location = new Point(10, y),
                Size = new Size(230, magassag)
            };
            btnUjFoglalas.Click += (s, e) => MutatUjFoglalas();
            menuPanel.Controls.Add(btnUjFoglalas);
            y += magassag + tavolsag + 30;

            var btnKilepes = new MaterialRaisedButton
            {
                Text = "KILÉPÉS",
                Location = new Point(10, y),
                Size = new Size(230, magassag),
                BackColor = Color.FromArgb(200, 50, 50)
            };
            btnKilepes.Click += (s, e) =>
            {
                var result = MessageBox.Show("Biztosan ki szeretne lépni?", "Kilépés",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    Application.Exit();
                }
            };
            menuPanel.Controls.Add(btnKilepes);
        }

        private void BetoltAdatok()
        {
            try
            {
                szallasok = FileIO.BetoltSzallasok();
                foglalasok = FileIO.BetoltFoglalasok(szallasok);

                if (szallasok.Count == 0)
                {
                    MintaAdatokLetrehozasa();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba az adatok betöltése közben: {ex.Message}", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                szallasok = new List<Szallas>();
                foglalasok = new List<Foglalas>();
                MintaAdatokLetrehozasa();
            }
        }

        private void MintaAdatokLetrehozasa()
        {
            szallasok.Add(new Hotel("Duna Palace Hotel", "Budapest, Váci utca 1.", 200, 25000, true, true, 5, true));
            szallasok.Add(new Hotel("City Hotel", "Budapest, Rákóczi út 44.", 80, 15000, true, false, 3, false));
            szallasok.Add(new Panzio("Balaton Panzió", "Siófok, Part utca 12.", 30, 8000, true, true, true));
            szallasok.Add(new Panzio("Tisza Panzió", "Szeged, Tisza part 5.", 20, 7000, false, false, true));
            szallasok.Add(new Vendeghaz("Mátra Vendégház", "Mátraháza, Erdő utca 8.", 15, 6000, "Nagy István", "+36301234567", true));
            szallasok.Add(new Apartman("Panoráma Apartman", "Eger, Vár utca 22.", 6, 12000, "Kovács Anna", "+36307654321", 3, true));

            MentAdatok();
        }

        private void MentAdatok()
        {
            try
            {
                FileIO.MentSzallasok(szallasok);
                FileIO.MentFoglalasok(foglalasok);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba az adatok mentése közben: {ex.Message}", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TisztitContentPanel()
        {
            contentPanel.Controls.Clear();
        }

        private void MutatSzallasok()
        {
            TisztitContentPanel();

            var cimLabel = new MaterialLabel
            {
                Text = "ELÉRHETŐ SZÁLLÁSOK",
                Location = new Point(20, 20),
                Font = new Font("Roboto", 24, FontStyle.Bold),
                AutoSize = true
            };
            contentPanel.Controls.Add(cimLabel);

            var listView = new ListView
            {
                Location = new Point(20, 80),
                Size = new Size(contentPanel.Width - 40, contentPanel.Height - 140),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            listView.Columns.Add("Típus", 100);
            listView.Columns.Add("Név", 200);
            listView.Columns.Add("Cím", 200);
            listView.Columns.Add("Kapacitás", 80);
            listView.Columns.Add("Alapár", 100);
            listView.Columns.Add("Státusz", 80);
            listView.Columns.Add("Részletek", 300);

            foreach (var szallas in szallasok)
            {
                var item = new ListViewItem(szallas.GetTipus());
                item.SubItems.Add(szallas.Nev);
                item.SubItems.Add(szallas.Cim);
                item.SubItems.Add(szallas.Kapacitas.ToString());
                item.SubItems.Add($"{szallas.AlapAr:N0} Ft");
                item.SubItems.Add(szallas.Foglalt ? "Foglalt" : "Szabad");
                item.SubItems.Add(szallas.LeirasKeszites());
                item.Tag = szallas;
                listView.Items.Add(item);
            }

            contentPanel.Controls.Add(listView);

            var btnTorles = new MaterialRaisedButton
            {
                Text = "KIJELÖLT SZÁLLÁS TÖRLÉSE",
                Location = new Point(20, contentPanel.Height - 50),
                Size = new Size(250, 40),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };

            btnTorles.Click += (s, e) =>
            {
                if (listView.SelectedItems.Count > 0)
                {
                    var szallas = listView.SelectedItems[0].Tag as Szallas;
                    if (szallas != null)
                    {
                        var result = MessageBox.Show(
                            $"Biztosan törli a(z) \"{szallas.Nev}\" szállást? A hozzá tartozó foglalások is törlődnek.",
                            "Megerősítés",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            
                            foglalasok.RemoveAll(f => f.Szallas == szallas);
                            szallasok.Remove(szallas);
                            MentAdatok();
                            MutatSzallasok();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Kérem válasszon ki egy szállást!", "Figyelmeztetés",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            contentPanel.Controls.Add(btnTorles);
        }

        private void MutatUjSzallas()
        {
            TisztitContentPanel();

            var cimLabel = new MaterialLabel
            {
                Text = "ÚJ SZÁLLÁS HOZZÁADÁSA",
                Location = new Point(20, 20),
                Font = new Font("Roboto", 24, FontStyle.Bold),
                AutoSize = true
            };
            contentPanel.Controls.Add(cimLabel);

            int y = 100;
            int labelWidth = 150;
            int inputWidth = 300;

            var tipusLabel = new MaterialLabel { Text = "Típus:", Location = new Point(20, y), AutoSize = true };
            var tipusCombo = new ComboBox
            {
                Location = new Point(20 + labelWidth, y),
                Width = inputWidth,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            tipusCombo.Items.AddRange(new[] { "Hotel", "Panzió", "Vendégház", "Apartman" });
            tipusCombo.SelectedIndex = 0;
            contentPanel.Controls.Add(tipusLabel);
            contentPanel.Controls.Add(tipusCombo);
            y += 50;

            var nevLabel = new MaterialLabel { Text = "Név:", Location = new Point(20, y), AutoSize = true };
            var nevText = new MaterialSingleLineTextField { Location = new Point(20 + labelWidth, y), Width = inputWidth };
            contentPanel.Controls.Add(nevLabel);
            contentPanel.Controls.Add(nevText);
            y += 50;

            var cimLabel2 = new MaterialLabel { Text = "Cím:", Location = new Point(20, y), AutoSize = true };
            var cimText = new MaterialSingleLineTextField { Location = new Point(20 + labelWidth, y), Width = inputWidth };
            contentPanel.Controls.Add(cimLabel2);
            contentPanel.Controls.Add(cimText);
            y += 50;

            var kapacitasLabel = new MaterialLabel { Text = "Kapacitás:", Location = new Point(20, y), AutoSize = true };
            var kapacitasText = new MaterialSingleLineTextField { Location = new Point(20 + labelWidth, y), Width = inputWidth };
            contentPanel.Controls.Add(kapacitasLabel);
            contentPanel.Controls.Add(kapacitasText);
            y += 50;

            var arLabel = new MaterialLabel { Text = "Alapár (Ft):", Location = new Point(20, y), AutoSize = true };
            var arText = new MaterialSingleLineTextField { Location = new Point(20 + labelWidth, y), Width = inputWidth };
            contentPanel.Controls.Add(arLabel);
            contentPanel.Controls.Add(arText);
            y += 70;

            var btnMentes = new MaterialRaisedButton
            {
                Text = "MENTÉS",
                Location = new Point(20, y),
                Size = new Size(150, 40)
            };

            btnMentes.Click += (s, e) =>
            {
                try
                {
                    string tipus = tipusCombo.SelectedItem.ToString();
                    string nev = nevText.Text;
                    string cim = cimText.Text;
                    int kapacitas = int.Parse(kapacitasText.Text);
                    decimal ar = decimal.Parse(arText.Text);

                    if (string.IsNullOrWhiteSpace(nev) || string.IsNullOrWhiteSpace(cim))
                    {
                        MessageBox.Show("Kérem töltse ki az összes mezőt!", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    Szallas ujSzallas = null;

                    switch (tipus)
                    {
                        case "Hotel":
                            ujSzallas = new Hotel(nev, cim, kapacitas, ar, true, true, 4, false);
                            break;
                        case "Panzió":
                            ujSzallas = new Panzio(nev, cim, kapacitas, ar, true, false, true);
                            break;
                        case "Vendégház":
                            ujSzallas = new Vendeghaz(nev, cim, kapacitas, ar, "Tulajdonos", "+36301111111", true);
                            break;
                        case "Apartman":
                            ujSzallas = new Apartman(nev, cim, kapacitas, ar, "Tulajdonos", "+36301111111", 2, true);
                            break;
                    }

                    if (ujSzallas != null)
                    {
                        szallasok.Add(ujSzallas);
                        MentAdatok();
                        MessageBox.Show("Szállás sikeresen hozzáadva!", "Siker", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        MutatSzallasok();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hiba történt: {ex.Message}", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            contentPanel.Controls.Add(btnMentes);
        }

        private void MutatFoglalasok()
        {
            TisztitContentPanel();

            var cimLabel = new MaterialLabel
            {
                Text = "FOGLALÁSOK",
                Location = new Point(20, 20),
                Font = new Font("Roboto", 24, FontStyle.Bold),
                AutoSize = true
            };
            contentPanel.Controls.Add(cimLabel);

            var listView = new ListView
            {
                Location = new Point(20, 80),
                Size = new Size(contentPanel.Width - 40, contentPanel.Height - 160),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            listView.Columns.Add("Vendég neve", 150);
            listView.Columns.Add("Szállás", 200);
            listView.Columns.Add("Érkezés", 120);
            listView.Columns.Add("Éjszakák", 80);
            listView.Columns.Add("Végösszeg", 120);
            listView.Columns.Add("Távozás", 120);

            foreach (var foglalas in foglalasok)
            {
                var item = new ListViewItem(foglalas.VendegNeve);
                item.SubItems.Add(foglalas.Szallas.Nev);
                item.SubItems.Add(foglalas.ErkezesiDatum.ToString("yyyy-MM-dd"));
                item.SubItems.Add(foglalas.EjszakakSzama.ToString());
                item.SubItems.Add($"{foglalas.Vegosszeg:N0} Ft");
                item.SubItems.Add(foglalas.ErkezesiDatum.AddDays(foglalas.EjszakakSzama).ToString("yyyy-MM-dd"));
                item.Tag = foglalas;
                listView.Items.Add(item);
            }

            contentPanel.Controls.Add(listView);

            var btnTorles = new MaterialRaisedButton
            {
                Text = "KIJELÖLT TÖRLÉSE",
                Location = new Point(20, contentPanel.Height - 60),
                Size = new Size(200, 40),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };

            btnTorles.Click += (s, e) =>
            {
                if (listView.SelectedItems.Count > 0)
                {
                    var foglalas = listView.SelectedItems[0].Tag as Foglalas;
                    if (foglalas != null)
                    {
                        var result = MessageBox.Show($"Biztosan törli {foglalas.VendegNeve} foglalását?",
                            "Megerősítés", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            foglalas.Szallas.LemondFoglalas();
                            foglalasok.Remove(foglalas);
                            MentAdatok();
                            MutatFoglalasok();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Kérem válasszon ki egy foglalást!", "Figyelmeztetés",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            contentPanel.Controls.Add(btnTorles);
        }

        private void MutatUjFoglalas()
        {
            TisztitContentPanel();

            var cimLabel = new MaterialLabel
            {
                Text = "ÚJ FOGLALÁS",
                Location = new Point(20, 20),
                Font = new Font("Roboto", 24, FontStyle.Bold),
                AutoSize = true
            };
            contentPanel.Controls.Add(cimLabel);

            int y = 100;
            int labelWidth = 150;
            int inputWidth = 300;

            var vendegLabel = new MaterialLabel { Text = "Vendég neve:", Location = new Point(20, y), AutoSize = true };
            var vendegText = new MaterialSingleLineTextField { Location = new Point(20 + labelWidth, y), Width = inputWidth };
            contentPanel.Controls.Add(vendegLabel);
            contentPanel.Controls.Add(vendegText);
            y += 50;

            var szallasLabel = new MaterialLabel { Text = "Szállás:", Location = new Point(20, y), AutoSize = true };
            var szallasCombo = new ComboBox
            {
                Location = new Point(20 + labelWidth, y),
                Width = inputWidth,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            foreach (var szallas in szallasok.Where(s => !s.Foglalt))
            {
                szallasCombo.Items.Add(szallas);
            }
            szallasCombo.DisplayMember = "Nev";

            if (szallasCombo.Items.Count > 0)
                szallasCombo.SelectedIndex = 0;

            contentPanel.Controls.Add(szallasLabel);
            contentPanel.Controls.Add(szallasCombo);
            y += 50;

            var erkezesLabel = new MaterialLabel { Text = "Érkezés:", Location = new Point(20, y), AutoSize = true };
            var erkezesDate = new DateTimePicker
            {
                Location = new Point(20 + labelWidth, y),
                Width = inputWidth,
                Format = DateTimePickerFormat.Short,
                MinDate = DateTime.Now
            };
            contentPanel.Controls.Add(erkezesLabel);
            contentPanel.Controls.Add(erkezesDate);
            y += 50;

            var ejszakakLabel = new MaterialLabel { Text = "Éjszakák száma:", Location = new Point(20, y), AutoSize = true };
            var ejszakakText = new MaterialSingleLineTextField { Location = new Point(20 + labelWidth, y), Width = inputWidth };
            contentPanel.Controls.Add(ejszakakLabel);
            contentPanel.Controls.Add(ejszakakText);
            y += 50;

            var arLabel = new MaterialLabel
            {
                Text = "Végösszeg: 0 Ft",
                Location = new Point(20, y),
                Font = new Font("Roboto", 14, FontStyle.Bold),
                AutoSize = true,
                ForeColor = Color.Green
            };
            contentPanel.Controls.Add(arLabel);
            y += 20;

            var reszletekLabel = new MaterialLabel
            {
                Text = "",
                Location = new Point(20, y),
                AutoSize = true,
                MaximumSize = new Size(600, 0)
            };
            contentPanel.Controls.Add(reszletekLabel);
            y += 70;

            
            ejszakakText.TextChanged += (s, e) =>
            {
                if (szallasCombo.SelectedItem is Szallas szallas && int.TryParse(ejszakakText.Text, out int ejszakak) && ejszakak > 0)
                {
                    decimal osszeg = szallas.SzamolVegosszeg(ejszakak);
                    arLabel.Text = $"Végösszeg: {osszeg:N0} Ft";
                    reszletekLabel.Text = $"Szállás részletei:\n{szallas.LeirasKeszites()}\n\nAlapár/éj: {szallas.AlapAr:N0} Ft\nÉjszakák: {ejszakak}\nTávozás: {erkezesDate.Value.AddDays(ejszakak):yyyy-MM-dd}";
                }
                else
                {
                    arLabel.Text = "Végösszeg: 0 Ft";
                    reszletekLabel.Text = "";
                }
            };

            szallasCombo.SelectedIndexChanged += (s, e) =>
            {
                ejszakakText.Text = "";
                arLabel.Text = "Végösszeg: 0 Ft";
                reszletekLabel.Text = "";
            };

            var btnFoglalas = new MaterialRaisedButton
            {
                Text = "FOGLALÁS LEADÁSA",
                Location = new Point(20, 600),
                Size = new Size(200, 40)
            };

            btnFoglalas.Click += (s, e) =>
            {
                try
                {
                    if (szallasCombo.Items.Count == 0)
                    {
                        MessageBox.Show("Nincs elérhető szabad szállás!", "Figyelmeztetés",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string vendegNeve = vendegText.Text;
                    var szallas = szallasCombo.SelectedItem as Szallas;
                    DateTime erkezes = erkezesDate.Value;
                    int ejszakak = int.Parse(ejszakakText.Text);

                    if (string.IsNullOrWhiteSpace(vendegNeve))
                    {
                        MessageBox.Show("Kérem adja meg a vendég nevét!", "Figyelmeztetés",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (ejszakak < 1)
                    {
                        MessageBox.Show("Az éjszakák számának legalább 1-nek kell lennie!", "Figyelmeztetés",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var ujFoglalas = new Foglalas(szallas, vendegNeve, erkezes, ejszakak);
                    szallas.Foglalas();
                    foglalasok.Add(ujFoglalas);
                    MentAdatok();

                    MessageBox.Show($"Foglalás sikeresen leadva!\n\nVendég: {vendegNeve}\nSzállás: {szallas.Nev}\nVégösszeg: {ujFoglalas.Vegosszeg:N0} Ft",
                        "Siker", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    MutatFoglalasok();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hiba történt: {ex.Message}", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            contentPanel.Controls.Add(btnFoglalas);
        }
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FoForm());
        }
    }
}