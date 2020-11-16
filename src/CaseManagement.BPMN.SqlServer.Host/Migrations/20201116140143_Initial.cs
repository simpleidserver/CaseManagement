using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaseManagement.BPMN.SqlServer.Host.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProcessInstances",
                columns: table => new
                {
                    AggregateId = table.Column<string>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    InstanceId = table.Column<string>(nullable: true),
                    ProcessId = table.Column<string>(nullable: true),
                    CommonId = table.Column<string>(nullable: true),
                    ProcessFileId = table.Column<string>(nullable: true),
                    CreateDateTime = table.Column<DateTime>(nullable: false),
                    UpdateDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessInstances", x => x.AggregateId);
                });

            migrationBuilder.CreateTable(
                name: "BPMNInterfaceModel",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    ImplementationRef = table.Column<string>(nullable: true),
                    ProcessInstanceModelAggregateId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPMNInterfaceModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BPMNInterfaceModel_ProcessInstances_ProcessInstanceModelAggregateId",
                        column: x => x.ProcessInstanceModelAggregateId,
                        principalTable: "ProcessInstances",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExecutionPathModel",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreateDateTime = table.Column<DateTime>(nullable: false),
                    ProcessInstanceModelAggregateId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutionPathModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExecutionPathModel_ProcessInstances_ProcessInstanceModelAggregateId",
                        column: x => x.ProcessInstanceModelAggregateId,
                        principalTable: "ProcessInstances",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlowNodeInstanceModel",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlowNodeId = table.Column<string>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    ActivityState = table.Column<int>(nullable: true),
                    Metadata = table.Column<string>(nullable: true),
                    ProcessInstanceModelAggregateId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowNodeInstanceModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlowNodeInstanceModel_ProcessInstances_ProcessInstanceModelAggregateId",
                        column: x => x.ProcessInstanceModelAggregateId,
                        principalTable: "ProcessInstances",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlowNodeModel",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    SerializedContent = table.Column<string>(nullable: true),
                    ProcessInstanceModelAggregateId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowNodeModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlowNodeModel_ProcessInstances_ProcessInstanceModelAggregateId",
                        column: x => x.ProcessInstanceModelAggregateId,
                        principalTable: "ProcessInstances",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemDefinitionModel",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemKind = table.Column<int>(nullable: false),
                    IsCollection = table.Column<bool>(nullable: false),
                    StructureRef = table.Column<string>(nullable: true),
                    ProcessInstanceModelAggregateId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemDefinitionModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemDefinitionModel_ProcessInstances_ProcessInstanceModelAggregateId",
                        column: x => x.ProcessInstanceModelAggregateId,
                        principalTable: "ProcessInstances",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MessageModel",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    ItemRef = table.Column<string>(nullable: true),
                    ProcessInstanceModelAggregateId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageModel_ProcessInstances_ProcessInstanceModelAggregateId",
                        column: x => x.ProcessInstanceModelAggregateId,
                        principalTable: "ProcessInstances",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SequenceFlowModel",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    SourceRef = table.Column<string>(nullable: true),
                    TargetRef = table.Column<string>(nullable: true),
                    ConditionExpression = table.Column<string>(nullable: true),
                    ProcessInstanceModelAggregateId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SequenceFlowModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SequenceFlowModel_ProcessInstances_ProcessInstanceModelAggregateId",
                        column: x => x.ProcessInstanceModelAggregateId,
                        principalTable: "ProcessInstances",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StateTransitionTokenModel",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    FlowNodeInstanceId = table.Column<string>(nullable: true),
                    SerializedContent = table.Column<string>(nullable: true),
                    ProcessInstanceModelAggregateId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateTransitionTokenModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StateTransitionTokenModel_ProcessInstances_ProcessInstanceModelAggregateId",
                        column: x => x.ProcessInstanceModelAggregateId,
                        principalTable: "ProcessInstances",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperationModel",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    ImplementationRef = table.Column<string>(nullable: true),
                    InMessageRef = table.Column<string>(nullable: true),
                    OutMessageRef = table.Column<string>(nullable: true),
                    BPMNInterfaceModelId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationModel_BPMNInterfaceModel_BPMNInterfaceModelId",
                        column: x => x.BPMNInterfaceModelId,
                        principalTable: "BPMNInterfaceModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExecutionPointerModel",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExecutionPathId = table.Column<string>(nullable: true),
                    InstanceFlowNodeId = table.Column<string>(nullable: true),
                    FlowNodeId = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    ExecutionPathModelId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutionPointerModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExecutionPointerModel_ExecutionPathModel_ExecutionPathModelId",
                        column: x => x.ExecutionPathModelId,
                        principalTable: "ExecutionPathModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityStateHistoryModel",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    State = table.Column<int>(nullable: false),
                    ExecutionDateTime = table.Column<DateTime>(nullable: false),
                    FlowNodeInstanceModelId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityStateHistoryModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityStateHistoryModel_FlowNodeInstanceModel_FlowNodeInstanceModelId",
                        column: x => x.FlowNodeInstanceModelId,
                        principalTable: "FlowNodeInstanceModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MessageTokenModel",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Direction = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    SerializedContent = table.Column<string>(nullable: true),
                    ExecutionPointerModelId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageTokenModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageTokenModel_ExecutionPointerModel_ExecutionPointerModelId",
                        column: x => x.ExecutionPointerModelId,
                        principalTable: "ExecutionPointerModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityStateHistoryModel_FlowNodeInstanceModelId",
                table: "ActivityStateHistoryModel",
                column: "FlowNodeInstanceModelId");

            migrationBuilder.CreateIndex(
                name: "IX_BPMNInterfaceModel_ProcessInstanceModelAggregateId",
                table: "BPMNInterfaceModel",
                column: "ProcessInstanceModelAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionPathModel_ProcessInstanceModelAggregateId",
                table: "ExecutionPathModel",
                column: "ProcessInstanceModelAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionPointerModel_ExecutionPathModelId",
                table: "ExecutionPointerModel",
                column: "ExecutionPathModelId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowNodeInstanceModel_ProcessInstanceModelAggregateId",
                table: "FlowNodeInstanceModel",
                column: "ProcessInstanceModelAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowNodeModel_ProcessInstanceModelAggregateId",
                table: "FlowNodeModel",
                column: "ProcessInstanceModelAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemDefinitionModel_ProcessInstanceModelAggregateId",
                table: "ItemDefinitionModel",
                column: "ProcessInstanceModelAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageModel_ProcessInstanceModelAggregateId",
                table: "MessageModel",
                column: "ProcessInstanceModelAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageTokenModel_ExecutionPointerModelId",
                table: "MessageTokenModel",
                column: "ExecutionPointerModelId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationModel_BPMNInterfaceModelId",
                table: "OperationModel",
                column: "BPMNInterfaceModelId");

            migrationBuilder.CreateIndex(
                name: "IX_SequenceFlowModel_ProcessInstanceModelAggregateId",
                table: "SequenceFlowModel",
                column: "ProcessInstanceModelAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_StateTransitionTokenModel_ProcessInstanceModelAggregateId",
                table: "StateTransitionTokenModel",
                column: "ProcessInstanceModelAggregateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityStateHistoryModel");

            migrationBuilder.DropTable(
                name: "FlowNodeModel");

            migrationBuilder.DropTable(
                name: "ItemDefinitionModel");

            migrationBuilder.DropTable(
                name: "MessageModel");

            migrationBuilder.DropTable(
                name: "MessageTokenModel");

            migrationBuilder.DropTable(
                name: "OperationModel");

            migrationBuilder.DropTable(
                name: "SequenceFlowModel");

            migrationBuilder.DropTable(
                name: "StateTransitionTokenModel");

            migrationBuilder.DropTable(
                name: "FlowNodeInstanceModel");

            migrationBuilder.DropTable(
                name: "ExecutionPointerModel");

            migrationBuilder.DropTable(
                name: "BPMNInterfaceModel");

            migrationBuilder.DropTable(
                name: "ExecutionPathModel");

            migrationBuilder.DropTable(
                name: "ProcessInstances");
        }
    }
}
