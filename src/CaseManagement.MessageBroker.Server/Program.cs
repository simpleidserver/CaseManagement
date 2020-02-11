// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using MassTransit;
using System;

namespace CaseManagement.MessageBroker.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var busControl = Bus.Factory.CreateUsingInMemory(cfg => cfg.Host(cb => { }));
            Console.WriteLine("Starting the message broker ...");
            busControl.Start();
            Console.WriteLine("The message broker is started !");
            Console.WriteLine("Press any key to quit the application");
            Console.ReadLine();
            busControl.Stop();
        }
    }
}
