How to setup a CaseManagement engine ?
======================================

The case engine can be hosted in a console application or in a web application.
The following parts describe the installation procedure for each type of project.

Console application
-------------------

1) Create an empty console application.

2) Install the nuget package **CaseManagement.CMMN**.

3) Insert the following code to start the workflow engine::

	static void Main(string[] args)
	{
		Console.WriteLine("Start CaseManagement engine");
		var serviceCollection = new ServiceCollection();
		var caseJobServer = new CaseJobServer(serviceCollection);
		caseJobServer.Start();
		Console.WriteLine("Press a key to quit");
		caseJobServer.Stop();
		Console.ReadKey();
	}

A sample project can be found `here <https://github.com/simpleidserver/CaseManagement/tree/master/src/CaseManagement.ConsoleApp>`_.

ASP.NET CORE application
------------------------

1) Create an empty ASP.NET CORE application.

2) Install the nuget package **CaseManagement.CMMN.AspNetCore**.

3) In the Startup.cs file, register the dependencies::

	services.AddCMMNEngine();

4) In the Startup.cs file, host the **BusHostedService** service::

	services.AddHostedService<BusHostedService>();

A sample project can be found `here <https://github.com/simpleidserver/CaseManagement/tree/master/src/CaseManagement.CMMN.Host>`_.

ASP.NET application
-------------------

1) Create an empty ASP.NET application.

2) Install the nuget package **CaseManagement.CMMN**.

3) In the Global.asax.cs, edit the method **Application_Start** to instantiate a ServiceCollection::

	var serviceCollection = new ServiceCollection();

3) In the Global.asax.cs file, edit the method **Application_Start** to start a new **CaseJobServer** instance::

	_jobServer = new CaseJobServer(serviceCollection, opts => {  });
	_jobServer.Start();

4) In the Global.asax.cs file, edit the method **Application_End** to stop the **CaseJobServer** instance::

	_jobServer.Stop();

A sample project can be found `here <https://github.com/simpleidserver/CaseManagement/tree/master/src/CaseManagement.AspNetWebApi>`_.