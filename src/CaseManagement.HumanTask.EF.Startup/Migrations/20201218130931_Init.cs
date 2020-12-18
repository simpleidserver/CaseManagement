using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaseManagement.HumanTask.EF.Startup.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HumanTaskDefinitions",
                columns: table => new
                {
                    AggregateId = table.Column<string>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    NbInstances = table.Column<int>(nullable: false),
                    CreateDateTime = table.Column<DateTime>(nullable: false),
                    UpdateDateTime = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ActualOwnerRequired = table.Column<bool>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Outcome = table.Column<string>(nullable: true),
                    SearchBy = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    InstantiationPattern = table.Column<int>(nullable: false),
                    CompletionAction = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HumanTaskDefinitions", x => x.AggregateId);
                });

            migrationBuilder.CreateTable(
                name: "HumanTaskInstanceAggregate",
                columns: table => new
                {
                    AggregateId = table.Column<string>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    HumanTaskInstanceId = table.Column<string>(nullable: true),
                    ParentHumanTaskName = table.Column<string>(nullable: true),
                    ParentHumanTaskId = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    HumanTaskDefName = table.Column<string>(nullable: true),
                    ActualOwner = table.Column<string>(nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    ActivationDeferralTime = table.Column<DateTime>(nullable: true),
                    ExpirationTime = table.Column<DateTime>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    InstantiationPattern = table.Column<int>(nullable: false),
                    CompletionBehavior = table.Column<int>(nullable: false),
                    CreateDateTime = table.Column<DateTime>(nullable: false),
                    UpdateDateTime = table.Column<DateTime>(nullable: false),
                    InputParameters = table.Column<string>(nullable: true),
                    OutputParameters = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HumanTaskInstanceAggregate", x => x.AggregateId);
                });

            migrationBuilder.CreateTable(
                name: "NotificationInstanceAggregate",
                columns: table => new
                {
                    AggregateId = table.Column<string>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    NotificationName = table.Column<string>(nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    CreateDateTime = table.Column<DateTime>(nullable: false),
                    UpdateDateTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    OperationParameters = table.Column<string>(nullable: true),
                    Rendering = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationInstanceAggregate", x => x.AggregateId);
                });

            migrationBuilder.CreateTable(
                name: "DomainEvent",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AggregateId = table.Column<string>(nullable: true),
                    Version = table.Column<int>(nullable: false),
                    HumanTaskDefinitionAggregateAggregateId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DomainEvent_HumanTaskDefinitions_HumanTaskDefinitionAggregateAggregateId",
                        column: x => x.HumanTaskDefinitionAggregateAggregateId,
                        principalTable: "HumanTaskDefinitions",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HumanTaskDefinitionDeadLine",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    For = table.Column<string>(nullable: true),
                    Until = table.Column<string>(nullable: true),
                    Usage = table.Column<int>(nullable: false),
                    HumanTaskDefinitionAggregateAggregateId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HumanTaskDefinitionDeadLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HumanTaskDefinitionDeadLine_HumanTaskDefinitions_HumanTaskDefinitionAggregateAggregateId",
                        column: x => x.HumanTaskDefinitionAggregateAggregateId,
                        principalTable: "HumanTaskDefinitions",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HumanTaskDefinitionSubTask",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskName = table.Column<string>(nullable: true),
                    HumanTaskDefinitionAggregateAggregateId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HumanTaskDefinitionSubTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HumanTaskDefinitionSubTask_HumanTaskDefinitions_HumanTaskDefinitionAggregateAggregateId",
                        column: x => x.HumanTaskDefinitionAggregateAggregateId,
                        principalTable: "HumanTaskDefinitions",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CallbackOperation",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    HumanTaskInstanceAggregateAggregateId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallbackOperation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CallbackOperation_HumanTaskInstanceAggregate_HumanTaskInstanceAggregateAggregateId",
                        column: x => x.HumanTaskInstanceAggregateAggregateId,
                        principalTable: "HumanTaskInstanceAggregate",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Completion",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Condition = table.Column<string>(nullable: true),
                    HumanTaskDefinitionAggregateAggregateId = table.Column<string>(nullable: true),
                    HumanTaskInstanceAggregateAggregateId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Completion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Completion_HumanTaskDefinitions_HumanTaskDefinitionAggregateAggregateId",
                        column: x => x.HumanTaskDefinitionAggregateAggregateId,
                        principalTable: "HumanTaskDefinitions",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Completion_HumanTaskInstanceAggregate_HumanTaskInstanceAggregateAggregateId",
                        column: x => x.HumanTaskInstanceAggregateAggregateId,
                        principalTable: "HumanTaskInstanceAggregate",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HumanTaskInstanceDeadLine",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Usage = table.Column<int>(nullable: false),
                    EndDateTime = table.Column<DateTime>(nullable: false),
                    HumanTaskInstanceAggregateAggregateId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HumanTaskInstanceDeadLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HumanTaskInstanceDeadLine_HumanTaskInstanceAggregate_HumanTaskInstanceAggregateAggregateId",
                        column: x => x.HumanTaskInstanceAggregateAggregateId,
                        principalTable: "HumanTaskInstanceAggregate",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "HumanTaskInstanceEventHistory",
                columns: table => new
                {
                    EventId = table.Column<string>(nullable: false),
                    EventTime = table.Column<DateTime>(nullable: false),
                    HumanTaskIdentifier = table.Column<string>(nullable: true),
                    UserPrincipal = table.Column<string>(nullable: true),
                    EventType = table.Column<int>(nullable: false),
                    EventData = table.Column<string>(nullable: true),
                    StartOwner = table.Column<string>(nullable: true),
                    EndOwner = table.Column<string>(nullable: true),
                    TaskStatus = table.Column<int>(nullable: false),
                    HumanTaskInstanceAggregateAggregateId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HumanTaskInstanceEventHistory", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_HumanTaskInstanceEventHistory_HumanTaskInstanceAggregate_HumanTaskInstanceAggregateAggregateId",
                        column: x => x.HumanTaskInstanceAggregateAggregateId,
                        principalTable: "HumanTaskInstanceAggregate",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HumanTaskInstanceSubTask",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HumanTaskName = table.Column<string>(nullable: true),
                    HumanTaskInstanceAggregateAggregateId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HumanTaskInstanceSubTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HumanTaskInstanceSubTask_HumanTaskInstanceAggregate_HumanTaskInstanceAggregateAggregateId",
                        column: x => x.HumanTaskInstanceAggregateAggregateId,
                        principalTable: "HumanTaskInstanceAggregate",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RenderingElement",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    XPath = table.Column<string>(nullable: true),
                    ValueType = table.Column<string>(nullable: true),
                    Default = table.Column<string>(nullable: true),
                    HumanTaskDefinitionAggregateAggregateId = table.Column<string>(nullable: true),
                    HumanTaskInstanceAggregateAggregateId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RenderingElement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RenderingElement_HumanTaskDefinitions_HumanTaskDefinitionAggregateAggregateId",
                        column: x => x.HumanTaskDefinitionAggregateAggregateId,
                        principalTable: "HumanTaskDefinitions",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RenderingElement_HumanTaskInstanceAggregate_HumanTaskInstanceAggregateAggregateId",
                        column: x => x.HumanTaskInstanceAggregateAggregateId,
                        principalTable: "HumanTaskInstanceAggregate",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PeopleAssignmentInstance",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(nullable: false),
                    Usage = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    HumanTaskInstanceAggregateAggregateId = table.Column<string>(nullable: true),
                    NotificationInstanceAggregateAggregateId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeopleAssignmentInstance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PeopleAssignmentInstance_HumanTaskInstanceAggregate_HumanTaskInstanceAggregateAggregateId",
                        column: x => x.HumanTaskInstanceAggregateAggregateId,
                        principalTable: "HumanTaskInstanceAggregate",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PeopleAssignmentInstance_NotificationInstanceAggregate_NotificationInstanceAggregateAggregateId",
                        column: x => x.NotificationInstanceAggregateAggregateId,
                        principalTable: "NotificationInstanceAggregate",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PresentationElementInstance",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Usage = table.Column<int>(nullable: false),
                    Language = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    ContentType = table.Column<string>(nullable: true),
                    HumanTaskInstanceAggregateAggregateId = table.Column<string>(nullable: true),
                    NotificationInstanceAggregateAggregateId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PresentationElementInstance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PresentationElementInstance_HumanTaskInstanceAggregate_HumanTaskInstanceAggregateAggregateId",
                        column: x => x.HumanTaskInstanceAggregateAggregateId,
                        principalTable: "HumanTaskInstanceAggregate",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PresentationElementInstance_NotificationInstanceAggregate_NotificationInstanceAggregateAggregateId",
                        column: x => x.NotificationInstanceAggregateAggregateId,
                        principalTable: "NotificationInstanceAggregate",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Copy",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    From = table.Column<string>(nullable: true),
                    To = table.Column<string>(nullable: true),
                    CompletionId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Copy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Copy_Completion_CompletionId",
                        column: x => x.CompletionId,
                        principalTable: "Completion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Escalation",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Condition = table.Column<string>(nullable: true),
                    HumanTaskDefinitionDeadLineId = table.Column<string>(nullable: true),
                    HumanTaskInstanceDeadLineId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Escalation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Escalation_HumanTaskDefinitionDeadLine_HumanTaskDefinitionDeadLineId",
                        column: x => x.HumanTaskDefinitionDeadLineId,
                        principalTable: "HumanTaskDefinitionDeadLine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Escalation_HumanTaskInstanceDeadLine_HumanTaskInstanceDeadLineId",
                        column: x => x.HumanTaskInstanceDeadLineId,
                        principalTable: "HumanTaskInstanceDeadLine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OptionValue",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(nullable: true),
                    RenderingElementId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OptionValue_RenderingElement_RenderingElementId",
                        column: x => x.RenderingElementId,
                        principalTable: "RenderingElement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationDefinition",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EscalationId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Rendering = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationDefinition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationDefinition_Escalation_EscalationId",
                        column: x => x.EscalationId,
                        principalTable: "Escalation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ToPart",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Expression = table.Column<string>(nullable: true),
                    EscalationId = table.Column<string>(nullable: true),
                    HumanTaskDefinitionSubTaskId = table.Column<long>(nullable: true),
                    HumanTaskInstanceSubTaskId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToPart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToPart_Escalation_EscalationId",
                        column: x => x.EscalationId,
                        principalTable: "Escalation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ToPart_HumanTaskDefinitionSubTask_HumanTaskDefinitionSubTaskId",
                        column: x => x.HumanTaskDefinitionSubTaskId,
                        principalTable: "HumanTaskDefinitionSubTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ToPart_HumanTaskInstanceSubTask_HumanTaskInstanceSubTaskId",
                        column: x => x.HumanTaskInstanceSubTaskId,
                        principalTable: "HumanTaskInstanceSubTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Translation",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Language = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    OptionValueId = table.Column<long>(nullable: true),
                    RenderingElementId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Translation_OptionValue_OptionValueId",
                        column: x => x.OptionValueId,
                        principalTable: "OptionValue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Translation_RenderingElement_RenderingElementId",
                        column: x => x.RenderingElementId,
                        principalTable: "RenderingElement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parameter",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Usage = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    IsRequired = table.Column<bool>(nullable: false),
                    HumanTaskDefinitionAggregateAggregateId = table.Column<string>(nullable: true),
                    HumanTaskInstanceAggregateAggregateId = table.Column<string>(nullable: true),
                    NotificationDefinitionId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parameter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parameter_HumanTaskDefinitions_HumanTaskDefinitionAggregateAggregateId",
                        column: x => x.HumanTaskDefinitionAggregateAggregateId,
                        principalTable: "HumanTaskDefinitions",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Parameter_HumanTaskInstanceAggregate_HumanTaskInstanceAggregateAggregateId",
                        column: x => x.HumanTaskInstanceAggregateAggregateId,
                        principalTable: "HumanTaskInstanceAggregate",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Parameter_NotificationDefinition_NotificationDefinitionId",
                        column: x => x.NotificationDefinitionId,
                        principalTable: "NotificationDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PeopleAssignmentDefinition",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(nullable: false),
                    Usage = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    HumanTaskDefinitionAggregateAggregateId = table.Column<string>(nullable: true),
                    NotificationDefinitionId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeopleAssignmentDefinition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PeopleAssignmentDefinition_HumanTaskDefinitions_HumanTaskDefinitionAggregateAggregateId",
                        column: x => x.HumanTaskDefinitionAggregateAggregateId,
                        principalTable: "HumanTaskDefinitions",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PeopleAssignmentDefinition_NotificationDefinition_NotificationDefinitionId",
                        column: x => x.NotificationDefinitionId,
                        principalTable: "NotificationDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PresentationElementDefinition",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Usage = table.Column<int>(nullable: false),
                    Language = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    ContentType = table.Column<string>(nullable: true),
                    HumanTaskDefinitionAggregateAggregateId = table.Column<string>(nullable: true),
                    NotificationDefinitionId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PresentationElementDefinition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PresentationElementDefinition_HumanTaskDefinitions_HumanTaskDefinitionAggregateAggregateId",
                        column: x => x.HumanTaskDefinitionAggregateAggregateId,
                        principalTable: "HumanTaskDefinitions",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PresentationElementDefinition_NotificationDefinition_NotificationDefinitionId",
                        column: x => x.NotificationDefinitionId,
                        principalTable: "NotificationDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PresentationParameter",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Expression = table.Column<string>(nullable: true),
                    HumanTaskDefinitionAggregateAggregateId = table.Column<string>(nullable: true),
                    NotificationDefinitionId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PresentationParameter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PresentationParameter_HumanTaskDefinitions_HumanTaskDefinitionAggregateAggregateId",
                        column: x => x.HumanTaskDefinitionAggregateAggregateId,
                        principalTable: "HumanTaskDefinitions",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PresentationParameter_NotificationDefinition_NotificationDefinitionId",
                        column: x => x.NotificationDefinitionId,
                        principalTable: "NotificationDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CallbackOperation_HumanTaskInstanceAggregateAggregateId",
                table: "CallbackOperation",
                column: "HumanTaskInstanceAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_Completion_HumanTaskDefinitionAggregateAggregateId",
                table: "Completion",
                column: "HumanTaskDefinitionAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_Completion_HumanTaskInstanceAggregateAggregateId",
                table: "Completion",
                column: "HumanTaskInstanceAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_Copy_CompletionId",
                table: "Copy",
                column: "CompletionId");

            migrationBuilder.CreateIndex(
                name: "IX_DomainEvent_HumanTaskDefinitionAggregateAggregateId",
                table: "DomainEvent",
                column: "HumanTaskDefinitionAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_Escalation_HumanTaskDefinitionDeadLineId",
                table: "Escalation",
                column: "HumanTaskDefinitionDeadLineId");

            migrationBuilder.CreateIndex(
                name: "IX_Escalation_HumanTaskInstanceDeadLineId",
                table: "Escalation",
                column: "HumanTaskInstanceDeadLineId");

            migrationBuilder.CreateIndex(
                name: "IX_HumanTaskDefinitionDeadLine_HumanTaskDefinitionAggregateAggregateId",
                table: "HumanTaskDefinitionDeadLine",
                column: "HumanTaskDefinitionAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_HumanTaskDefinitionSubTask_HumanTaskDefinitionAggregateAggregateId",
                table: "HumanTaskDefinitionSubTask",
                column: "HumanTaskDefinitionAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_HumanTaskInstanceDeadLine_HumanTaskInstanceAggregateAggregateId",
                table: "HumanTaskInstanceDeadLine",
                column: "HumanTaskInstanceAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_HumanTaskInstanceEventHistory_HumanTaskInstanceAggregateAggregateId",
                table: "HumanTaskInstanceEventHistory",
                column: "HumanTaskInstanceAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_HumanTaskInstanceSubTask_HumanTaskInstanceAggregateAggregateId",
                table: "HumanTaskInstanceSubTask",
                column: "HumanTaskInstanceAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationDefinition_EscalationId",
                table: "NotificationDefinition",
                column: "EscalationId",
                unique: true,
                filter: "[EscalationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OptionValue_RenderingElementId",
                table: "OptionValue",
                column: "RenderingElementId");

            migrationBuilder.CreateIndex(
                name: "IX_Parameter_HumanTaskDefinitionAggregateAggregateId",
                table: "Parameter",
                column: "HumanTaskDefinitionAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_Parameter_HumanTaskInstanceAggregateAggregateId",
                table: "Parameter",
                column: "HumanTaskInstanceAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_Parameter_NotificationDefinitionId",
                table: "Parameter",
                column: "NotificationDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_PeopleAssignmentDefinition_HumanTaskDefinitionAggregateAggregateId",
                table: "PeopleAssignmentDefinition",
                column: "HumanTaskDefinitionAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_PeopleAssignmentDefinition_NotificationDefinitionId",
                table: "PeopleAssignmentDefinition",
                column: "NotificationDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_PeopleAssignmentInstance_HumanTaskInstanceAggregateAggregateId",
                table: "PeopleAssignmentInstance",
                column: "HumanTaskInstanceAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_PeopleAssignmentInstance_NotificationInstanceAggregateAggregateId",
                table: "PeopleAssignmentInstance",
                column: "NotificationInstanceAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_PresentationElementDefinition_HumanTaskDefinitionAggregateAggregateId",
                table: "PresentationElementDefinition",
                column: "HumanTaskDefinitionAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_PresentationElementDefinition_NotificationDefinitionId",
                table: "PresentationElementDefinition",
                column: "NotificationDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_PresentationElementInstance_HumanTaskInstanceAggregateAggregateId",
                table: "PresentationElementInstance",
                column: "HumanTaskInstanceAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_PresentationElementInstance_NotificationInstanceAggregateAggregateId",
                table: "PresentationElementInstance",
                column: "NotificationInstanceAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_PresentationParameter_HumanTaskDefinitionAggregateAggregateId",
                table: "PresentationParameter",
                column: "HumanTaskDefinitionAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_PresentationParameter_NotificationDefinitionId",
                table: "PresentationParameter",
                column: "NotificationDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_RenderingElement_HumanTaskDefinitionAggregateAggregateId",
                table: "RenderingElement",
                column: "HumanTaskDefinitionAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_RenderingElement_HumanTaskInstanceAggregateAggregateId",
                table: "RenderingElement",
                column: "HumanTaskInstanceAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_ToPart_EscalationId",
                table: "ToPart",
                column: "EscalationId");

            migrationBuilder.CreateIndex(
                name: "IX_ToPart_HumanTaskDefinitionSubTaskId",
                table: "ToPart",
                column: "HumanTaskDefinitionSubTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_ToPart_HumanTaskInstanceSubTaskId",
                table: "ToPart",
                column: "HumanTaskInstanceSubTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Translation_OptionValueId",
                table: "Translation",
                column: "OptionValueId");

            migrationBuilder.CreateIndex(
                name: "IX_Translation_RenderingElementId",
                table: "Translation",
                column: "RenderingElementId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CallbackOperation");

            migrationBuilder.DropTable(
                name: "Copy");

            migrationBuilder.DropTable(
                name: "DomainEvent");

            migrationBuilder.DropTable(
                name: "HumanTaskInstanceEventHistory");

            migrationBuilder.DropTable(
                name: "Parameter");

            migrationBuilder.DropTable(
                name: "PeopleAssignmentDefinition");

            migrationBuilder.DropTable(
                name: "PeopleAssignmentInstance");

            migrationBuilder.DropTable(
                name: "PresentationElementDefinition");

            migrationBuilder.DropTable(
                name: "PresentationElementInstance");

            migrationBuilder.DropTable(
                name: "PresentationParameter");

            migrationBuilder.DropTable(
                name: "ToPart");

            migrationBuilder.DropTable(
                name: "Translation");

            migrationBuilder.DropTable(
                name: "Completion");

            migrationBuilder.DropTable(
                name: "NotificationInstanceAggregate");

            migrationBuilder.DropTable(
                name: "NotificationDefinition");

            migrationBuilder.DropTable(
                name: "HumanTaskDefinitionSubTask");

            migrationBuilder.DropTable(
                name: "HumanTaskInstanceSubTask");

            migrationBuilder.DropTable(
                name: "OptionValue");

            migrationBuilder.DropTable(
                name: "Escalation");

            migrationBuilder.DropTable(
                name: "RenderingElement");

            migrationBuilder.DropTable(
                name: "HumanTaskDefinitionDeadLine");

            migrationBuilder.DropTable(
                name: "HumanTaskInstanceDeadLine");

            migrationBuilder.DropTable(
                name: "HumanTaskDefinitions");

            migrationBuilder.DropTable(
                name: "HumanTaskInstanceAggregate");
        }
    }
}
