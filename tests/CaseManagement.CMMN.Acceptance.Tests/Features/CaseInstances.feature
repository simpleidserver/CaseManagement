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

Scenario: Launch caseWithTimerEventListener case instance and check his status is completed
	When execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value                      |
	| case_definition_id | caseWithTimerEventListener |
	| case_id            | Case_0d1ujq8               |

	And extract JSON from body
	And extract 'id' from JSON body
	And execute HTTP GET request 'http://localhost/case-instances/$id$/launch'
	And wait '5' seconds
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