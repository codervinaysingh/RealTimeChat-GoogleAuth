using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Security.AccessControl;
using System.Text;

public class Test
{
    private readonly IWebHostEnvironment hostEnvironment;

    public Test(IWebHostEnvironment hostEnvironment)
    {
        this.hostEnvironment = hostEnvironment;
    }

    public byte[] Main()
    {
        lockUnloackFolder();
        string path = Path.Combine(this.hostEnvironment.WebRootPath, "Upload", "era.jpg");
        byte[] bytes = System.IO.File.ReadAllBytes(path);
        

        // Create the file if it does not exist.
        if (!File.Exists(path))
        {
            return bytes;
        }

        FileAttributes attributes = File.GetAttributes(path);

        if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
        {
            // Show the file.
            attributes = RemoveAttribute(attributes, FileAttributes.ReadOnly);
            File.SetAttributes(path, attributes);
            Console.WriteLine("The {0} file is no longer hidden.", path);
        }
        else
        {
            // Hide the file.
            File.SetAttributes(path, File.GetAttributes(path) | FileAttributes.ReadOnly);
            Console.WriteLine("The {0} file is now hidden.", path);
        }
        
        return bytes;
    }

    private static FileAttributes RemoveAttribute(FileAttributes attributes, FileAttributes attributesToRemove)
    {
        return attributes & ~attributesToRemove;
    }

    public void lockUnloackFolder()
    {
        string path = Path.Combine(this.hostEnvironment.WebRootPath, "Upload", "era.jpg");
        string adminUserName = Environment.UserName;// getting your adminUserName
       // DirectorySecurity dirSecurity = Directory.(path);
        //FileSystemAccessRule fsa = new FileSystemAccessRule(adminUserName, FileSystemRights.FullControl, AccessControlType.Deny);
        //    dirSecurity.AddAccessRule(fsa);
        //Directory.SetAccessControl(path, dirSecurity);
    }


}