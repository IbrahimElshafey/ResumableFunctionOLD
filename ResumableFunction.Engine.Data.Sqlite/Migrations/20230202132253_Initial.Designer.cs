﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ResumableFunction.Engine.EfDataImplementation;

#nullable disable

namespace ResumableFunction.Engine.Data.Sqlite.Migrations
{
    [DbContext(typeof(EngineDataContext))]
    [Migration("20230202132253_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.2");

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.FunctionRuntimeInfo", b =>
                {
                    b.Property<Guid>("FunctionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("FunctionState")
                        .HasColumnType("TEXT");

                    b.Property<string>("InitiatedByClassType")
                        .HasColumnType("TEXT");

                    b.HasKey("FunctionId");

                    b.ToTable("FunctionRuntimeInfos", (string)null);
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.Wait", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("EventIdentifier")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("FunctionRuntimeInfoFunctionId")
                        .HasColumnType("TEXT");

                    b.Property<string>("InitiatedByFunctionName")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsFirst")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsNode")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("ParentFunctionWaitId")
                        .HasColumnType("TEXT");

                    b.Property<int?>("ReplayType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("StateAfterWait")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("FunctionRuntimeInfoFunctionId");

                    b.ToTable("Waits", (string)null);

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("ResumableFunction.Engine.InOuts.FunctionFolder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastScanDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("FunctionFolders");
                });

            modelBuilder.Entity("ResumableFunction.Engine.InOuts.TypeInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int?>("FunctionFolderId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("FunctionFolderId1")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FunctionFolderId");

                    b.HasIndex("FunctionFolderId1");

                    b.ToTable("TypeInfos", (string)null);
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.AllEventsWait", b =>
                {
                    b.HasBaseType("ResumableFunction.Abstraction.InOuts.Wait");

                    b.Property<string>("WhenCountExpression")
                        .HasColumnType("TEXT");

                    b.ToTable("AllEventsWaits", (string)null);
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.AllFunctionsWait", b =>
                {
                    b.HasBaseType("ResumableFunction.Abstraction.InOuts.Wait");

                    b.ToTable("AllFunctionsWaits", (string)null);
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.AnyEventWait", b =>
                {
                    b.HasBaseType("ResumableFunction.Abstraction.InOuts.Wait");

                    b.Property<Guid?>("MatchedEventId")
                        .HasColumnType("TEXT");

                    b.HasIndex("MatchedEventId");

                    b.ToTable("AnyEventWaits", (string)null);
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.AnyFunctionWait", b =>
                {
                    b.HasBaseType("ResumableFunction.Abstraction.InOuts.Wait");

                    b.Property<Guid?>("MatchedFunctionId")
                        .HasColumnType("TEXT");

                    b.HasIndex("MatchedFunctionId");

                    b.ToTable("AnyFunctionWaits", (string)null);
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.EventWait", b =>
                {
                    b.HasBaseType("ResumableFunction.Abstraction.InOuts.Wait");

                    b.Property<Guid?>("AllEventsWaitId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("AllEventsWaitId1")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("AnyEventWaitId")
                        .HasColumnType("TEXT");

                    b.Property<string>("EventData")
                        .HasColumnType("TEXT");

                    b.Property<string>("EventDataType")
                        .HasColumnType("TEXT");

                    b.Property<string>("EventProviderName")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsOptional")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MatchExpression")
                        .HasColumnType("TEXT");

                    b.Property<bool>("NeedFunctionData")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("ParentGroupId")
                        .HasColumnType("TEXT");

                    b.Property<string>("SetDataExpression")
                        .HasColumnType("TEXT");

                    b.HasIndex("AllEventsWaitId");

                    b.HasIndex("AllEventsWaitId1");

                    b.HasIndex("AnyEventWaitId");

                    b.ToTable("EventWaits", (string)null);
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.FunctionWait", b =>
                {
                    b.HasBaseType("ResumableFunction.Abstraction.InOuts.Wait");

                    b.Property<Guid?>("AllFunctionsWaitId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("AllFunctionsWaitId1")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("AnyFunctionWaitId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("CurrentWaitId")
                        .HasColumnType("TEXT");

                    b.Property<string>("FunctionName")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ParentFunctionGroupId")
                        .HasColumnType("TEXT");

                    b.HasIndex("AllFunctionsWaitId");

                    b.HasIndex("AllFunctionsWaitId1");

                    b.HasIndex("AnyFunctionWaitId");

                    b.HasIndex("CurrentWaitId");

                    b.ToTable("FunctionWaits", (string)null);
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.Wait", b =>
                {
                    b.HasOne("ResumableFunction.Abstraction.InOuts.FunctionRuntimeInfo", "FunctionRuntimeInfo")
                        .WithMany("FunctionWaits")
                        .HasForeignKey("FunctionRuntimeInfoFunctionId");

                    b.Navigation("FunctionRuntimeInfo");
                });

            modelBuilder.Entity("ResumableFunction.Engine.InOuts.TypeInfo", b =>
                {
                    b.HasOne("ResumableFunction.Engine.InOuts.FunctionFolder", null)
                        .WithMany("EventProviderTypes")
                        .HasForeignKey("FunctionFolderId");

                    b.HasOne("ResumableFunction.Engine.InOuts.FunctionFolder", null)
                        .WithMany("FunctionInfos")
                        .HasForeignKey("FunctionFolderId1");
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.AllEventsWait", b =>
                {
                    b.HasOne("ResumableFunction.Abstraction.InOuts.Wait", null)
                        .WithOne()
                        .HasForeignKey("ResumableFunction.Abstraction.InOuts.AllEventsWait", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.AllFunctionsWait", b =>
                {
                    b.HasOne("ResumableFunction.Abstraction.InOuts.Wait", null)
                        .WithOne()
                        .HasForeignKey("ResumableFunction.Abstraction.InOuts.AllFunctionsWait", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.AnyEventWait", b =>
                {
                    b.HasOne("ResumableFunction.Abstraction.InOuts.Wait", null)
                        .WithOne()
                        .HasForeignKey("ResumableFunction.Abstraction.InOuts.AnyEventWait", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ResumableFunction.Abstraction.InOuts.EventWait", "MatchedEvent")
                        .WithMany()
                        .HasForeignKey("MatchedEventId");

                    b.Navigation("MatchedEvent");
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.AnyFunctionWait", b =>
                {
                    b.HasOne("ResumableFunction.Abstraction.InOuts.Wait", null)
                        .WithOne()
                        .HasForeignKey("ResumableFunction.Abstraction.InOuts.AnyFunctionWait", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ResumableFunction.Abstraction.InOuts.FunctionWait", "MatchedFunction")
                        .WithMany()
                        .HasForeignKey("MatchedFunctionId");

                    b.Navigation("MatchedFunction");
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.EventWait", b =>
                {
                    b.HasOne("ResumableFunction.Abstraction.InOuts.AllEventsWait", null)
                        .WithMany("MatchedEvents")
                        .HasForeignKey("AllEventsWaitId");

                    b.HasOne("ResumableFunction.Abstraction.InOuts.AllEventsWait", null)
                        .WithMany("WaitingEvents")
                        .HasForeignKey("AllEventsWaitId1");

                    b.HasOne("ResumableFunction.Abstraction.InOuts.AnyEventWait", null)
                        .WithMany("WaitingEvents")
                        .HasForeignKey("AnyEventWaitId");

                    b.HasOne("ResumableFunction.Abstraction.InOuts.Wait", null)
                        .WithOne()
                        .HasForeignKey("ResumableFunction.Abstraction.InOuts.EventWait", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.FunctionWait", b =>
                {
                    b.HasOne("ResumableFunction.Abstraction.InOuts.AllFunctionsWait", null)
                        .WithMany("CompletedFunctions")
                        .HasForeignKey("AllFunctionsWaitId");

                    b.HasOne("ResumableFunction.Abstraction.InOuts.AllFunctionsWait", null)
                        .WithMany("WaitingFunctions")
                        .HasForeignKey("AllFunctionsWaitId1");

                    b.HasOne("ResumableFunction.Abstraction.InOuts.AnyFunctionWait", null)
                        .WithMany("WaitingFunctions")
                        .HasForeignKey("AnyFunctionWaitId");

                    b.HasOne("ResumableFunction.Abstraction.InOuts.Wait", "CurrentWait")
                        .WithMany()
                        .HasForeignKey("CurrentWaitId");

                    b.HasOne("ResumableFunction.Abstraction.InOuts.Wait", null)
                        .WithOne()
                        .HasForeignKey("ResumableFunction.Abstraction.InOuts.FunctionWait", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CurrentWait");
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.FunctionRuntimeInfo", b =>
                {
                    b.Navigation("FunctionWaits");
                });

            modelBuilder.Entity("ResumableFunction.Engine.InOuts.FunctionFolder", b =>
                {
                    b.Navigation("EventProviderTypes");

                    b.Navigation("FunctionInfos");
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.AllEventsWait", b =>
                {
                    b.Navigation("MatchedEvents");

                    b.Navigation("WaitingEvents");
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.AllFunctionsWait", b =>
                {
                    b.Navigation("CompletedFunctions");

                    b.Navigation("WaitingFunctions");
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.AnyEventWait", b =>
                {
                    b.Navigation("WaitingEvents");
                });

            modelBuilder.Entity("ResumableFunction.Abstraction.InOuts.AnyFunctionWait", b =>
                {
                    b.Navigation("WaitingFunctions");
                });
#pragma warning restore 612, 618
        }
    }
}