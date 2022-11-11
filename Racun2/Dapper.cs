using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Racun2
{

    //var prob = konekcija.Query<Covek>($"exec dbo.spStoredProcedura @Prezime", new { Prezime = prezime }).ToList();
    //return prob;
    internal class Dapper
    {
        public static List<Racun> UcitajRacun()
        {
            using (System.Data.IDbConnection konekcija = new System.Data.SqlClient.SqlConnection(Helper.KonekcioniString("Racun2")))
            { 


                return konekcija.Query<Racun>($"select * from Racun").ToList();
            }

            
        }

        public static List<Stavka> UcitajStavke(int ID)
        {
            using (System.Data.IDbConnection konekcija = new System.Data.SqlClient.SqlConnection(Helper.KonekcioniString("Racun2")))
            {
                return konekcija.Query<Stavka>($"select * from Stavka where RacunID={ID}").ToList();
            }


        }

        public static List<Proizvod> UcitajProizvode()
        {
            using (System.Data.IDbConnection konekcija = new System.Data.SqlClient.SqlConnection(Helper.KonekcioniString("Racun2")))
            {
                return konekcija.Query<Proizvod>($"select * from Proizvod").ToList();
            }


        }

        public static void InsertUpdate(DataTable dt, Racun rac)
        {
            //using (System.Data.IDbConnection konekcija = new System.Data.SqlClient.SqlConnection(Helper.KonekcioniString("Racun2")))
            //{
            //    konekcija.Execute(@"exec InsertUpdate3 @RacunID,@DatumKreiranja,@DatumIzmene,@UkunaCena,@NazivStatusa,@Tabela",rac,dt);

            //    //using (SqlCommand cmd1 = new SqlCommand("your_procedure_name"))
            //    //{
            //    //    cmd1.CommandType = CommandType.StoredProcedure;
            //    //    cmd1.Connection = con1;

            //    //    SqlParameter Param = cmd1.Parameters.AddWithValue("@parameter_name", dt);
            //    //    Param.SqlDbType = SqlDbType.Structured;
            //    //    con1.Open();
            //    //    cmd1.ExecuteNonQuery();
            //    //    con1.Close();
            //    //}
            //}


            using (SqlConnection con = new SqlConnection(Helper.KonekcioniString("Racun2")))
            {
                using (SqlCommand cmd1 = new SqlCommand(@"InsertUpdate4"))
                {
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Connection = con;

                    SqlParameter param1 = cmd1.Parameters.AddWithValue("@Tabela",dt);
                    SqlParameter param2 = cmd1.Parameters.AddWithValue("@RacunID",rac.RacunID);
                    SqlParameter param3 = cmd1.Parameters.AddWithValue("@DatumKreiranja",rac.DatumKreiranja);
                    SqlParameter param4 = cmd1.Parameters.AddWithValue("@DatumIzmene",rac.DatumIzmene);
                    SqlParameter param5 = cmd1.Parameters.AddWithValue("@UkupnaCena",rac.UkupnaCena);
                    SqlParameter param6 = cmd1.Parameters.AddWithValue("@NazivStatusa",rac.NazivStatusa);
                    param1.SqlDbType = SqlDbType.Structured;
                    con.Open();
                    cmd1.ExecuteNonQuery();
                    con.Close();
                }
            }
        }




    }
}
