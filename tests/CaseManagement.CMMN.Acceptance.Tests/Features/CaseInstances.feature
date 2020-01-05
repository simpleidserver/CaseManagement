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
	And wait '1000' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$insid$'
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
	And wait '2000' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$insid$'
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
	And wait '1000' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$insid$/terminate'
	And wait '1000' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$insid$'
	And extract JSON from body
	
	Then HTTP status code equals to '200'
	Then JSON 'state'='Terminated'
	Then JSON 'state_histories[0].state'='Active'
	Then JSON 'state_histories[1].state'='Terminated'
	Then JSON 'elements[0].transition_histories[0].transition'='Create'
	Then JSON 'elements[0].transition_histories[1].transition'='Start'
	Then JSON 'elements[0].transition_histories[2].transition'='Fault'
	Then JSON 'elements[0].state_histories[0].state'='Available'
	Then JSON 'elements[0].state_histories[1].state'='Active'
	Then JSON 'elements[0].state_histories[2].state'='Failed'
	Then JSON 'elements[1].transition_histories[0].transition'='Create'
	Then JSON 'elements[1].transition_histories[1].transition'='Start'
	Then JSON 'elements[1].transition_histories[2].transition'='Complete'
	Then JSON 'elements[1].state_histories[0].state'='Available'
	Then JSON 'elements[1].state_histories[1].state'='Active'
	Then JSON 'elements[1].state_histories[2].state'='Completed'
	Then JSON 'elements[2].transition_histories[0].transition'='Create'
	Then JSON 'elements[2].transition_histories[1].transition'='Start'
	Then JSON 'elements[2].transition_histories[2].transition'='Fault'
	Then JSON 'elements[2].state_histories[0].state'='Available'
	Then JSON 'elements[2].state_histories[1].state'='Active'
	Then JSON 'elements[2].state_histories[2].state'='Failed'
	Then JSON 'elements[3].transition_histories[0].transition'='Create'
	Then JSON 'elements[3].transition_histories[1].transition'='Start'
	Then JSON 'elements[3].transition_histories[2].transition'='Complete'
	Then JSON 'elements[3].state_histories[0].state'='Available'
	Then JSON 'elements[3].state_histories[1].state'='Active'
	Then JSON 'elements[3].state_histories[2].state'='Completed'

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
	And wait '100' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$insid$'
	And extract JSON from body
	And extract 'elements[0].id' from JSON body into 'eltid'
	And execute HTTP GET request 'http://localhost/case-instances/$insid$/suspend/$eltid$'
	And wait '100' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$insid$/resume/$eltid$'
	And wait '3000' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$insid$'
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
	And wait '100' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$insid$'
	And extract JSON from body
	And extract 'elements[0].id' from JSON body into 'eltid'
	And execute HTTP GET request 'http://localhost/case-instances/$insid$/suspend'
	And wait '100' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$insid$/reactivate'
	And wait '3000' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$insid$'
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

Scenario: Reactivate failed caseWithOneLongProcessTask and check his status is failed (scope = Case Instance)
	When execute HTTP GET request 'http://localhost/case-definitions/.search?cmmn_definition=caseWithOneFailProcessTask.cmmn'
	And extract JSON from body
	And extract 'content[0].id' from JSON body into 'defid'
	And execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value    |
	| case_definition_id | $defid$  |
	And extract JSON from body
	And extract 'id' from JSON body into 'insid'
	And execute HTTP GET request 'http://localhost/case-instances/$insid$/launch'	
	And wait '1000' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$insid$/reactivate'
	And wait '2000' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$insid$'
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
	And wait '100' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$insid$'
	And extract JSON from body
	And extract 'elements[0].id' from JSON body into 'eltid'
	And execute HTTP GET request 'http://localhost/case-instances/$insid$/terminate/$eltid$'
	And wait '3000' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$insid$'
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