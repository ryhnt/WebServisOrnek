using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace WebServisOrnek
{
    public partial class Default : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection("server=LAPTOP-9U5K7JLD\\SQLEXPRESS;Database=Okul;Integrated Security=True");
        SqlCommand komut;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                servissehir.WebService1SoapClient s = new servissehir.WebService1SoapClient();

                DataSet dt = new DataSet();

                dt = s.sehirgetir();




                drop.DataTextField = dt.Tables[0].Columns["sehir"].ToString();
                drop.DataValueField = dt.Tables[0].Columns["id"].ToString();
                drop.DataSource = dt.Tables[0];
                drop.DataBind();
            }
           
        }

        protected void Kaydet_Click(object sender, EventArgs e)
        {
            long tc = long.Parse(txttc.Text);
            int yil = int.Parse(txtdogum.Text);

            bool? durum;
            try
            {
                using(tcServis.KPSPublicSoapClient tcs=new tcServis.KPSPublicSoapClient())
                {
                    durum = tcs.TCKimlikNoDogrula(tc, txtad.Text, txtsoyad.Text, yil);
                }

            }
            catch 
            {

                durum = null;
            }

            if (durum==true)
            {
               
                komut = new SqlCommand("INSERT INTO Ogrenciler (Ad, Soyad,Tc,DogumYil,Sehir) VALUES(@adi, @soyadi,@tc,@yil,@sehir)",con);
                komut.Parameters.AddWithValue("@adi", txtad.Text);
                komut.Parameters.AddWithValue("@soyadi", txtsoyad.Text);
                komut.Parameters.AddWithValue("@tc", txttc.Text);
                komut.Parameters.AddWithValue("@yil", txtdogum.Text);
                komut.Parameters.AddWithValue("@sehir", drop.SelectedItem.Text.ToString());

                con.Open();
                komut.ExecuteNonQuery();
                Response.Write("<script language=javascript>alert('Kayıt Eklendi.');</script>");
            }
            else
            {
                Response.Write("<script language=javascript>alert('Tc Kimlik Hatalı.');</script>");
            }

           
        }
    }
}