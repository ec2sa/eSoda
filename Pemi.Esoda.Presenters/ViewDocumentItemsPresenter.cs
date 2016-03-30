using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.Tasks;
using Pemi.Esoda.Tools;
using System.IO;
using System.Web.UI;
using Pemi.Esoda.DataAccess;
using System.Data.SqlClient;

namespace Pemi.Esoda.Presenters
{
	public class ViewDocumentItemsPresenter:BasePresenter
	{
		private IViewDocumentItemsView view;
		private IViewDocumentItemsTask service;
		private ISessionProvider session;

		private ImageHelper tmpImg;

        private int documentId;
        public int DocumentId
        {
            set { documentId = value; session["documentId"] = value; }
            get { return view.DocumentId; } // int.Parse(session["documentId"].ToString()); }
        }

		protected ImageHelper currentPicture
		{
			get
			{
				if (tmpImg == null)
					tmpImg = session["094BB1D1-DADE-4dd5-89FC-0C4566F773DE"] as ImageHelper;
				return tmpImg;

			}
			set
			{
				tmpImg = value;
				session["094BB1D1-DADE-4dd5-89FC-0C4566F773DE"] = tmpImg;
			}
		}

		private string scanID
		{
			get
			{
				return session["{6C63DACB-F30C-4caf-B279-C616FB6CB6C6}"]!=null?session["{6C63DACB-F30C-4caf-B279-C616FB6CB6C6}"].ToString():string.Empty;
			}
			set
			{
				session["{6C63DACB-F30C-4caf-B279-C616FB6CB6C6}"] = value;
			}
		}

		public ViewDocumentItemsPresenter(IViewDocumentItemsView view, ISessionProvider session)
		{
			this.view = view;
			this.session = session;	
			this.service=new ViewDocumentItemsTask();
		//	session["idDokumentu"] = 2;
			subscribeToEvents();
			(view as IView).ViewTitle = "Skany/pliki dokumentu";
		}

		public override void Initialize()
		{
            //int documentId = 0;
            //string idds = session["idDokumentu"] != null ? session["idDokumentu"].ToString() : string.Empty;
            //if(!int.TryParse(idds,out documentId))
            //    throw new Exception(string.Format("Brak dokumentu {0} {1} {2}", session["idDokumentu"], idds, documentId));
            //view.DocumentId = documentId;
            //this.documentId = documentId;
            //session["documentId"] = documentId;
            //view.Items = service.GetDocumentItems(documentId);

            view.Items = service.GetDocumentItems(view.DocumentId);            
		}

        public void OnLoad()
        {
            MSOTemplateVisiblity();
        }

        private void MSOTemplateVisiblity()
        {
            try
            {
                view.IsMSOTemplateVisible = new DocumentDAO().CanCreateMSOTemplate();
            }
            catch (SqlException ex)
            {
                
                view.Message = ex.Message;
            }
            catch (Exception ex)
            {
                view.Message = ex.Message;
            }
        }

		protected override void subscribeToEvents()
		{
			view.ExecutingCommand += new EventHandler<ExecutingCommandEventArgs>(view_ExecutingCommand);
			view.ReturnToFileList += new EventHandler(view_ReturnToFileList);
		}

		void view_ReturnToFileList(object sender, EventArgs e)
		{
			File.Delete(string.Format("{0}\\{1}",Configuration.PhysicalTemporaryDirectory,currentPicture.NazwaPlikuRoboczego));
			view.IsInListMode = true;
		}

		void view_ExecutingCommand(object sender, ExecutingCommandEventArgs e)
		{
			switch (e.CommandName)
			{
				case "openFile":
					currentPicture = new ImageHelper(new Guid(e.CommandArgument.ToString()));
					view.previewImageUrl = currentPicture.UrlObrazka;
					view.CurrentPage = currentPicture.AktualnaStrona;
					view.CurrentScale = currentPicture.Skala; // < 50) ? 50 : currentPicture.Skala;
					view.PageCount = currentPicture.LiczbaStron;
					scanID = e.CommandArgument.ToString();
					view.IsInListMode = false;
                    view_ExecutingCommand(null, new ExecutingCommandEventArgs("scale", "width"));
					break;
				case "saveFile":
					break;
				case "changePageOrScale":

					int nrStrony=view.CurrentPage;
					
					if (nrStrony > 0 && nrStrony <= currentPicture.LiczbaStron && nrStrony != currentPicture.AktualnaStrona)
					{
						currentPicture.AktualnaStrona = nrStrony;
						view.CurrentPage = currentPicture.AktualnaStrona;
						view.previewImageUrl = currentPicture.UrlObrazka;
					}

					int skala = view.CurrentScale;
		
					if (skala > 0 && skala <= 100 && skala != currentPicture.Skala)
					{
						currentPicture.Skala = skala;
						view.CurrentScale = currentPicture.Skala;
						view.previewImageUrl = currentPicture.UrlObrazka;
					}
					break;
			
				#region nawigacja
				case "nav":
                    if (currentPicture != null)
                    {
                        switch (e.CommandArgument.ToString())
                        {
                            case "previous":
                                currentPicture.AktualnaStrona--;
                                view.CurrentPage = currentPicture.AktualnaStrona;
                                view.previewImageUrl = currentPicture.UrlObrazka;
                                break;
                            case "next":
                                currentPicture.AktualnaStrona++;
                                view.CurrentPage = currentPicture.AktualnaStrona;
                                view.previewImageUrl = currentPicture.UrlObrazka;
                                break;
                            case "first":
                                currentPicture.AktualnaStrona = 1;
                                view.CurrentPage = currentPicture.AktualnaStrona;
                                view.previewImageUrl = currentPicture.UrlObrazka;
                                break;
                            case "last":
                                currentPicture.AktualnaStrona = currentPicture.LiczbaStron;
                                view.CurrentPage = currentPicture.AktualnaStrona;
                                view.previewImageUrl = currentPicture.UrlObrazka;
                                break;
                        }
                    }
					break;
				#endregion
				#region obracanie
				case "rotate":
					switch (e.CommandArgument.ToString())
					{
						case "0":
							currentPicture.Orientacja = OrientacjaObrazka.Oryginalna;
							view.previewImageUrl = currentPicture.UrlObrazka;
							break;
						case "r90":
							currentPicture.Orientacja = OrientacjaObrazka.ObrotPrawo90;
							view.previewImageUrl = currentPicture.UrlObrazka;
							break;
						case "l90":
							currentPicture.Orientacja = OrientacjaObrazka.ObrotLewo90;
							view.previewImageUrl = currentPicture.UrlObrazka;
							break;
						case "180":
							currentPicture.Orientacja = OrientacjaObrazka.Obrot180;
							view.previewImageUrl = currentPicture.UrlObrazka;
							break;
					}
					break;
				#endregion
				#region skalowanie
				case "scale":
					switch (e.CommandArgument.ToString())
					{
						case "zoomin":
							currentPicture.Skala += 10;
							view.previewImageUrl = currentPicture.UrlObrazka;
							view.CurrentScale = currentPicture.Skala;
							break;
						case "zoomout":
							currentPicture.Skala -= 10;
							view.previewImageUrl = currentPicture.UrlObrazka;
							view.CurrentScale = currentPicture.Skala;
							break;
						case "width":
							currentPicture.AktualnaSzerokosc = 720;
							view.previewImageUrl = currentPicture.UrlObrazka;
							view.CurrentScale = currentPicture.Skala;
							break;
						case "height":
							currentPicture.AktualnaWysokosc = 500;
							view.previewImageUrl = currentPicture.UrlObrazka;
							view.CurrentScale = currentPicture.Skala;
							break;
					}
					break;
				#endregion
				#region pozostale
				case "other":
					switch (e.CommandArgument.ToString())
					{
						case "reset":
							view_ExecutingCommand(null, new ExecutingCommandEventArgs("openFile", scanID)); return;

						case "saveActual":
							//	Server.Transfer("download.aspx?id=" + pozycjaZalacznika + "&amp;tmp=1");
							break;
						case "saveOriginal":
							//	Server.Transfer("download.aspx?id=" + pozycjaZalacznika);
							break;
					}
					break;
				#endregion                
			}
		}

		protected override void redirectToPreviousView()
		{
		
		}
	}
}
