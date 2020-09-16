Feature: CaseInstances
	Check result returned by /case-plan-instances
	
Scenario: Launch caseWithOneTask and check his status is completed
	When execute HTTP POST JSON request 'http://localhost/case-plans/search'
	| Key        | Value           |
	| casePlanId | CaseWithOneTask |
	And extract JSON from body
	And extract 'content[0].id' from JSON body into 'casePlanId'
	And execute HTTP POST JSON request 'http://localhost/case-plan-instances'
	| Key        | Value        |
	| casePlanId | $casePlanId$ |
	And extract JSON from body
	And extract 'id' from JSON body into 'casePlanInstanceId'	
	And execute HTTP GET request 'http://localhost/case-plan-instances/$casePlanInstanceId$/launch'
	And poll 'http://localhost/case-plan-instances/$casePlanInstanceId$', until 'state'='Completed'	
	And extract JSON from body
	
	Then HTTP status code equals to '200'
	Then JSON 'state'='Completed'

Scenario: Launch caseWithTwoStages and check his status is completed
	When execute HTTP POST JSON request 'http://localhost/case-plans/search'
	| Key        | Value             |
	| casePlanId | CaseWithTwoStages |
	And extract JSON from body
	And extract 'content[0].id' from JSON body into 'casePlanId'
	And execute HTTP POST JSON request 'http://localhost/case-plan-instances'
	| Key        | Value        |
	| casePlanId | $casePlanId$ |
	And extract JSON from body
	And extract 'id' from JSON body into 'casePlanInstanceId'	
	And execute HTTP GET request 'http://localhost/case-plan-instances/$casePlanInstanceId$/launch'
	And poll 'http://localhost/case-plan-instances/$casePlanInstanceId$', until 'state'='Completed'
	And extract JSON from body
	
	Then HTTP status code equals to '200'
	Then JSON 'state'='Completed'
	Then JSON 'children[0].state'='Completed'
	Then JSON 'children[1].state'='Completed'
	Then JSON 'children[2].state'='Completed'
	Then JSON 'children[3].state'='Completed'
	Then JSON 'children[4].state'='Completed'

Scenario: Launch caseWithOneHumanTask and check his status is completed
	When execute HTTP POST JSON request 'http://localhost/case-plans/search'
	| Key        | Value                |
	| casePlanId | CaseWithOneHumanTask |
	And extract JSON from body
	And extract 'content[0].id' from JSON body into 'casePlanId'
	And execute HTTP POST JSON request 'http://localhost/case-plan-instances'
	| Key        | Value        |
	| casePlanId | $casePlanId$ |
	And extract JSON from body
	And extract 'id' from JSON body into 'casePlanInstanceId'
	And execute HTTP GET request 'http://localhost/case-plan-instances/$casePlanInstanceId$/launch'
	And poll 'http://localhost/case-plan-instances/$casePlanInstanceId$', until 'state'='Active'
	And extract JSON from body
	And extract 'children[1].id' from JSON body into 'humanTaskId'
	And execute HTTP GET request 'http://localhost/case-plan-instances/$casePlanInstanceId$/complete/$humanTaskId$'
	And poll 'http://localhost/case-plan-instances/$casePlanInstanceId$', until 'state'='Completed'
	And extract JSON from body
	
	Then HTTP status code equals to '200'
	Then JSON 'state'='Completed'
	Then JSON 'children[0].state'='Completed'
	Then JSON 'children[1].state'='Completed'

Scenario: Launch caseWithOneManualActivationTask and check his status is completed
	When execute HTTP POST JSON request 'http://localhost/case-plans/search'
	| Key        | Value                           |
	| casePlanId | CaseWithOneManualActivationTask |
	And extract JSON from body
	And extract 'content[0].id' from JSON body into 'casePlanId'
	And execute HTTP POST JSON request 'http://localhost/case-plan-instances'
	| Key        | Value        |
	| casePlanId | $casePlanId$ |
	And extract JSON from body
	And extract 'id' from JSON body into 'casePlanInstanceId'
	And execute HTTP GET request 'http://localhost/case-plan-instances/$casePlanInstanceId$/launch'
	And poll 'http://localhost/case-plan-instances/$casePlanInstanceId$', until 'state'='Active'
	And extract JSON from body
	And extract 'children[1].id' from JSON body into 'emptyTaskId'
	And execute HTTP GET request 'http://localhost/case-plan-instances/$casePlanInstanceId$/activate/$emptyTaskId$'	
	And poll 'http://localhost/case-plan-instances/$casePlanInstanceId$', until 'state'='Completed'
	And extract JSON from body
	
	Then HTTP status code equals to '200'
	Then JSON 'state'='Completed'
	Then JSON 'children[0].state'='Completed'
	Then JSON 'children[1].state'='Completed'

Scenario: Launch caseWithRepetitionRule and check his status is completed
	When execute HTTP POST JSON request 'http://localhost/case-plans/search'
	| Key        | Value                  |
	| casePlanId | CaseWithRepetitionRule |
	And extract JSON from body
	And extract 'content[0].id' from JSON body into 'casePlanId'
	And execute HTTP POST JSON request 'http://localhost/case-plan-instances'
	| Key        | Value        |
	| casePlanId | $casePlanId$ |
	And extract JSON from body
	And extract 'id' from JSON body into 'casePlanInstanceId'
	And execute HTTP GET request 'http://localhost/case-plan-instances/$casePlanInstanceId$/launch'
	And poll 'http://localhost/case-plan-instances/$casePlanInstanceId$', until 'state'='Active'
	And extract JSON from body
	
	Then HTTP status code equals to '200'
	Then JSON 'state'='Active'
	Then JSON 'children[0].state'='Active'
	Then JSON 'children[1].state'='Completed'
	Then JSON 'children[2].name'='EmptyTask'

Scenario: Launch caseWithOneSEntry and check his status is completed
	When execute HTTP POST JSON request 'http://localhost/case-plans/search'
	| Key        | Value             |
	| casePlanId | caseWithOneSEntry |
	And extract JSON from body
	And extract 'content[0].id' from JSON body into 'casePlanId'
	And execute HTTP POST JSON request 'http://localhost/case-plan-instances'
	| Key        | Value                      |
	| casePlanId | $casePlanId$               |
	| parameters | { "action": "secondTask" } |
	And extract JSON from body
	And extract 'id' from JSON body into 'casePlanInstanceId'
	And execute HTTP GET request 'http://localhost/case-plan-instances/$casePlanInstanceId$/launch'
	And poll 'http://localhost/case-plan-instances/$casePlanInstanceId$', until 'children[2].state'='Completed'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'state'='Active'
	Then JSON 'children[1].state'='Completed'
	Then JSON 'children[2].state'='Completed'
	Then JSON 'children[3].state'='Available'

Scenario: Launch caseWithOneTimerEventListener and check his status is completed
	When execute HTTP POST JSON request 'http://localhost/case-plans/search'
	| Key        | Value                         |
	| casePlanId | CaseWithOneTimerEventListener |
	And extract JSON from body
	And extract 'content[0].id' from JSON body into 'casePlanId'
	And execute HTTP POST JSON request 'http://localhost/case-plan-instances'
	| Key        | Value        |
	| casePlanId | $casePlanId$ |
	And extract JSON from body
	And extract 'id' from JSON body into 'casePlanInstanceId'
	And execute HTTP GET request 'http://localhost/case-plan-instances/$casePlanInstanceId$/launch'
	And poll 'http://localhost/case-plan-instances/$casePlanInstanceId$', until 'children[1].state'='Completed'
	And extract JSON from body
	
	Then HTTP status code equals to '200'
	Then JSON 'state'='Active'
	Then JSON 'children[1].state'='Completed'