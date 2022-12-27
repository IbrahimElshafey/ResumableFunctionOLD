﻿ /*
	* البداية هي دخول موظف جديد للشركة
	* بعد البداية يمكن تنفيذ العمليات
		* رسالة ترحيب بالموظف فيها أفراد إدارته
		* رسالة لمدير الموظف لتحديد مواصفات الجهاز المطلوبة للموظف الجديد
			* ارسال مهمة لقسم الأي تي لتخصيص جهاز مناسب للوظيفة بعد رد المدير بالمواصفات
				* إذا فشل في تخصيص جهاز يرسل طلب لقسم المشتريات لشراء جهاز
				* يرسل طلب للمدير بإمكانيات أفضل جهاز متاح أو بتخصيص الجهاز
					* إذا جاءت موافقة المدير يتم الغاء طلب الشراء إذا لم يكن تمت الموافقة عليه
					* إذا جاءت موافقة قسم المشتريات يتم اعلام المدير وقسم الأي تي
		* ارسال مهمة للمدير المباشر للموظف الجديد بتخصيص خطة عمل وتعريف بالشركة
		* ارسال مهمة للمتقدم لمرفة تفضيلاته في وجبات الطعام وأوقات العمل
			* ارسال ردود المتقدم لقسم الموارد البشرية للمراجعة
	* بعد انتهاء السابق
		* ارسال رسالة للمتقدم بتأكيد موعد أول يوم عمل
		* وبمواصفات الجهاز والبرامج عليه 
		* وخطة المدير لليوم الأول
		* ورأي قسم الموارد في أوقات العمل والوجبات
*/
var empData = WaitEvent(NewEmpAdded);
var hrWelcomeNewEmp = Task(WelcomeNewEmp,empData);
var machineSpecsResponse = Task(AskManagerAboutMachineSpecs,empData);
machineSpecsResponse.AfterDone(
		var itResponse = Task(AskItForMachine,machineSpecsResponse);
		itResponse.AfterDone(
			if(itResponse.NoMachine)
			{
				var bestMachineResponse = Task(AskManagerToAcceptBestMachine,itResponse.BestMachineSpecs);
				var askSalesForNewMachine = Task(askSalesForNewMachine,machineSpecsResponse);
				bestMachineResponse.AfterDone(
					if(bestMachineResponse.AcceptBestExist)
						askSalesForNewMachine.Cancel();
						TaskDone(machinReady);
				);
				askSalesForNewMachine.AfterDone(
					if(askSalesForNewMachine.Accepted)
						Command(NewMachineWillBePurchased);
						TaskDone(machinReady);
				);
				
			}
			else if(itResponse.MachineReady)
				Command(MachineReady,machineSpecsResponse);
				TaskDone(machinReady);
		);
	);
var firstDayPlan = Task(AskManagerAboutFirstDayPlan,empData);
var empPreferences = Task(AskEmpAboutPreferences,empData);
empPreferences.AfterDone(
	var hrReviewPrefrences = Task(HrReviewPrefrences,empPreferences);
);

AfterAllActionsDone(hrWelcomeNewEmp,machinReady,firstDayPlan,empPreferences)
	.Run(Command(NewWelcomeEmpDetailedMessage));