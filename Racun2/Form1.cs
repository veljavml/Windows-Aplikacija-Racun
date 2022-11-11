using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Racun2
{
    public partial class Form1 : Form
    {
       
        public Form1()
        {
            InitializeComponent();
        }
     
        public void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AllowUserToAddRows = true;
            UcitajGrid();
        }

        private void UcitajGrid()
        {
            try
            {
                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                dataGridView1.Rows.Clear();
                DataGridViewComboBoxColumn dgvCmb = new DataGridViewComboBoxColumn();
                List<Racun> Racuni = Dapper.UcitajRacun();

                List2DataTable konverter = new List2DataTable();

                DataTable dt = konverter.ToDataTable(Racuni);

                dataGridView1.DataSource = dt;
                dataGridView1.Columns.RemoveAt(3);
                dataGridView1.Columns.RemoveAt(3);

                dataGridView1.Columns.Add(dgvCmb);
                dgvCmb.Items.Add("aaa");
                dgvCmb.Items.Add("bbb");
                dgvCmb.Items.Add("ccc");

               // dgvCmb.DisplayMember = "lol";





                for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
                {
                   dataGridView1.Rows[i].Cells[4].Value = Racuni[i].NazivStatusa.ToString();
                }

                dataGridView1.Columns[0].HeaderText = "Broj racuna";
                dataGridView1.Columns[4].HeaderText = "Status";




            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message +" OVDE GRESKA ");
            }
        }

        Racun r = new Racun();
        
        private void buttonIzmeni_Click(object sender, EventArgs e)
        {
            UcitajRed();
            Form2 f2 = new Form2(r);
            f2.ShowDialog();
            UcitajGrid();
        }

        private void buttonDodaj_Click(object sender, EventArgs e)
        {
            UcitajRed();
            Form2 f2 = new Form2(r);
            f2.ShowDialog();
            UcitajGrid();
        }

        private void UcitajRed()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Selected)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        int index = cell.ColumnIndex;

                        switch (index)
                        {
                            case 0:

                                if (cell.Value == null)
                                {
                                    r.RacunID = 0;
                                }
                                else
                                {
                                    r.RacunID = Convert.ToInt32(cell.Value);
                                }
                                   

                                break;

                            case 1:
                                r.DatumKreiranja = Convert.ToDateTime(cell.Value);
                                break;

                            case 2:
                                r.DatumIzmene = Convert.ToDateTime(cell.Value);
                                break;

                            case 3:

                                if (cell.Value == null)
                                {
                                    r.UkupnaCena = 0;
                                }
                                else
                                    r.UkupnaCena = float.Parse(cell.Value.ToString());
                                break;

                            case 4:
                                if (cell.Value == null)
                                {
                                    r.NazivStatusa = "";
                                }
                                else
                                    r.NazivStatusa = cell.Value.ToString();
                                break;

                            default:
                                MessageBox.Show("Greska u switch-case");
                                break;
                        }
                    }
                }
            }
           // r.IspisRacuna();
        }
    }   
}
