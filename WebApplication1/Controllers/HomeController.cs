using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {

        private IHostingEnvironment Environment;

        public HomeController(IHostingEnvironment _environment)
        {
            Environment = _environment;
        }

        //HTTPGET
        public IActionResult Index()
        {
            return View();
        }

        
        //HTTPPOST
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)] //OK Object
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)] //Error Object
        public IActionResult Index(List<IFormFile> postedFiles)
        {
            string path = this.Environment.WebRootPath;

            //Creates Console Path
            //Remove App location
            string appPath = "\\WebApplication1\\wwwroot";
            int intAppPath = path.IndexOf(appPath);
            string consolePath = path.Substring(0, intAppPath);
            //Add Console Path
            string consoleExe = "\\ConsoleApp\\ConsoleApp.exe";
            consolePath = consolePath + consoleExe;


            //Saves File
            string foldername = "Uploads";

            string dataPath = Path.Combine(path, foldername);
            //Creates Directory if doesn't exist
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }

                List<string> uploadedFiles = new List<string>();
                foreach (IFormFile postedFile in postedFiles)
                {
                    if (CheckIfTxtFiles(postedFile))
                    {
                     string fileName = Path.GetFileName(postedFile.FileName);
                    
                    

                    using (FileStream stream = new FileStream(Path.Combine(dataPath, fileName), FileMode.Create))
                    { 
                    //Confirming Upload
                    
                         postedFile.CopyTo(stream);
                        uploadedFiles.Add(fileName);

                        
                     }
                    //Saving to Database

                    string[] dataPaths = { dataPath };
                    ConsoleApp.Program consoleApp = new ConsoleApp.Program();
                    ConsoleApp.Program.Main(dataPaths);

                    //Process ConsoleApp = new Process();
                    //ConsoleApp.StartInfo.FileName = consolePath;
                    //ConsoleApp.Start();
                    //ConsoleApp.WaitForExit();

                    ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);

                    DeleteFile(dataPath, fileName);
                    return View();
                    }
                    else
                    {
                    return BadRequest(new { message = "Invalid file extension" });
                    }
                
                }



            return BadRequest(new { message = "Files not uploaded" });
            




        }

        private bool CheckIfTxtFiles(IFormFile file)
        {
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            return (extension == ".txt");
        }

        private void DeleteFile(string dataPath, string fileName)
        {
            fileName = Path.Combine(dataPath, fileName);
            FileInfo file = new FileInfo(fileName);
            file.Delete();
        }
    }


}
