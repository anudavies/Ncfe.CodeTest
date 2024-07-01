using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using FluentAssertions;
using Ncfe.CodeTest.Interfaces;
using System.Threading.Tasks;

namespace Ncfe.CodeTest.Tests
{
    public class LearnerServiceTests
    {
        
            private LearnerService _learnerService;

            private Mock<IFailoverRepository> _failoverRepository;

            private Mock<IAppSettings> _appSettings;

            private Mock<ILearnerDataAccess> _learnerDataAccess;
        
            private Mock<IArchivedDataService> _archivedDataService;

            public LearnerServiceTests()
            {
                _failoverRepository = new Mock<IFailoverRepository>();
                _learnerDataAccess = new Mock<ILearnerDataAccess>();
                _appSettings = new Mock<IAppSettings>();
                _archivedDataService = new Mock<IArchivedDataService>();

                _learnerService = new LearnerService(_appSettings.Object, _failoverRepository.Object,
                    _learnerDataAccess.Object,_archivedDataService.Object);
            }

            [Fact]
            public async Task ReturnsLearnerFromDataStore_WHEN_Archived() 
            {
                // Arrange
                const int learnerId = 112355;
                var expectedLearner = new Learner { Id = learnerId, Name = "Samuel" };
                //_appSettings.Setup(mock => mock.IsFailoverModeEnabled).Returns(false);
                _archivedDataService.Setup(mock => mock.GetArchivedLearner(learnerId)).Returns(expectedLearner);

                // Act
                var result = await _learnerService.GetLearner(learnerId, true);

                // Assert
                result.Should().BeEquivalentTo(expectedLearner);
             }

            [Fact]
            public async Task ReturnsLearnerFromMainLearnerDataStore_WHEN_NotInFailOver_AND_NotArchived()
            {
                // Arrange
                const int learnerId = 39075;
                var emptyFailoverEntries = new List<FailoverEntry>();
                var expectedLearner = new Learner { Id = learnerId, Name = "David" };
                _appSettings.Setup(mock => mock.IsFailoverModeEnabled).Returns(true);
                _failoverRepository.Setup(mock => mock.GetFailOverEntries()).Returns(emptyFailoverEntries);

                _learnerDataAccess.Setup(mock => mock.LoadLearnerAsync(learnerId))
                    .ReturnsAsync(new LearnerResponse { Learner = expectedLearner });

                // Act
                var result = await _learnerService.GetLearner(learnerId, false);

                // Assert
                result.Should().BeEquivalentTo(expectedLearner);   
               
            }
    }
}
