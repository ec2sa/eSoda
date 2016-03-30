using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Pemi.Esoda.DataAccess
{
	public class FileSystemItemStorage:IItemStorage
	{
		private string _baseDirectory;

        public Guid GetFSGuid(Guid id)
        {
         return new DocumentDAO().GetDocumentItemStorageID(id);

        }


		public string BaseDirectory
		{
			get
			{
				if (!_baseDirectory.EndsWith("\\"))
					return _baseDirectory + "\\";
				return _baseDirectory;
			}
		}

		/// <summary>
		/// Tworzy repozytorium na bazie wskazanego katalogu
		/// </summary>
		/// <param name="baseDirectory">katalog bazowy dla repozytorium</param>
		public FileSystemItemStorage(string baseDirectory)
		{
			if (!Directory.Exists(baseDirectory)) throw new ArgumentException(string.Format("Wskazany katalog ({0}) nie istnieje",baseDirectory));
			this._baseDirectory = baseDirectory;
		}

		#region IItemStorage Members
	
		Guid IItemStorage.Save(System.IO.Stream contentStream)
		{
			const int BUFFSIZE=1024;
			Guid itemId = Guid.NewGuid();
			try
			{
				using (BinaryReader br = new BinaryReader(contentStream))
				{
					byte[] buff = new byte[BUFFSIZE];

					FileStream fs = File.Create(BaseDirectory + itemId.ToString());
					int bytesRead = 0;
					do
					{
						bytesRead = br.Read(buff, 0, BUFFSIZE);
						fs.Write(buff, 0, bytesRead);
					} while (bytesRead > 0);
					fs.Close();
					return itemId;
				}
			}
			catch(Exception ex)
			{
				DataAccessLogEntry.Create(ex.Message, false);
				return Guid.Empty;
			}
		}

		System.IO.Stream IItemStorage.Load(Guid itemID)
		{
            Guid fsitemID = GetFSGuid(itemID);
			const int BUFFSIZE=1024;
			if(!(this as IItemStorage).Exists(itemID)) 
			{
				string msg=string.Format("Nie istnieje element o id {0}",fsitemID);
				DataAccessLogEntry.Create(msg,false);
				throw new ArgumentException(msg);
			}
			MemoryStream ms = new MemoryStream();
			byte[] buff = new byte[BUFFSIZE];
			FileStream fs = File.Open(BaseDirectory + fsitemID, FileMode.Open);
			int bytesRead = 0;

			do{
				bytesRead = fs.Read(buff, 0, BUFFSIZE);
				ms.Write(buff, 0, bytesRead);
			}while(bytesRead>0);
			fs.Close();
			return ms;
		}

		bool IItemStorage.Exists(Guid itemID)
		{
            Guid fsitemID = GetFSGuid(itemID);
			return File.Exists(BaseDirectory + fsitemID);
		}

        bool IItemStorage.ExistsGuid(Guid itemId)
        {
            return File.Exists(BaseDirectory + itemId.ToString());
        }

        Stream IItemStorage.LoadGuid(Guid itemId)
        {
            const int BUFFSIZE = 1024;
            if (!(this as IItemStorage).ExistsGuid(itemId))
            {
                string msg = string.Format("Nie istnieje element o id {0}", itemId);
                DataAccessLogEntry.Create(msg, false);
                throw new ArgumentException(msg);
            }
            MemoryStream ms = new MemoryStream();
            byte[] buff = new byte[BUFFSIZE];
            FileStream fs = File.Open(BaseDirectory + itemId, FileMode.Open);
            int bytesRead = 0;

            do
            {
                bytesRead = fs.Read(buff, 0, BUFFSIZE);
                ms.Write(buff, 0, bytesRead);
            } while (bytesRead > 0);
            fs.Close();
            return ms;
        }

		Stream IItemStorage.Load(string itemGuid)
		{

            return (this as IItemStorage).Load(new Guid(itemGuid));
		}

		bool IItemStorage.Exists(string itemGuid)
		{
      
            return (this as IItemStorage).Exists(new Guid(itemGuid));
		}


    bool IItemStorage.Delete(Guid itemID)
    {

      if ((this as IItemStorage).Exists(itemID))
      {
        try
        {
            File.Delete(BaseDirectory + GetFSGuid(itemID).ToString());
          return true;
        }
        catch
        {
          return false;
        }
      }
      return false;
    }

    bool IItemStorage.Delete(string itemGuid)
    {
      return (this as IItemStorage).Delete(new Guid(itemGuid));
    }

    bool IItemStorage.DeleteGuid(Guid itemId)
    {
        return false;
    }

    #endregion
  }
}
