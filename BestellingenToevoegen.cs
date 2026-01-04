using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Proeflokaal.Controller;
using Proeflokaal.Models;

namespace Proeflokaal.View
{
    public partial class BestellingenToevoegen : Form
    {

        private BestelligenView parentView; 

        public BestellingenToevoegen(BestelligenView view)
        {
            InitializeComponent();
            parentView = view;
            InitializeFormControls();
            LoadReserveringsnummers();
        }

        private void InitializeFormControls()
        {
            DateTime geselecteerdeDatum = dtpBestel.Value.Date;
            TimeSpan huidigeTijd = DateTime.Now.TimeOfDay;
            dtpBestel.Value = DateTime.Now;
            DateTime bestelDatum = geselecteerdeDatum + huidigeTijd; 

            // Status opties
            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("Bevestigd");
            cmbStatus.Items.Add("In behandeling");
            cmbStatus.Items.Add("Afgerond");
            cmbStatus.SelectedIndex = 0;
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        private void LoadReserveringsnummers()
        {
            ReserveringenController controller = new ReserveringenController();
            var reserveringen = controller.Read();

            cmbReservering.Items.Clear();
            foreach (var r in reserveringen)
            {
                cmbReservering.Items.Add(new { r.ReserveringsnummerID, Display = $"Reservering #{r.ReserveringsnummerID}" });
            }

            if (cmbReservering.Items.Count > 0)
            {
                cmbReservering.DisplayMember = "Display";
                cmbReservering.ValueMember = "ReserveringsnummerID";
                cmbReservering.SelectedIndex = 0;
            }
        }


        private void btntoevoegen_Click(object sender, EventArgs e)
        {
            BestellingenController controller = new BestellingenController();

            // Controleer lege velden
            if (cmbReservering.SelectedItem == null || cmbStatus.SelectedItem == null || string.IsNullOrWhiteSpace(txtTafel.Text))
            {
                MessageBox.Show("Alle velden moeten worden ingevuld.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Controleer of tafelnummer een geldig getal is
            if (!int.TryParse(txtTafel.Text, out int tafelNummer) || tafelNummer <= 0)
            {
                MessageBox.Show("Tafelnummer moet een geldig getal zijn.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int reserveringsnummerID = (int)((dynamic)cmbReservering.SelectedItem).ReserveringsnummerID;

            BestellingenModel bestelling = new BestellingenModel
            {
                ReserveringsnummerID = reserveringsnummerID,
                TafelNummer = tafelNummer,
                BestelDatum = dtpBestel.Value,
                Status = cmbStatus.SelectedItem.ToString()
            };

            int result = controller.Create(bestelling);

            if (result == 1)
            {
                MessageBox.Show($"Bestelling toegevoegd voor tafel {bestelling.TafelNummer} op {bestelling.BestelDatum.ToShortDateString()}!");
                parentView.LoadBestellingen(); // ververs de lijst in het hoofdvenster
                this.Close();
            }
            else
            {
                MessageBox.Show("Er is een fout opgetreden bij het toevoegen van de bestelling.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BestellingenToevoegen_Load(object sender, EventArgs e)
        {

        }
    }
}
