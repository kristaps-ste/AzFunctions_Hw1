using System;
using System.Threading;
using System.Threading.Tasks;
using Application.RandomApiRequest;
using Application.Storage;
using Domain;
using Moq;
using Xunit;

namespace Application.Tests
{
    public class RandomApiRequestManagerTests
    {
        IRandomApiRequestManager _randomApiRequestManager;
        private Mock<IContentStorage> _contentStorage;
        private Mock<IRequestAttemptStorage> _requestAttemptStorage;
        private Mock<IRandomApiRequestService> _randomApiRequestServiceMock;
        
        public RandomApiRequestManagerTests()
        {
            _contentStorage = new Mock<IContentStorage>();
            _requestAttemptStorage = new Mock<IRequestAttemptStorage>();
            _randomApiRequestServiceMock = new Mock<IRandomApiRequestService>();
            _randomApiRequestManager = new RandomApiRequestManager(_contentStorage.Object, _requestAttemptStorage.Object, _randomApiRequestServiceMock.Object);
        }
        
        
        [Fact]
        public async Task ProcessRequestAsync_ShouldSaveContent_WhenPositiveResponseFromAPi()
        {
            // Arrange
            var testContent = "test";
            _randomApiRequestServiceMock.Setup(x => x.RequestRandomApiContentAsync()).ReturnsAsync(testContent);
            
            // Act
            await _randomApiRequestManager.ProcessRequestAsync(CancellationToken.None);
            
            // Assert
            _contentStorage.Verify(x => 
                x.UploadRequestContentAsync(It.Is<RequestAttemptResult>(props => (props.Content == testContent && props.Success)), true), Times.Once);
            _requestAttemptStorage.Verify(x =>
                x.SaveRequestAttempt(It.Is<RequestAttemptResult>(props => (props.Content == testContent && props.Success))), Times.Once);
        }

        [Fact]
        public async Task ProcessRequestAsync_ShouldRegisterFailure_WhenNegativeResponseFromAPi()
        {
            // Arrange
            _randomApiRequestServiceMock.Setup(x => x.RequestRandomApiContentAsync()).ThrowsAsync(new RequestFailedException(""));
            
            // Act
            await _randomApiRequestManager.ProcessRequestAsync(CancellationToken.None);
            
            // Assert
            _requestAttemptStorage.Verify(x =>
                x.SaveRequestAttempt(It.Is<RequestAttemptResult>(props => (!props.Success))), Times.Once);
            _contentStorage.Verify(x => 
                x.UploadRequestContentAsync(It.IsAny<RequestAttemptResult>(), true), Times.Never);
        }
    }
}