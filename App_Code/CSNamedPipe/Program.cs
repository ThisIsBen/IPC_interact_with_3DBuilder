using System;
using System.Text;


namespace CSNamedPipe
{
    class Program
    {
        static void Main(string[] args)
        {
            NamedPipeServer PServer1 = new NamedPipeServer(@"\\.\pipe\myNamedPipe1",0);
            NamedPipeServer PServer2 = new NamedPipeServer(@"\\.\pipe\myNamedPipe2",1);

            PServer1.Start();
            PServer2.Start();

            string Ms="Start";
            do
            {
                Console.WriteLine("Send message to 3DBuilder:");
                Ms = Console.ReadLine();
                PServer2.SendMessage(Ms, PServer2.clientse);
            } while (Ms != "quit");

            PServer1.StopServer();
            PServer2.StopServer();
        }
        
    }
}
