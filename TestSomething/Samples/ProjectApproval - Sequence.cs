﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ResumableFunction.Abstraction.InOuts;

namespace ResumableFunction.Abstraction.Samples
{
    /*
         * بعد إضافة مشروع يتم ارسال دعوة لمالك المشروع
         * ننتظر موافقة مالك المشروع ثم موافقة راعي المشروع ثم موافقة مدير المشروع بشكل متتابع
         * إذا رفض أحدهم يتم إلغاء المشروع وإعلام الآخرين
         * موافقة راعي المشروع ليست إجبارية, قد يرد أو لا يرد أبداً
         * 
         */
    //ProjectApprovalContextData is the data that will bes saved to the database 
    //When the engine match an event it will load the related Function class and set the 
    //FunctionData property by loading it from database
    //No other state saved just the FunctionData and Function author must keep that in mind
    //We can't depend on automatic serialize for state becuse compiler may remove fields and variables we defined
    public class ProjectApproval : ResumableFunction<ProjectApprovalContextData>
    {
        public ProjectApproval(ProjectApprovalContextData data, IFunctionEngine engine) : base(data, engine)
        {
        }


        //public ProjectApproval(ProjectRequestedEvent p, ManagerApprovalEvent po, ManagerApprovalEvent ps, ManagerApprovalEvent pm)
        //{
        //    ProjectRequested = p;
        //    OwnerApproval = po;
        //    SponsorApproval = ps;
        //    ManagerApproval = pm;
        //    FunctionData = new ProjectApprovalContextData();
        //}

        //any inherited ResumableFunction must implement 'RunFunction'
        protected override async IAsyncEnumerable<EventWaitingResult> RunFunction()
        {
            //any class that inherit FunctionInstance<T> has the methods
            //WaitEvent,WaitFirstEvent in a collection,WaitEvents and SaveFunctionData

            //the engine will wait for ProjectRequested event
            //no match function because it's the first one
            //context prop is prop in FunctionData that we will set with event result data
            yield return WaitEvent(typeof(ProjectRequestedEvent),"ProjectCreated").SetProp(() => FunctionData.Project);
            //the compiler will save state after executing the previous return
            //and wiating for the event
            //it will continue from the line below when event cames


            //FunctionData.Project is set by the previous event
            //we will initiate a task for Owner and wait to the Owner response
            //That matching function correlates the event to the right instance
            //The matching function will be translated to query language "MongoDB query for example" by the engine to search the active instance.
            await AskOwnerToApprove(FunctionData.Project);
            yield return WaitEvent(typeof(ManagerApprovalEvent), "OwnerApproval")
                .Match<ManagerApprovalEvent>(result => result.ProjectId == FunctionData.Project.Id)
                .SetProp(() => FunctionData.OwnerApprovalResult);
            if (FunctionData.OwnerApprovalResult.Rejected)
            {
                await ProjectRejected(FunctionData.Project, "Owner");
                yield break;
            }

            await AskSponsorToApprove(FunctionData.Project);
            yield return WaitEvent(typeof(ManagerApprovalEvent), "SponsorApproval")
                .Match<ManagerApprovalEvent>(result => result.ProjectId == FunctionData.Project.Id)
                .SetProp(() => FunctionData.SponsorApprovalResult);
            if (FunctionData.SponsorApprovalResult.Rejected)
            {
                await ProjectRejected(FunctionData.Project, "Sponsor");
                yield break;
            }

            await AskManagerToApprove(FunctionData.Project);
            yield return WaitEvent(typeof(ManagerApprovalEvent), "ManagerApproval")
                .Match<ManagerApprovalEvent>(result => result.ProjectId == FunctionData.Project.Id)
                .SetProp(() => FunctionData.ManagerApprovalResult);
            if (FunctionData.ManagerApprovalResult.Rejected)
            {
                await ProjectRejected(FunctionData.Project, "Manager");
                yield break;
            }

            Console.WriteLine("All three approved");
        }

        protected async Task ProjectRejected(ProjectRequestedEvent project, string v)
        {
            await Task.Delay(500);
        }

        protected async Task AskManagerToApprove(ProjectRequestedEvent project)
        {
            await Task.Delay(1000);
        }

        protected async Task AskSponsorToApprove(ProjectRequestedEvent project)
        {
            await Task.Delay(1000);
        }

        protected async Task AskOwnerToApprove(ProjectRequestedEvent project)
        {
            await Task.Delay(1000);
        }
    }
    public class ProjectApprovalContextData
    {
        public ProjectRequestedEvent Project { get; set; }
        public ManagerApprovalEvent OwnerApprovalResult { get; set; }
        public ManagerApprovalEvent SponsorApprovalResult { get; set; }
        public ManagerApprovalEvent ManagerApprovalResult { get; set; }
    }


    public class ManagerApprovalEvent: IEventData
    {
       public int ProjectId{get;set;}
       public bool Accepted{get;set;}
       public bool Rejected{get;set;}
        public string EventProviderName => Const.CurrentEventProvider;
    }

    public class ProjectRequestedEvent:IEventData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DueDate { get; set; }

        public string EventProviderName => Const.CurrentEventProvider;
    }

    public class Const
    {
        public const string CurrentEventProvider = "WebHookProvider";
    }
}
