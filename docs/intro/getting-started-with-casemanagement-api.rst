How to setup a CaseManagement API ?
===================================

CaseManagement API can be installed with the engine or separately from it. 
Like the engine, the API can be installed in two ways depending on the type of project.

ASP.NET CORE application
------------------------

1) Create an empty ASP.NET CORE application.

2) Install the nuget package **CaseManagement.CMMN.AspNetCore**.

3) In the Startup.cs file, register the dependencies like this:: 

	services.AddCMMNApi();

A sample project can be found `here <https://github.com/simpleidserver/CaseManagement/tree/master/src/CaseManagement.CMMN.Host>`_.

ASP.NET application
-------------------

1) Create an empty ASP.NET application.

2) Install the nuget package **CaseManagement.CMMN.AspNet**.

3) In the file where the dependency injection engine is configured, the CaseManagement dependencies should also be registered. The `project <https://github.com/simpleidserver/CaseManagement/tree/master/src/CaseManagement.AspNetWebApi>`_ shows how to inject the dependencies with Unity.