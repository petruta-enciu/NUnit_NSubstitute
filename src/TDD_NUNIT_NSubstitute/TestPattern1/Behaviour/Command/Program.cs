using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using DesignPatternTests.Behaviour.Command;

namespace DesignPatternTests
{
    class Program
    {
        static void Main(string[] args)
        {
            Action<object> toDo = new Action<object>( sInfo => Console.WriteLine("Receiver performed first action : {0} - {1} ", sInfo, DateTime.Now.ToLongTimeString()) ) ;
            Action<object> toDo1 = new Action<object>(sInfo =>
                            {
                                string ss = sInfo.ToString();
                                if (sInfo is string)
                                    ss = (string)sInfo;
                                Console.WriteLine("Receiver performed second action : {0} ", DateTime.Now.AddSeconds(ss.Length).ToLongTimeString() ) ;
                            } );

            CustomReceiverFactory recFactory = new CustomReceiverFactory();
            CustomCommandFactory cmdFactory = new CustomCommandFactory();
            CustomSenderFactory sendFactory = new CustomSenderFactory();

            ICustomReceiver rec = recFactory.Create(toDo);
            ICustomCommand cmd = cmdFactory.Create(rec);
            ICustomSender send = sendFactory.Create(cmd);

            send.ExecuteCommand("step1");
            send.ExecuteCommand("step2");

            rec.DettachCommand();
            rec.AttachCommand(toDo1, null);

            send.ExecuteCommand("this is dummy 1");
            send.ExecuteCommand("this is dummy 2 again");

            Console.WriteLine("quit?");
            Console.ReadLine();
        }
    }
}
