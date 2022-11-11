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
    public partial class Form2 : Form
    {
        Racun RacunPrenet = new Racun();
        public Form2(Racun r)
        {
            InitializeComponent();
            RacunPrenet = r;
        }
        List<Proizvod> proizvodList = new List<Proizvod>();
        List<string> NaziviProizvoda = new List<string>();
        private void Form2_Load(object sender, EventArgs e)
        {
            
            
            UcitajGrid();

            RacunPrenet.IspisRacuna();

            txtBrojRacuna.Text = RacunPrenet.RacunID.ToString();
            if (RacunPrenet.RacunID==0)
            {
                dateIzmena.Value = DateTime.Now;
            }
            else
                dateIzmena.Value = RacunPrenet.DatumIzmene;

            if (RacunPrenet.RacunID == 0)
            {
                dateKreiranje.Value = DateTime.Now;
            }
            else
                dateKreiranje.Value = RacunPrenet.DatumKreiranja;



            txtUkupnaCena.Text = RacunPrenet.UkupnaCena.ToString();
            comboBoxStatus.Text = RacunPrenet.NazivStatusa.ToString();

            txtBrojRacuna.Enabled = false;
            dateKreiranje.Enabled = false;
            dateIzmena.Enabled = false;
            txtUkupnaCena.Enabled=false;

            //MessageBox.Show(stavkaList.Count.ToString());     

        }
        List<Stavka> stavkaList = new List<Stavka>();
        private void UcitajGrid()
        {
            try
            {
                proizvodList = Dapper.UcitajProizvode();
                foreach (Proizvod pr in proizvodList)
                {
                    NaziviProizvoda.Add(pr.NazivProizvoda);
                }
                ProizvodGridView.DataSource = NaziviProizvoda;

                stavkaList = Dapper.UcitajStavke(RacunPrenet.RacunID);
                List2DataTable konverter = new List2DataTable();

                DataTable dt = konverter.ToDataTable(stavkaList);

                dataGridView1.DataSource = dt;

                for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
                {
                    int promenjiva = stavkaList[i].ProizvodID;
                    int porizvodID = 0;
                    foreach (Proizvod p in proizvodList)
                    {
                        if (promenjiva==p.ProizvodID)
                        {
                            porizvodID = promenjiva - 1;
                        }
                    }


                    dataGridView1.Rows[i].Cells[0].Value = proizvodList[porizvodID].NazivProizvoda.ToString() ;
                }

            //  dataGridView1.Columns.RemoveAt(1);
                dataGridView1.Columns.RemoveAt(2);
               dataGridView1.Columns.RemoveAt(2);

                dataGridView1.Columns[1].Visible = false;

                //staviti da je kolona hidden
                // napraviti i za delete u merge not matched by source
                //prikazati podatke racuna u drugoj formi

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            
        }
       
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            MiniUpdate();
        }
        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            MiniUpdate();
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            MiniUpdate();
        }
        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            MiniUpdate();
        }


        List<int> listaProizvodID = new List<int>();

        List<Stavka> listStavkeGrid = new List<Stavka>();
        
        private void button2_Click(object sender, EventArgs e)
        {
           // listStavkeGrid = stavkaList;
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                listStavkeGrid.Add(new Stavka());
                listStavkeGrid[i].RacunId = RacunPrenet.RacunID;
                //proizvod id 
                foreach (Proizvod p in proizvodList)
                {
                    if (p.NazivProizvoda == dataGridView1.Rows[i].Cells[0].Value.ToString())
                    {
                        listStavkeGrid[i].ProizvodID=p.ProizvodID;
                    }
                }
                if (dataGridView1.Rows[i].Cells[1].Value== DBNull.Value)
                {
                    dataGridView1.Rows[i].Cells[1].Value = 0;
                }
                listStavkeGrid[i].StavkaID = int.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString());
                listStavkeGrid[i].Kolicina = float.Parse( dataGridView1.Rows[i].Cells[2].Value.ToString());
                listStavkeGrid[i].Ukupno = float.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());

            }

            //dataGridView1.DataSource=listStavkeGrid;

            DataTable dt2 = ConvertToDatatable(listStavkeGrid);
            float ukupno = 0;

            foreach (Stavka a in listStavkeGrid)
            {
                ukupno += a.Ukupno;
            }
            DateTime dat = new DateTime();
            dat=DateTime.Now;
            RacunPrenet.UkupnaCena = ukupno;
            RacunPrenet.DatumIzmene = dat;
            dataGridView2.DataSource = dt2;
            Racun r = new Racun();
            r = RacunPrenet;

           r.NazivStatusa = comboBoxStatus.Text;
            r.DatumIzmene = dateIzmena.Value;
            r.DatumKreiranja = dateKreiranje.Value;
           r.UkupnaCena=float.Parse(txtUkupnaCena.Text);

            Dapper.InsertUpdate(dt2,r);
            dt2.Clear();
            listStavkeGrid.Clear();

            MessageBox.Show("Akcija uspesna");
    
            //refrosovati formu kada se unese racun
            //refresovati ukupno na formi2
        }

        private static DataTable ConvertToDatatable<T>(List<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    table.Columns.Add(prop.Name, prop.PropertyType.GetGenericArguments()[0]);
                else
                    table.Columns.Add(prop.Name, prop.PropertyType);
            }

            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

       

        public void MiniUpdate()
        {
            //izracunava ukupno

            float ukupnoTXT = 0;
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {

                float cena = 0;
                float ukupno = 0;
                foreach (Proizvod p in proizvodList)
                {
                    if (p.NazivProizvoda == dataGridView1.Rows[i].Cells[0].Value.ToString())
                    {
                        cena = p.Cena;
                    }
                }
                ukupno = cena * float.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString());
                dataGridView1.Rows[i].Cells[3].Value = ukupno;
                ukupnoTXT += ukupno;
                txtUkupnaCena.Text = ukupnoTXT.ToString();

            }
        }


    }
}
