using Ncfe.CodeTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ncfe.CodeTest
{
    public class LearnerService : ILearnerService
    {
        private readonly IAppSettings _settings;
        private readonly IFailoverRepository _failoverRepository;
        private readonly ILearnerDataAccess _LearnerDataAccess;
        private readonly IArchivedDataService _archivedDataService;

        public LearnerService(IAppSettings settings, IFailoverRepository failoverRepository, ILearnerDataAccess LearnerDataAccess, IArchivedDataService archivedDataService)
        {
            _settings = settings;
            _failoverRepository = failoverRepository;
            _LearnerDataAccess = LearnerDataAccess;
            _archivedDataService = archivedDataService; 
        }

        public async Task<Learner> GetLearner(int learnerId, bool isLearnerArchived)
        {

            Learner archivedLearner = null;

            if (isLearnerArchived)
            {
                
                archivedLearner = _archivedDataService.GetArchivedLearner(learnerId);

                return archivedLearner;
            }
            else
            {
                var failoverEntries = _failoverRepository.GetFailOverEntries();


                var failedRequests = 0;

                foreach (var failoverEntry in failoverEntries)
                {
                    if (failoverEntry.DateTime > DateTime.Now.AddMinutes(-10))
                    {
                        failedRequests++;
                    }
                }

                LearnerResponse LearnerResponse = null;
                Learner Learner = null;

                if (failedRequests > 100 && _settings.IsFailoverModeEnabled)
                {
                    LearnerResponse =  FailoverLearnerDataAccess.GetLearnerById(learnerId);
                }
                else
                {
                    LearnerResponse = await _LearnerDataAccess.LoadLearnerAsync(learnerId);


                }

                if (LearnerResponse.IsArchived)
                {
                    var archivedDataService = new ArchivedDataService();
                    Learner = archivedDataService.GetArchivedLearner(learnerId);
                }
                else
                {
                    Learner = LearnerResponse.Learner;
                }


                return Learner;
            }
        }
    }
}
