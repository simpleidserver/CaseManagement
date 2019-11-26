Feature: CaseInstances
	Check result returned by /case-instances

Scenario: Launch firstDiagram case instance
	When execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value			|
	| case_definition_id | firstDiagramm    |
	| case_id            | Case_1ey12wl		|
	
	And extract JSON from body
	And extract 'id' from JSON body
	And execute HTTP GET request 'http://localhost/case-instances/$id$/launch'
	And execute HTTP POST JSON request 'http://localhost/case-instances/$id$/confirm/PlanItem_1iqs5hf'
	| Key | Value |
	And execute HTTP GET request 'http://localhost/case-instances/$id$'
	And extract JSON from body
	
	Then HTTP status code equals to '200'
	Then JSON 'status'='completed'