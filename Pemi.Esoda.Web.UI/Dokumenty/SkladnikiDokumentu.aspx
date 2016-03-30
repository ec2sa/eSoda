<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumnNoUpdatePanel.Master"
    AutoEventWireup="true" CodeBehind="SkladnikiDokumentu.aspx.cs" Inherits="Pemi.Esoda.Web.UI.SkladnikiDokumentu"
    Title="Sk³adniki dokumentu" EnableEventValidation="false" %>

<%@ Register Src="~/Controls/DocumentItemUploader.ascx" TagName="DocumentItemUploader"
    TagPrefix="uc1" %>
<%@ Register TagName="ContextItem" TagPrefix="esoda" Src="~/Controls/DocumentContextItem.ascx" %>
<%@ Register TagName="DocumentOperations" TagPrefix="esoda" Src="~/Controls/OperacjeDokumentu.ascx" %>
<%@ Register TagName="ESPDcumentPreview" TagPrefix="esoda" Src="~/Controls/ESPDocumentPreview.ascx" %>
<%@ Register Src="../Controls/DocumentItemWordUploader.ascx" TagName="DocumentItemWordUploader"
    TagPrefix="uc2" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <asp:Timer ID="Timer1" runat="server" Interval="150" Enabled="true" OnTick="Timer1_Tick">
    </asp:Timer>
    <esoda:ContextItem ID="ContextItem2" runat="server"></esoda:ContextItem>
    <h2>
        Lista skanów/plików dokumentu</h2>
    <esoda:DocumentOperations runat="Server" ID="DocumentOperations1" />
    <hr />
    <asp:Label runat="server" ID="lblMessage" ForeColor="Red" />
    <div id="leftNarrowColumn">
        <div runat="server" id="blokListy">
            <h3>
                Lista skanów/plików dokumentu</h3>
            <div style="padding-bottom: 0.5em;">
                <asp:LinkButton Style="width: 7em;" runat="server" Text="dodaj plik..." ToolTip="Dodawanie nowego pliku do dokumentu logicznego"
                    ID="nowyPlik" OnClick="dodawanieNowegoPliku" />
                <asp:LinkButton runat="server" Text="utwórz plik Word..." ToolTip="Dodawanie nowego pliku Word do dokumentu logicznego"
                    ID="nowyWord" OnClick="nowyWord_Click" />
               
            </div>
            <asp:UpdatePanel runat="server" ID="up1">
                <ContentTemplate>
                    <div class="ramka">
                        <asp:Repeater runat="Server" ID="listaPlikow" EnableViewState="true" OnItemDataBound="listaPlikow_ItemDataBound">
                            <ItemTemplate>
                                <asp:Panel ID="blok" runat="server" CssClass="opcjePodgladuPliku">
                                    <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("ID","~/image.aspx?id={0}") %>' />
                                    <asp:Image ID="Image2" runat="Server" ImageUrl="~/App_Themes/StandardLayout/img/WordIcon.jpg" />
                                    <asp:Image ID="Image3" runat="Server" ImageUrl="~/App_Themes/StandardLayout/img/word07.jpg" />
                                    <div style="float: left;">
                                        <div>
                                            <asp:LinkButton runat="server" ID="openWord" CommandName="openWord" Text="Otwórz"
                                                CommandArgument='<%# Eval("ID") %>' /></div>
                                        <div>
                                            <asp:LinkButton runat="server" Text="Otwórz" CommandName="openFile" CommandArgument='<%# Eval("ID") %>' /></div>
                                        <div>
                                            <asp:HyperLink runat="server" ID="imageSave" NavigateUrl='<%# Eval("ID","~/download.aspx?id={0}") %>'
                                                Text="Zapisz" /></div>
                                       <div>
                                       <asp:HyperLink runat="server" ID="saveAsPDF" Text="Zapisz jako PDF" NavigateUrl='<%# Eval("ID","~/download.aspx?id={0}&p=1") %>'/>
                                       </div>
                                        <div>
                                            <asp:LinkButton runat="Server" Text="Dodaj now¹ wersjê" CommandName="newVersion"
                                                CommandArgument='<%# Eval("ID") %>' /></div>
                                        <div>
                                            <asp:LinkButton ID="LinkButton16" runat="server" Text="poka¿/ukryj wersje" CommandName="toggleVersions" /></div>
                                    </div>
                                    <p>
                                        <asp:Literal runat="Server" Text='<%# Eval("OriginalName") %>' /></p>
                                    <p>
                                        <asp:Literal ID="Literal3" runat="Server" Text='<%# Eval("CreationDate") %>' /></p>
                                    <p>
                                        <asp:Literal runat="Server" Text='<%# Eval("Description","({0})") %>' /></p>
                                    <asp:Panel ID="wersje" runat="server" Visible="false">
                                        <h4>
                                            Poprzednie wersje dokumentu</h4>
                                        <asp:Repeater runat="Server" DataSource='<%# Eval("PreviousVersions") %>' ID="listaWersji"
                                            EnableViewState="true" OnItemDataBound="listaPlikow_ItemDataBound" OnItemCommand="listaPlikow_ItemCommand">
                                            <ItemTemplate>
                                                <asp:Panel ID="blok" runat="server" CssClass="opcjePodgladuWersjiPliku">
                                                    <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("ID","~/image.aspx?id={0}") %>' />
                                                    <asp:Image ID="Image2" runat="Server" ImageUrl="~/App_Themes/StandardLayout/img/WordIcon.jpg" />
                                                    <asp:Image ID="Image3" runat="Server" ImageUrl="~/App_Themes/StandardLayout/img/word07.jpg" />
                                                    <div style="float: left">
                                                        <div>
                                                            <asp:LinkButton runat="server" ID="openWord" CommandName="openWord" Text="Otwórz"
                                                                CommandArgument='<%# Eval("ID") %>' /></div>
                                                        <div>
                                                            <asp:LinkButton runat="server" Text="Otwórz" CommandName="openFile" CommandArgument='<%# Eval("ID") %>' /></div>
                                                        <div>
                                                            <asp:HyperLink ID="imageSave" runat="server" NavigateUrl='<%# Eval("ID","~/download.aspx?id={0}") %>'
                                                                Text="Zapisz" /></div>
                                                                 <div>
                                       <asp:HyperLink runat="server" ID="saveAsPDF" Text="Zapisz jako PDF" NavigateUrl='<%# Eval("ID","~/download.aspx?id={0}&p=1") %>'/>
                                       </div>
                                                    </div>
                                                    <p>
                                                        <asp:Literal ID="Literal1" runat="Server" Text='<%# Eval("OriginalName") %>' /></p>
                                                    <p>
                                                        <asp:Literal ID="Literal3" runat="Server" Text='<%# Eval("CreationDate") %>' /></p>
                                                    <p>
                                                        <asp:Literal ID="Literal2" runat="Server" Text='<%# Eval("Description","({0})") %>' /></p>
                                                </asp:Panel>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </asp:Panel>
                                </asp:Panel>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div runat="server" id="blokNarzedzi" visible="false">
            <h3>
                Dostêpne opcje</h3>
            <div id="nawigacja">
                <asp:LinkButton runat="Server" ID="powrotDoListy" Text="« Powrót do listy" />
                <h4>
                    Nawigacja</h4>
                <ul>
                    <li>
                        <asp:LinkButton runat="server" ID="LinkButton1" ToolTip="pierwsza strona" Text="pierwsza"
                            CommandName="nav" CommandArgument="first" OnCommand="listaNarzedzi_Command" /></li>
                    <li>
                        <asp:LinkButton runat="server" ID="LinkButton2" ToolTip="poprzednia strona" Text="poprzednia"
                            CommandName="nav" CommandArgument="previous" OnCommand="listaNarzedzi_Command" /></li>
                    <li>
                        <asp:LinkButton runat="server" ID="LinkButton3" ToolTip="nastêpna strona" Text="nastêpna"
                            CommandName="nav" CommandArgument="next" OnCommand="listaNarzedzi_Command" /></li>
                    <li>
                        <asp:LinkButton runat="server" ID="LinkButton4" ToolTip="ostatnia strona" Text="ostatnia"
                            CommandName="nav" CommandArgument="last" OnCommand="listaNarzedzi_Command" /></li>
                </ul>
                <h4>
                    Obracanie</h4>
                <ul>
                    <li>
                        <asp:LinkButton runat="server" ID="LinkButton12" ToolTip="orientacja oryginalna"
                            Text="po³o¿enie oryginalne" CommandName="rotate" CommandArgument="0" OnCommand="listaNarzedzi_Command" /></li>
                    <li>
                        <asp:LinkButton runat="server" ID="LinkButton9" ToolTip="obrót 90° w prawo" Text="90° w prawo"
                            CommandName="rotate" CommandArgument="r90" OnCommand="listaNarzedzi_Command" /></li>
                    <li>
                        <asp:LinkButton runat="server" ID="LinkButton10" ToolTip="obrót 90° w lewo" Text="90° w lewo"
                            CommandName="rotate" CommandArgument="l90" OnCommand="listaNarzedzi_Command" /></li>
                    <li>
                        <asp:LinkButton runat="server" ID="LinkButton11" ToolTip="obrót 180°" Text="obrót 180°"
                            CommandName="rotate" CommandArgument="180" OnCommand="listaNarzedzi_Command" /></li>
                </ul>
                <h4>
                    Skalowanie</h4>
                <ul>
                    <li>
                        <asp:LinkButton runat="server" ID="LinkButton5" ToolTip="dopasowanie do szerokoœci okna"
                            Text="dopasuj szerokoœæ" CommandName="scale" CommandArgument="width" OnCommand="listaNarzedzi_Command" /></li>
                    <li>
                        <asp:LinkButton runat="server" ID="LinkButton6" ToolTip="dopasowanie do wysokoœci okna"
                            Text="dopasuj wysokoœæ" CommandName="scale" CommandArgument="height" OnCommand="listaNarzedzi_Command" /></li>
                    <li>
                        <asp:LinkButton runat="server" ID="LinkButton7" ToolTip="powiêkszenie o 10%" Text="powiêkszenie"
                            CommandName="scale" CommandArgument="zoomin" OnCommand="listaNarzedzi_Command" /></li>
                    <li>
                        <asp:LinkButton runat="server" ID="LinkButton8" ToolTip="zmniejszenie o 10%" Text="zmniejszenie"
                            CommandName="scale" CommandArgument="zoomout" OnCommand="listaNarzedzi_Command" /></li>
                </ul>
                <h4>
                    Pozsta³e operacje</h4>
                <ul>
                    <li>
                        <asp:LinkButton runat="server" ID="LinkButton13" ToolTip="przywraca ustawienia domyœlne"
                            Text="ustawienia pocz¹tkowe" CommandName="other" CommandArgument="reset" OnCommand="listaNarzedzi_Command" /></li>
                    <li><a href='<%=this.APath %>/download.aspx?fid=<%=this.temporaryFileName %>' title="zapisanie aktualnej postaci">
                        zapisz</a> </li>
                    <li><a href='<%=this.APath %>/download.aspx?id=<%=this.temporaryFileName.Substring(0, (temporaryFileName.IndexOf(".")>0) ? temporaryFileName.IndexOf(".") -1 : 0) %>'
                        title="zapisanie oryginalnej postaci">zapisz orygina³</a></li>
                    <li><a href='<%=this.APath %>/download.aspx?id=<%=this.temporaryFileName.Substring(0, (temporaryFileName.IndexOf(".")>0) ? temporaryFileName.IndexOf(".") -1 : 0) %>&p=1'
                        title="zapisanie oryginalnej postaci do PDF">zapisz orygina³ jako PDF</a></li>
                </ul>
            </div>
        </div>
    </div>
    <div id="rightWideColumn">
        <asp:UpdatePanel ID="up3" runat="server">
            <ContentTemplate>
                <esoda:ESPDcumentPreview ID="docPreview" runat="server" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:UpdatePanel runat="server" ID="up2">
            <ContentTemplate>
                <div runat="Server" id="imagePreviewContent" visible="false">
                    <h3>
                        Podgl¹d skanu</h3>
                    <div id="statusObrazka">
                        <asp:Label runat="server" AssociatedControlID="numerAktualnejStrony" Text="Strona:"
                            ID="label2" />
                        <asp:TextBox runat="Server" ID="numerAktualnejStrony" CssClass="poleStatusu" MaxLength="3"
                            OnTextChanged="zmianaParametrow" Style="width: 3em;" />
                        <span class="status" runat="server" id="liczbaStron" />
                        <asp:Label ID="Label1" runat="server" AssociatedControlID="aktualnaSkala" Text="Skala:" />
                        <asp:TextBox runat="Server" ID="aktualnaSkala" CssClass="poleStatusu" MaxLength="3"
                            OnTextChanged="zmianaParametrow" Style="width: 3em;" Text="50" />
                        <span class="status">%</span>
                        <asp:LinkButton runat="server" ID="zastosuj2" Text="»zastosuj" OnClick="zmianaParametrow" />
                    </div>
                    <hr />
                    <asp:Image runat="server" ID="imagePreview" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="documentItemUploaderContent" runat="server">
            <uc1:DocumentItemUploader ID="DocumentItemUploader1" runat="server" />
        </div>
        <div id="documentItemWordUploaderContetnt" runat="server">
            <uc2:DocumentItemWordUploader ID="DocumentItemWordUploader" runat="server" />
        </div>
    </div>
</asp:Content>
