Feature: RoleErrors
	Check errors returned by /roles

Scenario: Delete unknown role
	When execute HTTP DELETE request 'http://localhost/roles/role'
	And extract JSON from body
	
	Then HTTP status code equals to '404'

Scenario: Update unknown role
	When execute HTTP PUT JSON request 'http://localhost/roles/role'
	| Key   | Value     |
	| users | ['user1'] |

	And extract JSON from body
	
	Then HTTP status code equals to '404'

Scenario: Get unknown role
	When execute HTTP GET request 'http://localhost/roles/role'
	And extract JSON from body
	
	Then HTTP status code equals to '404'