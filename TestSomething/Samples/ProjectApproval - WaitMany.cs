﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowInCode.Abstraction.InOuts;

namespace WorkflowInCode.Abstraction.Samples
{
    /*
         * بعد إضافة مشروع يتم ارسال دعوة لمالك المشروع
         * ننتظر موافقة مالك المشروع ثم موافقة راعي المشروع ثم موافقة مدير المشروع بشكل متتابع
         * إذا رفض أحدهم يتم إلغاء المشروع وإعلام الآخرين
         */
    public class ProjectApprovalWaitMany : ProjectApproval
    {
        public ProjectApprovalWaitMany(ProjectRequestedEvent p, ManagerApprovalEvent po, ManagerApprovalEvent ps, ManagerApprovalEvent pm) : base(p, po, ps, pm)
        {
        }

        protected override async IAsyncEnumerable<EventWaitingResult> RunWorkflow()
        {
            yield return WaitEvent(ProjectRequested).SetProp(() => InstanceData.Project);
            if (InstanceData.Project is not null)
            {
                await AskOwnerToApprove(InstanceData.Project);
                await AskSponsorToApprove(InstanceData.Project);
                await AskManagerToApprove(InstanceData.Project);

                //different event types and use optional
                yield return WaitEvents(
                   new SingleEventWaiting(ProjectRequested)
                       .Match<ProjectRequestedEvent>(result => result.Id == InstanceData.Project.Id)
                       .SetProp(() => InstanceData.Project)
                       .SetOptional(),
                   new SingleEventWaiting(OwnerApproval)
                       .Match<ManagerApprovalEvent>(result => result.ProjectId == InstanceData.Project.Id)
                       .SetProp(() => InstanceData.OwnerApprovalResult));

                //same type
                yield return WaitEvents(
                     new SingleEventWaiting(OwnerApproval)
                         .Match<ManagerApprovalEvent>(result => result.ProjectId == InstanceData.Project.Id)
                         .SetProp(() => InstanceData.OwnerApprovalResult),
                     new SingleEventWaiting(SponsorApproval)
                         .Match<ManagerApprovalEvent>(result => result.ProjectId == InstanceData.Project.Id)
                         .SetProp(() => InstanceData.SponsorApprovalResult),
                     new SingleEventWaiting(ManagerApproval)
                         .Match<ManagerApprovalEvent>(result => result.ProjectId == InstanceData.Project.Id)
                         .SetProp(() => InstanceData.ManagerApprovalResult)
                     );

                //wait first matched event in group
                yield return WaitFirstEvent(
                   new SingleEventWaiting(OwnerApproval)
                       .Match<ManagerApprovalEvent>(result => result.ProjectId == InstanceData.Project.Id)
                       .SetProp(() => InstanceData.OwnerApprovalResult),
                   new SingleEventWaiting(SponsorApproval)
                       .Match<ManagerApprovalEvent>(result => result.ProjectId == InstanceData.Project.Id)
                       .SetProp(() => InstanceData.SponsorApprovalResult),
                   new SingleEventWaiting(ManagerApproval)
                       .Match<ManagerApprovalEvent>(result => result.ProjectId == InstanceData.Project.Id)
                       .SetProp(() => InstanceData.ManagerApprovalResult)
                   );

                //wait all but if two of them matched then return
                yield return WaitEvents(
                  new SingleEventWaiting(OwnerApproval)
                      .Match<ManagerApprovalEvent>(result => result.ProjectId == InstanceData.Project.Id)
                      .SetProp(() => InstanceData.OwnerApprovalResult),
                  new SingleEventWaiting(SponsorApproval)
                      .Match<ManagerApprovalEvent>(result => result.ProjectId == InstanceData.Project.Id)
                      .SetProp(() => InstanceData.SponsorApprovalResult),
                  new SingleEventWaiting(ManagerApproval)
                      .Match<ManagerApprovalEvent>(result => result.ProjectId == InstanceData.Project.Id)
                      .SetProp(() => InstanceData.ManagerApprovalResult)
                  ).WhenCount(count => count == 2);

                var allManagerResponse = InstanceData.ManagerApprovalResult.Accepted == InstanceData.OwnerApprovalResult.Accepted == InstanceData.SponsorApprovalResult.Accepted;
                if (allManagerResponse is true)
                {
                    Console.WriteLine("Final Acceptance");
                }
                else if (allManagerResponse is false)
                {
                    Console.WriteLine("Final Rejection");
                }
                else
                {
                    Console.WriteLine("Ask Upper Managment");
                }
            }
        }

    }
}