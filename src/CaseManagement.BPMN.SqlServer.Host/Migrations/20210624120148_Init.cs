using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaseManagement.BPMN.SqlServer.Host.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DelegateConfigurationAggregate",
                columns: table => new
                {
                    AggregateId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullQualifiedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DelegateConfigurationAggregate", x => x.AggregateId);
                });

            migrationBuilder.CreateTable(
                name: "ProcessFiles",
                columns: table => new
                {
                    AggregateId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FileId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NbInstances = table.Column<int>(type: "int", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessFiles", x => x.AggregateId);
                });

            migrationBuilder.CreateTable(
                name: "ProcessInstances",
                columns: table => new
                {
                    AggregateId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ProcessFileId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameIdentifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessInstances", x => x.AggregateId);
                });

            migrationBuilder.CreateTable(
                name: "BPMNTranslation",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DelegateConfigurationAggregateAggregateId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPMNTranslation", x => x.Key);
                    table.ForeignKey(
                        name: "FK_BPMNTranslation_DelegateConfigurationAggregate_DelegateConfigurationAggregateAggregateId",
                        column: x => x.DelegateConfigurationAggregateAggregateId,
                        principalTable: "DelegateConfigurationAggregate",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DelegateConfigurationRecord",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DelegateConfigurationAggregateAggregateId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DelegateConfigurationRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DelegateConfigurationRecord_DelegateConfigurationAggregate_DelegateConfigurationAggregateAggregateId",
                        column: x => x.DelegateConfigurationAggregateAggregateId,
                        principalTable: "DelegateConfigurationAggregate",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BPMNInterface",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImplementationRef = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessInstanceAggregateAggregateId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EltId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPMNInterface", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BPMNInterface_ProcessInstances_ProcessInstanceAggregateAggregateId",
                        column: x => x.ProcessInstanceAggregateAggregateId,
                        principalTable: "ProcessInstances",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExecutionPath",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessInstanceAggregateAggregateId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutionPath", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExecutionPath_ProcessInstances_ProcessInstanceAggregateAggregateId",
                        column: x => x.ProcessInstanceAggregateAggregateId,
                        principalTable: "ProcessInstances",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FlowNodeInstance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EltId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlowNodeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    ActivityState = table.Column<int>(type: "int", nullable: true),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessInstanceAggregateAggregateId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowNodeInstance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlowNodeInstance_ProcessInstances_ProcessInstanceAggregateAggregateId",
                        column: x => x.ProcessInstanceAggregateAggregateId,
                        principalTable: "ProcessInstances",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemDefinition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemKind = table.Column<int>(type: "int", nullable: false),
                    IsCollection = table.Column<bool>(type: "bit", nullable: false),
                    StructureRef = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessInstanceAggregateAggregateId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EltId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemDefinition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemDefinition_ProcessInstances_ProcessInstanceAggregateAggregateId",
                        column: x => x.ProcessInstanceAggregateAggregateId,
                        principalTable: "ProcessInstances",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemRef = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessInstanceAggregateAggregateId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EltId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_ProcessInstances_ProcessInstanceAggregateAggregateId",
                        column: x => x.ProcessInstanceAggregateAggregateId,
                        principalTable: "ProcessInstances",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SequenceFlow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceRef = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetRef = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConditionExpression = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessInstanceAggregateAggregateId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EltId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SequenceFlow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SequenceFlow_ProcessInstances_ProcessInstanceAggregateAggregateId",
                        column: x => x.ProcessInstanceAggregateAggregateId,
                        principalTable: "ProcessInstances",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SerializedFlowNodeDefinition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerializedContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ProcessInstanceAggregateAggregateId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SerializedFlowNodeDefinition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SerializedFlowNodeDefinition_ProcessInstances_ProcessInstanceAggregateAggregateId",
                        column: x => x.ProcessInstanceAggregateAggregateId,
                        principalTable: "ProcessInstances",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StateTransitionToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlowNodeInstanceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StateTransition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessInstanceAggregateAggregateId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateTransitionToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StateTransitionToken_ProcessInstances_ProcessInstanceAggregateAggregateId",
                        column: x => x.ProcessInstanceAggregateAggregateId,
                        principalTable: "ProcessInstances",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Operation",
                columns: table => new
                {
                    EltId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImplementationRef = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InMessageRef = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OutMessageRef = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BPMNInterfaceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operation", x => x.EltId);
                    table.ForeignKey(
                        name: "FK_Operation_BPMNInterface_BPMNInterfaceId",
                        column: x => x.BPMNInterfaceId,
                        principalTable: "BPMNInterface",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExecutionPointer",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExecutionPathId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InstanceFlowNodeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlowNodeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutionPointer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExecutionPointer_ExecutionPath_ExecutionPathId",
                        column: x => x.ExecutionPathId,
                        principalTable: "ExecutionPath",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityStateHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    State = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExecutionDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FlowNodeInstanceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityStateHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityStateHistory_FlowNodeInstance_FlowNodeInstanceId",
                        column: x => x.FlowNodeInstanceId,
                        principalTable: "FlowNodeInstance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MessageToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ExecutionPointerId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageToken_ExecutionPointer_ExecutionPointerId",
                        column: x => x.ExecutionPointerId,
                        principalTable: "ExecutionPointer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityStateHistory_FlowNodeInstanceId",
                table: "ActivityStateHistory",
                column: "FlowNodeInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_BPMNInterface_ProcessInstanceAggregateAggregateId",
                table: "BPMNInterface",
                column: "ProcessInstanceAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_BPMNTranslation_DelegateConfigurationAggregateAggregateId",
                table: "BPMNTranslation",
                column: "DelegateConfigurationAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_DelegateConfigurationRecord_DelegateConfigurationAggregateAggregateId",
                table: "DelegateConfigurationRecord",
                column: "DelegateConfigurationAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionPath_ProcessInstanceAggregateAggregateId",
                table: "ExecutionPath",
                column: "ProcessInstanceAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionPointer_ExecutionPathId",
                table: "ExecutionPointer",
                column: "ExecutionPathId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowNodeInstance_ProcessInstanceAggregateAggregateId",
                table: "FlowNodeInstance",
                column: "ProcessInstanceAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemDefinition_ProcessInstanceAggregateAggregateId",
                table: "ItemDefinition",
                column: "ProcessInstanceAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ProcessInstanceAggregateAggregateId",
                table: "Message",
                column: "ProcessInstanceAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageToken_ExecutionPointerId",
                table: "MessageToken",
                column: "ExecutionPointerId");

            migrationBuilder.CreateIndex(
                name: "IX_Operation_BPMNInterfaceId",
                table: "Operation",
                column: "BPMNInterfaceId");

            migrationBuilder.CreateIndex(
                name: "IX_SequenceFlow_ProcessInstanceAggregateAggregateId",
                table: "SequenceFlow",
                column: "ProcessInstanceAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_SerializedFlowNodeDefinition_ProcessInstanceAggregateAggregateId",
                table: "SerializedFlowNodeDefinition",
                column: "ProcessInstanceAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_StateTransitionToken_ProcessInstanceAggregateAggregateId",
                table: "StateTransitionToken",
                column: "ProcessInstanceAggregateAggregateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityStateHistory");

            migrationBuilder.DropTable(
                name: "BPMNTranslation");

            migrationBuilder.DropTable(
                name: "DelegateConfigurationRecord");

            migrationBuilder.DropTable(
                name: "ItemDefinition");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "MessageToken");

            migrationBuilder.DropTable(
                name: "Operation");

            migrationBuilder.DropTable(
                name: "ProcessFiles");

            migrationBuilder.DropTable(
                name: "SequenceFlow");

            migrationBuilder.DropTable(
                name: "SerializedFlowNodeDefinition");

            migrationBuilder.DropTable(
                name: "StateTransitionToken");

            migrationBuilder.DropTable(
                name: "FlowNodeInstance");

            migrationBuilder.DropTable(
                name: "DelegateConfigurationAggregate");

            migrationBuilder.DropTable(
                name: "ExecutionPointer");

            migrationBuilder.DropTable(
                name: "BPMNInterface");

            migrationBuilder.DropTable(
                name: "ExecutionPath");

            migrationBuilder.DropTable(
                name: "ProcessInstances");
        }
    }
}
