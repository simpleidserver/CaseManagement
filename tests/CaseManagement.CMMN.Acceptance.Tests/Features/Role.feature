Feature: Role
	Check errors returned by /roles

Scenario: Add and update role
	When execute HTTP POST JSON request 'http://localhost/roles'
	| Key  | Value     |
	| role | otherrole |

	And execute HTTP PUT JSON request 'http://localhost/roles/otherrole'
	| Key   | Value    |
	| users | ['user'] |

	And execute HTTP GET request 'http://localhost/roles/otherrole'
	And extract JSON from body
	And execute HTTP DELETE request 'http://localhost/roles/otherrole'	
	
	Then JSON 'id'='otherrole'
	Then JSON 'users' contains 'user'