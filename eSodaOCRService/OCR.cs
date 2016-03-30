using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Data.SqlClient;
using OCRService.Properties;
using System.IO;
using OCRLibrary;
using System.Timers;

namespace OCRService
{
    partial class ESodaOCR : ServiceBase
    {
        private OCRTask ocr;

        public ESodaOCR()
        {
            InitializeComponent();
            ocr = getOCRTaskInstance();
        }

        private OCRTask getOCRTaskInstance()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Settings.Default.Database))
                {
                    using (SqlCommand cmd = new SqlCommand(Settings.Default.GetConfigProcedureName, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (conn.State != ConnectionState.Open)
                            conn.Open();
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            if (!dr.Read())
                                throw new ArgumentException("Invalid service configuration in database");

                            string sh, eh;
                            sh = dr["OCRStartHour"].ToString();
                            eh = dr["OCREndHour"].ToString();

                            return new OCRTask(sh, eh,Settings.Default.TessdataDir);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Creating OCRTask instance: " + ex.Message);
                throw;
            }
        }

        protected override void OnStart(string[] args)
        {
            System.Timers.Timer timer = new System.Timers.Timer(10000);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Enabled = true;
        }

        public void StartForDebug()
        {
            System.Timers.Timer timer = new System.Timers.Timer(10000);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Enabled = true;
            
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!ocr.IsInTimeRange)
                return;

            try
            {
                (sender as Timer).Enabled = false;

                using (SqlConnection conn = new SqlConnection(Settings.Default.Database))
                {
                    using (SqlCommand cmd = new SqlCommand(Settings.Default.GetScanProcedureName, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (conn.State != ConnectionState.Open)
                            conn.Open();
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            if (!dr.Read())
                                return;

                            string filename, originalFilename;
                            Guid guid;
                            guid = (Guid)dr["guid"];
                            filename = dr["fsguid"].ToString();
                            originalFilename = dr["oryginalnanazwa"].ToString();

                            filename = Path.Combine(Settings.Default.DocumentsDirectory, filename);

                            List<PageInfo> pages = ocr.DoOCR(filename);

                            saveOCRResults(pages, guid);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
            }
            finally
            {
                (sender as Timer).Enabled = true;
            }
        }

        private void saveOCRResults(List<PageInfo> pages,Guid guid)
        {
            using (SqlConnection conn = new SqlConnection(Settings.Default.Database))
            {
                conn.Open();
                if (pages == null)
                {
                    using (SqlCommand cmd = new SqlCommand(Settings.Default.SaveOCRFailureProcedureName, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlCommandBuilder.DeriveParameters(cmd);
                        cmd.Parameters["@ScanID"].Value = guid;
                        cmd.ExecuteNonQuery();
                        return;
                    }
                }

                using (SqlCommand cmd = new SqlCommand(Settings.Default.SaveOCRSuccessProcedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlCommandBuilder.DeriveParameters(cmd);

                    foreach (PageInfo page in pages)
                    {
                        cmd.Parameters["@ScanID"].Value = guid;
                        cmd.Parameters["@PageNumber"].Value = page.PageNumber;
                        cmd.Parameters["@textContent"].Value = page.TextContent;
                        cmd.ExecuteNonQuery();
                    }

                }
            }
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }
    }
}
