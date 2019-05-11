using System;
using System.IO;
using System.Text;

namespace AheuiDotnet
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length < 1)
            {
                Console.WriteLine("usage: aheui [filename]");
                return;
            }
            string filename = args[0];
            byte[] buffer = new byte[1000000];
            FileStream codeStream = null;
            string codeContent = null;
            bool failedFlag = false;
            try
            {
                codeStream = File.OpenRead(filename);
                codeStream.Read(buffer);
                codeContent = Encoding.UTF8.GetString(buffer).Trim().TrimEnd('\0');
                Console.Error.WriteLine(codeContent);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
                Console.WriteLine("Failed to read file");
                failedFlag = true;
            }
            finally
            {
                codeStream?.Close();
            }
            if(failedFlag)
            {
                return;
            }

            var code = CodeArray.Parse(codeContent);
            var processor = new Processor();
            processor.LoadCode(code);
            while (processor.Step()) { };
        }
    }
}
