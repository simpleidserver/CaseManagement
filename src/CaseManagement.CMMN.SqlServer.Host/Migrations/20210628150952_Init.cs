using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaseManagement.CMMN.SqlServer.Host.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CaseFiles",
                columns: table => new
                {
                    AggregateId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FileId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseFiles", x => x.AggregateId);
                });

            migrationBuilder.CreateTable(
                name: "CasePlanInstances",
                columns: table => new
                {
                    AggregateId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CaseFileId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CasePlanId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameIdentifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<int>(type: "int", nullable: true),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExecutionContextVariables = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CasePlanInstances", x => x.AggregateId);
                });

            migrationBuilder.CreateTable(
                name: "CasePlans",
                columns: table => new
                {
                    AggregateId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CasePlanId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaseFileId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    XmlContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NbInstances = table.Column<int>(type: "int", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CasePlans", x => x.AggregateId);
                });

            migrationBuilder.CreateTable(
                name: "CaseWorkers",
                columns: table => new
                {
                    AggregateId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CasePlanInstanceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CasePlanInstanceElementId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseWorkers", x => x.AggregateId);
                });

            migrationBuilder.CreateTable(
                name: "ManualActivationRule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Expression = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManualActivationRule", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SEntry",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IfPart = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEntry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionLst",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CasePlanInstanceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CasePlanElementInstanceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCaptured = table.Column<bool>(type: "bit", nullable: false),
                    CaptureDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Parameters = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionLst", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CaseEltInstance",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EltId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NbOccurrence = table.Column<int>(type: "int", nullable: false),
                    MilestoneState = table.Column<int>(type: "int", nullable: true),
                    TakeStageState = table.Column<int>(type: "int", nullable: true),
                    FileState = table.Column<int>(type: "int", nullable: true),
                    ManualActivationRule = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepetitionRule = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsBlocking = table.Column<bool>(type: "bit", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CaseEltInstanceId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CasePlanInstanceAggregateAggregateId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseEltInstance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseEltInstance_CaseEltInstance_CaseEltInstanceId",
                        column: x => x.CaseEltInstanceId,
                        principalTable: "CaseEltInstance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaseEltInstance_CasePlanInstances_CasePlanInstanceAggregateAggregateId",
                        column: x => x.CasePlanInstanceAggregateAggregateId,
                        principalTable: "CasePlanInstances",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CasePlanInstanceFileItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CasePlanElementInstanceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaseFileItemType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CasePlanInstanceAggregateAggregateId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CasePlanInstanceFileItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CasePlanInstanceFileItem_CasePlanInstances_CasePlanInstanceAggregateAggregateId",
                        column: x => x.CasePlanInstanceAggregateAggregateId,
                        principalTable: "CasePlanInstances",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CasePlanInstanceRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EltId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CasePlanInstanceAggregateAggregateId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CasePlanInstanceRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CasePlanInstanceRole_CasePlanInstances_CasePlanInstanceAggregateAggregateId",
                        column: x => x.CasePlanInstanceAggregateAggregateId,
                        principalTable: "CasePlanInstances",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CasePlanInstanceWorkerTask",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CasePlanElementInstanceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CasePlanInstanceAggregateAggregateId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CasePlanInstanceWorkerTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CasePlanInstanceWorkerTask_CasePlanInstances_CasePlanInstanceAggregateAggregateId",
                        column: x => x.CasePlanInstanceAggregateAggregateId,
                        principalTable: "CasePlanInstances",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CasePlanFileItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EltId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefinitionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CasePlanAggregateAggregateId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CasePlanFileItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CasePlanFileItem_CasePlans_CasePlanAggregateAggregateId",
                        column: x => x.CasePlanAggregateAggregateId,
                        principalTable: "CasePlans",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CasePlanRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EltId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CasePlanAggregateAggregateId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CasePlanRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CasePlanRole_CasePlans_CasePlanAggregateAggregateId",
                        column: x => x.CasePlanAggregateAggregateId,
                        principalTable: "CasePlans",
                        principalColumn: "AggregateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaseFileItemOnPart",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceRef = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StandardEvent = table.Column<int>(type: "int", nullable: false),
                    IsConsumed = table.Column<bool>(type: "bit", nullable: false),
                    IncomingTokens = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SEntryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseFileItemOnPart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseFileItemOnPart_SEntry_SEntryId",
                        column: x => x.SEntryId,
                        principalTable: "SEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanItemOnPart",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceRef = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StandardEvent = table.Column<int>(type: "int", nullable: false),
                    IsConsumed = table.Column<bool>(type: "bit", nullable: false),
                    IncomingTokens = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SEntryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanItemOnPart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanItemOnPart_SEntry_SEntryId",
                        column: x => x.SEntryId,
                        principalTable: "SEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaseEltInstanceProperty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaseEltInstanceId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseEltInstanceProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseEltInstanceProperty_CaseEltInstance_CaseEltInstanceId",
                        column: x => x.CaseEltInstanceId,
                        principalTable: "CaseEltInstance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaseEltInstanceTransitionHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExecutionDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Transition = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaseEltInstanceId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseEltInstanceTransitionHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseEltInstanceTransitionHistory_CaseEltInstance_CaseEltInstanceId",
                        column: x => x.CaseEltInstanceId,
                        principalTable: "CaseEltInstance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Criteria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SEntryId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CaseEltInstanceId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Criteria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Criteria_CaseEltInstance_CaseEltInstanceId",
                        column: x => x.CaseEltInstanceId,
                        principalTable: "CaseEltInstance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Criteria_SEntry_SEntryId",
                        column: x => x.SEntryId,
                        principalTable: "SEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaseEltInstance_CaseEltInstanceId",
                table: "CaseEltInstance",
                column: "CaseEltInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseEltInstance_CasePlanInstanceAggregateAggregateId",
                table: "CaseEltInstance",
                column: "CasePlanInstanceAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseEltInstanceProperty_CaseEltInstanceId",
                table: "CaseEltInstanceProperty",
                column: "CaseEltInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseEltInstanceTransitionHistory_CaseEltInstanceId",
                table: "CaseEltInstanceTransitionHistory",
                column: "CaseEltInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseFileItemOnPart_SEntryId",
                table: "CaseFileItemOnPart",
                column: "SEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_CasePlanFileItem_CasePlanAggregateAggregateId",
                table: "CasePlanFileItem",
                column: "CasePlanAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_CasePlanInstanceFileItem_CasePlanInstanceAggregateAggregateId",
                table: "CasePlanInstanceFileItem",
                column: "CasePlanInstanceAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_CasePlanInstanceRole_CasePlanInstanceAggregateAggregateId",
                table: "CasePlanInstanceRole",
                column: "CasePlanInstanceAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_CasePlanInstanceWorkerTask_CasePlanInstanceAggregateAggregateId",
                table: "CasePlanInstanceWorkerTask",
                column: "CasePlanInstanceAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_CasePlanRole_CasePlanAggregateAggregateId",
                table: "CasePlanRole",
                column: "CasePlanAggregateAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_Criteria_CaseEltInstanceId",
                table: "Criteria",
                column: "CaseEltInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Criteria_SEntryId",
                table: "Criteria",
                column: "SEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanItemOnPart_SEntryId",
                table: "PlanItemOnPart",
                column: "SEntryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaseEltInstanceProperty");

            migrationBuilder.DropTable(
                name: "CaseEltInstanceTransitionHistory");

            migrationBuilder.DropTable(
                name: "CaseFileItemOnPart");

            migrationBuilder.DropTable(
                name: "CaseFiles");

            migrationBuilder.DropTable(
                name: "CasePlanFileItem");

            migrationBuilder.DropTable(
                name: "CasePlanInstanceFileItem");

            migrationBuilder.DropTable(
                name: "CasePlanInstanceRole");

            migrationBuilder.DropTable(
                name: "CasePlanInstanceWorkerTask");

            migrationBuilder.DropTable(
                name: "CasePlanRole");

            migrationBuilder.DropTable(
                name: "CaseWorkers");

            migrationBuilder.DropTable(
                name: "Criteria");

            migrationBuilder.DropTable(
                name: "ManualActivationRule");

            migrationBuilder.DropTable(
                name: "PlanItemOnPart");

            migrationBuilder.DropTable(
                name: "SubscriptionLst");

            migrationBuilder.DropTable(
                name: "CasePlans");

            migrationBuilder.DropTable(
                name: "CaseEltInstance");

            migrationBuilder.DropTable(
                name: "SEntry");

            migrationBuilder.DropTable(
                name: "CasePlanInstances");
        }
    }
}
