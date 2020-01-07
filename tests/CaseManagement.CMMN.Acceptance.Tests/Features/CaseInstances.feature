Feature: CaseInstances
	Check result returned by /case-instances
	
Scenario: Launch caseWithOneTask and check his status is completed
	When execute HTTP GET request 'http://localhost/case-definitions/.search?cmmn_definition=caseWithOneTask.cmmn'
	And extract JSON from body
	And extract 'content[0].id' from JSON body into 'defid'
	And execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value    |
	| case_definition_id | $defid$  |
	And extract JSON from body
	And extract 'id' from JSON body into 'insid'
	And execute HTTP GET request 'http://localhost/case-instances/$insid$/launch'	
	And poll 'http://localhost/case-instances/$insid$', until 'state'='Completed'
	And extract JSON from body
	
	Then HTTP status code equals to '200'
	Then JSON 'state'='Completed'
	
Scenario: Launch caseWithOneLongProcessTask and check his status is completed
	When execute HTTP GET request 'http://localhost/case-definitions/.search?cmmn_definition=caseWithOneLongProcessTask.cmmn'
	And extract JSON from body
	And extract 'content[0].id' from JSON body into 'defid'
	And execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value    |
	| case_definition_id | $defid$  |
	And extract JSON from body
	And extract 'id' from JSON body into 'insid'
	And execute HTTP GET request 'http://localhost/case-instances/$insid$/launch'	
	And poll 'http://localhost/case-instances/$insid$', until 'state'='Completed'
	And extract JSON from body
	
	Then HTTP status code equals to '200'
	Then JSON 'state'='Completed'
	Then JSON 'state_histories[0].state'='Active'
	Then JSON 'state_histories[1].state'='Completed'
	Then JSON 'elements[0].transition_histories[0].transition'='Create'
	Then JSON 'elements[0].transition_histories[1].transition'='Start'
	Then JSON 'elements[0].transition_histories[2].transition'='Complete'
	Then JSON 'elements[0].state_histories[0].state'='Available'
	Then JSON 'elements[0].state_histories[1].state'='Active'
	Then JSON 'elements[0].state_histories[2].state'='Completed'
	
Scenario: Launch caseWithTwoStages and check his status is completed
	When execute HTTP GET request 'http://localhost/case-definitions/.search?cmmn_definition=caseWithTwoStages.cmmn'
	And extract JSON from body
	And extract 'content[0].id' from JSON body into 'defid'
	And execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value    |
	| case_definition_id | $defid$  |
	And extract JSON from body
	And extract 'id' from JSON body into 'insid'
	And execute HTTP GET request 'http://localhost/case-instances/$insid$/launch'		
	And poll 'http://localhost/case-instances/$insid$', until '$.elements[?(@.definition_id == 'PlanItem_1291396')].state'='Failed'
	And poll 'http://localhost/case-instances/$insid$', until '$.elements[?(@.definition_id == 'PlanItem_0ox3upg')].state'='Failed'
	And poll 'http://localhost/case-instances/$insid$', until '$.elements[?(@.definition_id == 'PlanItem_0ctfxhw')].state'='Completed'
	And poll 'http://localhost/case-instances/$insid$', until '$.elements[?(@.definition_id == 'PlanItem_1fd01pl')].state'='Completed'
	And execute HTTP GET request 'http://localhost/case-instances/$insid$/terminate'
	And poll 'http://localhost/case-instances/$insid$', until 'state'='Terminated'
	And extract JSON from body
	
	Then HTTP status code equals to '200'
	Then JSON 'state'='Terminated'
	Then JSON 'state_histories[0].state'='Active'
	Then JSON 'state_histories[1].state'='Terminated'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_1291396')].transition_histories[0].transition'='Create'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_1291396')].transition_histories[1].transition'='Start'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_1291396')].transition_histories[2].transition'='Fault'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_1291396')].state_histories[0].state'='Available'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_1291396')].state_histories[1].state'='Active'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_1291396')].state_histories[2].state'='Failed'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_0ox3upg')].transition_histories[0].transition'='Create'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_0ox3upg')].transition_histories[1].transition'='Start'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_0ox3upg')].transition_histories[2].transition'='Fault'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_0ox3upg')].state_histories[0].state'='Available'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_0ox3upg')].state_histories[1].state'='Active'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_0ox3upg')].state_histories[2].state'='Failed'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_0ctfxhw')].transition_histories[0].transition'='Create'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_0ctfxhw')].transition_histories[1].transition'='Start'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_0ctfxhw')].transition_histories[2].transition'='Complete'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_0ctfxhw')].state_histories[0].state'='Available'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_0ctfxhw')].state_histories[1].state'='Active'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_0ctfxhw')].state_histories[2].state'='Completed'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_1fd01pl')].transition_histories[0].transition'='Create'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_1fd01pl')].transition_histories[1].transition'='Start'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_1fd01pl')].transition_histories[2].transition'='Complete'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_1fd01pl')].state_histories[0].state'='Available'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_1fd01pl')].state_histories[1].state'='Active'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_1fd01pl')].state_histories[2].state'='Completed'

Scenario: Launch caseWithOneHumanTask and check his status is completed
	When execute HTTP GET request 'http://localhost/case-definitions/.search?cmmn_definition=caseWithOneHumanTask.cmmn'
	And extract JSON from body
	And extract 'content[0].id' from JSON body into 'defid'
	And execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value    |
	| case_definition_id | $defid$  |
	And extract JSON from body
	And extract 'id' from JSON body into 'insid'
	And execute HTTP GET request 'http://localhost/case-instances/$insid$/launch'	
	And poll 'http://localhost/case-instances/$insid$', until 'elements[0].transition_histories[1].transition'='Start'
	And extract JSON from body
	And extract 'elements[0].id' from JSON body into 'eltid'
	And execute HTTP POST JSON request 'http://localhost/case-instances/$insid$/confirm/$eltid$'
	| Key  | Value |
	| name | name  |
	And poll 'http://localhost/case-instances/$insid$', until 'state'='Completed'
	And extract JSON from body
	
	Then HTTP status code equals to '200'
	Then JSON 'state'='Completed'
	Then JSON 'state_histories[0].state'='Active'
	Then JSON 'state_histories[1].state'='Completed'
	Then JSON 'elements[0].transition_histories[0].transition'='Create'
	Then JSON 'elements[0].transition_histories[1].transition'='Start'
	Then JSON 'elements[0].transition_histories[2].transition'='Complete'
	Then JSON 'elements[0].state_histories[0].state'='Available'
	Then JSON 'elements[0].state_histories[1].state'='Active'
	Then JSON 'elements[0].state_histories[2].state'='Completed'

Scenario: Launch caseWithOneManualActivationTask and check his status is completed
	When execute HTTP GET request 'http://localhost/case-definitions/.search?cmmn_definition=caseWithOneManualActivationTask.cmmn'
	And extract JSON from body
	And extract 'content[0].id' from JSON body into 'defid'
	And execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value    |
	| case_definition_id | $defid$  |
	And extract JSON from body
	And extract 'id' from JSON body into 'insid'
	And execute HTTP GET request 'http://localhost/case-instances/$insid$/launch'	
	And poll 'http://localhost/case-instances/$insid$', until 'elements[0].state_histories[1].state'='Enabled'
	And extract JSON from body
	And extract 'elements[0].id' from JSON body into 'eltid'
	And execute HTTP GET request 'http://localhost/case-instances/$insid$/activate/$eltid$'	
	And poll 'http://localhost/case-instances/$insid$', until 'state'='Completed'
	And extract JSON from body
	
	Then HTTP status code equals to '200'
	Then JSON 'state'='Completed'
	Then JSON 'state_histories[0].state'='Active'
	Then JSON 'state_histories[1].state'='Completed'
	Then JSON 'elements[0].transition_histories[0].transition'='Create'
	Then JSON 'elements[0].transition_histories[1].transition'='Enable'
	Then JSON 'elements[0].transition_histories[2].transition'='ManualStart'
	Then JSON 'elements[0].transition_histories[3].transition'='Complete'
	Then JSON 'elements[0].state_histories[0].state'='Available'
	Then JSON 'elements[0].state_histories[1].state'='Enabled'
	Then JSON 'elements[0].state_histories[2].state'='Active'
	Then JSON 'elements[0].state_histories[3].state'='Completed'

Scenario: Launch caseWithRepetitionRule and check his status is completed
	When execute HTTP GET request 'http://localhost/case-definitions/.search?cmmn_definition=caseWithRepetitionRule.cmmn'
	And extract JSON from body
	And extract 'content[0].id' from JSON body into 'defid'
	And execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value    |
	| case_definition_id | $defid$  |
	And extract JSON from body
	And extract 'id' from JSON body into 'insid'
	And execute HTTP GET request 'http://localhost/case-instances/$insid$/launch'	
	And poll 'http://localhost/case-instances/$insid$', until 'state'='Completed'
	And extract JSON from body
	
	Then HTTP status code equals to '200'
	Then JSON 'state'='Completed'
	Then JSON 'state_histories[0].state'='Active'
	Then JSON 'state_histories[1].state'='Completed'
	Then JSON 'elements[0].transition_histories[0].transition'='Create'
	Then JSON 'elements[0].transition_histories[1].transition'='Start'
	Then JSON 'elements[0].transition_histories[2].transition'='Complete'
	Then JSON 'elements[0].state_histories[0].state'='Available'
	Then JSON 'elements[0].state_histories[1].state'='Active'
	Then JSON 'elements[0].state_histories[2].state'='Completed'
	Then JSON 'elements[1].transition_histories[0].transition'='Create'
	Then JSON 'elements[1].transition_histories[1].transition'='Start'
	Then JSON 'elements[1].transition_histories[2].transition'='Complete'
	Then JSON 'elements[1].state_histories[0].state'='Available'
	Then JSON 'elements[1].state_histories[1].state'='Active'
	Then JSON 'elements[1].state_histories[2].state'='Completed'

Scenario: Launch caseWithOneSEntry and check his status is completed
	When execute HTTP GET request 'http://localhost/case-definitions/.search?cmmn_definition=caseWithOneSEntry.cmmn'
	And extract JSON from body
	And extract 'content[0].id' from JSON body into 'defid'
	And execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value    |
	| case_definition_id | $defid$  |
	And extract JSON from body
	And extract 'id' from JSON body into 'insid'
	And execute HTTP GET request 'http://localhost/case-instances/$insid$/launch'
	And poll 'http://localhost/case-instances/$insid$', until 'state'='Completed'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'state'='Completed'
	Then JSON 'state_histories[0].state'='Active'
	Then JSON 'state_histories[1].state'='Completed'
	Then JSON 'elements[0].transition_histories[0].transition'='Create'
	Then JSON 'elements[0].transition_histories[1].transition'='Start'
	Then JSON 'elements[0].transition_histories[2].transition'='Complete'
	Then JSON 'elements[0].state_histories[0].state'='Available'
	Then JSON 'elements[0].state_histories[1].state'='Active'
	Then JSON 'elements[0].state_histories[2].state'='Completed'
	Then JSON 'elements[1].transition_histories[0].transition'='Create'
	Then JSON 'elements[1].transition_histories[1].transition'='Start'
	Then JSON 'elements[1].transition_histories[2].transition'='Complete'
	Then JSON 'elements[1].state_histories[0].state'='Available'
	Then JSON 'elements[1].state_histories[1].state'='Active'
	Then JSON 'elements[1].state_histories[2].state'='Completed'
	Then JSON 'elements[2].transition_histories[0].transition'='Create'
	Then JSON 'elements[2].transition_histories[1].transition'='Start'
	Then JSON 'elements[2].transition_histories[2].transition'='Complete'
	Then JSON 'elements[2].state_histories[0].state'='Available'
	Then JSON 'elements[2].state_histories[1].state'='Active'
	Then JSON 'elements[2].state_histories[2].state'='Completed'
	Then JSON 'elements[3].transition_histories[0].transition'='Create'
	Then JSON 'elements[3].transition_histories[1].transition'='Start'
	Then JSON 'elements[3].transition_histories[2].transition'='Complete'
	Then JSON 'elements[3].state_histories[0].state'='Available'
	Then JSON 'elements[3].state_histories[1].state'='Active'
	Then JSON 'elements[3].state_histories[2].state'='Completed'

Scenario: Launch caseWithOneTimerEventListener and check his status is completed
	When execute HTTP GET request 'http://localhost/case-definitions/.search?cmmn_definition=caseWithOneTimerEventListener.cmmn'
	And extract JSON from body
	And extract 'content[0].id' from JSON body into 'defid'
	And execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value    |
	| case_definition_id | $defid$  |
	And extract JSON from body
	And extract 'id' from JSON body into 'insid'
	And execute HTTP GET request 'http://localhost/case-instances/$insid$/launch'	
	And poll 'http://localhost/case-instances/$insid$', until 'state'='Completed'
	And extract JSON from body
	
	Then HTTP status code equals to '200'
	Then JSON 'state'='Completed'
	Then JSON 'state_histories[0].state'='Active'
	Then JSON 'state_histories[1].state'='Completed'
	Then JSON 'elements[0].transition_histories[0].transition'='Create'
	Then JSON 'elements[0].transition_histories[1].transition'='Start'
	Then JSON 'elements[0].transition_histories[2].transition'='Complete'
	Then JSON 'elements[0].state_histories[0].state'='Available'
	Then JSON 'elements[0].state_histories[1].state'='Active'
	Then JSON 'elements[0].state_histories[2].state'='Completed'
	Then JSON 'elements[1].transition_histories[0].transition'='Create'
	Then JSON 'elements[1].transition_histories[1].transition'='Occur'
	Then JSON 'elements[1].state_histories[0].state'='Available'
	Then JSON 'elements[1].state_histories[1].state'='Completed'
	Then JSON 'elements[2].transition_histories[0].transition'='Create'
	Then JSON 'elements[2].transition_histories[1].transition'='Occur'
	Then JSON 'elements[2].state_histories[0].state'='Available'
	Then JSON 'elements[2].state_histories[1].state'='Completed'

Scenario: Suspend caseWithOneLongProcessTask and check his status is suspended (scope = ProcessTask)
	When execute HTTP GET request 'http://localhost/case-definitions/.search?cmmn_definition=caseWithOneLongProcessTask.cmmn'
	And extract JSON from body
	And extract 'content[0].id' from JSON body into 'defid'
	And execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value    |
	| case_definition_id | $defid$  |
	And extract JSON from body
	And extract 'id' from JSON body into 'insid'
	And execute HTTP GET request 'http://localhost/case-instances/$insid$/launch'	
	And poll 'http://localhost/case-instances/$insid$', until 'elements[0].state_histories[1].state'='Active'
	And extract JSON from body
	And extract 'elements[0].id' from JSON body into 'eltid'
	And execute HTTP GET request 'http://localhost/case-instances/$insid$/suspend/$eltid$'
	And poll 'http://localhost/case-instances/$insid$', until 'elements[0].state_histories[2].state'='Suspended'
	And execute HTTP GET request 'http://localhost/case-instances/$insid$/resume/$eltid$'
	And poll 'http://localhost/case-instances/$insid$', until 'state'='Completed'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'state'='Completed'
	Then JSON 'state_histories[0].state'='Active'
	Then JSON 'state_histories[1].state'='Completed'
	Then JSON 'elements[0].transition_histories[0].transition'='Create'
	Then JSON 'elements[0].transition_histories[1].transition'='Start'
	Then JSON 'elements[0].transition_histories[2].transition'='Suspend'
	Then JSON 'elements[0].transition_histories[3].transition'='Resume'
	Then JSON 'elements[0].transition_histories[4].transition'='Complete'
	Then JSON 'elements[0].state_histories[0].state'='Available'
	Then JSON 'elements[0].state_histories[1].state'='Active'
	Then JSON 'elements[0].state_histories[2].state'='Suspended'
	Then JSON 'elements[0].state_histories[3].state'='Active'
	Then JSON 'elements[0].state_histories[4].state'='Completed'

Scenario: Suspend caseWithOneLongProcessTask and check his status is suspended (scope = Case Instance)
	When execute HTTP GET request 'http://localhost/case-definitions/.search?cmmn_definition=caseWithOneLongProcessTask.cmmn'
	And extract JSON from body
	And extract 'content[0].id' from JSON body into 'defid'
	And execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value    |
	| case_definition_id | $defid$  |
	And extract JSON from body
	And extract 'id' from JSON body into 'insid'
	And execute HTTP GET request 'http://localhost/case-instances/$insid$/launch'	
	And poll 'http://localhost/case-instances/$insid$', until 'elements[0].state_histories[1].state'='Active'
	And extract JSON from body
	And extract 'elements[0].id' from JSON body into 'eltid'
	And execute HTTP GET request 'http://localhost/case-instances/$insid$/suspend'
	And poll 'http://localhost/case-instances/$insid$', until 'elements[0].state_histories[2].state'='Suspended'
	And execute HTTP GET request 'http://localhost/case-instances/$insid$/resume'
	And poll 'http://localhost/case-instances/$insid$', until 'state'='Completed'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'state'='Completed'
	Then JSON 'state_histories[0].state'='Active'
	Then JSON 'state_histories[1].state'='Suspended'
	Then JSON 'state_histories[2].state'='Active'
	Then JSON 'state_histories[3].state'='Completed'
	Then JSON 'elements[0].transition_histories[0].transition'='Create'
	Then JSON 'elements[0].transition_histories[1].transition'='Start'
	Then JSON 'elements[0].transition_histories[2].transition'='ParentSuspend'
	Then JSON 'elements[0].transition_histories[3].transition'='ParentResume'
	Then JSON 'elements[0].transition_histories[4].transition'='Complete'
	Then JSON 'elements[0].state_histories[0].state'='Available'
	Then JSON 'elements[0].state_histories[1].state'='Active'
	Then JSON 'elements[0].state_histories[2].state'='Suspended'
	Then JSON 'elements[0].state_histories[3].state'='Active'
	Then JSON 'elements[0].state_histories[4].state'='Completed'

Scenario: Reactive failed caseWithTwoStages and check his status is failed (scope = Stage)
	When execute HTTP GET request 'http://localhost/case-definitions/.search?cmmn_definition=caseWithTwoStages.cmmn'
	And extract JSON from body
	And extract 'content[0].id' from JSON body into 'defid'
	And execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value    |
	| case_definition_id | $defid$  |
	And extract JSON from body
	And extract 'id' from JSON body into 'insid'
	And execute HTTP GET request 'http://localhost/case-instances/$insid$/launch'		
	And poll 'http://localhost/case-instances/$insid$', until '$.elements[?(@.definition_id == 'PlanItem_1291396')].state'='Failed'
	And poll 'http://localhost/case-instances/$insid$', until '$.elements[?(@.definition_id == 'PlanItem_0ox3upg')].state'='Failed'
	And poll 'http://localhost/case-instances/$insid$', until '$.elements[?(@.definition_id == 'PlanItem_0ctfxhw')].state'='Completed'
	And poll 'http://localhost/case-instances/$insid$', until '$.elements[?(@.definition_id == 'PlanItem_1fd01pl')].state'='Completed'
	And extract JSON from body
	And extract '$.elements[?(@.definition_id == 'PlanItem_1291396')].id' from JSON body into 'eltid'
	And execute HTTP GET request 'http://localhost/case-instances/$insid$/reactivate/$eltid$'
	And poll 'http://localhost/case-instances/$insid$', until '$.elements[?(@.definition_id == 'PlanItem_1291396')].state_histories[4].state'='Failed'
	And execute HTTP GET request 'http://localhost/case-instances/$insid$/terminate'
	And poll 'http://localhost/case-instances/$insid$', until 'state'='Terminated'
	And extract JSON from body
	
	Then HTTP status code equals to '200'
	Then JSON 'state'='Terminated'
	Then JSON 'state_histories[0].state'='Active'
	Then JSON 'state_histories[1].state'='Terminated'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_1291396')].transition_histories[0].transition'='Create'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_1291396')].transition_histories[1].transition'='Start'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_1291396')].transition_histories[2].transition'='Fault'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_1291396')].transition_histories[3].transition'='Reactivate'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_1291396')].transition_histories[4].transition'='Fault'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_1291396')].state_histories[0].state'='Available'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_1291396')].state_histories[1].state'='Active'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_1291396')].state_histories[2].state'='Failed'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_1291396')].state_histories[3].state'='Active'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_1291396')].state_histories[4].state'='Failed'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_0ox3upg')].transition_histories[0].transition'='Create'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_0ox3upg')].transition_histories[1].transition'='Start'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_0ox3upg')].transition_histories[2].transition'='Fault'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_0ox3upg')].transition_histories[3].transition'='Reactivate'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_0ox3upg')].transition_histories[4].transition'='Fault'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_0ox3upg')].state_histories[0].state'='Available'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_0ox3upg')].state_histories[1].state'='Active'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_0ox3upg')].state_histories[2].state'='Failed'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_0ox3upg')].state_histories[3].state'='Active'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_0ox3upg')].state_histories[4].state'='Failed'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_0ctfxhw')].transition_histories[0].transition'='Create'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_0ctfxhw')].transition_histories[1].transition'='Start'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_0ctfxhw')].transition_histories[2].transition'='Complete'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_0ctfxhw')].state_histories[0].state'='Available'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_0ctfxhw')].state_histories[1].state'='Active'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_0ctfxhw')].state_histories[2].state'='Completed'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_1fd01pl')].transition_histories[0].transition'='Create'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_1fd01pl')].transition_histories[1].transition'='Start'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_1fd01pl')].transition_histories[2].transition'='Complete'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_1fd01pl')].state_histories[0].state'='Available'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_1fd01pl')].state_histories[1].state'='Active'
	Then JSON '$.elements[?(@.definition_id == 'PlanItem_1fd01pl')].state_histories[2].state'='Completed'


Scenario: Reactivate failed caseWithOneFailProcessTask and check his status is failed (scope = Case Instance)
	When execute HTTP GET request 'http://localhost/case-definitions/.search?cmmn_definition=caseWithOneFailProcessTask.cmmn'
	And extract JSON from body
	And extract 'content[0].id' from JSON body into 'defid'
	And execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value    |
	| case_definition_id | $defid$  |
	And extract JSON from body
	And extract 'id' from JSON body into 'insid'
	And execute HTTP GET request 'http://localhost/case-instances/$insid$/launch'	
	And poll 'http://localhost/case-instances/$insid$', until 'state_histories[1].state'='Failed'
	And execute HTTP GET request 'http://localhost/case-instances/$insid$/reactivate'
	And poll 'http://localhost/case-instances/$insid$', until 'state_histories[3].state'='Failed'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'state'='Failed'
	Then JSON 'state_histories[0].state'='Active'
	Then JSON 'state_histories[1].state'='Failed'
	Then JSON 'state_histories[2].state'='Active'
	Then JSON 'state_histories[3].state'='Failed'
	Then JSON 'elements[0].transition_histories[0].transition'='Create'
	Then JSON 'elements[0].transition_histories[1].transition'='Start'
	Then JSON 'elements[0].transition_histories[2].transition'='Fault'
	Then JSON 'elements[0].transition_histories[3].transition'='Reactivate'
	Then JSON 'elements[0].transition_histories[4].transition'='Fault'
	Then JSON 'elements[0].state_histories[0].state'='Available'
	Then JSON 'elements[0].state_histories[1].state'='Active'
	Then JSON 'elements[0].state_histories[2].state'='Failed'
	Then JSON 'elements[0].state_histories[3].state'='Active'
	Then JSON 'elements[0].state_histories[4].state'='Failed'
	
Scenario: Terminate caseWithOneLongProcessTask and check his status is terminated
	When execute HTTP GET request 'http://localhost/case-definitions/.search?cmmn_definition=caseWithOneLongProcessTask.cmmn'
	And extract JSON from body
	And extract 'content[0].id' from JSON body into 'defid'
	And execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value    |
	| case_definition_id | $defid$  |
	And extract JSON from body
	And extract 'id' from JSON body into 'insid'
	And execute HTTP GET request 'http://localhost/case-instances/$insid$/launch'	
	And poll 'http://localhost/case-instances/$insid$', until 'elements[0].state_histories[1].state'='Active'
	And extract JSON from body
	And extract 'elements[0].id' from JSON body into 'eltid'
	And execute HTTP GET request 'http://localhost/case-instances/$insid$/terminate/$eltid$'
	And poll 'http://localhost/case-instances/$insid$', until 'state'='Completed'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'state'='Completed'
	Then JSON 'state_histories[0].state'='Active'
	Then JSON 'state_histories[1].state'='Completed'
	Then JSON 'elements[0].transition_histories[0].transition'='Create'
	Then JSON 'elements[0].transition_histories[1].transition'='Start'
	Then JSON 'elements[0].transition_histories[2].transition'='Terminate'
	Then JSON 'elements[0].state_histories[0].state'='Available'
	Then JSON 'elements[0].state_histories[1].state'='Active'
	Then JSON 'elements[0].state_histories[2].state'='Terminated'