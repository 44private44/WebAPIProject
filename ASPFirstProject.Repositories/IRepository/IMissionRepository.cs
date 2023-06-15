using APIFirstProject.Entities.DataModels;
using APIFirstProject.Entities.MissionDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ASPFirstProject.Repositories.IRepository
{
    public interface IMissionRepository
    {

        List<AllMissionDataModel> AllMissionRecords();

        AllMissionDataModel OneMissionRecord(long missionID);
    }
}
