Welcome to CaseManagement
=========================

CaseManagement is an open source framework enabling any DOTNETCORE solutions to host a CMMN engine.
It supports most of the concepts introduced by the Case Management Model And Notation (CMMN) standard version 1.1 for examples :

1) Human task : Task that is performed by a Case worker.

2) Automatic task : Can be used in the Case to call a Business process.

3) Sentry :  combination of an "event and/or condition".

4) Milestone : represents an achievable target.

5) Case file : represent a piece of information of any nature, ranging from unstructured to structured and from simple to complex, which information can be defined based on any information modeling language.

Github: https://github.com/simpleidserver/SimpleIdServer 

Nuget feed: https://www.myget.org/F/advance-ict/api/v3/index.json 


.. toctree::
   :maxdepth: 3
   :hidden:
   :caption: Getting started

   intro/glossary
   intro/architecture
   intro/getting-started


.. toctree::
   :maxdepth: 3
   :hidden:
   :caption: Case Management User Guide
   
   casemanagement-usermanual/publishcaseplan
   casemanagement-usermanual/launchcaseinstance
   casemanagement-usermanual/viewcaseplaninstance
   
.. toctree::
   :maxdepth: 3
   :hidden:
   :caption: Reference
   
   reference/cmmn