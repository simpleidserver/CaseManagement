# CaseManagement

CaseManagement is an open source framework enabling any DOTNETCORE to host a CMMN engine.
It supports most of the concepts introduced by the Case Management Model And Notation (CMMN) standard version 1.1 for examples :

1) Human task : Task that is performed by a Case worker.

2) Automatic task : Can be used in the Case to call a Business process.

3) Sentry :  combination of an "event and/or condition".

4) Milestone : represents an achievable target.

5) Case file : represent a piece of information of any nature, ranging from unstructured to structured and from simple to complex, which information can be defined based on any information modeling language.

[![Build status](https://ci.appveyor.com/api/projects/status/q2ra83o0rcla41oc?svg=true)](https://ci.appveyor.com/project/simpleidserver/casemanagement)
[![MyGet (dev)](https://img.shields.io/myget/advance-ict/v/CaseManagement.CMMN.svg)](http://myget.org/gallery/advance-ict)
[![Documentation Status](https://readthedocs.org/projects/casemanagement/badge/?version=latest)](https://casemanagement.readthedocs.io/en/latest/)
[![Join the chat at https://gitter.im/simpleidserver/CaseManagement](https://badges.gitter.im/simpleidserver/CaseManagement.svg)](https://gitter.im/simpleidserver/CaseManagement?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

For project documentation, please visit [readthedocs](https://casemanagement.readthedocs.io/en/latest/).

## Packages

|                         			 						|      																															  																					|																																										|																																								|
| --------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `CaseManagement.CMMN` 			 						| [![MyGet (dev)](https://img.shields.io/myget/advance-ict/v/CaseManagement.CMMN.svg)](https://www.myget.org/feed/advance-ict/package/nuget/CaseManagement.CMMN)													| [![NuGet](https://img.shields.io/nuget/v/CaseManagement.CMMN.svg)](https://nuget.org/packages/CaseManagement.CMMN) 													| [![NuGet](https://img.shields.io/nuget/dt/CaseManagement.CMMN.svg)](https://nuget.org/packages/CaseManagement.CMMN) 											|
| `CaseManagement.CMMN.AspNetCore`							| [![MyGet (dev)](https://img.shields.io/myget/advance-ict/v/CaseManagement.CMMN.AspNetCore.svg)](https://www.myget.org/feed/advance-ict/package/nuget/CaseManagement.CMMN.AspNetCore)								| [![NuGet](https://img.shields.io/nuget/v/CaseManagement.CMMN.AspNetCore.svg)](https://nuget.org/packages/CaseManagement.CMMN.AspNetCore) 								| [![NuGet](https://img.shields.io/nuget/dt/CaseManagement.CMMN.AspNetCore.svg)](https://nuget.org/packages/CaseManagement.CMMN.AspNetCore) 					|
| `CaseManagement.CMMN.SqlServer`							| [![MyGet (dev)](https://img.shields.io/myget/advance-ict/v/CaseManagement.CMMN.SqlServer.svg)](https://www.myget.org/feed/advance-ict/package/nuget/CaseManagement.CMMN.SqlServer)								| [![NuGet](https://img.shields.io/nuget/v/CaseManagement.CMMN.SqlServer.svg)](https://nuget.org/packages/CaseManagement.CMMN.SqlServer) 								| [![NuGet](https://img.shields.io/nuget/dt/CaseManagement.CMMN.SqlServer.svg)](https://nuget.org/packages/CaseManagement.CMMN.SqlServer) 						|
| `CaseManagement.CMMN.Persistence.EF`						| [![MyGet (dev)](https://img.shields.io/myget/advance-ict/v/CaseManagement.CMMN.Persistence.EF.svg)](https://www.myget.org/feed/advance-ict/package/nuget/CaseManagement.CMMN.Persistence.EF)						| [![NuGet](https://img.shields.io/nuget/v/CaseManagement.CMMN.Persistence.EF.svg)](https://nuget.org/packages/CaseManagement.CMMN.Persistence.EF) 						| [![NuGet](https://img.shields.io/nuget/dt/CaseManagement.CMMN.Persistence.EF.svg)](https://nuget.org/packages/CaseManagement.CMMN.Persistence.EF) 			|

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

Case worker credentials :

| Property      |      Value      |
|---------------|-----------------|
| login         | caseworker      |
| value         | password        |