Feature: CasePlanInstanceErrors
	Check errors returned by /case-plan-instances

Scenario: Create unknown case definition and check error is returned	
	When execute HTTP POST JSON request 'http://localhost/case-plan-instances'
	| Key        | Value |
	| casePlanId | 1     |
	And extract JSON from body

	Then HTTP status code equals to '404'
	Then JSON 'errors.bad_request[0]'='case definition doesn't exist'

Scenario: Launch unknown case instance and check error is returned
	When execute HTTP GET request 'http://localhost/case-plan-instances/1/launch'	
	And extract JSON from body
	
	Then HTTP status code equals to '404'
	Then JSON 'errors.bad_request[0]'='case instance doesn't exist'

Scenario: Get unknown case instance and check error is returned
	When execute HTTP GET request 'http://localhost/case-plan-instances/1'
	And extract JSON from body
	
	Then HTTP status code equals to '404'

Scenario: Suspend unknown case instance and check error is returned
	When execute HTTP GET request 'http://localhost/case-plan-instances/1/suspend'
	And extract JSON from body
	
	Then HTTP status code equals to '404'
	Then JSON 'errors.bad_request[0]'='case instance doesn't exist'