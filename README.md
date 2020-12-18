# CaseManagement

<img width="100px" src="https://raw.githubusercontent.com/simpleidserver/CaseManagement/release/1.0.3/logos/logo.png" />

CaseManagement is an open source framework enabling any DOTNETCORE applications to host a CMMN / WS-HumanTask or BPMN engine.
It supports most of the concepts introduced by CMMN1.1, BPMN2.0.2 and WS-HumanTask.

[![Build status](https://ci.appveyor.com/api/projects/status/q2ra83o0rcla41oc?svg=true)](https://ci.appveyor.com/project/simpleidserver/casemanagement)
[![MyGet (dev)](https://img.shields.io/myget/advance-ict/v/CaseManagement.CMMN.svg)](http://myget.org/gallery/advance-ict)
[![Documentation Status](https://readthedocs.org/projects/casemanagement/badge/?version=latest)](https://casemanagement.readthedocs.io/en/latest/)
[![Join the chat at https://gitter.im/simpleidserver/CaseManagement](https://badges.gitter.im/simpleidserver/CaseManagement.svg)](https://gitter.im/simpleidserver/CaseManagement?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

For project documentation, please visit [readthedocs](https://casemanagement.readthedocs.io/en/latest/).

## Packages

|                         			 						|      																															  																					|																																										|																																								|
| --------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `CaseManagement.BPMN` 			 						| [![MyGet (dev)](https://img.shields.io/myget/advance-ict/v/CaseManagement.BPMN.svg)](https://www.myget.org/feed/advance-ict/package/nuget/CaseManagement.BPMN)													| [![NuGet](https://img.shields.io/nuget/v/CaseManagement.BPMN.svg)](https://nuget.org/packages/CaseManagement.BPMN) 													| [![NuGet](https://img.shields.io/nuget/dt/CaseManagement.BPMN.svg)](https://nuget.org/packages/CaseManagement.BPMN) 											|
| `CaseManagement.BPMN.AspNetCore` 			 				| [![MyGet (dev)](https://img.shields.io/myget/advance-ict/v/CaseManagement.BPMN.AspNetCore.svg)](https://www.myget.org/feed/advance-ict/package/nuget/CaseManagement.BPMN.AspNetCore)								| [![NuGet](https://img.shields.io/nuget/v/CaseManagement.BPMN.AspNetCore.svg)](https://nuget.org/packages/CaseManagement.BPMN.AspNetCore) 								| [![NuGet](https://img.shields.io/nuget/dt/CaseManagement.BPMN.AspNetCore.svg)](https://nuget.org/packages/CaseManagement.BPMN.AspNetCore) 					|
| `CaseManagement.BPMN.Common` 			 					| [![MyGet (dev)](https://img.shields.io/myget/advance-ict/v/CaseManagement.BPMN.Common.svg)](https://www.myget.org/feed/advance-ict/package/nuget/CaseManagement.BPMN.Common)										| [![NuGet](https://img.shields.io/nuget/v/CaseManagement.BPMN.Common.svg)](https://nuget.org/packages/CaseManagement.BPMN.Common) 										| [![NuGet](https://img.shields.io/nuget/dt/CaseManagement.BPMN.Common.svg)](https://nuget.org/packages/CaseManagement.BPMN.Common) 							|
| `CaseManagement.BPMN.Persistence.EF`						| [![MyGet (dev)](https://img.shields.io/myget/advance-ict/v/CaseManagement.BPMN.Persistence.EF.svg)](https://www.myget.org/feed/advance-ict/package/nuget/CaseManagement.BPMN.Persistence.EF)						| [![NuGet](https://img.shields.io/nuget/v/CaseManagement.BPMN.Persistence.EF.svg)](https://nuget.org/packages/CaseManagement.BPMN.Persistence.EF) 						| [![NuGet](https://img.shields.io/nuget/dt/CaseManagement.BPMN.Persistence.EF.svg)](https://nuget.org/packages/CaseManagement.BPMN.Persistence.EF) 			|
| `CaseManagement.CMMN`										| [![MyGet (dev)](https://img.shields.io/myget/advance-ict/v/CaseManagement.CMMN.svg)](https://www.myget.org/feed/advance-ict/package/nuget/CaseManagement.CMMN)													| [![NuGet](https://img.shields.io/nuget/v/CaseManagement.CMMN.svg)](https://nuget.org/packages/CaseManagement.CMMN) 													| [![NuGet](https://img.shields.io/nuget/dt/CaseManagement.CMMN.svg)](https://nuget.org/packages/CaseManagement.CMMN) 											|
| `CaseManagement.CMMN.AspNetCore`							| [![MyGet (dev)](https://img.shields.io/myget/advance-ict/v/CaseManagement.CMMN.AspNetCore.svg)](https://www.myget.org/feed/advance-ict/package/nuget/CaseManagement.CMMN.AspNetCore)								| [![NuGet](https://img.shields.io/nuget/v/CaseManagement.CMMN.AspNetCore.svg)](https://nuget.org/packages/CaseManagement.CMMN.AspNetCore) 								| [![NuGet](https://img.shields.io/nuget/dt/CaseManagement.CMMN.AspNetCore.svg)](https://nuget.org/packages/CaseManagement.CMMN.AspNetCore) 					|
| `CaseManagement.CMMN.Persistence.EF`						| [![MyGet (dev)](https://img.shields.io/myget/advance-ict/v/CaseManagement.CMMN.Persistence.EF.svg)](https://www.myget.org/feed/advance-ict/package/nuget/CaseManagement.CMMN.Persistence.EF)						| [![NuGet](https://img.shields.io/nuget/v/CaseManagement.CMMN.Persistence.EF.svg)](https://nuget.org/packages/CaseManagement.CMMN.Persistence.EF) 						| [![NuGet](https://img.shields.io/nuget/dt/CaseManagement.CMMN.Persistence.EF.svg)](https://nuget.org/packages/CaseManagement.CMMN.Persistence.EF) 			|
| `CaseManagement.Common`									| [![MyGet (dev)](https://img.shields.io/myget/advance-ict/v/CaseManagement.Common.svg)](https://www.myget.org/feed/advance-ict/package/nuget/CaseManagement.Common)												| [![NuGet](https://img.shields.io/nuget/v/CaseManagement.Common.svg)](https://nuget.org/packages/CaseManagement.Common) 												| [![NuGet](https://img.shields.io/nuget/dt/CaseManagement.Common.svg)](https://nuget.org/packages/CaseManagement.Common) 										|
| `CaseManagement.Common.SqlServer`							| [![MyGet (dev)](https://img.shields.io/myget/advance-ict/v/CaseManagement.Common.SqlServer.svg)](https://www.myget.org/feed/advance-ict/package/nuget/CaseManagement.Common.SqlServer)							| [![NuGet](https://img.shields.io/nuget/v/CaseManagement.Common.SqlServer.svg)](https://nuget.org/packages/CaseManagement.Common.SqlServer) 							| [![NuGet](https://img.shields.io/nuget/dt/CaseManagement.Common.SqlServer.svg)](https://nuget.org/packages/CaseManagement.Common.SqlServer) 					|
| `CaseManagement.HumanTask`								| [![MyGet (dev)](https://img.shields.io/myget/advance-ict/v/CaseManagement.HumanTask.svg)](https://www.myget.org/feed/advance-ict/package/nuget/CaseManagement.HumanTask)											| [![NuGet](https://img.shields.io/nuget/v/CaseManagement.HumanTask.svg)](https://nuget.org/packages/CaseManagement.HumanTask) 											| [![NuGet](https://img.shields.io/nuget/dt/CaseManagement.HumanTask.svg)](https://nuget.org/packages/CaseManagement.HumanTask) 								|
| `CaseManagement.HumanTask.AspNetCore`						| [![MyGet (dev)](https://img.shields.io/myget/advance-ict/v/CaseManagement.HumanTask.AspNetCore.svg)](https://www.myget.org/feed/advance-ict/package/nuget/CaseManagement.HumanTask.AspNetCore)					| [![NuGet](https://img.shields.io/nuget/v/CaseManagement.HumanTask.AspNetCore.svg)](https://nuget.org/packages/CaseManagement.HumanTask.AspNetCore) 					| [![NuGet](https://img.shields.io/nuget/dt/CaseManagement.HumanTask.AspNetCore.svg)](https://nuget.org/packages/CaseManagement.HumanTask.AspNetCore) 			|
| `CaseManagement.HumanTask.Persistence.EF`					| [![MyGet (dev)](https://img.shields.io/myget/advance-ict/v/CaseManagement.HumanTask.Persistence.EF.svg)](https://www.myget.org/feed/advance-ict/package/nuget/CaseManagement.HumanTask.Persistence.EF)			| [![NuGet](https://img.shields.io/nuget/v/CaseManagement.HumanTask.Persistence.EF.svg)](https://nuget.org/packages/CaseManagement.HumanTask.Persistence.EF) 			| [![NuGet](https://img.shields.io/nuget/dt/CaseManagement.HumanTask.Persistence.EF.svg)](https://nuget.org/packages/CaseManagement.HumanTask.Persistence.EF) 	|

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on our code of conduct, and the process for submitting pull requests to us.

## Live demo

### Case management website

CaseManagement live demo : [http://simpleidserver.northeurope.cloudapp.azure.com/casemanagement](http://simpleidserver.northeurope.cloudapp.azure.com/casemanagement).

Business analyst credentials :

| Property      |      Value      |
|---------------|-----------------|
| login         | businessanalyst |
| value         | password        |

### Tasklist website

Tasklist live demo : [http://simpleidserver.northeurope.cloudapp.azure.com/casemanagement](http://simpleidserver.northeurope.cloudapp.azure.com/tasklist).

Business analyst credentials :

| Property      |      Value      |
|---------------|-----------------|
| login         | businessanalyst |
| value         | password        |