﻿// <auto-generated />
using System;
using CaseManagement.BPMN.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CaseManagement.BPMN.SqlServer.Host.Migrations
{
    [DbContext(typeof(BPMNDbContext))]
    [Migration("20210624120148_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CaseManagement.BPMN.Domains.ActivityStateHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ExecutionDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("FlowNodeInstanceId")
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FlowNodeInstanceId");

                    b.ToTable("ActivityStateHistory");
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.BPMNInterface", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("EltId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImplementationRef")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProcessInstanceAggregateAggregateId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ProcessInstanceAggregateAggregateId");

                    b.ToTable("BPMNInterface");
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.BPMNTranslation", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DelegateConfigurationAggregateAggregateId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Language")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Key");

                    b.HasIndex("DelegateConfigurationAggregateAggregateId");

                    b.ToTable("BPMNTranslation");
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.DelegateConfiguration.DelegateConfigurationAggregate", b =>
                {
                    b.Property<string>("AggregateId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreateDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("FullQualifiedName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdateDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Version")
                        .HasColumnType("int");

                    b.HasKey("AggregateId");

                    b.ToTable("DelegateConfigurationAggregate");
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.DelegateConfiguration.DelegateConfigurationRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DelegateConfigurationAggregateAggregateId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DelegateConfigurationAggregateAggregateId");

                    b.ToTable("DelegateConfigurationRecord");
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.ExecutionPath", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreateDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProcessInstanceAggregateAggregateId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ProcessInstanceAggregateAggregateId");

                    b.ToTable("ExecutionPath");
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.ExecutionPointer", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ExecutionPathId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FlowNodeId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InstanceFlowNodeId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ExecutionPathId");

                    b.ToTable("ExecutionPointer");
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.FlowNodeInstance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ActivityState")
                        .HasColumnType("int");

                    b.Property<string>("EltId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FlowNodeId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Metadata")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProcessInstanceAggregateAggregateId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProcessInstanceAggregateAggregateId");

                    b.ToTable("FlowNodeInstance");
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.ItemDefinition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("EltId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsCollection")
                        .HasColumnType("bit");

                    b.Property<int>("ItemKind")
                        .HasColumnType("int");

                    b.Property<string>("ProcessInstanceAggregateAggregateId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("StructureRef")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProcessInstanceAggregateAggregateId");

                    b.ToTable("ItemDefinition");
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("EltId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ItemRef")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProcessInstanceAggregateAggregateId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ProcessInstanceAggregateAggregateId");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.MessageToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ExecutionPointerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MessageContent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ExecutionPointerId");

                    b.ToTable("MessageToken");
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.Operation", b =>
                {
                    b.Property<string>("EltId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("BPMNInterfaceId")
                        .HasColumnType("int");

                    b.Property<string>("ImplementationRef")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InMessageRef")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OutMessageRef")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EltId");

                    b.HasIndex("BPMNInterfaceId");

                    b.ToTable("Operation");
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.ProcessFileAggregate", b =>
                {
                    b.Property<string>("AggregateId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreateDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NbInstances")
                        .HasColumnType("int");

                    b.Property<string>("Payload")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdateDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Version")
                        .HasColumnType("int");

                    b.HasKey("AggregateId");

                    b.ToTable("ProcessFiles");
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.ProcessInstanceAggregate", b =>
                {
                    b.Property<string>("AggregateId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreateDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("NameIdentifier")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProcessFileId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProcessFileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdateDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Version")
                        .HasColumnType("int");

                    b.HasKey("AggregateId");

                    b.ToTable("ProcessInstances");
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.SequenceFlow", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConditionExpression")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EltId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProcessInstanceAggregateAggregateId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SourceRef")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TargetRef")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProcessInstanceAggregateAggregateId");

                    b.ToTable("SequenceFlow");
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.SerializedFlowNodeDefinition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ProcessInstanceAggregateAggregateId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SerializedContent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProcessInstanceAggregateAggregateId");

                    b.ToTable("SerializedFlowNodeDefinition");
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.StateTransitionToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FlowNodeInstanceId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProcessInstanceAggregateAggregateId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("StateTransition")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProcessInstanceAggregateAggregateId");

                    b.ToTable("StateTransitionToken");
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.ActivityStateHistory", b =>
                {
                    b.HasOne("CaseManagement.BPMN.Domains.FlowNodeInstance", null)
                        .WithMany("ActivityStates")
                        .HasForeignKey("FlowNodeInstanceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.BPMNInterface", b =>
                {
                    b.HasOne("CaseManagement.BPMN.Domains.ProcessInstanceAggregate", null)
                        .WithMany("Interfaces")
                        .HasForeignKey("ProcessInstanceAggregateAggregateId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.BPMNTranslation", b =>
                {
                    b.HasOne("CaseManagement.BPMN.Domains.DelegateConfiguration.DelegateConfigurationAggregate", null)
                        .WithMany("Translations")
                        .HasForeignKey("DelegateConfigurationAggregateAggregateId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.DelegateConfiguration.DelegateConfigurationRecord", b =>
                {
                    b.HasOne("CaseManagement.BPMN.Domains.DelegateConfiguration.DelegateConfigurationAggregate", null)
                        .WithMany("Records")
                        .HasForeignKey("DelegateConfigurationAggregateAggregateId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.ExecutionPath", b =>
                {
                    b.HasOne("CaseManagement.BPMN.Domains.ProcessInstanceAggregate", null)
                        .WithMany("ExecutionPathLst")
                        .HasForeignKey("ProcessInstanceAggregateAggregateId");
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.ExecutionPointer", b =>
                {
                    b.HasOne("CaseManagement.BPMN.Domains.ExecutionPath", null)
                        .WithMany("Pointers")
                        .HasForeignKey("ExecutionPathId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.FlowNodeInstance", b =>
                {
                    b.HasOne("CaseManagement.BPMN.Domains.ProcessInstanceAggregate", null)
                        .WithMany("ElementInstances")
                        .HasForeignKey("ProcessInstanceAggregateAggregateId");
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.ItemDefinition", b =>
                {
                    b.HasOne("CaseManagement.BPMN.Domains.ProcessInstanceAggregate", null)
                        .WithMany("ItemDefs")
                        .HasForeignKey("ProcessInstanceAggregateAggregateId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.Message", b =>
                {
                    b.HasOne("CaseManagement.BPMN.Domains.ProcessInstanceAggregate", null)
                        .WithMany("Messages")
                        .HasForeignKey("ProcessInstanceAggregateAggregateId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.MessageToken", b =>
                {
                    b.HasOne("CaseManagement.BPMN.Domains.ExecutionPointer", null)
                        .WithMany("Tokens")
                        .HasForeignKey("ExecutionPointerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.Operation", b =>
                {
                    b.HasOne("CaseManagement.BPMN.Domains.BPMNInterface", null)
                        .WithMany("Operations")
                        .HasForeignKey("BPMNInterfaceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.SequenceFlow", b =>
                {
                    b.HasOne("CaseManagement.BPMN.Domains.ProcessInstanceAggregate", null)
                        .WithMany("SequenceFlows")
                        .HasForeignKey("ProcessInstanceAggregateAggregateId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.SerializedFlowNodeDefinition", b =>
                {
                    b.HasOne("CaseManagement.BPMN.Domains.ProcessInstanceAggregate", null)
                        .WithMany("SerializedElementDefs")
                        .HasForeignKey("ProcessInstanceAggregateAggregateId");
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.StateTransitionToken", b =>
                {
                    b.HasOne("CaseManagement.BPMN.Domains.ProcessInstanceAggregate", null)
                        .WithMany("StateTransitions")
                        .HasForeignKey("ProcessInstanceAggregateAggregateId");
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.BPMNInterface", b =>
                {
                    b.Navigation("Operations");
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.DelegateConfiguration.DelegateConfigurationAggregate", b =>
                {
                    b.Navigation("Records");

                    b.Navigation("Translations");
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.ExecutionPath", b =>
                {
                    b.Navigation("Pointers");
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.ExecutionPointer", b =>
                {
                    b.Navigation("Tokens");
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.FlowNodeInstance", b =>
                {
                    b.Navigation("ActivityStates");
                });

            modelBuilder.Entity("CaseManagement.BPMN.Domains.ProcessInstanceAggregate", b =>
                {
                    b.Navigation("ElementInstances");

                    b.Navigation("ExecutionPathLst");

                    b.Navigation("Interfaces");

                    b.Navigation("ItemDefs");

                    b.Navigation("Messages");

                    b.Navigation("SequenceFlows");

                    b.Navigation("SerializedElementDefs");

                    b.Navigation("StateTransitions");
                });
#pragma warning restore 612, 618
        }
    }
}
