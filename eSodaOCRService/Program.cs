﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace OCRService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
//#if (!DEBUG)

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new ESodaOCR() 
			};
            ServiceBase.Run(ServicesToRun);
//#else

//            ESodaOCR svc = new ESodaOCR();
//            svc.StartForDebug();
//            while (true)
//                ;
//#endif
        }
    }
}
