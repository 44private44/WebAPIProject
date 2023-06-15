using APIFirstProject.Entities.DataModels;
using APIFirstProject.Entities.MissionDataModel;
using ASPFirstProject.Repositories.IRepository;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;


namespace ASPFirstProject.Repositories.Repository
{
    public class MissionRepository : IMissionRepository
    {
        private readonly CiplatformContext _Context;
        private readonly string connectionString = "Server=PCA172\\SQL2017;Database=CIPlatform;Trusted_Connection=True;MultipleActiveResultSets=true;User ID=sa;Password=Tatva@123;Integrated Security=False; TrustServerCertificate=True";

        public MissionRepository(CiplatformContext context)
        {
            _Context = context;
        }

        // Get all Mission Records

        public List<AllMissionDataModel> AllMissionRecords()
        {
            // make connection 
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                List<AllMissionDataModel> missionDataList = connection.Query<AllMissionDataModel>("MissionDataByDapper", commandType: CommandType.StoredProcedure).ToList();
               
                // Return the data
                return missionDataList;
            }
        }

        // Get one Mission Record 
        public AllMissionDataModel OneMissionRecord(long missionID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new { missionID = missionID };
                AllMissionDataModel missionData = connection.QueryFirstOrDefault<AllMissionDataModel>("OneMissionDataByDapper", parameters, commandType: CommandType.StoredProcedure);
                return missionData;
            }
        }
    }
}
