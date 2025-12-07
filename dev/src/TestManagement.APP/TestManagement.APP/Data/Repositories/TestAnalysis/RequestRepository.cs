using Microsoft.EntityFrameworkCore;
using TestManagement.APP.Models.TestAnalysis;

namespace TestManagement.APP.Data.Repositories.TestAnalysis
{
    public class RequestRepository : IRequestRepository
    {
        private readonly AnalysisRequestDbContext _dbContext;

        public RequestRepository(AnalysisRequestDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<Request>> GetAllAsync()
        {
            return await _dbContext.Requests
                .Include(_ => _.Status)
                .Include(_ => _.Result)
                .ToListAsync();
        }

        public async Task<Request?> GetByIdAsync(int id)
        {
            return await _dbContext.Requests
                .Include(_ => _.Status)
                .Include(_ => _.Result)
                .Where(_ => _.Id == id)
                .FirstAsync();
        }

        public async Task AddAsync(Request testRun)
        {
            ResultMaster result = await _dbContext.ResultMasters.FindAsync(testRun.ResultId);
            StatusMaster status = await _dbContext.StatusMasters.FindAsync(testRun.StatusId);
            
            testRun.Result = result;
            testRun.Status = status;

            _dbContext.Requests.Add(testRun);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddAsync(ICollection<Request> testRuns)
        {
            _dbContext.Requests.AddRange(testRuns);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var storedRequest = await _dbContext.Requests.SingleAsync(_ => _.Id == id);
            if (storedRequest != null)
            {
                _dbContext.Requests.Remove(storedRequest);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Request with ID {id} not found.");
            }
        }

        public async Task UpdateAsync(Request testRun)
        {
            Request storedRequest = await _dbContext.Requests.FindAsync(testRun.Id);

            // Get statusId and resultId from related tables, not string values.
            int statusId = _dbContext.StatusMasters
                .Where(_ => _.Id == testRun.StatusId)
                .Select(_ => _.Id)
                .First();
            int resultId = _dbContext.ResultMasters
                .Where(_ => _.Id == testRun.ResultId)
                .Select(_ => _.Id)
                .First();

            // Update fields.
            storedRequest.StatusId = statusId;
            storedRequest.ResultId = resultId;
            storedRequest.UpdateAt = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
        }
    }
}
