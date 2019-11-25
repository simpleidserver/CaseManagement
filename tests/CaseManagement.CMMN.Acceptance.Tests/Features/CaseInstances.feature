Feature: CaseInstances
	Check result returned by /case-instances
		
Scenario: Check case instance can be created
	When execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value        |
	| case_definition_id | Test         |
	| case_id            | Case_1ey12wl |

	And extract JSON from body
	
	Then HTTP status code equals to '201'
	Then JSON contains 'id'