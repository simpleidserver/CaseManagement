Feature: CaseInstances
	Check result returned by /case-instances

Scenario: Launch sEntryWithCondition case instance and check his status is completed
	When execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value               |
	| case_definition_id | sEntryWithCondition |
	| case_id            | Case_1ey12wl        |
	
	And extract JSON from body
	And extract 'id' from JSON body
	And execute HTTP GET request 'http://localhost/case-instances/$id$/launch'
	And wait '5' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$id$'
	And extract JSON from body
	
	Then HTTP status code equals to '200'
	Then JSON 'status'='completed'
	Then JSON 'items[0].status'='finished'
	Then JSON 'items[1].status'='finished'

Scenario: Launch caseWithProcessTask case instance and check his status is completed
	When execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value               |
	| case_definition_id | caseWithProcessTask |
	| case_id            | testCase			   |
	
	And extract JSON from body
	And extract 'id' from JSON body
	And execute HTTP GET request 'http://localhost/case-instances/$id$/launch'
	And wait '5' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$id$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'status'='completed'
	Then JSON 'items[0].status'='finished'
	Then JSON 'items[1].status'='finished'
	Then JSON 'context.processName'='firstTestProcess'
	Then JSON 'context.processTaskValue'='value value value'

Scenario: Launch caseWithHumanTask case instance and check his status is completed
	When execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value               |
	| case_definition_id | caseWithHumanTask   |
	| case_id            | testCase			   |

	And extract JSON from body
	And extract 'id' from JSON body
	And execute HTTP GET request 'http://localhost/case-instances/$id$/launch'
	And wait '1' seconds
	And execute HTTP POST JSON request 'http://localhost/case-instances/$id$/confirm/PI_ProcessTask_1'
	| Key  | Value |	
	| name | name  |
	And wait '5' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$id$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'status'='completed'
	
Scenario: Launch caseWithHumanTaskAndRole and check his status is completed
	When execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value					|
	| case_definition_id | caseWithHumanTaskAndRole	|
	| case_id            | testCase					|
	
	And extract JSON from body
	And extract 'id' from JSON body
	And execute HTTP GET request 'http://localhost/case-instances/$id$/launch'
	And wait '1' seconds
	And execute HTTP POST JSON request 'http://localhost/case-instances/$id$/confirm/PI_ProcessTask_1'
	| Key  | Value |
	| name | name  |
	And wait '5' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$id$'
	And extract JSON from body
	
	Then HTTP status code equals to '200'
	Then JSON 'status'='completed'

Scenario: Launch caseWithTimerEventListener case instance and check his status is completed
	When execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value                      |
	| case_definition_id | caseWithTimerEventListener |
	| case_id            | Case_0d1ujq8               |

	And extract JSON from body
	And extract 'id' from JSON body
	And execute HTTP GET request 'http://localhost/case-instances/$id$/launch'
	And wait '15' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$id$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'status'='completed'

Scenario: Launch caseWithTimerEventListener case instance, stop the instance and check is status in cancelled
	When execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value                      |
	| case_definition_id | caseWithTimerEventListener |
	| case_id            | Case_0d1ujq8               |

	And extract JSON from body
	And extract 'id' from JSON body
	And execute HTTP GET request 'http://localhost/case-instances/$id$/launch'
	And wait '1' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$id$/stop'
	And wait '5' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$id$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'status'='cancelled'

Scenario: Launch caseWithTimerEventListener case instance and relaunch the case instance
	When execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value                      |
	| case_definition_id | caseWithTimerEventListener |
	| case_id            | Case_0d1ujq8               |

	And extract JSON from body
	And extract 'id' from JSON body
	And execute HTTP GET request 'http://localhost/case-instances/$id$/launch'
	And wait '1' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$id$/stop'
	And wait '5' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$id$/launch'
	And wait '5' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$id$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'status'='completed'
	Then JSON 'items[0].status'='finished'
	Then JSON 'items[1].status'='finished'

Scenario: Launch caseWithMilestone case instance and check his status is completed
	When execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value             |
	| case_definition_id | caseWithMilestone |
	| case_id            | Case_1ey12wl		 |

	And extract JSON from body
	And extract 'id' from JSON body
	And execute HTTP GET request 'http://localhost/case-instances/$id$/launch'
	And wait '5' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$id$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'status'='completed'

Scenario: Launch caseWithMilestoneAndOneRepetitionRule case instance and check his status is completed
	When execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value                                 |
	| case_definition_id | caseWithMilestoneAndOneRepetitionRule |
	| case_id            | Case_1ey12wl                          |

	And extract JSON from body
	And extract 'id' from JSON body
	And execute HTTP GET request 'http://localhost/case-instances/$id$/launch'
	And wait '5' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$id$'
	And extract JSON from body into 'caseInstance'
	And execute HTTP GET request 'http://localhost/case-instances/$id$/steps/.search'
	And extract JSON from body into 'executionSteps'

	Then extract JSON 'caseInstance', JSON 'status'='completed'
	Then extract JSON 'caseInstance', JSON 'context.nbTasks'='3'
	Then extract JSON 'caseInstance', JSON 'items[0].version'='3'
	Then extract JSON 'caseInstance', JSON 'items[1].version'='3'
	Then extract JSON 'caseInstance', JSON 'items[0].status'='finished'
	Then extract JSON 'caseInstance', JSON 'items[1].status'='finished'
	Then extract JSON 'executionSteps', JSON 'content[0].id'='PlanItem_1twjtol'
	Then extract JSON 'executionSteps', JSON exists 'content[0].start_datetime'
	Then extract JSON 'executionSteps', JSON exists 'content[0].end_datetime'
	Then extract JSON 'executionSteps', JSON 'content[0].histories[0].transition'='create'
	Then extract JSON 'executionSteps', JSON 'content[0].histories[0].transition'='start'
	Then extract JSON 'executionSteps', JSON 'content[0].histories[0].transition'='complete'
	Then extract JSON 'executionSteps', JSON 'content[1].id'='PlanItem_1iqs5hf'
	Then extract JSON 'executionSteps', JSON 'content[1].histories[0].transition'='create'
	Then extract JSON 'executionSteps', JSON 'content[1].histories[0].transition'='occur'
	Then extract JSON 'executionSteps', JSON exists 'content[1].start_datetime'
	Then extract JSON 'executionSteps', JSON exists 'content[1].end_datetime'

Scenario: Launch caseWithLongProcessTask case instance and stop case instance
	When execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value                   |
	| case_definition_id | caseWithLongProcessTask |
	| case_id            | testCase                |

	And extract JSON from body
	And extract 'id' from JSON body
	And execute HTTP GET request 'http://localhost/case-instances/$id$/launch'
	And wait '1' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$id$/stop'
	And wait '5' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$id$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'status'='cancelled'
	Then JSON 'items[0].status'='finished'
	Then JSON 'items[1].status'='cancelled'

Scenario: Launch caseWithLongProcessTask case instance and relaunch the case instance
	When execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value                   |
	| case_definition_id | caseWithLongProcessTask |
	| case_id            | testCase                |

	And extract JSON from body
	And extract 'id' from JSON body
	And execute HTTP GET request 'http://localhost/case-instances/$id$/launch'
	And wait '1' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$id$/stop'
	And wait '5' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$id$/launch'
	And wait '5' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$id$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'status'='completed'
	Then JSON 'items[0].status'='finished'
	Then JSON 'items[1].status'='finished'

Scenario: Launch three times the caseWithLongProcessTask case instance
	When execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value                   |
	| case_definition_id | caseWithLongProcessTask |
	| case_id            | testCase                |

	And extract JSON from body
	And extract 'id' from JSON body
	And execute HTTP GET request 'http://localhost/case-instances/$id$/launch'	
	And execute HTTP GET request 'http://localhost/case-instances/$id$/launch'
	And execute HTTP GET request 'http://localhost/case-instances/$id$/launch'
	And wait '10' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$id$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'status'='completed'
	Then JSON 'items[0].status'='finished'
	Then JSON 'items[1].status'='finished'

Scenario: Launch caseWithCaseFileItem case instance, stop the instance and check temporary directory is returned
	When execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value				|
	| case_definition_id | caseWithCaseFileItem |
	| case_id            | testCase				|	

	And extract JSON from body
	And extract 'id' from JSON body
	And execute HTTP GET request 'http://localhost/case-instances/$id$/launch'
	And wait '5' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$id$/stop'
	And wait '5' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$id$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'status'='cancelled'
	Then JSON 'fileitems[0].status'='cancelled'
	Then JSON exists 'fileitems[0].metadata.directory'

Scenario: Launch caseWithCaseFileItem case instance, submit the form, add a file into the temporary folder and check his status is completed
	When execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value				|
	| case_definition_id | caseWithCaseFileItem |
	| case_id            | testCase				|	

	And extract JSON from body
	And extract 'id' from JSON body
	And execute HTTP GET request 'http://localhost/case-instances/$id$/launch'
	And wait '1' seconds
	And execute HTTP POST JSON request 'http://localhost/case-instances/$id$/confirm/PlanItem_0hg1xp5'
	| Key  | Value |	
	| name | name  |
	And wait '5' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$id$'
	And extract JSON from body
	And add a file into the folder 'fileitems[0].metadata.directory'
	And wait '5' seconds	
	And execute HTTP GET request 'http://localhost/case-instances/$id$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'status'='completed'
	Then JSON exists 'fileitems[0].metadata.directory'
	Then JSON exists 'context.fileContent'

Scenario: Launch caseWithManualActivation and check his status is completed
	When execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value					|
	| case_definition_id | caseWithManualActivation |
	| case_id            | testCase					|
	
	And extract JSON from body
	And extract 'id' from JSON body
	And execute HTTP GET request 'http://localhost/case-instances/$id$/launch'
	And wait '5' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$id$/activate/PI_ProcessTask_1'
	And wait '5' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$id$'
	And extract JSON from body
	
	Then HTTP status code equals to '200'
	Then JSON 'status'='completed'
	Then JSON 'items[0].status'='finished'

Scenario: Launch caseWithRepetitionRule and check his status is completed
	When execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value					|
	| case_definition_id | caseWithRepetitionRule	|
	| case_id            | testCase					|
	
	And extract JSON from body
	And extract 'id' from JSON body
	And execute HTTP GET request 'http://localhost/case-instances/$id$/launch'
	And wait '5' seconds
	And execute HTTP GET request 'http://localhost/case-instances/$id$'
	And extract JSON from body
	
	Then HTTP status code equals to '200'
	Then JSON 'status'='completed'
	Then JSON 'items[0].status'='finished'
	Then JSON 'items[1].status'='finished'	
	Then JSON 'items[2].status'='finished'	
	Then JSON 'items[3].status'='finished'	
	Then JSON 'context.nbClients'='10'