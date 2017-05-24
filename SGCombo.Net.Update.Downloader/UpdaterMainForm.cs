////////////////////////////////////////////////////////////////////////////
//	Copyright 2008 : Vladimir Novick    https://www.linkedin.com/in/vladimirnovick/  
//
//    NO WARRANTIES ARE EXTENDED. USE AT YOUR OWN RISK. 
//
// To contact the author with suggestions or comments, use  :vlad.novick@gmail.com
//
////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Security.AccessControl;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.Net ;
using Microsoft.Win32;
using System.Security;
using System.Security.Permissions;

using System.Net.Cache;
using System.Net.Security;
using System.Runtime.Serialization;
using System.Security.Principal;
using System.Security.Cryptography;
using System.EnterpriseServices.Internal;

namespace SGCombo.Net.Update.Downloader
{
    [SecurityPermission(SecurityAction.InheritanceDemand)]
    public partial class UpdaterMainForm : Form
    {
        private const string STR_StartUpdate = "Start Update";


        private String ErrorMassage = "";
        private string strAdditionalInfo = "";
        private String strTempDirectory;
        private String strWorkinDirectory = "";
       // private String strRepositoryDirectory = "";

   //     Publish publish ; 


        private ArrayList arrFileList = new ArrayList();
        private String strRepositary = "http://www.sgcombo.com/client";
        private String strRepositaryPath = "/ClientApp/UserSystem/download5.aspx?file=/local/";
        private String urlRepositoryLocation ;
        private String strExternalLogFile = null;

        private bool isClientInstancesExist()
        {

            try
            {
                Process current = Process.GetCurrentProcess();



                Process[] procs = Process.GetProcesses();

                foreach (Process pr in procs)
                {

                        if ((pr.ProcessName.Contains("SGCombo.Net.Client")))
                        {
                            return true;
                        }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning");
            }
            return false;
        }


        private bool isPackegeDownloader = false;


        public UpdaterMainForm()
        {
            InitializeComponent();
            urlRepositoryLocation = strRepositary + strRepositaryPath;
            strWorkinDirectory = "";
            String runner2 = CommandLineHelper.CommandLineArguments.Instance["Inline"];

            String Out = CommandLineHelper.CommandLineArguments.Instance["Out"];
            String ext = CommandLineHelper.CommandLineArguments.Instance["Log"];
            if (!String.IsNullOrEmpty(ext))
            {
                strExternalLogFile = ext;
             }
            if (Out != null)
            {
                strWorkinDirectory = Out.Replace("$",":") ;
                isPackegeDownloader = true;
            }
            strTempDirectory = Path.GetDirectoryName(Application.ExecutablePath) + "\\Download\\0";
        }


        static bool IsWinVistaOrHigher()
        {
            OperatingSystem OS = Environment.OSVersion;
            return (OS.Platform == PlatformID.Win32NT) && (OS.Version.Major >= 6);
        }



        private static RegistryKey getSGComboDirectory()
        {
            RegistryKey registryKey2 = null;
            try
            {
  
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\SGCombo\\SystemUpdate", true);
                if (registryKey == null)
                {
                    registryKey = Registry.CurrentUser.CreateSubKey("Software\\SGCombo\\SystemUpdate", RegistryKeyPermissionCheck.ReadSubTree);
                }
                registryKey2 = registryKey.OpenSubKey(registryKey.GetSubKeyNames()[0]);
            }
            catch (Exception ex)
            {
                MessageBox.Show("SGCombo: Critical Error. :: Registry.CurrentUser.OpenSubKey ::  " + ex.Message);
                Application.Exit();  
            }
         

            return registryKey2;
        }


        public static String GetInstalledDirectory()
        {
            try {
            RegistryKey regestryKey = getSGComboDirectory();
            if (regestryKey != null)
            {
                Object strLocalDIR = "";
                strLocalDIR = (String)regestryKey.GetValue("AppPath", "", RegistryValueOptions.None);
                return (String)strLocalDIR;
            }
            } catch ( Exception ) {
            
            
            
            
            }
            return "";
        }

        private Boolean FindSoftwareDirectory()
        {
            Boolean ret = true;
            if (!isPackegeDownloader)
            {
                addToLog("Check SGCombo.com modules installed  ... ");
                addToStatusLine("Check SGCombo.com modules installed  ... ");


                strWorkinDirectory = Path.GetDirectoryName(Application.ExecutablePath);


                FileInfo fileInfo = new FileInfo(strWorkinDirectory + "\\SGCombo.NetClientV5.exe");
                if (fileInfo.Exists)
                {
                    addToLog("***** Update directory : " + strWorkinDirectory);
                }
                else
                {
                    addToLog("Find external SGCombo.com  software ....");
                    String LocalPatch = GetInstalledDirectory();
                    if (LocalPatch.Length > 0)
                    {
                        LocalPatch = LocalPatch.Substring(0, LocalPatch.Length - 4);
                        strWorkinDirectory = LocalPatch;
                        addToLog("***** Update directory: " + LocalPatch);

                    }
                    else
                    {
                        ret = false;
                    }
                }

                if (ret)
                {
                    addToLog("OK");
                    addToStatusLine("OK");

                    try
                    {
                        addToLog("Find Install services ... ");
                        addToStatusLine("Find Install services ... ");
                   //     publish = new Publish();
                        addToLog("OK");
                        addToStatusLine("OK");
                    }
                    catch (Exception ex)
                    {
                        ret = false;
                        addToLog("Filed. We don't find Install services. " + Environment.NewLine + ex.Message);
                        addToStatusLine("Filed. We don't find Install services");
                    }


                }
                else
                {
                    addToLog("Filed. We don't find SGCombo.com software on the local disk");
                    addToStatusLine("Filed. We don't find SGCombo.com software on the local disk");
                }
            }
            else
            {
                addToLog("***** Update directory: " + strWorkinDirectory);
            }
            return ret;
        }

        private Boolean checkUpdateIsActive()
        {
            String str_UrlHostUpdate = urlRepositoryLocation;
            Boolean ret = false;
            addToLog("Check in Software Repository ... ");
            addToStatusLine("Check in Software Repository ... ");


                try
                {

                    String url = str_UrlHostUpdate + "../enabled.txt&start=0&len=1000";
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

                    IWebProxy proxy = WebRequest.GetSystemWebProxy();
                    proxy.Credentials = CredentialCache.DefaultCredentials;
                    req.Proxy = proxy;
                    req.Method = "GET";
                    req.UserAgent = "Mozilla/4.0+(compatible;+MSIE+6.0;+Windows+NT+5.1;+.NET+CLR+1.1.4322)";
                    req.Timeout = 10000;
                    req.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);

                    HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                    int dataLength = (int)response.ContentLength;
                    if (dataLength > 0)
                    {
                        ret = true;
                    }
                    else
                    {
                        addToLog(String.Format(SGCombo.Net.Update.Downloader.Properties.Resources.FormatValue, strRepositary));

                    }
                    response.Close();


                }
                catch (Exception ex) { ret = false;
                addToLog("SGCombo.com Software Repository is Failed :" + ex.Message);

                }
                if ((ret))
                {
                    addToLog("OK");
                    addToStatusLine("OK");

                }
                else
                {
                    addToStatusLine(String.Format(SGCombo.Net.Update.Downloader.Properties.Resources.FormatValue, strRepositary));

                }
                return ret;

        }



        private string UpdateXML;

        private void writeUpdateXML(){
            string u = strWorkinDirectory + "\\Update.xml";
            TextWriter tw = new StreamWriter(u);
            tw.Write(UpdateXML);
            tw.Close();
        }

        protected Boolean getRepositoreFileList(String filName)
        {
            FileInfo NewFileInfo = new FileInfo(filName);

            FileStream s = NewFileInfo.OpenRead();
            StreamReader reader = new StreamReader(s);
            UpdateXML = reader.ReadToEnd();
            s.Close();

            using (XmlTextReader readerUpdateRepository = new XmlTextReader(new System.IO.StringReader(UpdateXML)))
            {


                String dateUpdate = "";


                XmlDocument xdUpdateRepository = new XmlDocument();

                xdUpdateRepository.Load(readerUpdateRepository);



                XmlNodeList memberNodes = xdUpdateRepository.SelectNodes("//SGCombo.Net.Updates//Information");


                foreach (XmlNode node in memberNodes)
                {
                    dateUpdate = node.Attributes["Created"].Value;
                }

                addToLog(">> Software repository update: " + dateUpdate);


                memberNodes = xdUpdateRepository.SelectNodes("//SGCombo.Net.Updates//FilesInfo//File");

                foreach (XmlNode node in memberNodes)
                {

                    UpdateFileInfo info = new UpdateFileInfo(node);
                    arrFileList.Add(info);

                }
            }

            addToLog("OK ");
            return true;
        }


        protected Boolean getRepositoreFileList()
        {
            Stream webstream = null;
            arrFileList.Clear();

            String url = urlRepositoryLocation + "update.xml";
            addToStatusLine("Connecting ... " + strRepositary);
            
            addToLog("Searching SGCombo.com Software Repository ... ");

            Boolean ret = true;

            try
            {



                  FileDownloader     downloader = new FileDownloader();

                        DirectoryInfo d = new DirectoryInfo(strTempDirectory);
                        if (!d.Exists) d.Create();
                        String filName = strTempDirectory +  "\\Update.xml";
                       
                        downloader.downloadData(url, filName, callback,DateTime.Now.ToFileTimeUtc());
                        getRepositoreFileList(filName);
            }

            catch (Exception ex)
            {
                addToLog("SGCombo.com Repository is Failed :" + ex.Message);
                addToStatusLine(String.Format("Software repository {0} is temporarily unavailable, please try later. We are updating the repository. ", strRepositary));
                ret = false;

            } finally {

                if (webstream != null)
                {
                    webstream.Close();
                    webstream.Dispose();
                }
            
            }



            return ret;
        }


        protected BackgroundWorker backgroundWorker;

        protected bool bOperationOK;


        private void StartWorkingProcess()
        {


            backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.DoWork +=  new DoWorkEventHandler(backgroundWorkerLoadRepositoryFileList_DoWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorkerLoadRepositoryFileList_RunWorkerCompleted);

            backgroundWorker.RunWorkerAsync();

        }

        private int countNewFiles;
        private int countUpdateFiles;
        private int countExistFiles;
 //       private int countOldVershion;


        private bool verifyLocalFiles()
        {
            bool ret = true;
            addToLog("Starting verify pass .... ");

            countNewFiles = 0;
            countUpdateFiles = 0;
            countExistFiles = 0;
        //    countOldVershion = 0;
            try
            {

                foreach (UpdateFileInfo info in arrFileList)
                {

                    String strFile = strWorkinDirectory + info.UpdateFile;

                    FileInfo fileInfo = new FileInfo(strFile);

                    addToStatusLine("Verify Local Files>>" + info.UpdateFile);
                    if (fileInfo.Exists)
                    {
                        if (fileInfo.Length != info.Length)
                        {
                            info.doProcess = true;
                            countUpdateFiles++;
                        }
                        else
                        {
                            if (fileInfo.LastWriteTimeUtc.ToBinary() != info.LastWriteTimeUtc)
                            {
                                info.doProcess = true;
                                countUpdateFiles++;
                            }
                            else
                            {
                                countExistFiles++;
                            }
                        }
                    }
                    else
                    {
                        info.doProcess = true;
                        countNewFiles++;
                        info.NewFile = true;
                    }

                }
                ret = true;
                addToLog(String.Format("OK \nFiles: \n  {0}- new;\n  {1} - update;\n  {2} - Exists\n ", countNewFiles, countUpdateFiles, countExistFiles));
            }
            catch (Exception ex)
            {
                ErrorMassage = ex.Message;
                ret = false;
                addToLog("Failed: .... " + ex.Message);
                ret = false;
            }
            if (ret)
            {
                addToStatusLine("OK");
            }
            else
            {
                addToStatusLine("Failed");
            }
            return ret;
        }

        private static String FindActualDirectoryName(String FileName)
        {
            String f = null;
            try
            {
                f = FileName.Replace("/", "\\");
                int ip = f.LastIndexOf("\\");

                while (ip > 0)
                {
                    f = f.Substring(0, ip);
                    DirectoryInfo dirInfo = new DirectoryInfo(f);
                    if (dirInfo.Exists)
                    {
                        return f;
                    }
                    ip = f.LastIndexOf("\\");
                }
            }
            catch (Exception) { }
            return f;
        }

        private bool CheckFileAccess()
        {
            bool ret = true;
            addToLog("Check File Access .... ");

            try
            {

                foreach (UpdateFileInfo info in arrFileList)
                {



                    if (info.doProcess)
                    {

                        String strFile = strWorkinDirectory + info.UpdateFile;


                        if (info.NewFile)
                        {
                            strFile = strFile.Replace("\\", "/");
                            addToStatusLine("CheckFileAccess >> " + strFile);

                            String strDirectory = FindActualDirectoryName(strFile);
                            DirectoryInfo DirectoryInfo = new DirectoryInfo(strDirectory);
                            if (DirectoryInfo.Exists)
                            {
                                UserFileAccessRights rights = new UserFileAccessRights(strDirectory);
                                Boolean facc = ((rights.canCreateDirectories() && rights.canCreateFiles() && rights.canDelete() && rights.canWrite() && rights.canWriteAttributes() && rights.canCreateFiles()) || rights.canFullControl());
                                if (!facc)
                                {
                                    addToLog(String.Format(" *** Error : {0} \" - The program should have Control permissions to this Directory ", strDirectory));
                                    ret = false;
                                }
                            }
                            else
                            {
                                addToLog(String.Format(" *** Error : Home directory does not exist ", strFile));
                                ret = false;
                                break;
                            }
                        }
                        else
                        {

                            FileAttributes f = File.GetAttributes(strFile);
                            if ((f & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                            {
                                addToLog(String.Format(" *** Error : File {0} is ReadOnly ", strFile));
                                ret = false;
                            }
                            else
                            {
                                try
                                {
                                    FileSecurity fSecurity = File.GetAccessControl(strFile);
                                    UserFileAccessRights rights = new UserFileAccessRights(strFile);
                                    if (!((rights.canDelete() && rights.canRead() && rights.canWrite()) || rights.canFullControl()))
                                    {
                                        addToLog(String.Format(" *** Error : {0}  - The program should have Delete/Read/Write permissions to the Files ", strFile));
                                        ret = false;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    addToLog(String.Format(" *** Error : {0}  - Access is Denided\" :  {1} ", strFile, ex.Message));
                                    ret = false;
                                }

                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorMassage = ex.Message;
                ret = false;
                addToLog(" *** Error: " + ex.Message);
                ret = false;
            }
            if (ret)
            {
                addToLog("OK ");
                addToStatusLine("CheckFileAccess OK");
            }
            else
            {
                addToLog("Failed.");
                addToStatusLine("CheckFileAccess Is Failed");
            }
            return ret;
        }



        private void callback( DowloadInfoLineHandlerEventArgs e)
        {
            try
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    String fName = e.Link.Substring(urlRepositoryLocation.Length);
                    int t = fName.IndexOf('&');
                    if (t > 0)
                    {
                        fName = fName.Substring(0, t - 1); 
                    }
                    addToStatusLine(String.Format("Download : {0}/{1} - {2}", e.Count, e.FileLength, fName));
                });
            }
            catch (Exception) { }

 

        }


        string MD5hash(string filename)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] hash;
                using (FileStream s = File.OpenRead(filename))
                {
                    hash = md5.ComputeHash(s);
                }
                string r = BitConverter.ToString(hash);
                return r;
            }
        }



        private bool DownloadFiles()
        {
            bool ret = true;
            addToLog("Downloading .... ");



            try
            {

                foreach (UpdateFileInfo info in arrFileList)
                {
                    if (UpdateTerminate) return false;

                    if (!info.doProcess) continue;
                    FileDownloader downloader = null;
                    try
                    {
                      
                        String url = urlRepositoryLocation + info.UpdateFile.Replace("\\", "/").Substring(1);
                        addToStatusLine("Download >> " + info.UpdateFile);
                        DirectoryInfo d = new DirectoryInfo(strTempDirectory);
                        if (!d.Exists) d.Create();
                        String filName = strTempDirectory + info.UpdateFile + ".part";
                        int iCount = 0;
                        while (true)
                        {
                            downloader = new FileDownloader();
                            downloader.downloadData(url, filName, callback,info.CreationTimeUtc);
                            FileInfo NewFileInfo = new FileInfo(filName);

                            string md5 = "{" + MD5hash(NewFileInfo.FullName)+ "}";

                            if ((info.Length != NewFileInfo.Length) || (md5 != info.Keypad))
                            {
                                if (iCount > 5)
                                {
                                    addToLog(String.Format(" *** Failed download data: {0} info: {1}-bytes, Data: {2}-bytes", url.Substring(urlRepositoryLocation.Length), info.Length, NewFileInfo.Length));
                                    return false;
                                }
                                iCount++;
                                if ((info.Length != NewFileInfo.Length))
                                {
                                    addToLog(String.Format("  >> Restore  ({3}). Reason: Invalid size", url.Substring(urlRepositoryLocation.Length), info.Length, NewFileInfo.Length, iCount));
                                    addToLog(String.Format("      {0} ", url.Substring(urlRepositoryLocation.Length), info.Length, NewFileInfo.Length, iCount));
                                    addToLog(String.Format("      info: {1}-bytes, Data: {2}-bytes", url.Substring(urlRepositoryLocation.Length), info.Length, NewFileInfo.Length, iCount));
                                }
                                else
                                {
                                    addToLog(String.Format("   >> Restore  ({3}). Reason: Invalid Security Code", url.Substring(urlRepositoryLocation.Length), info.Length, NewFileInfo.Length, iCount,md5));
                                    addToLog(String.Format("      {0} ", url.Substring(urlRepositoryLocation.Length), info.Length, NewFileInfo.Length, iCount));
                                    addToLog(String.Format("      info: {1}-bytes", url.Substring(urlRepositoryLocation.Length), info.Length, NewFileInfo.Length, iCount));
                                    addToLog(String.Format("      Base MD5 {0}", String.Copy(info.Keypad)));
                                    addToLog(String.Format("      New MD5 {0}", String.Copy(md5)));

                                }
                           
                            }
                            else
                            {
                                addToLog(String.Format(">> {0}  - OK:  {1}-bytes,  MD5 - OK", url.Substring(urlRepositoryLocation.Length), info.Length,String.Copy(md5)));
                                break;
                            }
                        }


                    }
                    catch (Exception ex)
                    {
                        addToLog(" *** Download Error: " + ex.Message);
                        ret = false;
                        break;
                    }
                    finally
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMassage = ex.Message;
                addToLog(" *** System Error: " + ex.Message);
                addToStatusLine("*** System Error");
                ret = false;
            }
            if (ret)
            {
                addToLog("OK ");
                addToStatusLine("Download OK");
            }
            else
            {
                addToLog("Failed.");
                addToStatusLine("Download Is Failed");
            }
            return ret;
        }


        void IntallGac()
        {
            addToLog("Reinstall SGCombo.com Application");
            addToStatusLine("Reinstall SGCombo.com Application.");
         
            try
            {

                foreach (UpdateFileInfo info in arrFileList)
                {
                    string fileName = info.Name.ToLower();
                    if (fileName.EndsWith(".dll") || fileName.EndsWith(".exe"))
                    {
                        String strFile = strWorkinDirectory + info.UpdateFile;
                  //      publish.GacRemove(strFile);
 //                       publish.GacInstall(strFile);
                        addToLog("Install >> ..." + info.UpdateFile);
                    }
                }
                addToLog("OK");
                addToStatusLine("OK");
            }
            catch (Exception ex)
            {
                addToLog("Failed. Error reinstall ..." + ex.Message);
                addToStatusLine("Failed. Error reinstall ...");
            }

        }


        void UnistallGac()
        {


            try
            {

                foreach (UpdateFileInfo info in arrFileList)
                {
                    string fileName = info.Name.ToLower();
                    if (fileName.EndsWith(".dll") || fileName.EndsWith(".exe"))
                    {
                        String strFile = strWorkinDirectory + info.UpdateFile;
                      //  publish.GacRemove(strFile);
                        addToLog("Uninstall >> ..." + info.UpdateFile);
                    }
                }
                addToLog("OK");
                addToStatusLine("OK");
            }
            catch (Exception ex)
            {
                addToLog("Failed. Error reinstall ..." + ex.Message);
                addToStatusLine("Failed. Error reinstall ...");
            }

        }


        void backgroundWorkerLoadRepositoryFileList_DoWork(object sender, DoWorkEventArgs e)
        {


            try
            {
                addToLog("Check Operations Configuration ...");
                addToStatusLine("Check Operations Configuration ...");
                Thread.Sleep(3000);  
                addToLog("OK");
                addToStatusLine("OK");
                addToLog("Check Application instances ...");
                addToStatusLine("Check Application instances ...");
                bool enatherSGCombo = isClientInstancesExist();

                if (enatherSGCombo)
                {
                    addToLog("Failed. Please close the other SGCombo.com applications and try again.");
                    addToStatusLine("Failed. Please close the other SGCombo.com applications and try again.");

                    e.Result = false;
                    return;
                }



                addToLog("OK");
                addToStatusLine("OK");

                if (UpdateTerminate) return;

                if (!installBtn && !Uninstall)
                {

                    addToLog("Check your network connectivity ...");
                    addToStatusLine("Check your network connectivity ...");

                    Boolean internet = IsConnectedToInternet();

                    if (!internet)
                    {
                        addToLog("Update system has not been able to connect to the update server. Please check your network connectivity and try again.");
                        addToStatusLine("Update system has not been able to connect to the update server.");

                        e.Result = false;
                        return;
                    }


                    addToLog("OK");
                    addToStatusLine("OK");

                    if (UpdateTerminate) return;
                }


                if (!FindSoftwareDirectory()){
                    return;
                }

                if (UpdateTerminate) return;

                if ((!Uninstall) && (!this.installBtn))
                {

                    if (!checkUpdateIsActive())
                    {

                        e.Result = false;
                        return;
                    }
                    else
                    {
                        if (!getRepositoreFileList())
                        {
                            e.Result = false;
                            return;
                        }
                    }

                }
                else
                {
                    string u = strWorkinDirectory + "\\Update.xml";
                    addToLog("Read local repository : " + u);
                    if (!getRepositoreFileList(u))
                    {
                        e.Result = false;
                        return;
                    }
                }

                if (UpdateTerminate) return;

         

                if (UpdateTerminate) return;



                if (!verifyLocalFiles())
                {
                    e.Result = false;
                    return;
                }
                if (UpdateTerminate) return;

                  addToLog("Unistall SGCombo.com Application ...");
                  addToStatusLine("Unistall SGCombo.com Application ...");
                  UnistallGac();

                if (Uninstall)
                {
 
                    return;
                }

                if (this.installBtn)
                {
                    IntallGac();
                    return;
                }

                if ((countNewFiles == 0) && (countUpdateFiles == 0))
                {
                    addToLog("Update of the software not necessary. The installed software is current");
                    addToStatusLine("Update of the software not necessary. The installed software is current");
                    IntallGac();
                    return;
                }

                if (UpdateTerminate) return;

                if (!CheckFileAccess())
                {
                    e.Result = false;
                    return;
                }

                if (UpdateTerminate) return;

                if (!DownloadFiles())
                {
                    e.Result = false;
                    return;
                }



                this.Invoke((MethodInvoker)delegate()
                {
                    btnOK.Enabled = false;
                    UpdateTerminate = false;
                });




                if (!renameFiles())
                {
                    e.Result = false;
                    return;
                }


            }
            catch (Exception )
            {
                e.Result = false;
            }
            IntallGac();
        }





        void backgroundWorkerLoadRepositoryFileList_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                BoxLoad.Visible = false;
                if (UpdateTerminate)
                {
                    addToLog("Operation terminated by user  !!! ");
                    addToStatusLine("Operation terminated by user  !!!");
                }


            }
            finally
            {
                BackgroundWorker backgroundWorker = sender as BackgroundWorker;
//                 backgroundWorker.DoWork -= backgroundWorkerLoadRepositoryFileList_DoWork;
//                 backgroundWorker.RunWorkerCompleted -= backgroundWorkerLoadRepositoryFileList_RunWorkerCompleted;

                backgroundWorker.Dispose();
                btnOK.Text = STR_StartUpdate;
                btnOK.Enabled = true;


                if (runner != null)
                {
                    btnCancel.PerformClick();
                }

                if (e.Result != null)
                {
                    if ((bool)(e.Result))
                    {
                        String runner2 = CommandLineHelper.CommandLineArguments.Instance["Inline"];
                        if (!string.IsNullOrEmpty(runner2))
                        {
                            btnCancel.PerformClick();
                        }
                    }


                }

            }

        }



        private bool UpdateTerminate = false;

        private bool Uninstall = false;


        private void btnOK_Click(object sender, EventArgs e)
        {
            Uninstall = false;
            installBtn = false;

            if (btnOK.Text == STR_StartUpdate)
            {
                btnOK.Text = "Stop";
                UpdateTerminate = false;
                BoxLoad.Visible = true;


                StatusMessages.Text = "";
                StartWorkingProcess();
            }
            else
            {
                UpdateTerminate = true;

            }
        }



        private static Object MyCriticalSection = new Object();

        public void LogToExternalFile(String message)
        {




            if (strExternalLogFile != null)
            {
                lock (MyCriticalSection)
                {

                    StreamWriter sw = null;

                    try
                    {
                        if (!File.Exists(strExternalLogFile))
                        {
                            FileInfo info = new FileInfo(strExternalLogFile);
                            info.Create().Close();
                        }

                        sw = File.AppendText(strExternalLogFile);

                        string logLine =
                           System.String.Format(
                              "{0:G}: {1}.", System.DateTime.Now, message);
                        sw.WriteLine(logLine);
                        sw.Flush();
                    }
                    catch (Exception)
                    {
                        // do noting 
                    }
                    finally
                    {
                        if (sw != null)
                        {
                            try
                            {
                                sw.Close();
                                sw.Dispose();
                            }
                            catch (Exception)
                            {
                                // do noting
                            }
                        }
                    }

                }

            }


        }

        private void addToLog(String strMessege)
        {
            this.Invoke((MethodInvoker)delegate()
            {
                String ws = strMessege.Replace("\n", Environment.NewLine) + Environment.NewLine;
                StatusMessages.AppendText(ws);

                LogToExternalFile(ws);


            });
        }


        private void addToStatusLine(String strMessege)
        {
            this.Invoke((MethodInvoker)delegate()
            {
                processLineControl.Text = strMessege;
            });
        }



        private void setWarning()
        {
            this.Invoke((MethodInvoker)delegate()
            {
                LabelFormTile.ForeColor = Color.Red;
            });
        }


        private void addToTitle(String strMessege)
        {
            this.Invoke((MethodInvoker)delegate()
            {
                LabelFormTile.Text = strMessege;
            });
        }



        protected bool renameFiles()
        {


            addToLog("Committing ....");
            addToStatusLine("Committing ....");
            bool ret = true;


            try
            {

                foreach (UpdateFileInfo info in arrFileList)
                {
                    if (info.doProcess)
                    {

                        addToStatusLine("Committing ...>>" + info.UpdateFile);


                        String strOutputFile = strWorkinDirectory + info.UpdateFile;
                        String strBackUpFile = String.Format("{0}{1}.SGCombo.com.bak", strWorkinDirectory, info.UpdateFile);
                        FileDownloader.checkDir(strOutputFile);
                        FileDownloader.checkDir(strBackUpFile);

                        String strOutputTempFile = String.Format("{0}{1}" + ".part", strTempDirectory, info.UpdateFile);
                        if (File.Exists(strOutputFile))
                        {
                            if (File.Exists(strBackUpFile))
                            {
                                try
                                {
                                    File.Delete(strBackUpFile);
                                }
                                catch (Exception)
                                {
                                    int ip = 0;
                                    while (File.Exists(strBackUpFile+ip.ToString() )){
                                        ip++;
                                    }

                                    File.Move(strBackUpFile, strBackUpFile + ip.ToString());
                                }
                            }


                        //    publish.GacRemove(strOutputFile);
                            File.Move(strOutputFile, strBackUpFile);
                            info.backUp = true;


                        }

                        File.Move(strOutputTempFile, strOutputFile);


                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMassage = ex.Message;

                addToLog("Failed " + ErrorMassage);
                addToStatusLine("Failed " + ErrorMassage);
                ret = false;
            }

            if (ret)
            {


                setFileInCompleted();

                clearBackupFiles();
            }
            else
            {
                restoreFiles();
            }


            return ret;

        }



        protected void setFileInCompleted()
        {
            foreach (UpdateFileInfo info in arrFileList)
            {

                if (info.doProcess)
                {
                    String strFile = strWorkinDirectory + info.UpdateFile;


                    try
                    {
                        FileInfo fnfo = new FileInfo(strFile) { LastWriteTimeUtc = DateTime.FromBinary(info.LastWriteTimeUtc) };



                        addToLog(">>" + info.UpdateFile);
                    }
                    catch (Exception )
                    {

                    }
                }


            }
        }



        protected Boolean restoreFiles()
        {
            Boolean ret = true;
            try
            {

                foreach (UpdateFileInfo info in arrFileList)
                {
                    if (info.isCopy)
                    {
                        String strBackUpFile = String.Format("{0}{1}.SGCombo.com.bak", strWorkinDirectory, info.UpdateFile);
                        String strOutFile = strWorkinDirectory + info.UpdateFile;
                        File.Copy(strBackUpFile, strOutFile, true);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMassage = "Error Restore *.SGCombo.com.Bak files";
                strAdditionalInfo = ex.Message;
                ret = false;
            }

            if (ret)
            {
                addToLog("Restore OK \n Update - Failed ");
                addToStatusLine("Failed " + ErrorMassage);
            }
            else
            {
                addToLog("Restore Failed \n Update - Failed  ");
                addToStatusLine("Restore Failed \n Update - Failed  ");
            }
            return ret;
        }

        protected Boolean  clearBackupFiles()
        {

            Boolean ret = true;
            addToLog("Clear Backup files ");
            addToStatusLine("Clear Backup files");
            foreach (UpdateFileInfo info in arrFileList)
            {


                try
                {
                    String strBackUpFile = String.Format("{0}{1}.SGCombo.com.bak", strWorkinDirectory, info.UpdateFile);
                    FileInfo fileInfo = new FileInfo(strBackUpFile);
                    if (fileInfo.Exists)
                    {
                        fileInfo.Delete();
                    }

                }
                catch (Exception )
                {
                }


            }
            if (ret)
            {
                writeUpdateXML();
                addToLog("OK");
                addToStatusLine("OK");
            }
            else
            {
                addToLog("Failed");
                addToStatusLine("Failed");
            }
            return ret;

        }













        [DllImport("kernel32.dll")]

        static extern bool ProcessIdToSessionId(uint dwProcessId, out uint pSessionId);



        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
        //Creating a function that uses the API function...
        public static bool IsConnectedToInternet()
        {

            int Desc;

            try
            {
                if (InternetGetConnectedState(out Desc, 0))
                {
                    return true;
                }
            }
            catch (Exception) { }

            Process _currentProcess = Process.GetCurrentProcess();

            uint _processID = (uint)_currentProcess.Id;


            uint _sessionID;


            bool _result = ProcessIdToSessionId(_processID, out _sessionID);


            if (_sessionID != 0) return true;

            return false;


        }


        private String runner = null;

        private void UpdaterMainForm_Load(object sender, EventArgs e)
        {

            FindSoftwareDirectory();
            btnOK.Text = STR_StartUpdate;
            if (!IsConnectedToInternet())
            {
                addToTitle("Downloader IS FAILED.");
                addToLog("SGCombo.com Updater is unable to update any components. Please check your internet connection, and then try again.");
                setWarning();
                btnOK.Enabled = false;
                return;
            }

            runner = CommandLineHelper.CommandLineArguments.Instance["Run"];


            if (runner != null)
            {
                btnOK.PerformClick(); 
            }


              String runner2 = CommandLineHelper.CommandLineArguments.Instance["Inline"];
             if (!string.IsNullOrEmpty(runner2))
             {
                  btnOK.PerformClick();
             }



        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bntStartSGCombo_Click(object sender, EventArgs e)
        {


            Process process = new Process();
            process.EnableRaisingEvents = false;
            if (strWorkinDirectory.Length == 0)
            {
                strWorkinDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            }
                process.StartInfo.WorkingDirectory = "\"" + strWorkinDirectory + "\"";
                process.StartInfo.FileName = "\"" + strWorkinDirectory + "\\SGCombo.NetClientV.exe" + "\"";
           
            if ((File.Exists(process.StartInfo.FileName.Substring(1, process.StartInfo.FileName.Length - 2))))
            {
                this.Hide();
                String cmdLine = CommandLineHelper.CommandLineArguments.Instance["config"];

                if (!(String.IsNullOrEmpty(cmdLine)))
                {
                    process.StartInfo.Arguments = String.Format(" -config:\"{0}\"", cmdLine);
                }


                process.StartInfo.UseShellExecute = true;
                process.Start();
                Thread.Sleep(1000);
                process.Dispose();
                this.Close();
            }
            else
            {

            }

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://www.sgcombo.com");
            }
            catch (Exception) { }
        }

  //      bool start = true;

        private void UpdaterMainForm_Activated(object sender, EventArgs e)
        {

        }


        private void buttonUnistall_Click(object sender, EventArgs e)
        {
            Uninstall = true;
            installBtn = false;

            if (btnOK.Text == STR_StartUpdate)
            {
                btnOK.Text = "Stop";
                UpdateTerminate = false;
                BoxLoad.Visible = true;


                StatusMessages.Text = "";
                StartWorkingProcess();
            }
            else
            {
                UpdateTerminate = true;

            }


        }



        private bool installBtn = false;

        private void button1_Click(object sender, EventArgs e)
        {
            Uninstall = false;
            installBtn = true;
            btnOK.Text = "Stop";
            UpdateTerminate = false;
            BoxLoad.Visible = true;


            StatusMessages.Text = "";
            StartWorkingProcess();
        }

    }
}
