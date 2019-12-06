Feature: CaseInstanceErrors
	Check errors returned by /case-instances

Scenario: Check errors are returned when submit form and case-instance doesn't exist
	When execute HTTP POST JSON request 'http://localhost/case-instances/unknown/confirm/PI_ProcessTask_1'
	| Key | Value |
	And extract JSON from body
	
	Then HTTP status code equals to '404'
	Then JSON 'errors.bad_request[0]'='case instance doesn't exist'

Scenario: Check errors are returned when submit form and case-instance-element doesn't exist
	When execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value               |
	| case_definition_id | caseWithHumanTask   |
	| case_id            | testCase			   |
	And extract JSON from body
	And extract 'id' from JSON body
	And wait '15' seconds
	And execute HTTP POST JSON request 'http://localhost/case-instances/$id$/confirm/ProcessTask'
	| Key | Value |
	And extract JSON from body
	
	Then HTTP status code equals to '404'
	Then JSON 'errors.bad_request[0]'='case instance element doesn't exist'


Scenario: Check errors are returned when submit form with missing required fields
	When execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value               |
	| case_definition_id | caseWithHumanTask   |
	| case_id            | testCase			   |

	And extract JSON from body
	And extract 'id' from JSON body
	And execute HTTP GET request 'http://localhost/case-instances/$id$/launch'
	And wait '15' seconds
	And execute HTTP POST JSON request 'http://localhost/case-instances/$id$/confirm/PI_ProcessTask_1'
	| Key | Value |
	And extract JSON from body

	Then HTTP status code equals to '400'
	Then JSON 'errors.validation_error[0]'='field name is required'