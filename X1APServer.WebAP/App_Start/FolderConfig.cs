using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace X1APServer
{


    public class FolderConfig
    {

        public static void CreateFolder()
        {
            string manageFolder = ConfigurationManager.AppSettings["ManagedFolderPath"];
            string workingFolder = ConfigurationManager.AppSettings["WorkingFolderPath"];
            string finishDataFolder = ConfigurationManager.AppSettings["X1ImagingFinishDataPath"];
            //string imageFolder = ConfigurationManager.AppSettings["ImageFolderPath"];
            string wordFolder = ConfigurationManager.AppSettings["ConvertWordPath"];
            string pdfFolder = ConfigurationManager.AppSettings["ConvertPdfPath"];

            Directory.CreateDirectory(manageFolder);

            Directory.CreateDirectory(workingFolder);
            var acl = Directory.GetAccessControl(workingFolder);
            acl.AddAccessRule(new FileSystemAccessRule("X1ImagingUsers", FileSystemRights.Modify, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            acl.AddAccessRule(new FileSystemAccessRule("IIS_IUSRS", FileSystemRights.Modify, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.InheritOnly, AccessControlType.Allow));

            Directory.SetAccessControl(workingFolder, acl);

            var finishFolderacl = Directory.GetAccessControl(workingFolder);
            finishFolderacl.AddAccessRule(new FileSystemAccessRule("X1ImagingUsers", FileSystemRights.Modify, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.InheritOnly, AccessControlType.Allow));
            finishFolderacl.AddAccessRule(new FileSystemAccessRule("IIS_IUSRS", FileSystemRights.Modify, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.InheritOnly, AccessControlType.Allow));

            string finishFolderPath = Path.Combine(workingFolder, finishDataFolder);
            Directory.CreateDirectory(finishFolderPath, finishFolderacl);

            //Directory.CreateDirectory(imageFolder);

            Directory.CreateDirectory(wordFolder);

            Directory.CreateDirectory(pdfFolder);
        }
    }
}