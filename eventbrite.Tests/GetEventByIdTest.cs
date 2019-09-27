using System.Threading;
using System.Threading.Tasks;
using eventbrite.Queries.GetEventById;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace eventbrite.Tests
{
    [TestClass]
    public class GetEventByIdTest
    {
        [TestMethod]
        public async Task HandlerReturnsCorrectUserViewModel()
        {
            var mediator = new Mock<IMediator>();
            
            var sut = new GetEventByIdQueryHandler();
            var result = await sut.Handle(new GetEventByIdQuery() { EventId = "74445312935" }, CancellationToken.None);

            Assert.IsNotNull(result);
        }
    }
}
