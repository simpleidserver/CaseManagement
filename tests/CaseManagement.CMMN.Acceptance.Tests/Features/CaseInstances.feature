Feature: CaseInstances
	Check result returned by /case-instances

Scenario: Launch sEntryWithCondition case instance and check his status is completed
	When execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value               |
	| case_definition_id | sEntryWithCondition |
	| case_id            | Case_1ey12wl        |
	
	And extract JSON from body
	And extract 'id' from JSON body
	And execute HTTP GET request 'http://localhost/case-instances/$id$/launch'
	And execute HTTP GET request 'http://localhost/case-instances/$id$'
	And extract JSON from body
	
	Then HTTP status code equals to '200'
	Then JSON 'status'='completed'
	Then JSON 'items[0].status'='finished'
	Then JSON 'items[1].status'='finished'
