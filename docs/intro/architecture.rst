Architecture overview
=====================

.. image:: images/architecture-1.png
   :align: center

* Admin Website : the website is developed with angular. It offers some facilities to the end users like : 

  * Audit screens : audit performance, view statistics like the number of cases closed since one month.
  * Case management : manage the lifecycle of case instances for example : start, suspend or terminate a case instance.
 
* CaseManagement API : REST.API service which exposes operations to interact with the engine.

* CaseManagement Engine : Execute a case instance.


Clustering model
----------------

The case engine can be distributed to different nodes in a cluster. Each case engine must then connect to a shared database.