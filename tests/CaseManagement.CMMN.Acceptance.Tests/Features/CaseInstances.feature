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
	And wait '3600' seconds