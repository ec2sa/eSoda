using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using Pemi.Esoda.DTO;

namespace Pemi.Esoda.Web.UI.Aplikacje.Statystyki
{
    public partial class PrzegladanieStatystyk : System.Web.UI.Page
    {
        protected string generujKontrolke(int nr, DTO.ParametrStatystyki parametr)
        {
            StringBuilder ctrl = new StringBuilder();
            string klasaPola = parametr.Wymagany ? "CssClass=\"wymagane\"" : "";

            if (String.IsNullOrEmpty(parametr.ZrodloDanych))
            {
                ctrl.Append("<br/><asp:Label runat=\"Server\" AssociatedControlID=\"pole{0}\" ID=\"etyk{0}\" Text=\"{1}\" /><br/><asp:TextBox validationGroup=\"df\" runat=\"Server\" ID=\"pole{0}\" Text=\"{2}\" " + klasaPola + " />");
            }
            else
            {
                ctrl.Append("<br/><asp:Label runat=\"Server\" AssociatedControlID=\"pole{0}\" ID=\"etyk{0}\" Text=\"{1}\" /><br/><asp:DropDownList validationGroup=\"df\" runat=\"Server\" ID=\"pole{0}\"  " + klasaPola + " >");
                
                Collection<SimpleLookupDTO> list = Pemi.Esoda.Core.Domain.Statystyki.ListaStatusowSpraw(parametr.ZrodloDanych);

                foreach (SimpleLookupDTO item in list)
                {
                    ctrl.Append(String.Format("<asp:ListItem Text=\"{0}\" Value=\"{1}\"></asp:ListItem>",item.Description, item.ID));
                }
                ctrl.Append("</asp:DropDownList>");
            }

            if (parametr.Wymagany)
                ctrl.Append("<asp:RequiredFieldValidator validationGroup=\"df\" runat=\"server\" ControlToValidate=\"pole{0}\" ErrorMessage='Pole \"{1}\" jest wymagane!' Display=\"Dynamic\" />");

            switch (parametr.Typ.ToLower())
            {
                case "data":
                    ctrl.Append("(rrrr-mm-dd)<asp:CompareValidator validationGroup=\"df\" runat=\"server\" Type=\"Date\" Operator=\"DataTypeCheck\" ControlToValidate=\"pole{0}\" ErrorMessage=\"Niepoprawny format daty!\" Display=\"Dynamic\"/>");
                    break;
                case "liczbacalkowita":
                    ctrl.Append("<asp:CompareValidator validationGroup=\"df\" runat=\"server\" Type=\"Integer\" Operator=\"DataTypeCheck\" ControlToValidate=\"pole{0}\" ErrorMessage=\"Niepoprawny format liczby ca³kowitej!\" Display=\"Dynamic\"/>");
                    break;
                case "liczbarzeczywista":
                    ctrl.Append("<asp:CompareValidator validationGroup=\"df\" runat=\"server\" Type=\"Double\" Operator=\"DataTypeCheck\" ControlToValidate=\"pole{0}\" ErrorMessage=\"Niepoprawny format liczby rzeczywistej!\" Display=\"Dynamic\"/>");
                    break;
            }

            return string.Format(ctrl.ToString(), nr, parametr.Nazwa, parametr.DomyslnaWartosc);
        }

        protected List<DTO.StatystykaDTO> statystyki
        {
            get
            {
                if (Session["1BFD93DC-0E47-4169-8026-E7E0DA561A5A"] == null)
                    Session["1BFD93DC-0E47-4169-8026-E7E0DA561A5A"] = Pemi.Esoda.Core.Domain.Statystyki.ListaStatystyk();
                return Session["1BFD93DC-0E47-4169-8026-E7E0DA561A5A"] as List<DTO.StatystykaDTO>;
            }
            set
            {
                if (value == null)
                    Session.Remove("1BFD93DC-0E47-4169-8026-E7E0DA561A5A");
            }
        }

        protected StanyStrony stan
        {
            get
            {
                if (Session["5F25B7DC-29A4-466f-9AD6-FEA409FE098E"] == null)
                    Session["5F25B7DC-29A4-466f-9AD6-FEA409FE098E"] = StanyStrony.listaStatystyk;
                return (StanyStrony)Session["5F25B7DC-29A4-466f-9AD6-FEA409FE098E"];
            }
            set
            {
                Session["5F25B7DC-29A4-466f-9AD6-FEA409FE098E"] = value;
                switch (value)
                {
                    case StanyStrony.listaStatystyk:
                        blokListyStatystyk.Visible = true;
                        blokParametrowStatystyki.Visible = false;
                        blokTresciStatystyki.Visible = false;
                        blokBledu.Visible = false;
                        break;
                    case StanyStrony.parametryStatystyki:
                        blokListyStatystyk.Visible = false;
                        blokParametrowStatystyki.Visible = true;
                        blokTresciStatystyki.Visible = false;
                        blokBledu.Visible = false;
                        nazwaStatystyki.InnerText = statystyki[listaStatystyk.SelectedIndex].Tytul;
                        opisStatystyki.InnerText = statystyki[listaStatystyk.SelectedIndex].Opis;
                        break;
                    case StanyStrony.przegladanieStatystyki:
                        blokListyStatystyk.Visible = false;
                        blokParametrowStatystyki.Visible = true;
                        blokTresciStatystyki.Visible = true;
                        blokBledu.Visible = false;
                        break;
                    case StanyStrony.Blad:
                        blokListyStatystyk.Visible = false;
                        blokParametrowStatystyki.Visible = true;
                        blokTresciStatystyki.Visible = false;
                        blokBledu.Visible = true;
                        break;
                }

            }
        }

        private void generujPolaFormularza()
        {

            if (listaStatystyk.SelectedIndex < 0) return;
            DTO.StatystykaDTO statystyka = statystyki[listaStatystyk.SelectedIndex];

            if (statystyka.Parametry.Count == 0)
            {
                dynamicznePola.Controls.Add(new LiteralControl("Statystyka nie wymaga podawania parametrów"));
                return;
            }
            StringBuilder szablon = new StringBuilder();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            int i = 0;
            foreach (DTO.ParametrStatystyki parametr in statystyka.Parametry)
            {
                sb.Append(generujKontrolke(i, parametr));
                i++;
            }
            dynamicznePola.Controls.Add(ParseControl(sb.ToString()));
        }

        protected object[] generujParametry(DTO.StatystykaDTO st)
        {
            object[] parametry = new object[st.Parametry.Count];

            string ps = string.Empty;

            for (int i = 0; i < st.Parametry.Count; i++)
            {
                if (String.IsNullOrEmpty(st.Parametry[i].ZrodloDanych))
                {
                    ps = (dynamicznePola.FindControl("pole" + i.ToString()) as TextBox).Text;
                }
                else
                {
                    ps = (dynamicznePola.FindControl("pole" + i.ToString()) as DropDownList).SelectedValue;
                }

                if (ps == "") ps = null;
                switch (st.Parametry[i].Typ.ToLower())
                {
                    case "data":
                        if (ps == null)
                            parametry[i] = null;
                        else
                            parametry[i] = DateTime.Parse(ps);
                        break;
                    case "liczbacalkowita":
                        if (ps == null)
                            parametry[i] = null;
                        else
                            parametry[i] = int.Parse(ps);
                        break;
                    case "liczbarzeczywista":
                        if (ps == null)
                            parametry[i] = null;
                        else
                            parametry[i] = decimal.Parse(ps);
                        break;
                    default:
                        parametry[i] = ps;
                        break;
                }
            }
            return parametry;
        }

        protected void czyscDynamicznePola()
        {
            if (listaStatystyk.SelectedIndex < 0) return;
            for (int i = 0; i < statystyki[listaStatystyk.SelectedIndex].Parametry.Count; i++)
                (dynamicznePola.FindControl("pole" + i.ToString()) as TextBox).Text = string.Empty;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                stan = StanyStrony.listaStatystyk;
                statystyki = null;
                listaStatystyk.DataSource = statystyki;
                listaStatystyk.DataTextField = "Tytul";
                listaStatystyk.DataValueField = "Id";
                listaStatystyk.DataBind();
            }
            generujPolaFormularza();
        }

        protected void wyborStatystyki_Click(object sender, EventArgs e)
        {
            stan = StanyStrony.parametryStatystyki;
        }

        protected void powrotDoListyStatystyk_Click(object sender, EventArgs e)
        {
            //czyscDynamicznePola();
            stan = StanyStrony.listaStatystyk;
            Response.Redirect("~/Aplikacje/Statystyki/przegladanieStatystyk.aspx");
        }

        protected void generowanieStatystyki_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            stan = StanyStrony.przegladanieStatystyki;

            DTO.StatystykaDTO stat = statystyki[listaStatystyk.SelectedIndex];

            object[] parametry = generujParametry(stat);

            if (!stat.PosiadaXslt)
            {
                DataSet ds = Pemi.Esoda.Core.Domain.Statystyki.WywolanieStatystyki(stat.NazwaProcedury, parametry);
                foreach (DataTable dt in ds.Tables)
                {
                    GridView gv = new GridView();
                    gv.GridLines = GridLines.None;
                    gv.CssClass = "grid fullWidth";
                    blokTresciStatystyki.Controls.Add(gv);
                    gv.DataSource = dt;
                    gv.DataBind();
                }
            }
            else
            {
                System.Xml.Xsl.XslTransform tr = new System.Xml.Xsl.XslTransform();
                tr.Load(System.Xml.XmlReader.Create(new System.IO.StringReader(stat.Xslt)));
                statystykaXml.Transform = tr;
                System.Xml.XmlReader xr = Pemi.Esoda.Core.Domain.Statystyki.WywolanieStatystykiXml(stat.NazwaProcedury, parametry);
                if (xr.Read())
                    statystykaXml.DocumentContent = xr.ReadOuterXml();
            }
        }
    }

    public enum StanyStrony
    {
        listaStatystyk,
        parametryStatystyki,
        przegladanieStatystyki,
        Blad
    }
}
