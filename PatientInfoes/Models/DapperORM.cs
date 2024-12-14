using Dapper;
using System.Data;
using System.Data.SqlClient;
using YourNamespace.Models;

namespace YourNamespace.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly string _connectionString;

        public PatientRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
        {
            using (var connection = CreateConnection())
            {
                var sql = "SELECT * FROM Patients";
                return await connection.QueryAsync<Patient>(sql);
            }
        }

        public async Task<Patient> GetPatientByIdAsync(int patientId)
        {
            using (var connection = CreateConnection())
            {
                var sql = "SELECT * FROM Patients WHERE PatientId = @PatientId";
                return await connection.QueryFirstOrDefaultAsync<Patient>(sql, new { PatientId = patientId });
            }
        }

        public async Task<int> AddPatientAsync(Patient patient)
        {
            using (var connection = CreateConnection())
            {
                var sql = @"
                    INSERT INTO Patients (PatientName, DiseasesName, Epilepsy, OtherNCDs, Allergies)
                    VALUES (@PatientName, @DiseasesName, @Epilepsy, @OtherNCDs, @Allergies);
                    SELECT CAST(SCOPE_IDENTITY() as int);";

                return await connection.ExecuteScalarAsync<int>(sql, patient);
            }
        }

        public async Task<int> UpdatePatientAsync(Patient patient)
        {
            using (var connection = CreateConnection())
            {
                var sql = @"
                    UPDATE Patients
                    SET 
                        PatientName = @PatientName,
                        DiseasesName = @DiseasesName,
                        Epilepsy = @Epilepsy,
                        OtherNCDs = @OtherNCDs,
                        Allergies = @Allergies
                    WHERE PatientId = @PatientId";

                return await connection.ExecuteAsync(sql, patient);
            }
        }

        public async Task<int> DeletePatientAsync(int patientId)
        {
            using (var connection = CreateConnection())
            {
                var sql = "DELETE FROM Patients WHERE PatientId = @PatientId";
                return await connection.ExecuteAsync(sql, new { PatientId = patientId });
            }
        }
    }
}