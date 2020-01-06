Feature: CaseInstanceErrors
	Check errors returned by /case-instances

Scenario: Create unknown case definition and check error is returned	
	When execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value |
	| case_definition_id | 1     |
	And extract JSON from body

	Then HTTP status code equals to '404'
	Then JSON 'errors.bad_request[0]'='case definition doesn't exist'

Scenario: Launch unknown case instance and check error is returned
	When execute HTTP GET request 'http://localhost/case-instances/1/launch'	
	And extract JSON from body
	
	Then HTTP status code equals to '404'
	Then JSON 'errors.bad_request[0]'='case instance doesn't exist'

Scenario: Get unknown case instance and check error is returned
	When execute HTTP GET request 'http://localhost/case-instances/1'
	And extract JSON from body
	
	Then HTTP status code equals to '404'

Scenario: Suspend unknown case instance and check error is returned
	When execute HTTP GET request 'http://localhost/case-instances/1/suspend'
	And extract JSON from body
	
	Then HTTP status code equals to '404'
	Then JSON 'errors.bad_request[0]'='case instance doesn't exist'

Scenario: Suspend unknown case element instance and check error is returned
	When execute HTTP GET request 'http://localhost/case-definitions/.search?cmmn_definition=caseWithOneTask.cmmn'
	And extract JSON from body
	And extract 'content[0].id' from JSON body into 'defid'
	And execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value    |
	| case_definition_id | $defid$  |
	And extract JSON from body
	And extract 'id' from JSON body into 'insid'
	And execute HTTP GET request 'http://localhost/case-instances/$insid$/suspend/1'
	And extract JSON from body
	
	Then HTTP status code equals to '404'
	Then JSON 'errors.bad_request[0]'='case instance element doesn't exist'

Scenario: Reactivate unknown case instance and check error is returned
	When execute HTTP GET request 'http://localhost/case-instances/1/reactivate'
	And extract JSON from body
	
	Then HTTP status code equals to '404'
	Then JSON 'errors.bad_request[0]'='case instance doesn't exist'

Scenario: Reactivate unknown case element instance and check error is returned
	When execute HTTP GET request 'http://localhost/case-definitions/.search?cmmn_definition=caseWithOneTask.cmmn'
	And extract JSON from body
	And extract 'content[0].id' from JSON body into 'defid'
	And execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value    |
	| case_definition_id | $defid$  |
	And extract JSON from body
	And extract 'id' from JSON body into 'insid'
	And execute HTTP GET request 'http://localhost/case-instances/$insid$/reactivate/1'
	And extract JSON from body
	
	Then HTTP status code equals to '404'
	Then JSON 'errors.bad_request[0]'='case instance element doesn't exist'

Scenario: Reactivate not failed case instance and check error is returned
	When execute HTTP GET request 'http://localhost/case-definitions/.search?cmmn_definition=caseWithOneTask.cmmn'
	And extract JSON from body
	And extract 'content[0].id' from JSON body into 'defid'
	And execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value    |
	| case_definition_id | $defid$  |
	And extract JSON from body
	And extract 'id' from JSON body into 'insid'
	And execute HTTP GET request 'http://localhost/case-instances/$insid$/reactivate'
	And extract JSON from body
	
	Then HTTP status code equals to '400'
	Then JSON 'errors.transition[0]'='case instance is not completed / terminated / failed / suspended'
