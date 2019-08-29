using System;
using System.Text;


namespace CSNamedPipe
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            NamedPipeServer PServer1 = new NamedPipeServer(@"\\.\pipe\myNamedPipe1",0);
            NamedPipeServer PServer2 = new NamedPipeServer(@"\\.\pipe\myNamedPipe2",1);
            */

            /*            
            NamedPipeServer PServer1 = new NamedPipeServer(@"\\.\pipe\1", 0);
            NamedPipeServer PServer2 = new NamedPipeServer(@"\\.\pipe\2", 1);
             */

            
            string namedPipe3DToHints = args[0] + "_3DToHints";
            string namedPipeHintsTo3D = args[0] + "_HintsTo3D";
            
            //For debugging
            /*
            string namedPipe3DToHints = "tea1" + "_3DToHints";
            string namedPipeHintsTo3D = "tea1" + "_HintsTo3D";
            */
            NamedPipeServer PServer3DToHints = new NamedPipeServer(@"\\.\pipe\" + namedPipe3DToHints, 0);
            NamedPipeServer PServerHintsTo3D = new NamedPipeServer(@"\\.\pipe\" + namedPipeHintsTo3D, 1);
            PServer3DToHints.Start();
            PServerHintsTo3D.Start();

            string Ms="Start";
            do
            {
                //Console.WriteLine("Send message to 3DBuilder:");
                Ms = Console.ReadLine();
                PServerHintsTo3D.SendMessage(Ms, PServerHintsTo3D.clientse);
            } while (Ms != "quit");

            PServer3DToHints.StopServer();
            PServerHintsTo3D.StopServer();
        }
        
    }
}
