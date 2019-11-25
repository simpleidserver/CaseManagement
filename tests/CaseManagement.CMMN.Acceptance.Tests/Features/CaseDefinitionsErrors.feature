Feature: CaseDefinitionsErrors
	Check errors returned by /case-definitions
		
Scenario: Check error is returned when case-definition doesn't exist
	When execute HTTP GET request 'http://localhost/case-definitions/invalid'

	Then HTTP status code equals to '404'