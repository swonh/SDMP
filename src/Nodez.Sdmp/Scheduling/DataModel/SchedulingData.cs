using Nodez.Data.Interface;
using Nodez.Sdmp.Scheduling.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Scheduling.DataModel
{
    public class SchedulingData : IData
    {
        public List<IPlanInfoData> PlanInfoDataList { get; private set; }

        public List<IJobData> JobDataList { get; private set; }

        public List<IEqpData> EqpDataList { get; private set; }

        public List<IEqpGroupData> EqpGroupDataList { get; private set; }

        public List<IProcessData> ProcessDataList { get; private set; }

        public List<IArrangeData> ArrangeDataList { get; private set; }

        public List<ISetupInfoData> SetupInfoDataList { get; private set; }

        public List<ITargetInfoData> TargetInfoDataList { get; private set; }

        public List<IPMScheduleData> PMScheduleDataList { get; private set; }

        public void SetPlanInfoData(List<IPlanInfoData> planInfoDataList) 
        {
            this.PlanInfoDataList = planInfoDataList;
        }

        public void SetJobDataList(List<IJobData> jobDataList)
        {
            this.JobDataList = jobDataList;
        }

        public void SetEqpDataList(List<IEqpData> eqpDataList)
        {
            this.EqpDataList = eqpDataList;
        }

        public void SetEqpGroupDataList(List<IEqpGroupData> eqpGroupDataList)
        {
            this.EqpGroupDataList = eqpGroupDataList;
        }

        public void SetProcessDataList(List<IProcessData> processDataList)
        {
            this.ProcessDataList = processDataList;
        }

        public void SetArrangeDataList(List<IArrangeData> arrangeDataList)
        {
            this.ArrangeDataList = arrangeDataList;
        }

        public void SetSetupInfoDataList(List<ISetupInfoData> setupInfoDataList) 
        {
            this.SetupInfoDataList = setupInfoDataList;
        }

        public void SetTargetInfoDataList(List<ITargetInfoData> targetInfoDataList)
        {
            this.TargetInfoDataList = targetInfoDataList;
        }

        public void SetPMScheduleDataList(List<IPMScheduleData> pmScheduleDataList)
        {
            this.PMScheduleDataList = pmScheduleDataList;
        }
    }
}
