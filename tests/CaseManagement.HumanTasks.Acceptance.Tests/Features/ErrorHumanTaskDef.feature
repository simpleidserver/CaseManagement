Feature: ErrorHumanTaskDef
	Check errors returned by /humantasksdefs
	
Scenario: Check error is returned when trying to create task with missing name
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key           | Value       |
	And extract JSON from body

	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='Parameter 'name' is missing'

Scenario: Check error is returned when trying to add humantaskdef with same name
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value |
	| name | name  |
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value |
	| name | name  |
	And extract JSON from body

	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='The human task def 'name' already exists'

Scenario: Check error is returned when trying to get an unknown humantaskdef
	When execute HTTP GET request 'http://localhost/humantasksdefs/def'
	And extract JSON from body

	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'def''

Scenario: Check error is returned when trying to update information and no name parameter is passed
	When execute HTTP PUT JSON request 'http://localhost/humantasksdefs/id/info'
	| Key | Value |
	And extract JSON from body

	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='Parameter 'name' is missing'

Scenario: Check error is returned when trying to update information of an unknown humantaskdef
	When execute HTTP PUT JSON request 'http://localhost/humantasksdefs/id/info'
	| Key  | Value |
	| name | name  |
	And extract JSON from body

	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'id''

Scenario: Check error is returned when trying to update information with an existing name
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value |
	| name | n1    |
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value |
	| name | n2    |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP PUT JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/info'
	| Key  | Value |
	| name | n1    |
	And extract JSON from body

	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='The human task def 'n1' already exists'

Scenario: Check error is returned when trying to add parameter and parameter is missing
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value           |
	| name | inputParameter1 |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/parameters'
	| Key  | Value |
	And extract JSON from body
	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='Parameter 'parameter' is missing'

Scenario: Check error is returned when trying to add already existing parameter
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value           |
	| name | inputParameter2 |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/parameters'
	| Key       | Value                                                 |
	| parameter | { name: 'parameter', type: 'STRING', usage: 'INPUT' } |
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/parameters'
	| Key       | Value                                                 |
	| parameter | { name: 'parameter', type: 'STRING', usage: 'INPUT' } |
	And extract JSON from body
	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.validation[0]'='Operation parameter 'parameter' already exists'
	
Scenario: Check error is returned when trying to add parameter to unknown humantaskdef
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs/def/parameters'
	| Key  | Value |
	And extract JSON from body
	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'def''

Scenario: Check error is returned when trying to add output parameter and parameter is missing
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value           |
	| name | outputParameter1 |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/parameters'
	| Key  | Value |
	And extract JSON from body
	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='Parameter 'parameter' is missing'

Scenario: Check error is returned when trying to delete input parameter from unknown humantaskdef
	When execute HTTP DELETE request 'http://localhost/humantasksdefs/def/parameters/name'
	And extract JSON from body
	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'def''

Scenario: Check error is returned when trying to delete unknown input parameter
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value            |
	| name | deleteParameter1 |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP DELETE request 'http://localhost/humantasksdefs/$humanTaskDefId$/parameters/name'
	And extract JSON from body
	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.validation[0]'='Operation parameter 'name' doesn't exist'

Scenario: Check error is returned when trying to delete parameter from unknown humantaskdef
	When execute HTTP DELETE request 'http://localhost/humantasksdefs/def/parameters/name'
	And extract JSON from body
	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'def''

Scenario: Check error is returned when trying to delete unknown parameter
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value            |
	| name | deleteParameter2 |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP DELETE request 'http://localhost/humantasksdefs/$humanTaskDefId$/parameters/name'
	And extract JSON from body
	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.validation[0]'='Operation parameter 'name' doesn't exist'

Scenario: Check error is returned when trying to add deadline and 'deadLine' parameter is missing
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs/id/deadlines'
	| Key | Value |
	And extract JSON from body

	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='Parameter 'deadLine' is missing'

Scenario: Check error is returned when trying to add deadline and humantaskdef is missing
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs/id/deadlines'
	| Key      | Value            |
	| deadLine | { name: "Name" } |
	And extract JSON from body
	
	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'id''

Scenario: Check error is returned when trying to add deadline and deadline name is missing
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value                      |
	| name | addStartDeadlineParameter1 |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines'
	| Key      | Value                |
	| deadLine | { 'usage': 'START' } |
	And extract JSON from body
	
	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.validation[0]'='Parameter 'deadline.name' is missing'

Scenario: Check error is returned when trying to add deadline and for & until parameters are missing
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value                      |
	| name | addStartDeadlineParameter2 |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines'
	| Key      | Value                              |
	| deadLine | { name: "name", "usage": "START" } |
	And extract JSON from body
	
	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.validation[0]'='Parameter 'deadline.for,deadline.until' is missing'

Scenario: Check error is returned when trying to add deadline and for & until parameters are present
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value                      |
	| name | addStartDeadlineParameter3 |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines'
	| Key      | Value                                                          |
	| deadLine | { name: "name", for: "for", until: "until", "usage": "START" } |
	And extract JSON from body
	
	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.validation[0]'='Parameters 'deadline.for,deadline.until' cannot be specified at the same time'

Scenario: Check error is returned when trying to add deadline and until is not a valid ISO8601 expression
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value                      |
	| name | addStartDeadlineParameter4 |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines'
	| Key      | Value                                              |
	| deadLine | { name: "name", until: "until", "usage": "START" } |
	And extract JSON from body
	
	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.validation[0]'='Parameter 'deadline.until' is not a valid ISO8601 expression'

Scenario: Check error is returned when trying to remove deadline from unknown humantask definition
	When execute HTTP DELETE request 'http://localhost/humantasksdefs/def/deadlines/deadLineId'
	And extract JSON from body

	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'def''

Scenario: Check error is returned when trying to remove unknown deadline
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value                   |
	| name | startDeadlineParameter1 |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP DELETE request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/deadLineId'
	And extract JSON from body

	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.validation[0]'='Deadline doesn't exist'

Scenario: Check error is returned when trying to update deadline and parameter is missing
	When execute HTTP PUT JSON request 'http://localhost/humantasksdefs/def/deadlines/deadLineId'
	| Key | Value |
	And extract JSON from body

	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='Parameter 'deadLineInfo' is missing'

Scenario: Check error is returned when trying to update deadline and humantaskdef is unknown
	When execute HTTP PUT JSON request 'http://localhost/humantasksdefs/def/deadlines/deadLineId'
	| Key          | Value                              |
	| deadLineInfo | { name: "name", "usage": "START" } |
	And extract JSON from body
	
	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'def''

Scenario: Check error is returned when trying to add deadline escalation and the condition parameter is missing
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs/def/deadlines/deadLineId/escalations'
	| Key          | Value            |
	And extract JSON from body

	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='Parameter 'condition' is missing'


Scenario: Check error is returned when trying to add deadline escalation and the humantask is unknown
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs/def/deadlines/deadLineId/escalations'
	| Key       | Value |
	| condition | true  |
	And extract JSON from body
	
	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'def''

Scenario: Check error is returned when trying to remove escalation from unknown humantask definition
	When execute HTTP DELETE request 'http://localhost/humantasksdefs/def/deadlines/deadLineId/escalations/escId'
	And extract JSON from body

	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'def''

Scenario: Check error is returned when trying to remove escalation from unknown deadline
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value                      |
	| name | removeEscalationParameter1 |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP DELETE request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/deadLineId/escalations/escId'
	And extract JSON from body
	
	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.validation[0]'='Unknown deadline'

Scenario: Check error is returned when trying to remove unknown escalation
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value                      |
	| name | removeEscalationParameter2 |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines'
	| Key      | Value                                                       |
	| deadLine | { name: "name", until: "P0Y0M0DT0H0M2S", "usage": "START" } |
	And extract JSON from body
	And extract 'id' from JSON body into 'deadLineId'
	And execute HTTP DELETE request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/$deadLineId$/escalations/escId'
	And extract JSON from body
	
	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.validation[0]'='Unknown escalation'

Scenario: Check error is returned when trying to remove escalation from deadline
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value                      |
	| name | removeEscalationParameter3 |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP DELETE request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/deadLineId/escalations/escId'
	And extract JSON from body
	
	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.validation[0]'='Unknown deadline'

Scenario: Check error is returned when trying to update rendering and parameter rendering is missing
	When execute HTTP PUT JSON request 'http://localhost/humantasksdefs/def/rendering'
	| Key        | Value                  |
	And extract JSON from body

	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='Parameter 'rendering' is missing'

Scenario: Check error is returned when trying to update rendering and humantaskdef is missing
	When execute HTTP PUT JSON request 'http://localhost/humantasksdefs/def/rendering'
	| Key       | Value                   |
	| rendering | { "type": "container" } |
	And extract JSON from body

	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'def''