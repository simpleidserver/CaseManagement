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

Scenario: Check error is returned when trying to update people assignment
	When execute HTTP PUT JSON request 'http://localhost/humantasksdefs/def/assignment'
	| Key  | Value |
	And extract JSON from body
	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'def''

Scenario: Check error is returned when trying to add input parameter to unknown humantaskdef
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs/def/parameters/input'
	| Key  | Value |
	And extract JSON from body
	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'def''

Scenario: Check error is returned when trying to add input parameter and parameter is missing
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value           |
	| name | inputParameter1 |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/parameters/input'
	| Key  | Value |
	And extract JSON from body
	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='Parameter 'parameter' is missing'

Scenario: Check error is returned when trying to add already existing input parameter
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value           |
	| name | inputParameter2 |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/parameters/input'
	| Key       | Value                                 |
	| parameter | { name: 'parameter', type: 'STRING' } |
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/parameters/input'
	| Key       | Value                                 |
	| parameter | { name: 'parameter', type: 'STRING' } |
	And extract JSON from body
	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='Input parameter 'parameter' already exists'
	
Scenario: Check error is returned when trying to add output parameter to unknown humantaskdef
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs/def/parameters/output'
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
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/parameters/output'
	| Key  | Value |
	And extract JSON from body
	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='Parameter 'parameter' is missing'

Scenario: Check error is returned when trying to add already existing output parameter
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value           |
	| name | outputParameter2 |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/parameters/output'
	| Key       | Value                                 |
	| parameter | { name: 'parameter', type: 'STRING' } |
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/parameters/output'
	| Key       | Value                                 |
	| parameter | { name: 'parameter', type: 'STRING' } |
	And extract JSON from body
	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='Output parameter 'parameter' already exists'

Scenario: Check error is returned when trying to delete input parameter from unknown humantaskdef
	When execute HTTP DELETE request 'http://localhost/humantasksdefs/def/parameters/input/name'
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
	And execute HTTP DELETE request 'http://localhost/humantasksdefs/$humanTaskDefId$/parameters/input/name'
	And extract JSON from body
	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='Input parameter 'parameter' doesn't exist'

Scenario: Check error is returned when trying to delete ouput parameter from unknown humantaskdef
	When execute HTTP DELETE request 'http://localhost/humantasksdefs/def/parameters/output/name'
	And extract JSON from body
	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'def''

Scenario: Check error is returned when trying to delete unknown output parameter
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value            |
	| name | deleteParameter2 |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP DELETE request 'http://localhost/humantasksdefs/$humanTaskDefId$/parameters/output/name'
	And extract JSON from body
	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='Output parameter 'parameter' doesn't exist'

Scenario: Check error is returned when trying to update presentation element and 'presentationElements' parameter is missing
	When execute HTTP PUT JSON request 'http://localhost/humantasksdefs/id/presentationelts'
	| Key  | Value            |
	And extract JSON from body
	
	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='Parameter 'presentationElements' is missing'

Scenario: Check error is returned when trying to update presentation element of an unknown humantaskdef
	When execute HTTP PUT JSON request 'http://localhost/humantasksdefs/id/presentationelts'
	| Key                    | Value                                                      |
	| presentationElements   | [ { usage: "SUBJECT", language: "fr", value: "bonjour" } ] |
	| presentationParameters | [ ]                                                        |
	And extract JSON from body
	
	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'id''

Scenario: Check error is returned when trying to add start deadline and 'deadLine' parameter is missing
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs/id/deadlines/start'
	| Key | Value |
	And extract JSON from body

	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='Parameter 'deadLine' is missing'

Scenario: Check error is returned when trying to add start deadline and humantaskdef is missing
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs/id/deadlines/start'
	| Key      | Value            |
	| deadLine | { name: "Name" } |
	And extract JSON from body
	
	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'id''

Scenario: Check error is returned when trying to add start deadline and deadline name is missing
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value                      |
	| name | addStartDeadlineParameter1 |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/start'
	| Key      | Value |
	| deadLine | { }   |
	And extract JSON from body
	
	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.validation[0]'='Parameter 'deadline.name' is missing'

Scenario: Check error is returned when trying to add start deadline and for & until parameters are missing
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value                      |
	| name | addStartDeadlineParameter2 |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/start'
	| Key      | Value            |
	| deadLine | { name: "name" } |
	And extract JSON from body
	
	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.validation[0]'='Parameter 'deadline.for,deadline.until' is missing'

Scenario: Check error is returned when trying to add start deadline and for & until parameters are present
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value                      |
	| name | addStartDeadlineParameter3 |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/start'
	| Key      | Value                                        |
	| deadLine | { name: "name", for: "for", until: "until" } |
	And extract JSON from body
	
	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.validation[0]'='Parameters 'deadline.for,deadline.until' cannot be specified at the same time'

Scenario: Check error is returned when trying to add start deadline and until is not a valid ISO8601 expression
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value                      |
	| name | addStartDeadlineParameter4 |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/start'
	| Key      | Value                            |
	| deadLine | { name: "name", until: "until" } |
	And extract JSON from body
	
	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.validation[0]'='Parameter 'deadline.until' is not a valid ISO8601 expression'

Scenario: Check error is returned when trying to remove start deadline from unknown humantask definition
	When execute HTTP DELETE request 'http://localhost/humantasksdefs/def/deadlines/start/deadLineId'
	And extract JSON from body

	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'def''

Scenario: Check error is returned when trying to remove unknown start deadline
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value                   |
	| name | startDeadlineParameter1 |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP DELETE request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/start/deadLineId'
	And extract JSON from body

	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.validation[0]'='Start deadline doesn't exist'
	
Scenario: Check error is returned when trying to remove completion deadline from unknown humantask definition
	When execute HTTP DELETE request 'http://localhost/humantasksdefs/def/deadlines/completion/deadLineId'
	And extract JSON from body

	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'def''

Scenario: Check error is returned when trying to remove unknown completion deadline
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value                   |
	| name | startDeadlineParameter2 |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP DELETE request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/completion/deadLineId'
	And extract JSON from body

	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.validation[0]'='Completion deadline doesn't exist'

Scenario: Check error is returned when trying to update start deadline and parameter is missing
	When execute HTTP PUT JSON request 'http://localhost/humantasksdefs/def/deadlines/start/deadLineId'
	| Key | Value |
	And extract JSON from body

	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='Parameter 'deadLineInfo' is missing'

Scenario: Check error is returned when trying to update start deadline and humantaskdef is unknown
	When execute HTTP PUT JSON request 'http://localhost/humantasksdefs/def/deadlines/start/deadLineId'
	| Key          | Value            |
	| deadLineInfo | { name: "name" } |
	And extract JSON from body
	
	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'def''
	

Scenario: Check error is returned when trying to update completion deadline and parameter is missing
	When execute HTTP PUT JSON request 'http://localhost/humantasksdefs/def/deadlines/completion/deadLineId'
	| Key | Value |
	And extract JSON from body

	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='Parameter 'deadLineInfo' is missing'

Scenario: Check error is returned when trying to update completion deadline and humantaskdef is unknown
	When execute HTTP PUT JSON request 'http://localhost/humantasksdefs/def/deadlines/completion/deadLineId'
	| Key          | Value            |
	| deadLineInfo | { name: "name" } |
	And extract JSON from body
	
	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'def''

Scenario: Check error is returned when trying to add start deadline escalation and the condition parameter is missing
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs/def/deadlines/start/deadLineId/escalations'
	| Key          | Value            |
	And extract JSON from body

	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='Parameter 'condition' is missing'


Scenario: Check error is returned when trying to add start deadline escalation and the humantask is unknown
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs/def/deadlines/start/deadLineId/escalations'
	| Key       | Value |
	| condition | true  |
	And extract JSON from body
	
	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'def''

Scenario: Check error is returned when trying to add completion deadline escalation and the condition parameter is missing
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs/def/deadlines/completion/deadLineId/escalations'
	| Key          | Value            |
	And extract JSON from body

	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='Parameter 'condition' is missing'


Scenario: Check error is returned when trying to add completion deadline escalation and the humantask is unknown
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs/def/deadlines/completion/deadLineId/escalations'
	| Key       | Value |
	| condition | true  |
	And extract JSON from body
	
	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'def''

Scenario: Check error is returned when trying to remove escalation (start deadline) from unknown humantask definition
	When execute HTTP DELETE request 'http://localhost/humantasksdefs/def/deadlines/start/deadLineId/escalations/escId'
	And extract JSON from body

	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'def''

Scenario: Check error is returned when trying to remove escalation from unknown start deadline
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value                      |
	| name | removeEscalationParameter1 |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP DELETE request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/start/deadLineId/escalations/escId'
	And extract JSON from body
	
	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.validation[0]'='Unknown start deadline'

Scenario: Check error is returned when trying to remove unknown escalation (start deadline)
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value                      |
	| name | removeEscalationParameter2 |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/start'
	| Key      | Value                                     |
	| deadLine | { name: "name", until: "P0Y0M0DT0H0M2S" } |
	And extract JSON from body
	And extract 'id' from JSON body into 'deadLineId'
	And execute HTTP DELETE request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/start/$deadLineId$/escalations/escId'
	And extract JSON from body
	
	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.validation[0]'='Unknown escalation'
	

Scenario: Check error is returned when trying to remove escalation (completion deadline) from unknown humantask definition
	When execute HTTP DELETE request 'http://localhost/humantasksdefs/def/deadlines/completion/deadLineId/escalations/escId'
	And extract JSON from body

	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'def''

Scenario: Check error is returned when trying to remove escalation from unknown completion deadline
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value                      |
	| name | removeEscalationParameter3 |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP DELETE request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/completion/deadLineId/escalations/escId'
	And extract JSON from body
	
	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.validation[0]'='Unknown completion deadline'

Scenario: Check error is returned when trying to remove unknown escalation (completion deadline)
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value                      |
	| name | removeEscalationParameter4 |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/completion'
	| Key      | Value                                     |
	| deadLine | { name: "name", until: "P0Y0M0DT0H0M2S" } |
	And extract JSON from body
	And extract 'id' from JSON body into 'deadLineId'
	And execute HTTP DELETE request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/completion/$deadLineId$/escalations/escId'
	And extract JSON from body
	
	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.validation[0]'='Unknown escalation'

Scenario: Check error is returned when trying to update escalation of a start deadline and escalation parameter is missing
	When execute HTTP PUT JSON request 'http://localhost/humantasksdefs/def/deadlines/start/deadLineId/escalations/escalationId'
	| Key           | Value       |
	And extract JSON from body

	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='Parameter 'escalation' is missing'

Scenario: Check error is returned when trying to update escalation of a start deadline and humantask definition is unknown
	When execute HTTP PUT JSON request 'http://localhost/humantasksdefs/def/deadlines/start/deadLineId/escalations/escalationId'
	| Key        | Value                  |
	| escalation | { condition: "false" } |
	And extract JSON from body

	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'def''

Scenario: Check error is returned when trying to update escalation of a completion deadline and escalation parameter is missing
	When execute HTTP PUT JSON request 'http://localhost/humantasksdefs/def/deadlines/completion/deadLineId/escalations/escalationId'
	| Key           | Value       |
	And extract JSON from body

	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='Parameter 'escalation' is missing'

Scenario: Check error is returned when trying to update escalation of a completion deadline and humantask definition is unknown
	When execute HTTP PUT JSON request 'http://localhost/humantasksdefs/def/deadlines/completion/deadLineId/escalations/escalationId'
	| Key        | Value                  |
	| escalation | { condition: "false" } |
	And extract JSON from body

	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'def''

Scenario: Check error is returned when trying to update rendering and parameter rendering is missing
	When execute HTTP PUT JSON request 'http://localhost/humantasksdefs/def/rendering'
	| Key        | Value                  |
	And extract JSON from body

	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='Parameter 'renderingelements' is missing'

Scenario: Check error is returned when trying to update rendering and humantaskdef is missing
	When execute HTTP PUT JSON request 'http://localhost/humantasksdefs/def/rendering'
	| Key               | Value                 |
	| renderingElements | [ { xPath: 'xpath' }] |
	And extract JSON from body

	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'def''