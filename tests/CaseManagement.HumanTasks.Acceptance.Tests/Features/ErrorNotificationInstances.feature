Feature: ErrorNotificationInstance
	Check errors returned by /notificationinstances
	
Scenario: Check error is returned when trying to get unknown notification
	When execute HTTP POST JSON request 'http://localhost/notificationinstances/1'
	| Key           | Value       |
	And extract JSON from body

	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown notification '1''