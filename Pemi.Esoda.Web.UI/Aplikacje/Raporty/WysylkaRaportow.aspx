<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="WysylkaRaportow.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.Raporty.WysylkaRaportow" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
<script type="text/javascript">
function OnChange(dropdown) {

    var myindex = dropdown.selectedIndex;
    var SelValue = dropdown.options[myindex].value;
    
    if (SelValue == "CasesPending")
        showdiv("DaysParams");
    if (SelValue == "CasesOutOfDate")
        hidediv("DaysParams");
    return true;
}

function hidediv(id) {
	//safe function to hide an element with a specified id
	if (document.getElementById) { // DOM3 = IE5, NS6
		document.getElementById(id).style.display = 'none';
	}
	else {
		if (document.layers) { // Netscape 4
			document.id.display = 'none';
		}
		else { // IE 4
			document.all.id.style.display = 'none';
		}
	}
}

function showdiv(id) {
	//safe function to show an element with a specified id
		  
	if (document.getElementById) { // DOM3 = IE5, NS6
		document.getElementById(id).style.display = 'block';
	}
	else {
		if (document.layers) { // Netscape 4
			document.id.display = 'block';
		}
		else { // IE 4
			document.all.id.style.display = 'block';
		}
	}
}
</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <h2>Wysyłka raportów</h2>
   
    <fieldset style="margin:1em 0;">
        <p>
            <label><strong>Zawartość raportu:</strong></label>
        </p>
        <label>Raport</label>
        <asp:DropDownList runat="server" ID="ddlReports" onchange='OnChange(this);'/>
        <p>
            <br />
        </p>
        <p>
            <label>Sprawy z wydziału</label>
            <asp:DropDownList runat="server" ID="ddlOUID" />
        </p>
        <p>
            <label>Teczki z roku</label>
            <asp:TextBox runat="server" ID="tbYear" Style="width: 4em" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <label>Teczki z JRWA</label> 
            <asp:TextBox runat="server" ID="tbJRWA" Style="width: 4em" />
        </p>
        <p id="DaysParams">
            <label>Termin upływa za</label>  
            <asp:TextBox runat="server" ID="tbDaysFrom" Style="width: 4em" /> do
            <asp:TextBox runat="server" ID="tbDaysTo" Style="width: 4em" /> dni.
        </p>
        <p>
            <br />
        </p>
        <p>
            <label><strong>Odbiorca raportu:</strong></label>
        </p>
        <p>
            <label>Wydział</label>
            <asp:DropDownList runat="server" ID="ddlDepartment" AutoPostBack="true" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChange" />
        </p>
        <p>
            <label>Pracownik</label>
            <asp:DropDownList runat="server" ID="ddlEmployee" />
        </p>
        
        <p>
            <label>Format: </label>  
            <asp:DropDownList runat="server" ID="ddlReportType" />
        </p>
        <p>
            <asp:LinkButton runat="server" ID="lbtnAdd" Text="Dodaj" OnClick="lbtnAdd_Click"/>
        </p>
    </fieldset>
    <asp:Button ID="btnSend" runat="server" Text="Wyślij" onclick="btnSend_click"/>
    <asp:GridView ID="gvSubscriptions" runat="server" CssClass="grid fullWidth" 
        AutoGenerateColumns="False" onrowcommand="gvSubscriptions_RowCommand" 
        DataKeyNames="ID" >
        <Columns>
            <asp:BoundField HeaderText="Raport" DataField="ReportFullName" />
            <asp:BoundField HeaderText="Parametry" DataField="ReportParameters" />
            <asp:BoundField HeaderText="Odbiorca" DataField="Recipient" />
            <asp:BoundField HeaderText="Email odbiorcy" DataField="emailAddress" />
            <asp:BoundField HeaderText="Format" DataField="Format" />
            
            <asp:TemplateField HeaderText="Operacje">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDelete" runat="server" Text="usuń" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            
        </Columns>
    </asp:GridView>
    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
