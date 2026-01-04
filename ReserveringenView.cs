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
    public partial class ReserveringenView : Form
    {
        ReserveringenController controller = new ReserveringenController();

        public ReserveringenView()
        {
            InitializeComponent();
        }

        private void ReserveringenView_Load(object sender, EventArgs e)
        {
            lvReservering.View = System.Windows.Forms.View.Details;
            lvReservering.FullRowSelect = true;

            lvReservering.Columns.Add("AantalPersonen", 150);
            lvReservering.Columns.Add("Status", 100);
            lvReservering.Columns.Add("Datum", 100);
            lvReservering.Columns.Add("Tijdslot", 100);

            LoadReserveringen();
        }

        public void LoadReserveringen()
        {
            lvReservering.Items.Clear();

            List<ReserveringenModel> reserveringenLijst = controller.Read();

            foreach (var reservering in reserveringenLijst)
            {
                ListViewItem item = new ListViewItem(reservering.AantalPersonen.ToString());
                item.SubItems.Add(reservering.Status);
                item.SubItems.Add(reservering.Datum.ToShortDateString());
                item.SubItems.Add(reservering.Tijdslot.ToString());

                item.Tag = reservering;
                lvReservering.Items.Add(item);
            }

        }

        private void btnToevoegen_Click(object sender, EventArgs e)
        {
            Form openform = Application.OpenForms.OfType<ReserveringenToevoegen>().FirstOrDefault();

            if (openform == null)
            {
                ReserveringenToevoegen reserveringen = new ReserveringenToevoegen(this);
                reserveringen.Show();
            }
            else
            {
                openform.BringToFront();
                MessageBox.Show("Dit venster is al open.");
            }
        }

        private void btnVerwijderen_Click(object sender, EventArgs e)
        {
            if (lvReservering.SelectedItems.Count > 0)
            {
                ReserveringenModel taak = (ReserveringenModel)lvReservering.SelectedItems[0].Tag;
                if (controller.Delete(taak.ReserveringsnummerID) == 1)
                {
                    MessageBox.Show($"Reservering {taak.ReserveringsnummerID} is verwijdred");
                }
                else
                {
                    MessageBox.Show("Er is iets mis gegaan bij het verwijderen");
                }
                LoadReserveringen();
            }
        }

        private void btnBewerken_Click(object sender, EventArgs e)
        {
            Form openform = Application.OpenForms.OfType<ReserveringenBewerken>().FirstOrDefault();

            if (lvReservering.SelectedItems.Count == 0)
            {
                MessageBox.Show("Selecteer eerst een speler om te bewerken.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ReserveringenModel geselecteerdeResevering =
            (ReserveringenModel)lvReservering.SelectedItems[0].Tag;

            // Open het bewerkvenster als het nog niet open is
            if (openform == null)
            {
                ReserveringenBewerken bewerkenForm = new ReserveringenBewerken(this, geselecteerdeResevering);
                bewerkenForm.Show();
            }
            else
            {
                // Breng het bestaande venster naar voren
                openform.BringToFront();
                MessageBox.Show("Het bewerkvenster is al open.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }



        }
    }
}
